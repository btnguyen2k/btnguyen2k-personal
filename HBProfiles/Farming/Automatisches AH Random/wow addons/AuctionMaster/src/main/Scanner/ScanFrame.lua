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

vendor.ScanFrame = {}
vendor.ScanFrame.prototype = {}
vendor.ScanFrame.metatable = {__index = vendor.ScanFrame.prototype}

local FRAME_HEIGHT = 500
local FRAME_WIDTH = 890
local TREE_WIDTH = 150
local SNIPERS_HEIGHT = 230
local TYPES_HEIGHT = 300
local TABLE_TOP = -51
local TABLE_WIDTH = 609
local TABLE_HEIGHT = 358
local STOP_WIDTH = 90

vendor.ScanFrame.SCAN_SPEED_OFF = 0
vendor.ScanFrame.SCAN_SPEED_SLOW = 1
vendor.ScanFrame.SCAN_SPEED_MEDIUM = 2
vendor.ScanFrame.SCAN_SPEED_FAST = 3
vendor.ScanFrame.SCAN_SPEED_HURRY = 4

vendor.ScanFrame.SPEEDS = {
	[vendor.ScanFrame.SCAN_SPEED_OFF] = 0,
	[vendor.ScanFrame.SCAN_SPEED_SLOW] = 200,
	[vendor.ScanFrame.SCAN_SPEED_MEDIUM] = 400,
	[vendor.ScanFrame.SCAN_SPEED_FAST] = 700,
	[vendor.ScanFrame.SCAN_SPEED_HURRY] = 1200
}

local log = vendor.Debug:new("ScanFrame")
local L = vendor.Locale.GetInstance()

local function _HideAuctionFrame(self)
	self.auctionFrameScale = AuctionFrame:GetScale()
	AuctionFrame:SetScale(0.0001)
end

local function _RestoreAuctionFrame(self)
	AuctionFrame:SetScale(self.auctionFrameScale or 1)
end

local function _Stop(self)
	vendor.Scanner:StopScan()
end

local function _Hide(self)
	_Stop(self)
	self.frame:Hide()
	_RestoreAuctionFrame(self)
end

local function _CheckUpdate(but)
	if (but:GetChecked()) then
		vendor.Scanner.db.profile.getAll = true
	else
		vendor.Scanner.db.profile.getAll = false
	end
end

local function _DisableOtherTabs(self)
	for i=1,10 do
		if (i ~= self.id) then
			local tabBut = _G["AuctionFrameTab"..i]
			if (tabBut) then
				tabBut:Disable()
			end
		end
	end
end

local function _EnableOtherTabs(self)
	for i=1,10 do
		if (i ~= self.id) then
			local tabBut = _G["AuctionFrameTab"..i]
			if (tabBut) then
				tabBut:Enable()
			end
		end
	end
end

local function _OnUpdateFrame(self)
	if (vendor.Scanner:IsScanning()) then
		self.scanBut:Hide()
		self.stopBut:Show()
		_DisableOtherTabs(self)
		if (self.getAll) then
			self:SetProgress(L["Performing getAll scan. This may last up to several minutes..."], 0)
		end
	else
		_EnableOtherTabs(self)
		self.scanBut:Show()
		local _, allAllowed = vendor.Scanner:MaySendAuctionQuery()
		allAllowed = allAllowed and vendor.Scanner.db.profile.getAll
		if (allAllowed and vendor.ScanFrame.SCAN_SPEED_OFF ~= vendor.Scanner.db.profile.scanSpeed) then
			self.scanBut:SetNormalTexture("Interface\\Addons\\AuctionMaster\\src\\resources\\UI-Panel-Button-Up-green")
		else
			self.scanBut:SetNormalTexture("Interface\\Buttons\\UI-Panel-Button-Up")
		end
		self.stopBut:Hide()
	end
	
	-- status bar
	if (self.statusBar:GetValue() > 0.001 or string.len(self.statusBarText:GetText() or "") > 0) then
		self.title:Hide()
		if (vendor.AuctionHouse.db.profile.amButtonPos == 0) then
			-- AuctionMaster button is at the right pos
			self.statusBar:SetPoint("TOPLEFT", 70, -17)
			self.statusBar:SetWidth(623)
		else
			-- AuctionMaster button is at the left pos
			self.statusBar:SetPoint("TOPLEFT", 185, -17)
			self.statusBar:SetWidth(618)
		end
		self.statusBar:Show()
	else
		self.title:Show()
		self.statusBar:Hide()
	end
end

local function _OnHideFrame(self)
	_EnableOtherTabs(self)
end

local function _Scan(self)
	self:Clear()
	vendor.Scanner:Scan()
end

local function _OnSniperClick(checkbox)
	local self = checkbox.obj
	local selected = checkbox:GetChecked()
	self.itemModel:SetSniperVisibility(checkbox.sniperId, selected)
	if (selected) then
		vendor.Scanner.db.profile.snipers[checkbox.sniperId] = true
	else
		vendor.Scanner.db.profile.snipers[checkbox.sniperId] = nil
	end
end

local function _CreateSnipers(self)
	local snipers = vendor.Sniper:GetSnipers()
	local yOff = -80
	local title = self.frame:CreateFontString(nil, "OVERLAY", "GameFontHighlight")
	title:SetText(L["Snipers"])
	local xOff = 27 
	title:SetPoint("TOPLEFT", xOff, yOff)
	yOff = yOff - 12
	local checkbox
	for i=1,#snipers do
		local sniper = snipers[i]
		local selected = vendor.Scanner.db.profile.snipers[sniper:GetId()]
		if (selected) then
			self.itemModel:SetSniperVisibility(sniper:GetId(), selected)
		end
		checkbox = vendor.GuiTools.CreateCheckButton(nil, self.frame, "UICheckButtonTemplate", 24, 24, selected)
		checkbox.obj = self
		checkbox.sniperId = sniper:GetId()
		checkbox:SetPoint("TOPLEFT", xOff, yOff)
		checkbox:SetScript("OnClick", _OnSniperClick)
		local f = self.frame:CreateFontString(nil, "OVERLAY", "GameFontHighlight")
		f:SetPoint("LEFT", checkbox, "RIGHT", 0, 0)
		f:SetText(sniper:GetDisplayName())
		yOff = yOff - 24
	end
	return checkbox 
end


local function _NotScanning(self)
	local isScanning, getAll = vendor.Scanner:IsScanning()
	if (isScanning) then
		self.getAll = getAll
	end
	return not isScanning
end

local function _Bid(self)
	log:Debug("_Bid")
	local rows = wipe(self.tmpList1)
	local auctions = wipe(self.buyTable)
   	for _, row in pairs(self.itemModel:GetSelectedItems()) do
   		local _, itemLink, _, name, _, count, minBid, minIncrement, buyout, bidAmount, _, _, index = self.itemModel:Get(row)
   		local bid = minBid
   		if (bidAmount and bidAmount > 0) then
   			bid = bidAmount + (minIncrement or 0) 
   		end
   		local info = {itemLink = itemLink, name = name, count = count, bidAmount = bidAmount, minBid = minBid, buyout = buyout, bid = bid, index = index, reason = L["Bid"]}
   		log:Debug("name [%s] minBid [%s] minIncrement [%s]", name, minBid, minIncrement)
   		table.insert(rows, row)
   		table.insert(auctions, info)
   	end
   	self.itemModel:RemoveRows(rows)
   	local n = #auctions
 	if (self.getAll) then
    	vendor.Scanner:PlaceAuctionBid("list", auctions, self.possibleGap)
    else
    	vendor.Scanner:BuyScan(auctions)
    end
    self.possibleGap = self.possibleGap + n
end

local function _Buyout(self)
	log:Debug("_Buyout")
	local rows = wipe(self.tmpList1)
	local auctions = wipe(self.buyTable)
   	for _, row in pairs(self.itemModel:GetSelectedItems()) do
   		local _, itemLink, _, name, _, count, minBid, minIncrement, buyout, _, _, _, index = self.itemModel:Get(row)
   		log:Debug("name [%s] minBid [%s] minIncrement [%s]", name, minBid, minIncrement)
   		local info = {name = name, itemLink = itemLink, count = count, bidAmount = bidAmount, minBid = minBid, buyout = buyout, bid = buyout, index = index, reason = L["Buy"]}
   		table.insert(rows, row)
   		table.insert(auctions, info)
   	end
   	local n = #auctions
   	self.itemModel:RemoveRows(rows)
	if (self.getAll) then
    	vendor.Scanner:PlaceAuctionBid("list", auctions, self.possibleGap)
    else
    	vendor.Scanner:BuyScan(auctions)
    end
    self.possibleGap = self.possibleGap + n
end

local function _InitFrame(self)
	local frame = vendor.AuctionHouse:CreateTabFrame("AMScannerTab", L["Scanner"], L["Scanner"], self)
	self.frame = frame
	frame.obj = self
	
	-- status bar 
	local statusBar = CreateFrame("StatusBar", nil, frame, "TextStatusBar")
	self.statusBar = statusBar
	statusBar.obj = self
	statusBar:SetHeight(14)
	statusBar:SetWidth(622)
	statusBar:SetStatusBarTexture("Interface\\TargetingFrame\\UI-StatusBar")
	statusBar:SetPoint("TOPLEFT", 70, -17)
	statusBar:SetMinMaxValues(0, 1)
	statusBar:SetValue(1)
	statusBar:SetStatusBarColor(0, 1, 0)
	
	-- status bar text
	local text = statusBar:CreateFontString(nil, "ARTWORK")
	self.statusBarText = text
	text:SetPoint("CENTER", statusBar)
	text:SetFontObject("GameFontHighlight")
		
--	-- enable getAll
--	local checkbox = vendor.GuiTools.CreateCheckButton(nil, frame, "UICheckButtonTemplate", 24, 24, vendor.Scanner.db.profile.getAll, L["Enables the \"GetAll\" scan. This is faster, but may cause disconnects. Deactivate it, if you encounter disconnects during scans."])
--	checkbox.obj = self
--	checkbox:SetPoint("TOPLEFT", 70, -45)
--	checkbox:SetScript("OnClick", _CheckUpdate)
--	local f = checkbox:CreateFontString(nil, "OVERLAY", "GameFontHighlight")
--	f:SetPoint("LEFT", checkbox, "RIGHT", 0, 0)
--	f:SetText(L["GetAll"])
--	self.getAllCheck = checkbox
	-- scan speed
	scanSpeed = vendor.DropDownButton:new(nil, frame, 55, L["Speed"], L["Too high scan speed may lead to disconnects. You should lower it, if you encounter problems."]);
	scanSpeed:SetPoint("TOPLEFT", 54, -45)
    local speeds = {}
	table.insert(speeds, {value = vendor.ScanFrame.SCAN_SPEED_OFF, text = L["off"]})
	table.insert(speeds, {value = vendor.ScanFrame.SCAN_SPEED_SLOW, text = L["slow"]})
	table.insert(speeds, {value = vendor.ScanFrame.SCAN_SPEED_MEDIUM, text = L["easy"]})
	table.insert(speeds, {value = vendor.ScanFrame.SCAN_SPEED_FAST, text = L["fast"]})
	table.insert(speeds, {value = vendor.ScanFrame.SCAN_SPEED_HURRY, text = L["hurry"]})
	scanSpeed:SetItems(speeds, vendor.Scanner.db.profile.scanSpeed or vendor.ScanFrame.SCAN_SPEED_FAST)
	scanSpeed:SetListener(self)
	
	-- stop button 
	local but = CreateFrame("Button", nil, frame, "UIPanelButtonTemplate")
	but.obj = self
	but:SetText(L["Stop"])
	but:SetWidth(STOP_WIDTH)
	but:SetHeight(20)
	but:SetPoint("LEFT", scanSpeed.button, "RIGHT", -15, 3)
	but:SetScript("OnClick", function(but) _Stop(but.obj) end)
	vendor.GuiTools.AddTooltip(but, L["Aborts the current scan."])
	self.stopBut = but
	
	-- scan button 
	local but = CreateFrame("Button", nil, frame, "UIPanelButtonTemplate")
	but.obj = self
	but:SetText(L["Scan"])
	but:SetWidth(STOP_WIDTH)
	but:SetHeight(20)
	but:SetPoint("TOPLEFT", self.stopBut, "TOPLEFT", 0, 0)
	but:SetScript("OnClick", function(but) _Scan(but.obj) end)
	vendor.GuiTools.AddTooltip(but, L["Scans the auction house for updating statistics and sniping items. Uses a fast \"GetAll\" scan, if the scan button is displayed with a green background. This is only possible each 15 minutes."])
	self.scanBut = but

	-- scan/stop switch
	frame:SetScript("OnUpdate", function(frame) _OnUpdateFrame(frame.obj) end)
	frame:SetScript("OnHide", function(frame) _OnHideFrame(frame.obj) end)
	
	-- close button
	vendor.AuctionHouse:CreateCloseButton(frame, "AMScanFrameClose")
	
	-- selected snipers
	_CreateSnipers(self)
--   -- create the frame
--	local frame = CreateFrame("Frame", nil, UIParent, "VendorDialogTemplate")
--	frame.obj = self
--	self.frame = frame
--	frame:SetWidth(FRAME_WIDTH)
--	frame:SetHeight(FRAME_HEIGHT)
--	frame:SetPoint("TOPLEFT", 0, -104)
--	--frame:SetFrameStrata("DIALOG")
--	frame:SetMovable(true)
--	frame:EnableMouse(true)
--	frame:SetToplevel(true)
----	frame:SetClampedToScreen(true)
--	frame:SetScript("OnMouseDown", function() this:StartMoving() end)
--	frame:SetScript("OnMouseUp", function() this:StopMovingOrSizing() end)
--				
--	-- title string
--	local text = frame:CreateFontString(nil, "OVERLAY")
--	text:SetPoint("TOP", frame, "TOP", 0, -10)
--	text:SetFontObject("GameFontHighlightLarge")
--	text:SetText(L["Scanner"])
--	
--	-- close button
--	local but = CreateFrame("Button", nil, frame, "UIPanelCloseButton")
--	but.obj = self
--	but:SetPoint("TOPRIGHT", 3, 0)
--	but:SetScript("OnClick", function(but) _Hide(but.obj) end)
--	
--	-- status bar 
--	local statusBar = CreateFrame("StatusBar", name, frame, "TextStatusBar")
--	self.statusBar = statusBar
--	statusBar:SetHeight(14)
--	statusBar:SetWidth(FRAME_WIDTH - STOP_WIDTH - 15)
--	statusBar:SetStatusBarTexture("Interface\\TargetingFrame\\UI-StatusBar")
--	statusBar:SetPoint("TOPLEFT", 5, -30)
--	statusBar:SetMinMaxValues(0, 1)
--	statusBar:SetValue(0)
--	statusBar:SetStatusBarColor(0, 1, 0)
--	
--	-- status bar text
--	local text = statusBar:CreateFontString(nil, "ARTWORK")
--	self.statusBarText = text
--	text:SetPoint("CENTER", statusBar)
--	text:SetFontObject("GameFontHighlight")
--	text:SetText("1000/166363 auctions")
--	
--	-- stop button 
--	local but = CreateFrame("Button", nil, frame, "UIPanelButtonTemplate")
--	but.obj = self
--	but:SetText(L["Stop"])
--	but:SetWidth(STOP_WIDTH)
--	but:SetHeight(20)
--	but:SetPoint("TOPRIGHT", -5, -27)
--	but:SetScript("OnClick", function(but) _Stop(but.obj) end)
--	self.stopBut = but
--	
--	-- scan button 
--	local but = CreateFrame("Button", nil, frame, "UIPanelButtonTemplate")
--	but.obj = self
--	but:SetText(L["Scan"])
--	but:SetWidth(STOP_WIDTH)
--	but:SetHeight(20)
--	but:SetPoint("TOPRIGHT", -5, -27)
--	but:SetScript("OnClick", function(but) _Scan(but.obj) end)
--	self.scanBut = but
--	
--	-- scan/stop switch
--	frame:SetScript("OnUpdate", function(frame) _OnUpdateFrame(frame.obj) end)
--	
--	-- selected snipers
--	_CreateSnipers(self)
--	
--	-- selected types
----	local types = AceGUI:Create("TreeMenu")
----	types:SetTree(tree)
----	types:SetWidth(TREE_WIDTH)
----	types:SetHeight(TYPES_HEIGHT)
----	types.frame:SetParent(frame)
----	types:SetPoint("TOPLEFT", snipes.frame, "BOTTOMLEFT", 0, -15)
----	types:SetTitle("Types")
--	
--	frame:Hide()
end

local function _Init(self)
	local itemModel = vendor.ScannerItemModel:new()
	self.itemModel = itemModel
	_InitFrame(self)
	local cmds = {
		[1] = {
			title = L["Bid"],
			tooltip = L["Bids on all selected items."].." "..L["Auctions may be selected with left clicks. Press the ctrl button, if you want to select multiple auctions. Press the shift button, if you want to select a range of auctions."],
    		arg1 = self,
    		func = _Bid,
    		enabledFunc = _NotScanning
    	},
    	[2] = {
    		title = L["Buyout"],
    		tooltip = L["Buys all selected items."].." "..L["Auctions may be selected with left clicks. Press the ctrl button, if you want to select multiple auctions. Press the shift button, if you want to select a range of auctions."],
    		arg1 = self,
    		func = _Buyout,
    		enabledFunc = _NotScanning
    	},
	}
	local cfg = {
		name = "AMScannerAuctions",
		parent = self.frame,
		itemModel = itemModel,
		cmds = cmds,
		config = vendor.Scanner.db.profile.scannerItemTableCfg,
		width = TABLE_WIDTH,
		height = TABLE_HEIGHT,
		xOff = TREE_WIDTH + 10,
		yOff = TABLE_TOP,
		xOff = 214,
		yOff = -51,
		--sortButtonBackground = true,
	}
	local itemTable = vendor.ItemTable:new(cfg)
	self.itemTable = itemTable
	self.buyTable = {}
	self.tmpList1 = {}
	self.possibleGap = 0
	--self.frame:Hide()
	--self:Clear()
end

--[[ 
	Creates a new instance.
--]]
function vendor.ScanFrame:new()
	local instance = setmetatable({}, self.metatable)
	_Init(instance)
	return instance
end

--[[
	Sets the progress together with the given message.
--]]
function vendor.ScanFrame.prototype:SetProgress(msg, percent)
--	self:Show()
	self.statusBar:SetValue(percent)
	self.statusBarText:SetText(msg)
end

--[[
	Notifies the termination of a scan. May play a sound, if the scan frame is currently
	visible.
--]]
function vendor.ScanFrame.prototype:ScanFinished()
	if (self.frame:IsVisible()) then
		PlaySound("AuctionWindowClose")
		self:SetProgress("", 0)
	end
end

--[[
	Clears the progres bar.
--]]
function vendor.ScanFrame.prototype:Clear()
	self.statusBar:SetValue(0)
	self.statusBarText:SetText("")
	self.itemModel:Clear()
	self.itemTable:Update()
	self.possibleGap = 0
end

--[[
	Closes the scanner frame.
--]]
function vendor.ScanFrame.prototype:Hide()
	_Hide(self)
end

--[[
	Updates the gui for displaying the frame (Interface method).
--]]
function vendor.ScanFrame.prototype:UpdateTabFrame()
	AuctionFrameTopLeft:SetTexture("Interface\\Addons\\AuctionMaster\\src\\resources\\UI-AuctionFrame-Auction-TopLeft")
	--AuctionFrameTop:SetTexture("Interface\\AuctionFrame\\UI-AuctionFrame-Auction-Top")
	AuctionFrameTop:SetTexture("Interface\\Addons\\AuctionMaster\\src\\resources\\UI-AuctionFrame-Auction-Top")
	--AuctionFrameTopRight:SetTexture("Interface\\AuctionFrame\\UI-AuctionFrame-Auction-TopRight")
	AuctionFrameTopRight:SetTexture("Interface\\Addons\\AuctionMaster\\src\\resources\\UI-AuctionFrame-Auction-TopRight")
	AuctionFrameBotLeft:SetTexture("Interface\\Addons\\AuctionMaster\\src\\resources\\UI-AuctionFrame-Auction-BotLeft")
	--AuctionFrameBot:SetTexture("Interface\\AuctionFrame\\UI-AuctionFrame-Auction-Bot")
	AuctionFrameBot:SetTexture("Interface\\Addons\\AuctionMaster\\src\\resources\\UI-AuctionFrame-Auction-Bot")
	AuctionFrameBotRight:SetTexture("Interface\\Addons\\AuctionMaster\\src\\resources\\UI-AuctionFrame-Auction-BotRight")
end

--[[
	Returns the type of this auction house tab.
--]]
function vendor.ScanFrame.prototype:GetTabType()
	return "scanner"
end
	
--[[
	Shows the tabbed frame (Interface method).
--]]
function vendor.ScanFrame.prototype:ShowTabFrame()
	log:Debug("ShowTabFrame")
	if (not self.frame:IsVisible()) then
		--_HideAuctionFrame(self)
		self:Clear()
		self.frame:Show()
	end
end

--[[
	Hides the tabbed frame (Interface method).
--]]
function vendor.ScanFrame.prototype:HideTabFrame()
	self.frame:Hide()
end

--[[
	Returns the id of the Scanner tab.
--]]
function vendor.ScanFrame.prototype:GetTabId()
	log:Debug("GetTabId [%s]", self.id)
	return self.id
end

--[[
	Returns the name of the single selected item, if any. Returns nil, if no item is selected. 
--]]
function vendor.ScanFrame.prototype:GetSingleSelected()
	local map = self.itemModel:GetSelectedItems()
	if (#map > 0) then
		local _, _, _, name = self.itemModel:Get(map[1])
		return name
	end
	return nil
end

function vendor.ScanFrame.prototype:DropDownButtonSelected(button, value)
	vendor.Scanner.db.profile.scanSpeed = value
end