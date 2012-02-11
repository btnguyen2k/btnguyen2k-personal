-- GloomySunday@CWDG
local L = LibStub("AceLocale-3.0"):NewLocale("AuctionMaster", "zhCN", false)
if (L) then
	-- AuctionMaster main section
	L["AuctionMaster"] = "拍卖大师"
	L["Toggles the debugging mode."] = "开启纠错模式"
	L["Debug"] = "纠错"
	L["Configuration"] = "设置"
	L["Opens a configuration window."] = "打开设置窗口"
	L["Developer"] = "开发者"
	L["Locale"] = "本地化"
    L["Selects the locale to be used for AuctionMaster. Reload of the UI with /rl needed."] = "选择拍卖大师的本地化语言，需要使用 /rl 重载插件"
	L["Error: %s"] = "错误：%s"
	L["Activated"] = "启用"
	L["auctionmaster_conf_help"] = "这是拍卖大师的设置区域，这里有一些设置模块，按“+”扩展选项。\n\n如果你找不到想要的选项，可以去www.curse.com上留言，如果有意见和建议，可以去wow.curse.com联系我"
	L["Release notes"] = "版本信息"
	L["Opens the documentation for AuctionsMaster."] = "打开拍卖大师的的文档"
	L["Shows the release notes for AuctionsMaster."] = "显示拍卖大师的版本信息"
	L["Resets the complete database of AuctionMaster. This will set everything to it's default values. The GUI will be restarted for refreshing all modules of AuctionMaster."] = "重置拍卖大师的所有全部数据库，所有的值会设定为默认值，拍卖大师将会重新启动以重载模块"
	L["Reset"] = "重置"
	L["Do you really want to reset the AuctionMaster database? All data gathered will be lost!"] = "你真的想要重置数据库吗？所有已收集的数据将会丢失！"

	-- auction house
	L["AuctionHouse"] = "拍卖行"
	L["Start tab"] = "开始标签"
	L["Tab to be selected when opening the auction house."] = "拍卖行打开时选中标签"
	L["Snipe"] = "书签"
	L["Scan"] = "搜索"
	L["Browse"] = "浏览"
	L["Bids"] = "竞标"
	L["Auctions"] = "拍卖"
	L["Close"] = "关闭"
	L["Opens a window to select maximum prices for an item\nto be sniped during the next auction house scans."] = "打开一个设定书签物品\n下次搜索时的最高价"
	L["Starts to scan the auction house to update\nstatistics and snipe for items."] = "开始搜索拍卖行更新统计数据\n以及查找设定的书签物品"
	L["Opens a configuration dialog for AuctionMaster."] = "打开 拍卖大师 的设置窗口"
	L["Toggle position"] = "允许移动"
	L["Left click for starting/stopping scan.\nRight click for selecting preferred scan mode."] = "左键点击开始/停止搜索。\n右键点击选择需要的搜索方式"
	L["Left click for starting/stopping scan.\nRight click for opening the scan window."] = "左键点击开始/停止搜索。\n右键点击打开搜索窗口"
	L["Please select a scan mode first. You can do that with a right click on the portrait icon."] = "请先选择一个搜索模式，你可以通过在拍卖行头像图标上右击来实现"
	L["Help"] = "帮助"
	L["New AuctionMaster release available"] = "有新的 拍卖大师 版本可用"
	L["Tiny"] = "极小"
	L["Very small"] = "很小"
	L["Small"] = "小"
	L["Medium"] = "中"
	L["Large"] = "大"
	L["Extra large"] = "超大"
	L["Deactivated"] = "禁用"
	L["Size"] = "大小"
	L["Columns"] = "列"
	L["Texture"] = "材质"
	
	-- scanner
	L["Found no auctions, press \"Refresh\" to update the list."] = "没找到拍卖品，请按下刷新更新列表"
	L["Itms."] = "堆叠数"
	L["Stop"] = "停止"
	L["There is already a running scan."] = "搜索已经开始..."
	L["Name"] = "名称"
	L["Seller"] = "出售者"
	L["Bid"] = "竞标"
	L["Buyout"] = "一口价"
	L["Auction Scan"] = "拍卖行搜索"
	L["Scan page %d/%d\nPer page %.2f sec\nEstimated time remaining: %s"] = "搜索页面 %d/%d\n每页 %.2f 秒\n预计剩余时间: %s"
	L["Scan page %d/%d"] = "搜索页面 %d/%d"
	L["Finished scan"] = "搜索完毕"
	L["Scanner"] = "搜索者"
	L["Poor"] = "粗糙"
	L["Common"] = "一般"
	L["Uncommon"] = "优秀"
	L["Rare"] = "稀有"
	L["Epic"] = "史诗"
	L["Legendary"] = "传奇"
	L["Artifact"] = "神器"
	L["Minimum quality"] = "最低物品质量"
	L["Selects the minimum quality for items to be scanned in the auction house."] = "设置拍卖行搜索的最低物品质量"
	L["Resets the database of scan snapshots for the current realm and server."] = "重置当前搜索收藏物品的数据库"
	L["Database of scan snapshots for current realm and server where reset."] = "收藏物品的数据库重置完毕"
	L["Continue"] = "继续"
	L["Pause"] = "暂停"
	L["Pauses any snipes for the current scan."] = "暂停搜索时对收藏物品的检查"
	L["Opens the snipe dialog for this item."] = "为该物品打开收藏对话框"
	L["Scan auction %s/%s - time left: %s"] = "搜索拍卖品 %s/%s - 剩余时间：%s"
	L["Reason"] = "原因"
	L["Scan finished after %s"] = "搜索将在 %s 后完成"
	L["Do you want to bid on the following auctions?"] = "你想要竞标以下拍卖品吗？"
	L["Buy confirmation"] = "购买确认"
	L["Buy"] = "购买"
	L["GetAll"] = "快速搜索"
	L["Enables the \"GetAll\" scan. This is faster, but may cause disconnects. Deactivate it, if you encounter disconnects during scans."] = "启用\"快速搜索\" 方式。这个模式更快，但可能会导致短线。如果在使用中遇到这样的问题，请禁用之"
	L["Performing getAll scan. This may last up to several minutes..."] = "正在进行快速搜索，这可能会持续几分钟"
	L["Aborts the current scan."] = "取消当前的搜索"
	L["Scans the auction house for updating statistics and sniping items. Uses a fast \"GetAll\" scan, if the scan button is displayed with a green background. This is only possible each 15 minutes."] = "搜索拍卖行以更新数据和跟踪收藏物品。图标绿色时可以使用快速搜索，该功能每15分钟只能使用一次。"	

	-- seller
	L["Can't create stack of %d items, not enough available in inventory."] = " %d 不能堆叠, 背包空间不够"
	L["Found no empty bag place for building a new stack."] = "没有空间来创建一个新的堆叠"
	L["Error while picking up item."] = "提取物品时出错"
	L["Failed to create stack of %d items."] = "%d 堆叠时失败"
	L["Per item"] = "每个物品"
	L["Stack"] = "堆叠"
	L["Overall"] = "全部"
	L["Fixed price"] = "固定价格"
	L["Average price"] = "平均价格"
	L["Market price"] = "商场价格"
	L["Undercut"] = "压价"
	L["Buyout < bid"] = "一口价 < 竞标价"
	L["Starting Price"] = "起始价"
	L["Buyout Price"] = "一口价"
	L["optional"] = "可选"
	L["Deposit:"] = "保管费:"
	L["Create Auction"] = "创建拍卖"
	L["Refresh"] = "刷新"
	L["Failed to sell item"] = "售卖失败"
	L["Auction Item"] = "拍卖物品"
	L["Auction Duration"] = "拍卖时长"
	L["Stack size"] = "堆叠大小"
	L["Amount"] = "数量"
	L["Price calculation"] = "价格计算"
	L["Bid type"] = "拍卖方式"
	L["Drag an item here to auction it"] = "拖动一个物品过来拍卖"
	L["Selects the size of the stacks to be sold."] = "选择拍卖时的堆叠数量"
	L["Selects the number of stacks to be sold.\nThe number with the +-suffix sells\nalso any remaining items."] = "选择出售多少堆叠"
	L["Selects the mode for calculating the sell prices.\nThe default mode Fixed price just select the last sell price."] = "选择计算售价的模式 \n默认的模式是上一次的固定价格"
	L["Selects which prices should be shown in the bid and buyout input fields."] = "选择哪个价钱在竞标和\n一口价输入的地方显示"
	L["Refreshes the list of current auctions for the selected item to be sold."] = "为当前选中出售的物品刷新拍卖品列表"
	L["Up-to-dateness: <= "] = "当前: <= "
	L["Sell"] = "出售"
	L["Autorefresh time"] = "自动刷新时间"
	L["Selects the minimum time in seconds that has to be passed, before the auction house is automatically scanned for the item to be sold."] = "设置拍卖行自动搜索出售物品的最小时间（秒）"
	L["Pickup by click"] = "点击拾取"
	L["Pickup items to be soled, when they are shift left clicked."] = "Shift+左键按下时出售该物品"
	L["Remember stacksize"] = "记住堆叠大小"
	L["Automatically selects the stacksize used the last time for a given item."] = "自动选择物品上使用次的堆叠大小"
	L["Remember duration"] = "记住拍卖时长"
	L["Automatically selects the duration used the last time for a given item."] = "自动选择物品上次使用的拍卖时长"
	L["Remember price model"] = "记住拍卖方式"
	L["Automatically selects the price model used the last time for a given item."] = "自动选择物品上次使用的拍卖方式"
	L["Selling price"] = "出售价"
	L["Percentage to be multiplied with the min bid price."] = "最小竞标价的累乘百分比"
	L["Percentage to be multiplied with the buyout price."] = "一口价的累乘百分比"
	L["Automatic"] = "自动"
	L["Activates the automatic selection mode for the appropriate pricing model."] = "启用当前价格模式的自动选择"
	L["Current price"] = "当前价格"
	L["Auto selling"] = "自动出售"
	L["Settings for automatically selecting the best fitting price model."] = "自动选择最佳价格模式的设置选项"
	L["Upper market threshold"] = "商场模式上限"
	L["Minimal needed percentage of buyouts compared to market price, until they are assumed to be considerably above the market price."] = "取商场价最小百分比，直到远高于商场价"
	L["Lower market threshold"] = "商场模式下限"
	L["Maximal allowed percentage of buyouts compared to market price, until they are assumed to be considerably under the market price."] = "取商场价最大百分比，直到远低于商场价"
	L["There are no other auctions for this item."] = "拍卖行没有该物品"
	L["All other auctions are considerably above the market price."] = "所有其他的拍卖品远高于商场价"
	L["Some other auctions are considerably under the market price."] = "一些拍卖品远低于商场价"
	L["Other auctions have to be undercut."] = "其他被压价的拍卖品"
	L["Default duration"] = "默认持续时间"
	L["Default duration for creating auctions."] = "当前拍卖品的默认持续时间"
	L["Cancel Auction"] = "取消拍卖"
	L["Price modifier"] = "价格调整模块"
	L["Bid multiplier"] = "竞价累乘"
	L["Selects the percentage of the buyout price the bid value should be set to. A value of 100 will set it to the equal value as the buyout price. It will never fall under blizzard's suggested starting price, which is based on the vendor selling value of the item."] = "选择一口价相对于竞标价的百分比，填入100即竞标价等于一口价，永远不会低于BLZ的拍卖行推荐价，该价格和NPC的收购价相关"
	L["Selects the type of price modification."] = "选择价格调整模块的类型"
	L["Please correct the drop downs first!"] = "请先在下拉菜单填入正确的数值"
	L["Calculate starting price"] = "计算初始价格"
	L["Should the starting price be calculated? Otherwise it is dependant from the buyout price."] = "是否开始计算初始价格？不然该价格取决于一口价"
	L["Aucts."] = "拍卖品"
	L["Left click auctions to select them for the available operations."] = "左键选择拍卖品以进行进一步的操作"
	L["No matching auctions found."] = "没有匹配的拍卖品"
	L["Scans the auction house for the item to be sold."] = "在拍卖行搜索该待售物品"
	L["Bids on all selected items."] = "竞标所有选中的物品"
	L["Buys all selected items."] = "一口价所有选中的物品"
	L["Cancels all own auctions that has been selected."] = "取消所有自己选中的拍卖物"
	L["%s (My)"] = "%s (我的)"
	L["%s (Bid)"] = "%s (竞标)"
	L["Placing bid with %s on %s x \"%s\""] = "正在竞标 %s 在 %s x \"%s\""
	L["Couldn't find auction \"%s\""] = "找不到拍卖品 \"%s\""
	
	-- sniper
	L["Set snipe"] = "设置收藏物品"
	L["Name:"] = "名称:"
	L["Type in name of the item here,/nor just drop it in."] = "这里输入物品名称,或者直接拖进来"
	L["Bid:"] = "竞标:"
	L["Buyout:"] = "一口价:"
	L["Set"] = "设置"
	L["Cancel"] = "取消"
	L["Buyout is less or equal %s."] = "一口价小于等于 %s."
	L["Bid is less or equal %s."] = "竞标价小于等于 %s."
	L["Ok"] = "确定"
	L["Item not found"] = "物品没有找到"
	L["Not enough money"] = "没有足够的金钱"
	L["Own auction"] = "竞标"
	L["Higher bid"] = "最高竞标"
	L["No more items of this type possible"] = "只显示这种类型的物品"
	L["Snipe bid (%d)"] = "收藏物品的竞标价 (%d)"
	L["Snipe bid"] = "收藏物品的竞标价"
	L["Snipe buyout (%d)"] = "收藏物品的一口价 (%d)"
	L["Snipe buyout"] = "收藏物品的一口价"
	L["Database of snipes for current realm where reset."] = "当前阵营收藏物品的数据库已重置"
	L["Sniper"] = "收藏"
	L["Resets the database of snipes for the current realm."] = "重置当前阵营收藏物品的数据库"
	L["Show snipes"] = "显示收藏物品"
	L["Selects whether any existing snipes should be shown in the GameTooltip."] = "选择是否在游戏提示框显示收藏物品"
	L["Selects the number of (approximated) values, that should be taken for the moving average of the historically auction scan statistics."] = "选择拍卖行历史统计均线算法参数的（近似）数值"
	L["Delete"] = "删除"
	L["Snipe bookmarked"] = "该物品已被加入收藏"
	L["Snipe for bookmarked items."] = "为收藏中的物品收集数据"
	L["Snipe sell prices"] = "搜索物品的出售价"
	L["Snipe for items with higher sell prices."] = "搜索更高出售价的物品"
	L["Buyout is %s less than sell price (%d%%)."] = "一口价低于出售价%s(%d%%)"
	L["Bid is %s less than sell price (%d%%)."] = "竞标价低于出售价%s(%d%%)"
	L["Minimum profit"] = "最低利润"
	L["Minimum profit in silver that is needed before sniping for items. Will be ignored for bookmarked items."] = "最低利润的设定（银为单位）在书签物品前是必需的，书签物品无视该规则"
	L["Snipe average"] = "收集平均价数据"
	L["Snipe if the estimated profit according to the average values is higher as the given percent number. Set to zero percent to turn it off."] = "如果提供的预期利润超过了平均值的设定百分比, 则将重置为0"
	L["Buyout for %s possible profit (%d%%)."] = "用一口价买下%s的利润可能为(%d%%)"
	L["Bid for %s possible profit (%d%%)."] = "用竞标价买下%s的利润可能为(%d%%)"
	L["Fast scan"] = "快速搜索"
	L["Settings for the normal scan mode."] = "设置一般搜索的方式"
	L["Settings for the fast scan mode."] = "设置快速搜索的方式"
	L["Do you really want to bid on this item?"] = "你真的想要竞标这个物品吗？"
	L["Do you really want to buy this item?"] = "你真的想要买下这个物品吗？"
	L["Snipe average auctions count"] = "检测物品的出现频率"
	L["How often has the item in question to be seen in auction house, until it may be sniped according to the market price."] = "在拍卖行中该物品有多常见，按照商场价格进行监测"
	L["Sell prices"] = "出售价格"
	L["Bookmarked"] = "已收藏"
	L["Market prices"] = "商场价格"
	L["Snipers"] = "搜索系统"
	
	-- statistic
	L["Bid (%d)"] = "竞标 (%d)"
	L["Buyout (%d)"] = "一口价 (%d)"
	L["Lower bid (%d)"] = "更低的竞标价 (%d)"
	L["Lower bid"] = "更低的竞标价"
	L["Lower buyout (%d)"] = "更低的一口价 (%d)"
	L["Lower buyout"] = "更低的一口价"
	L["Show historically averages"] = "显示历史平均价"
	L["Selects whether historically average values from auction scans should be shown in the GameTooltip."] = "选择历史平均价是否在游戏提示框显示"
	L["Show current averages"] = "显示当前平均价"
	L["Selects whether current average values from auction scans should be shown in the GameTooltip."] = "选择当前平均价是否显示在游戏提示框"
	L["Statistics"] = "统计"
	L["Moving average"] = "均线算法"
	L["< standard deviation multiplicator"] = "< 标准偏差乘数"
	L["Selects the standard deviation multiplicator for statistical values to be removed, which are smaller than the average. The larger the multiplicator is selected, the lesser values are removed from the average calculation."] = "当标准差累乘值比平均值小的时候会被移除，\n所以设定的累乘值越大，\n越少的变量会被从平均值计算中去除"
	L["> standard deviation multiplicator"] = "> 标准偏差乘数"
	L["Selects the standard deviation multiplicator for statistical values to be removed, which are larger than the average. The larger the multiplicator is selected, the lesser values are removed from the average calculation."] = "当标准差累乘值比平均值大的时候会被移除，\n所以设定的累乘值越大，\n越少的变量会被从平均值计算中去除"
	
	-- tooltip hook
	L["AuctionMaster statistics"] = "拍卖大师 统计"
	L["Selects whether any informations from AuctionMaster should be shown in the GameTooltip."] = "选择是否所有的 拍卖大师 信息都在游戏提示框显示"
	L["Tooltip"] = "提示框"
	L["AuctionMaster label"] = "拍卖大师的标签"
	L["Selects whether the AuctionMaster label should be shown in the GameTooltip."] = "选择 拍卖大师 的标签是否显示在游戏提示框中"
	L["Average min bid"] = "最小竞标价的平均值"
	L["Average buyout"] = "平均一口价"
	L["Current auctions label"] = "当前拍卖品标签"
	L["Current min bid"] = "当前最小竞标价"
	L["Current buyout"] = "当前一口价"
	L["Selects whether the average minimum bid prize should be shown in the GameTooltip."] = "选择最小竞标价的平均值标签是否显示在游戏提示框中"
	L["Selects whether the average buyout prize should be shown in the GameTooltip."] = "选择平均一口价的标签是否显示在游戏提示框中"
	L["Selects whether the label for current auctions should be shown in the GameTooltip."] = "选择当前拍卖品的标签是否显示在游戏提示框中"
	L["Selects whether the current minimum bid prize should be shown in the GameTooltip."] = "选择当前最小竞标价的标签是否显示在游戏提示框中"
	L["Selects whether the current buyout prize should be shown in the GameTooltip."] = "选择当前一口价的标签是否显示在游戏提示框中"
	L["All time auctions label"] = "历史拍卖品标签"
	L["Selects whether the label for all time auctions should be shown in the GameTooltip."] = "选择历史拍卖品标签的标签是否显示在游戏提示框中"
	L["Current lower min bid"] = "当前更低的最小竞标价"
	L["Selects whether the current lower minimum bid prize should be shown in the GameTooltip."] = "选择当前更低的最小竞标价的标签是否显示在游戏提示框中"
	L["Current lower buyout"] = "当前更低的最小一口价"
	L["Selects whether the current lower buyout prize should be shown in the GameTooltip."] = "选择当前更低的最小一口价的标签是否显示在游戏提示框中"
	L["Compact auction information"] = "精简的拍卖品信息"
	L["Detailed auction information"] = "详细的拍卖品信息"
	L["Will display the number of current auctions and the corresponding lower buyout, if any. Otherwise it will display the number of historical values and the current market price."] = "显示该拍卖品的数量以及存在的相应的较低一口价。如果不存在，将显示历史价值和当前的商场价格"
	L["Current auctions [%s]"] = "当前拍卖品 [%s]"
	L["All time auctions [%s]"] = "历史拍卖品 [%s]"
	L["Current auctions [%s](%d)"] = "当前拍卖品 [%s](%d)"
	L["All time auctions [%s](%d)"] = "历史拍卖品 [%s](%d)"
	L["Only market price color"] = "仅着色商场价格"
	L["The color for the market price label in the compact auction info if there are no current auctions."] = "给拍卖品精简信息模式着色商场价格的标签，当当前无该拍卖品"
	L["Always market price"] = "一直显示商场价格"
	L["Will display a compact market price information even if there are current auctions."] = "依旧显示一个精简的商场价格信息当当前拍卖品存在的时候"
	L["Adjust current prices"] = "调整当前的价格"
	L["Will adjust the current prices with a standard deviation, configured in the statistics section."] = "将会在一定的浮动范围调整当前的价格，可以在统计标签中设置"
	
	-- items
	L["Items"] = "物品"
	L["Reset database"] = "重置数据库"
	L["Resets the database of items for the current realm and server. This will delete all alltime statistics and sell prizes, so be careful."] = "重置当前服务器/阵营的数据库会丢失所有的统计和售价，谨慎操作！"
	L["Database of items for current realm/server where reset."] = "当前服务器/阵营的物品数据库已重置"
	L["Do you really want to reset the database?"] = "你确定要重置数据库？"
	L["Yes"] = "确定"
	L["No"] = "取消"
	L["Item Settings"] = "物品设置"
	L["Default"] = "默认"
	L["Stacksize"] = "堆叠数量"
	L["General"] = "常规"
	L["Remember amount"] = "记住数量"
    L["Revert"] = "反转"
    L["Edit"] = "编辑"
    L["Pricing modifier"] = "定价因数"
    L["Specifies a modification to be done on the calculated prices, before starting an auctions."] = "在开始拍卖之前指定一个变量用于计算价格"
	L["Sub. money"] = "小于...(金钱数)"
	L["Sub. percent"] = "小于...(百分比)"
	L["Add money"] = "添加 金钱数"
	L["Add percent"] = "添加 百分比"
    L["None"] = "无"
	
	-- auctions
	L["Lower"] = "小"
	L["Upper"] = "大"
	L["Average"] = "平均"
	L["Extra Large"] = "超大"
	L["Cancel Auctions"] = "取消拍卖品"
	L["Auctions may be selected with left clicks. Press the ctrl button, if you want to select multiple auctions. Press the shift button, if you want to select a range of auctions."] = "拍卖物可以用左键选中，按住Ctrl键时可以多选，按住Shift键以连续选中"
	L["Cancels the selected auctions with just one click."] = "单击以取消物品选择"
	L["Scan Auctions"] = "搜索拍卖品"
	L["Scans the auction house for your own auctions to update the statistics. Afterwards you will be able to see, whether you have been undercut (lower buyouts do exist). "] = "搜索拍卖行中自己的拍卖品，之后你可以得知自己的拍卖品是否被压价"
	L["Cancel Undercut"] = "取消被压价的物品"
	L["Automatically cancels all auctions where you have been undercut. There is no need to select them. Out-dated (greyed-out) statistics won't be considered, you have to press the scan button to refresh them."] = "自动取消所有自己被压价的物品。不必自己去选中它们。过时的（灰色的）数据不会起作用，你需要单击搜索按钮来刷新数据"
	L["There are no auctions to be scanned."] = "没有可被搜索的拍卖品"
	L["Time Left"] = "剩余时间"
	L["%s - Sold"] = "%s - 出售"
	L["%s - %s"] = "%s - %s"
	L["Scanning auction %s/%s"] = "正在搜索 拍卖品 %s/%s"
	L["Can't cancel already sold auction"] = "不能取消已经卖出的物品"
	L["There are out-dated statistics, you should press the scan-button first."] = "过时的（灰色的）数据不会起作用，你需要单击搜索按钮来刷新数据"
end
