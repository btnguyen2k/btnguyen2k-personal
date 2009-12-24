<#if page.content.topic??>
	<#assign _topic = page.content.topic />
	<#assign tabs = [
		{"url":page.content.urlPcHome, "title":language.getMessage('msg.pcHome')},
		{"selected":true, "title":language.getMessage('msg.pcUnlockTopic')},
		{"url":page.content.urlDraftTopics, "title":language.getMessage('msg.pcDraftTopics')+" ("+page.content.numDraftTopics+")"},
		{"url":page.content.urlPublishedTopics, "title":language.getMessage('msg.pcPublishedTopics')+" ("+page.content.numPublishedTopics+")"}
	] />
	<@MAKE_TABS tabs = tabs />
	<@CONTENT_BLOCK colorScheme=THEME_HIGHLIGHTED_BLOCK>
		<#assign form=page.form>
		<form method="post" action="${form.action}" name="${form.name}" enctype="multipart/form-data">
			<#if form.hasErrorMessage()>
				<table border="0" cellpadding="4" cellspacing="1" align="center">
				<tr>
					<td>
						<#list form.errorMessages as msg>
							<span class="errorMessage">${msg}</span>
							<br/>
						</#list>
						<br/>
					</td>
				</tr>
				</table>
			</#if>
			<#if form.hasInfoMessage()>
				<table border="0" cellpadding="4" cellspacing="1" align="center">
				<tr>
					<td>
						<#list form.infoMessages as msg>
							<span class="infoMessage">${msg}</span>
							<br/>
						</#list>
						<br/>
					</td>
				</tr>
				</table>
			</#if>

			<@CONTENT_BLOCK_FREESTYLE colorScheme=THEME_NORMAL_BLOCK>
				<@BLOCK_CONTENT>
					<table border="0" cellpadding="0" cellspacing="0" align="center">
					<tr>
						<td>
							<@CONTENT_BLOCK colorScheme=THEME_INFORMATION title=language.getMessage('msg.information')>
								<table border="0" cellpadding="4" cellspacing="0" align="center">
								<tr>
									<td valign="top"><img border="0" alt="Information" src="images/dialog-information.png" /></td>
									<td valign="top">
										<span class="infoMessage">${language.getMessage('msg.pcUnlockTopic.confirmation')}</span>
									</td>
								</tr>
								</table>
							</@CONTENT_BLOCK>
						</td>
					</tr>
					</table>
				</@BLOCK_CONTENT>
			
				<@BLOCK_HIGHLIGHTED>
					<input type="submit" style="width: 96px" value="${language.getMessage('msg.unlock')}"/>
					<input type="button" style="width: 96px" value="${language.getMessage('msg.cancel')}"
						onclick="javascript: location.href='${form.cancelAction}';"/>
				</@BLOCK_HIGHLIGHTED>

				<@BLOCK_CONTENT>
					<table border="0" cellpadding="4" cellspacing="1" align="center" class="tblListView" width="100%">
					<tr>
						<td class="row1">${language.getMessage('msg.preview')}: <#if _topic.title??><b>${_topic.title?html}</b></#if></td>
					</tr>
					<tr>
						<td class="row0 topic_content">
							<#assign _SCAH = true />
							<#list _topic.contentsForDisplay as content>
								${content}
								<br />
							</#list>
						</td>
					</tr>
					</table>
				</@BLOCK_CONTENT>
			</@CONTENT_BLOCK_FREESTYLE>	
		</form>
	</@CONTENT_BLOCK>
</#if>