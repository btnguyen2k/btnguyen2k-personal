<#assign tabs = [
	{"url":page.content.urlPcHome, "title":language.getMessage('msg.pcHome')},
	{"selected":true, "title":language.getMessage('msg.pcCreateTopic')},
	{"url":page.content.urlDraftTopics, "title":language.getMessage('msg.pcDraftTopics')+" ("+page.content.numDraftTopics+")"},
	{"url":page.content.urlPublishedTopics, "title":language.getMessage('msg.pcPublishedTopics')+" ("+page.content.numPublishedTopics+")"}
] />
<@MAKE_TABS tabs = tabs />
<@CONTENT_BLOCK colorScheme=THEME_HIGHLIGHTED_BLOCK>
	<#assign form=page.form>
	<form method="post" action="${form.action}" name="${form.name}">
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
			<@BLOCK_TITLE title=language.getMessage('msg.topic.type') />
			<@BLOCK_CONTENT>
				<table border="0" cellpadding="4" cellspacing="1" align="center" class="tblListView" width="100%">
				<tr>
					<td align="right" class="row1" width="40%">${language.getMessage('msg.topic.type')}</td>
					<td class="row0" width="60%">
						<select name="topicType">
							<option value="${TOPIC_TYPE_QUESTION}">${language.getMessage('msg.topic.type.question')}</option>
							<option value="${TOPIC_TYPE_ARTICLE}">${language.getMessage('msg.topic.type.article')}</option>
							<option value="${TOPIC_TYPE_INFORMATION}">${language.getMessage('msg.topic.type.information')}</option>
						</select>
					</td>
				</tr>
				</table>
			</@BLOCK_CONTENT>
			
			<@BLOCK_HIGHLIGHTED>
				<input type="submit" style="width: 96px" value="${language.getMessage('msg.ok')}"/>
				<input type="button" style="width: 96px" value="${language.getMessage('msg.cancel')}"
					onclick="javascript: location.href='${form.cancelAction}';"/>
			</@BLOCK_HIGHLIGHTED>
		</@CONTENT_BLOCK_FREESTYLE>	
	</form>
</@CONTENT_BLOCK>
