<#macro HORIZONTAL_LINE>
	<div style="border-top: 1px solid ${COLOR_THEME}; <#if clientRequest.isIE()>width: 100%</#if>"/>
</#macro>

<#macro PAGINATION_TABLE table=table>
	<div class="pagination">
		${language.getMessage('msg.page')}:
		<#if table.currentPage gt 1><a href="${table.urlFirstPage}"><img src="images/go-first.png" alt="First"
			border="0" style="padding-bottom: 4px; vertical-align: middle;"/></a></#if>
		<#if table.currentPage gt 1><a href="${table.urlPreviousPage}"><img src="images/go-previous.png" alt="Previous"
			border="0" style="padding-bottom: 4px; vertical-align: middle;"/></a></#if>
		<#list table.pagination as pagination>
			<#if pagination.page==table.currentPage>
				&nbsp;
				(${pagination.page})
			<#else>
				&nbsp;
				<a href="${pagination.url}">${pagination.page}</a>
			</#if>
		</#list>
		&nbsp;
		<#if table.currentPage lt table.numPages><a href="${table.urlNextPage}"><img src="images/go-next.png" alt="Next"
			border="0" style="padding-bottom: 4px; vertical-align: middle;"/></a></#if>
		<#if table.currentPage lt table.numPages><a href="${table.urlLastPage}"><img src="images/go-last.png" alt="last"
			border="0" style="padding-bottom: 4px; vertical-align: middle;"/></a></#if>
	</div>
</#macro>

<#macro MAKE_TABS tabs>
	<table border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td style="width: 4px"><img src="images/dot_bkground.gif" width="4" alt="Blank" /></td>
		<#list tabs as tab>
			<td style="padding-right: 1px">
				<div class="<#if tab.selected?? && tab.selected>${THEME_SELECTED_TAB}<#else>${THEME_NORMAL_TAB}</#if>">
					<div class="block_top">
						<div class="block_tl"></div>
						<div class="block_tr"></div>
					</div>
					<div style="padding-left: 16px; padding-right: 16px;" class="block_title <#if tab.selected?? && tab.selected>selected_tab<#else>other_tab</#if>"
						><#if !(tab.selected?? && tab.selected)><a href="${tab.url}"
						></#if>${tab.title}<#if !(tab.selected?? && tab.selected)></a></#if></div>
				</div>
			</td>
		</#list>
	</tr>
	</table>
</#macro>

<#macro BLOCK_TITLE title>
	<div class="block_title">${title?html}</div>
</#macro>

<#macro BLOCK_HIGHLIGHTED>
	<div class="block_title"><#nested /></div>
</#macro>

<#macro BLOCK_CONTENT>
	<div class="block_content"
			<#if clientRequest.isIE() && clientRequest.browserVersionMajor==6>style="width: 100%"</#if>>
		<#nested />
	</div>
</#macro>

<#macro CONTENT_BLOCK_FREESTYLE colorScheme>
	<div class="${colorScheme}">
		<div class="block_top">
			<div class="block_tl"></div>
			<div class="block_tr"></div>
		</div>
		<div class="block_body">
			<#nested />
		</div>
		<div class="block_bottom">
			<div class="block_bl"></div>
			<div class="block_br"></div>
		</div>
	</div>
</#macro>

<#macro CONTENT_BLOCK title="" url="">
	<#if title!=""><h1><#if url!=""><a href="${url}"></#if>${title?html}<#if url!=""></a></#if></h1></#if>
	<#nested />
</#macro>

<#macro RENDER_PORTLET_LIST portlets>
	<#list portlets as portlet>
		<#if portlet.isVisible()>
			<@RENDER_PORTLET portlet = portlet />
		</#if>
	</#list>
</#macro> 

<#macro RENDER_PORTLET portlet>
	<#if portlet.hasBorder()>
		<@RENDER_PORTLET_WITH_BORDER portlet=portlet />
	<#else>
		<@RENDER_PORTLET_WITHOUT_BORDER portlet=portlet />
	</#if>
</#macro>

<#macro RENDER_PORTLET_WITH_BORDER portlet>
	<#assign _hasTitle = (portlet.hasTitle() && portlet.title?? && portlet.title!="") />
	<#assign _isMenu = portlet.isMenu() />
	<#if _hasTitle><h1>${portlet.title?html}</h1></#if>
	<div class="content-box<#if !_isMenu> freestyle-box</#if>">
		<#if _isMenu>
			<@RENDER_MENU_PORTLET portlet=portlet />
		<#else>
			${portlet.content}
		</#if>
	</div>
</#macro>

<#macro RENDER_PORTLET_WITHOUT_BORDER portlet>
	<#assign _isMenu = portlet.isMenu() />
	<div class="content-box<#if !_isMenu> freestyle-box</#if>">
		<#if _isMenu>
			<@RENDER_MENU_PORTLET portlet=portlet />
		<#else>
			${portlet.content}
		</#if>
	</div>
</#macro>

<#macro RENDER_MENU_PORTLET portlet>
	<ul class="sidemenu">
		<#list portlet.menuItems as menuItem>
			<li><#if menuItem.url?? && menuItem.url!=""><a
					href="${menuItem.url}"></#if>${menuItem.text?html}<#if menuItem.url?? && menuItem.url!=""></a></#if></li>
			<#--
			<div <#if clientRequest.isIE() && clientRequest.browserVersionMajor==6>style="width: 100%"</#if>
					class="menuItem <#if
						menuItem.isHighlighted()>highlighted</#if> <#if
						menuItem.isSelected()>selected</#if> <#if
						menuItem.isBulleted()>bullet</#if> <#if
						! menuItem_has_next>last</#if>"
						<#if menuItem.url?? && menuItem.url!="">onclick='javascript:location.href="${menuItem.url}"'</#if>>
				<#if portlet.myModule??>
					<#if 
						menuItem.isBulleted()><img src="images/menuitem_bullet.gif" alt="Bullet"/><#elseif
						menuItem.icon?? && menuItem.icon!=""><img src="../${portlet.myModule.urlMapping}/images/menuicons/${menuItem.icon}.png"
						alt="${menuItem.text?html}" width="16" height="16"/></#if>
				</#if>
				<#if menuItem.url?? && menuItem.url!=""><a
					href="${menuItem.url}"></#if>${menuItem.text?html}<#if menuItem.url?? && menuItem.url!=""></a></#if>
			</div>
			-->
		</#list>
	</ul>
</#macro>
