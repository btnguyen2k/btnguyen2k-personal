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
	Bid or buys a given list of auctions.
--]]
vendor.BidOrBuyTask = {}
vendor.BidOrBuyTask.prototype = {}
vendor.BidOrBuyTask.metatable = {__index = vendor.BidOrBuyTask.prototype}

local log = vendor.Debug:new("BidOrBuyTask")

--[[
	Callback for the sniper.
--]]
local function _AskToBuy(itemLink, itemName, count, minBid, bidAmount, minIncrement, buyout, highBidder, self)
	log:Debug("_AskToBuy minBid: %d bidAmount: %d", minBid or 777, bidAmount or 777)
	for k,v in pairs(self.auctions) do
		if (not v.wasBought) then
			local itemLinkKey = vendor.Items:GetItemLinkKey(itemLink)
			if (v.itemLinkKey == itemLinkKey and v.count == count) then
				local bid = math.max(minBid, bidAmount or 0)
				local doBid = math.max(v.minBid or 0, v.bidAmount or 0)
				log:Debug("bid: %d doBid: %d", bid or 777, doBid or 777)
				if (bid == doBid and not highBidder) then
					log:Debug("bid it")
					v.wasBought = true
					return true, false
				end
				if (buyout and buyout > 0 and buyout == v.buyout) then
					log:Debug("buyout it")
					v.wasBought = true
					return false, true
				end
			end
		end
	end
	return false, false
end

local function _ScanFinished(self)
	log:Debug("_ScanFinished")
	self.scanFinished = true
end

--[[ 
	Creates a new instance.
--]]
function vendor.BidOrBuyTask:new(auctions)
	local instance = setmetatable({}, self.metatable)
	instance.auctions = auctions
	return instance
end

--[[
	Run function of the task.
--]]
function vendor.BidOrBuyTask.prototype:Run()
	log:Debug("Run enter")
	self.isRunning = true
	vendor.Sniper:RegisterExclusiveSniper(_AskToBuy, self)
	-- currently only possible for the same auctions
	local prevKey = nil
	for k,v in pairs(self.auctions) do
		if (prevKey and prevKey ~= v.itemLinkKey) then
			assert(false)
		else
			local itemLink = vendor.Items:GetItemLink(v.itemLinkKey)
			vendor.Scanner:Scan(itemLink, true, _ScanFinished, self)
			while (not self.scanFinished) do
				coroutine.yield()
			end
		end
	end
	vendor.Sniper:RegisterExclusiveSniper(nil)
	log:Debug("Run exit")
end

--[[
	Cancels the task and leaves it as soon as possible. 
--]]
function vendor.BidOrBuyTask.prototype:Cancel()
	self.isCancelled = true
	self.isRunning = false
	vendor.Sniper:RegisterExclusiveSniper(nil)
end

--[[
	Returns whether the task was canecelled.
--]]
function vendor.BidOrBuyTask.prototype:IsCancelled()
	return self.isCancelled
end

--[[
	Will be called by the TaskQueue, if the task has failed with an 
	unexpected error.
--]]
function vendor.BidOrBuyTask.prototype:Failed()
	self:Cancel()
end
