<#if page.content.box??>
	<@MAKE_TABS tabs = [
		{"url":page.content.urlAdminHome, "title":language.getMessage('msg.adminHome')},
		{"url":page.content.urlListBoxes, "title":language.getMessage('msg.boxManagement')},
		{"selected":true, "title":language.getMessage('msg.deleteBox') + ' | ' + page.content.box.title?html}
	]/>
	<@CONTENT_BLOCK colorScheme=THEME_HIGHLIGHTED_BLOCK>
		<#assign form=page.form>
		<form method="post" action="${form.action}" name="${form.name}">
			<center>
				<input type="submit" style="width: 96px" value="${language.getMessage('msg.yes')}"/>
				<input type="button" style="width: 96px" value="${language.getMessage('msg.no')}"
					onclick="javascript: location.href='${form.cancelAction}';"/>
			</center>
		</form>
	</@CONTENT_BLOCK>
</#if>
