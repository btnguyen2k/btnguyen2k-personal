--[[
	Copyright (C) Udorn (Blackhand)
	
	This program is free software; you can redistribute it and/or
	modify it under the terms of the GNU General Public License
	as published by the Free Software Foundation; either version 2
	of the License, or (at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.	
--]]

--[[
	Sceleton for performing auction scans. The processing is done in ScanModules.
--]]
vendor.ScanTask = {}
vendor.ScanTask.prototype = {}
vendor.ScanTask.metatable = {__index = vendor.ScanTask.prototype}

local log = vendor.Debug:new("ScanTask")

local L = vendor.Locale.GetInstance()

local SCAN_PAGE_NONE = 0 -- not interested in scanning pages
local SCAN_PAGE_WAIT = 1 -- waiting to scan next page
local SCAN_PAGE_PERFORM = 2 -- got permission to scan

local function _GetAuctionItemInfo(self, typ, index)
	if (self.completeScan) then
    	local now = math.floor(GetTime())
    	if (not self.throttleSecond) then
    		self.throttleSecond = now
    	elseif (now == self.throttleSecond) then
        	self.getCalls = self.getCalls + 1
        	if (self.getCalls > self.getsPerSecond) then
        		-- wait until next second is reached
        		log:Debug("have to throttle")
        		while (true) do
        			coroutine.yield()
        			local now2 = math.floor(GetTime())
        			if (now2 > now) then
        				log:Debug("leave throttle")
        				self.getCalls = 1
        				self.throttleSecond = now2
        				break;
        			end
        		end
        	end
    	else
    		self.getCalls = 0
    	end
	end
	return GetAuctionItemInfo(typ, index)
end
    			
local function _StopScan(self, complete)
	log:Debug("_StopScan cancelled [%s] complete [%s]", self.cancelled, complete)
	if (not self.stopped) then
		if (not self.silent) then
			vendor.Scanner.scanFrame:ScanFinished()
		end
		if (self.completeScan) then
			vendor.Vendor:Print(L["Scan finished after %s"]:format(SecondsToTime(GetTime() - self.startedAt)))
		end
		for i=1,#self.modules do
			self.modules[i]:StopScan(complete)
		end
		vendor.Scanner:AbandonScan(complete)
		self.stopped = true
	end
end
 
--[[
	Blocks until the permission to send a query is given.
--]]
local function _WaitForPermission(self)
	local queryAllowed = false
	while (not queryAllowed and self.running) do
		queryAllowed = vendor.Scanner:MaySendAuctionQuery()
		if (not queryAllowed) then
			coroutine.yield()
		end
	end
end

--[[
	Waits until the owner is available or the the timeout time is reached.
--]]
local function _WaitForOwners(self, timeoutTime)
	local doCheck, now, lastCheck, owner
	-- init the missing owner index
	local index = wipe(self.ownerIndex)
	local numBatchAuctions = GetNumAuctionItems("list")
	for i=1,numBatchAuctions do
		owner = select(12, _GetAuctionItemInfo(self, "list", i))
		if (not owner) then
			table.insert(index, i)
		end
	end
	-- wait for missing owners
	while (#index > 0 and self.running) do
		now = GetTime()
		if (not lastCheck or (now - lastCheck) >= 0.2) then
			lastCheck = now
    		for i=#index,1,-1 do
    			owner = select(12, _GetAuctionItemInfo(self, "list", index[i]))
    			if (owner) then
    				table.remove(index, i)
    			end
    		end
    	end
		if (now >= timeoutTime) then
			break
		end
		coroutine.yield()
	end
	if (#index > 0) then
		log:Debug("[%s] owners not found", #index)
	end
	return #index == 0
end

local function _UpdateStatus(self)
	if (not self.silent) then
    	local now = GetTime()
    	local totalTime = (now - self.startedAt) / ((self.page or 0) / (self.maxPages or 1))
    	local remaining = math.max(0, totalTime - (now - self.startedAt))
    	local restTime = SecondsToTime(remaining)
    	local msg = L["Scan auction %s/%s - time left: %s"]:format(self.currentIndex, self.total, restTime)
    	vendor.Scanner.scanFrame:SetProgress(msg, self.currentIndex / self.total)
    end
end

--[[ 
	Reads in the list of auction items.
--]]
local function _ReadPage(self)
	if (self.completeScan) then
		log:Debug("_ReadPage complete enter")
	end
	local status = 0
	for i=1,#self.modules do
		status = math.max(status, self.modules[i]:StartPage(self.page) or 0)
	end
	if (status == 1 or self.waitForOwners) then
		log:Debug("waitForOnwers")
		if (self.getAll) then
			_WaitForOwners(self, GetTime() + 7)
		else
			_WaitForOwners(self, GetTime() + 30)
		end
	end
	local numBatchAuctions, total = GetNumAuctionItems("list")
	if (numBatchAuctions == total) then
		self.maxPages = 1
	else
		self.maxPages = math.ceil(total / NUM_AUCTION_ITEMS_PER_PAGE)
	end
	local batch = 0
	for index = numBatchAuctions, 1, -1 do
		local itemLink = GetAuctionItemLink("list", index)
		local timeLeft = GetAuctionItemTimeLeft("list", index)
		if (itemLink) then
			local itemLinkKey = vendor.Items:GetItemLinkKey(itemLink)
			if (not (self.itemLinkKey and self.itemLinkKey ~= itemLinkKey)) then
    			local name, texture, count, quality, canUse, level, 
    			minBid, minIncrement, buyoutPrice, bidAmount, 
    			highBidder, owner, saleStatus = _GetAuctionItemInfo(self, "list", index)
				for i=1,#self.modules do
					self.modules[i]:NotifyAuction(itemLinkKey, itemLink, index, name, texture, count, 
							quality, canUse, level, minBid, minIncrement, buyoutPrice, bidAmount, highBidder, 
							owner, saleStatus, timeLeft)
				end
			end
		end
		batch = batch + 1
		if (batch >= 200) then
			-- don't let the game freeze
			coroutine.yield()
			batch = 0
		end
	end
	if (self.completeScan) then
		log:Debug("_ReadPage notified all auctions")
	end
	local status = 0
	for i=1,#self.modules do
		status = math.max(status, self.modules[i]:StopPage() or 0)
	end
	if (status == 1) then
		-- read the same page again
		self.page = self.page - 1
	end
	self.total = total
	if (numBatchAuctions == total) then
		self.currentIndex = total
	else
		self.currentIndex = self.page * NUM_AUCTION_ITEMS_PER_PAGE
	end
	_UpdateStatus(self)
	if (self.completeScan) then
		log:Debug("_ReadPage complete exit")
	end
end

--[[
	Blocks until the current page may be scanned.
--]]
local function _WaitForScan(self)
	while (self.scanPage ~= SCAN_PAGE_NONE and self.running) do
		if (self.scanPage < SCAN_PAGE_PERFORM) then
			coroutine.yield()
		else
			_ReadPage(self)
			self.scanPage = SCAN_PAGE_NONE
		end
	end
end

--[[ 
	Creates a new instance with the given name (scanId) and a query description containing the fields:
	itemLinkKey, name, minLevel, maxLevel, invTypeIndex, classIndex, subclassIndex, isUsable, qualityIndex
	At the end several ScanModules may be added. 
--]]
function vendor.ScanTask:new(scanId, queryInfo, ...)
	local instance = setmetatable({}, self.metatable)
	instance.queryInfo = queryInfo
	instance.running = true
	instance.scanPage = SCAN_PAGE_NONE
	instance.scanId = scanId
	instance.ownerIndex = {}
	instance.modules = {}
	instance.getCalls = 0
	instance.getsPerSecond = vendor.ScanFrame.SPEEDS[vendor.Scanner.db.profile.scanSpeed or vendor.ScanFrame.SCAN_SPEED_FAST]
	for i=1,select('#', ...) do
		local module = select(i, ...)
		table.insert(instance.modules, module)
	end
	return instance
end

--[[
	Run function of the task, performs the scan.
--]]
function vendor.ScanTask.prototype:Run()
	log:Debug("Run enter")
	local info = self.queryInfo
	self.startedAt = GetTime()
	for i=1,#self.modules do
		self.modules[i]:StartScan(self.queryInfo, "list")
	end
	while (self.running) do
		_WaitForScan(self)
		if (not self.page) then
			self.page = 0
		else
			self.page = self.page + 1
		end
		if (self.page > 0 and self.page >= self.maxPages) then
			self.running = false
		else
			_WaitForPermission(self)
			if (not self.running) then
				break
			end
			self.scanPage = SCAN_PAGE_WAIT -- has to scan this page, before we can continue
			local name = info.name
			if (name) then
				-- too long names may cause a disconnect
				name = string.sub(name, 1, 62)
			end
			local _, getAll = vendor.Scanner:MaySendAuctionQuery()
			local scanSpeedOff = vendor.ScanFrame.SCAN_SPEED_OFF == vendor.Scanner.db.profile.scanSpeed
			if (not self.batchScan and (getAll and not name) and not scanSpeedOff) then
				log:Debug("start complete scan")
				self.completeScan = true
				self.getAll = true
				QueryAuctionItems(name, info.minLevel, info.maxLevel, info.invTypeIndex, 
					info.classIndex, info.subclassIndex, self.page, info.isUsable, info.qualityIndex, true)
				log:Debug("auctions where queried")
				-- TODO we may terminate afterwards
			else
				self.batchScan = true
				QueryAuctionItems(name, info.minLevel, info.maxLevel, info.invTypeIndex, 
					info.classIndex, info.subclassIndex, self.page, info.isUsable, info.qualityIndex)
			end
		end
	end
	log:Debug("exit name [%s] failed [%s] canceled [%s]", info.name, self.failed, self.cancelled)
	_StopScan(self, not self.cancelled)
	log:Debug("Run exit")
end

--[[
	Cancels the task and leaves it as soon as possible. 
--]]
function vendor.ScanTask.prototype:Cancel()
	self.cancelled = true
	self.running = false
end

--[[
	Returns whether the task was canecelled.
--]]
function vendor.ScanTask.prototype:IsCancelled()
	return self.cancelled
end

--[[ 
	Reads in the list of auction items, will be called by the Scanner.
--]]
function vendor.ScanTask.prototype:AuctionListUpdate()
	if (self.scanPage == SCAN_PAGE_WAIT) then
		self.scanPage = SCAN_PAGE_PERFORM
	end	
end

--[[
	Will be called by the TaskQueue, if the task has failed with an 
	unexpected error.
--]]
function vendor.ScanTask.prototype:Failed()
	log:Debug("Failed")
	self.failed = true
	self:Cancel()
	_StopScan(self)
end

--[[
	Returns the unique scanId.
--]]
function vendor.ScanTask.prototype:GetScanId()
	return self.scanId
end

--[[
	Returns the result as a map with: scanId
--]]
function vendor.ScanTask.prototype:GetResult()
	return {scanId = self.scanId}
end
