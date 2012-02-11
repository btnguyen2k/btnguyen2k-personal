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
	Snipe using bookmarks.
--]]
vendor.BookmarkSniper = {}
vendor.BookmarkSniper.prototype = {}
vendor.BookmarkSniper.metatable = {__index = vendor.BookmarkSniper.prototype}

local L = vendor.Locale.GetInstance()

--[[ 
	Creates a new instance.
--]]
function vendor.BookmarkSniper:new(auctions)
	local instance = setmetatable({}, self.metatable)
	instance.auctions = auctions
	return instance
end

--[[
	Checks whether to snipe for the item and returns doBuy, reason
--]]
function vendor.BookmarkSniper.prototype:Snipe(itemLink, itemName, count, bid, buyout, highBidder)
	local maxBid, maxBuyout = vendor.Sniper:GetWanted(itemName)
	if (maxBid > 0 or maxBuyout > 0) then
		maxBid = maxBid * count
		maxBuyout = maxBuyout * count
		if (buyout > 0 and maxBuyout > 0 and maxBuyout >= buyout) then
			local reason = L["Buyout is less or equal %s."]:format(vendor.Format.FormatMoney(maxBuyout))
			return true, reason
		elseif (maxBid > 0 and maxBid >= bid and not highBidder) then
			local reason = L["Bid is less or equal %s."]:format(vendor.Format.FormatMoney(maxBid))
			return true, reason
		end		
	end
	return false
end

--[[
	Returns the unique identifier of the sniper.
--]]
function vendor.BookmarkSniper.prototype:GetId()
	return "bookmarkSniper"
end

--[[
	Returns the name to be displayed for this sniper module.
--]]
function vendor.BookmarkSniper.prototype:GetDisplayName()
	return L["Bookmarked"]
end

--[[
	Returns an ordering information for the sniper. Lower numbers will be executed first.
--]]
function vendor.BookmarkSniper.prototype:GetOrder()
	return 1
end
