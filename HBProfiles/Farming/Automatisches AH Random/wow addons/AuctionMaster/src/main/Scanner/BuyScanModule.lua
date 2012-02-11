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
	ScanModule to be integrated in a ScanTask for buying auctions.
--]]
vendor.BuyScanModule = {}
vendor.BuyScanModule.prototype = {}
vendor.BuyScanModule.metatable = {__index = vendor.BuyScanModule.prototype}

local log = vendor.Debug:new("BuyScanModule")

--[[ 
	Creates a new instance.
--]]
function vendor.BuyScanModule:new(auctions)
	local instance = setmetatable({}, self.metatable)
	instance.index = {}
	instance.auctions = {}
	vendor.Tables.Copy(auctions, instance.auctions)
	return instance
end

--[[
	Notifies the beginning of the scan. The info struct contains:
	itemLinkKey, name, minLevel, maxLevel, invTypeIndex, classIndex, subclassIndex, isUsable, qualityIndex
--]]
function vendor.BuyScanModule.prototype:StartScan(info, ahType)
	log:Debug("StartScan name [%s]", info.name)
	self.info = info
	self.ahType = ahType
end

--[[
	Notifies the termination of the scan.
--]]
function vendor.BuyScanModule.prototype:StopScan(complete)
	log:Debug("StopScan name [%s] complete [%s] itemLinkKey [%s]", self.info.name, complete, self.info.itemLinkKey)
end

--[[
	Notifies the ScanModule that a page is about to be read. The ScanModule
	may return:
	1 for letting the ScanTask wait for the owners.
--]]
function vendor.BuyScanModule.prototype:StartPage(page)
	log:Debug("StartPage enter")
	wipe(self.index)
	log:Debug("StartPage exit")
end

--[[
	Notifies the ScanModule that a page is now finished. The ScanModule
	may return:
	1 for scanning the same page again (perhaps something was bought, which modified the current page)
--]]
function vendor.BuyScanModule.prototype:StopPage()
	log:Debug("StopPage enter")
	if (#self.index > 0) then
		log:Debug("[%s]", #self.index)
--		for i=1,#self.index do
--			local info = self.index[i]
--			-- double check
--		end
		-- ask the user
		log:Debug("dialog [%s]", vendor.Scanner.buyDialog)
		local buy = vendor.Scanner.buyDialog:AskToBuy(self.ahType, self.index)
		log:Debug("buy [%s]", buy)
		log:Debug("StopPage exit 1")
		return 1
	end
	log:Debug("StopPage exit")
end

--[[
	Notifies about the given auction data read. The auctions will be notified once for each index.
--]]
function vendor.BuyScanModule.prototype:NotifyAuction(itemLinkKey, itemLink, index, name, texture, count, 
		quality, canUse, level, minBid, minIncrement, buyout, bidAmount, highBidder, owner, saleStatus, 
		timeLeft)
	for i=#self.auctions,1,-1 do
		local info = self.auctions[i]
		if (info.name == name and info.count == count and info.minBid == minBid and info.buyout == buyout) then
			log:Debug("add item index [%s]", index)
			info.quality = quality
			info.texture = texture
			info.index = index
			table.insert(self.index, info)
			table.remove(self.auctions, i)
			break
		end
	end
end
