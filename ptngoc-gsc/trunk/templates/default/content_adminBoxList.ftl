<#if page.content.urlCreateBox??>
	<@MAKE_TABS tabs = [
		{"url":page.content.urlAdminHome, "title":language.getMessage('msg.adminHome')},
		{"selected":true, "title":language.getMessage('msg.boxManagement')},
		{"url":page.content.urlCreateBox, "title":language.getMessage('msg.createBox')}
	]/>
<#else>
	<@MAKE_TABS tabs = [
		{"url":page.content.urlAdminHome, "title":language.getMessage('msg.adminHome')},
		{"selected":true, "title":language.getMessage('msg.boxManagement')}
	]/>
</#if>
<@CONTENT_BLOCK colorScheme=THEME_HIGHLIGHTED_BLOCK>
	<table border="0" cellpadding="4" cellspacing="1" width="100%" class="tblListView">
	<thead>
		<tr>
			<th>
				${language.getMessage('msg.box')}
			</th>
			<th colspan="3">
				${language.getMessage('msg.actions')}
			</th>
		</tr>
	</thead>
	<tbody>
		<#assign _box_index = 0>
		<#macro _LIST_BOX box isFirst isLast indent=0>
			<tr>
				<td class="row${_box_index % 2}">
					<#if indent gt 0>
						<#list 1..indent as i>
							&nbsp;&nbsp;&nbsp;
						</#list>
						<tt>+--</tt>
					</#if>
					<img src="images/small_box_hasnew.gif" alt="" border="0" /> ${box.title?html}
				</td>
				<td class="row${_box_index % 2}" width="70" align="center">
					<a href="${box.urlEdit}">${language.getMessage('msg.edit')}</a>
				</td>
				<td class="row${_box_index % 2}" width="70" align="center">
					<a href="${box.urlDelete}">${language.getMessage('msg.delete')}</a>
				</td>
				<td class="row${_box_index % 2}" width="40" align="center">
					<#if isFirst><img src="images/dot_bkground.gif" alt="" border="0" width="16" height="16"
						></a><#else><a href="${box.urlMoveUp}"><img src="images/moveup.png" alt="${language.getMessage('msg.moveUp')}" border="0"
						></a></#if><#if !isLast><img src="images/dot_bkground.gif" alt="" border="0" width="16" height="16"
						></a><#else><a href="${box.urlMoveDown}"><img src="images/movedown.png" alt="${language.getMessage('msg.moveDown')}" border="0"
						></a></#if>
				</td>
			</tr>
			<#assign _box_index = _box_index+1>
			<#list box.children as child>
				<@_LIST_BOX box=child isFirst=child_index==0 isLast=child_has_next indent=indent+1 />
			</#list>
		</#macro>
		<#list page.content.boxList as box>
			<@_LIST_BOX box=box isFirst=box_index==0 isLast=box_has_next />
		</#list>
	</tbody>
	</table>
</@CONTENT_BLOCK>
