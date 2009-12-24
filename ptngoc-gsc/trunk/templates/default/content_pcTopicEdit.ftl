<#if page.content.topic??>
	<#assign _topic = page.content.topic />
	<#assign tabs = [
		{"url":page.content.urlPcHome, "title":language.getMessage('msg.pcHome')},
		{"selected":true, "title":language.getMessage('msg.pcEditTopic')},
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
				<@BLOCK_TITLE title=language.getMessage('msg.topic.type') />
				<@BLOCK_CONTENT>
					<table border="0" cellpadding="4" cellspacing="1" align="center" class="tblListView" width="100%">
					<tr>
						<td align="right" class="row1" width="40%">${language.getMessage('msg.topic.type')}:</td>
						<td class="row0" width="60%"><#-- background image for <option> does not work on IE! -->
							<select name="topicType">
								<option value="${TOPIC_TYPE_QUESTION}" style="padding-left: 18px; background: url('images/topic_type_question.png'); background-repeat: no-repeat;"
									<#if form.getAttribute('topicType')=TOPIC_TYPE_QUESTION>selected="selected"</#if>>${language.getMessage('msg.topic.type.question')}</option>
								<option value="${TOPIC_TYPE_ARTICLE}" style="padding-left: 18px; background: url('images/topic_type_article.png'); background-repeat: no-repeat;"
									<#if form.getAttribute('topicType')=TOPIC_TYPE_ARTICLE>selected="selected"</#if>>${language.getMessage('msg.topic.type.article')}</option>
								<option value="${TOPIC_TYPE_INFORMATION}" style="padding-left: 18px; background: url('images/topic_type_information.png'); background-repeat: no-repeat;"
									<#if form.getAttribute('topicType')=TOPIC_TYPE_INFORMATION>selected="selected"</#if>>${language.getMessage('msg.topic.type.information')}</option>
							</select>
						</td>
					</tr>
					</table>
				</@BLOCK_CONTENT>
				
				<@BLOCK_TITLE title=language.getMessage('msg.topic.title') />
				<@BLOCK_CONTENT>
					<table border="0" cellpadding="4" cellspacing="1" align="center" class="tblListView" width="100%">
					<tr>
						<td align="right" class="row1" width="40%">${language.getMessage('msg.topic.title')}:</td>
						<td class="row0" width="60%"><input type="text" name="topicTitle"
							value="<#if form.getAttribute('topicTitle')??>${form.getAttribute('topicTitle')?html}</#if>"
							title="${language.getMessage('msg.topic.title')}"
							style="width: 99%" maxlength="255"/></td>
					</tr>
					</table>
				</@BLOCK_CONTENT>
				
				<@BLOCK_TITLE title=language.getMessage('msg.topic.content') />
				<@BLOCK_CONTENT>
					<table border="0" cellpadding="4" cellspacing="1" align="center" class="tblListView" width="100%">
					<tr>
						<td align="right" class="row1" width="40%">${language.getMessage('msg.topic.content')}:</td>
						<td class="row0" width="60%">
							<@TXBB_EDITOR_TOOLBAR textareaName="document."+form.name+".topicContent">
								<textarea style="width: 99%;" rows="10" cols="60" name="topicContent"
									><#if form.getAttribute('topicContent')??>${form.getAttribute('topicContent')?html}</#if></textarea>
							</@TXBB_EDITOR_TOOLBAR>
						</td>
					</tr>
					</table>
				</@BLOCK_CONTENT>

				<@BLOCK_TITLE title=language.getMessage('msg.topic.attachments') />
				<@BLOCK_CONTENT>
				</@BLOCK_CONTENT>
				
				<@BLOCK_HIGHLIGHTED>
					<input type="submit" style="width: 96px" value="${language.getMessage('msg.update')}"/>
					<input type="button" style="width: 96px" value="${language.getMessage('msg.done')}"
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