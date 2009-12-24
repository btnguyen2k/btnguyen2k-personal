<#if page.content.topic??>
	<#assign _topic = page.content.topic />
	<#assign tabs = [
		{"url":page.content.urlPcHome, "title":language.getMessage('msg.pcHome')},
		{"selected":true, "title":language.getMessage('msg.pcPublishTopic')},
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
										<span class="infoMessage">${language.getMessage('msg.pcPublishTopic.confirmation')}</span>
									</td>
								</tr>
								</table>
							</@CONTENT_BLOCK>
						</td>
					</tr>
					</table>
				</@BLOCK_CONTENT>
			
				<@BLOCK_TITLE title=language.getMessage('msg.pcPublishToBox') />
				<@BLOCK_CONTENT>
					<table border="0" cellpadding="4" cellspacing="1" width="100%" class="tblListView">
					<tbody>
						<#assign _box_index = 0>
						<#macro _LIST_BOX box isFirst isLast indent=0>
							<tr>
								<td class="row${_box_index % 2}" width="32" align="center">
									<input type="radio" name="publishedBoxes" value="${box.id?c}"/>
								</td>
								<td class="row${_box_index % 2}">
									<#if indent gt 0>
										<#list 1..indent as i>
											&nbsp;&nbsp;&nbsp;
										</#list>
										<tt>+--</tt>
									</#if>
									<img src="images/small_box_hasnew.gif" alt="" border="0" /> ${box.title?html}
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
				</@BLOCK_CONTENT>
			
				<@BLOCK_HIGHLIGHTED>
					<input type="submit" style="width: 96px" value="${language.getMessage('msg.publish')}"/>
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