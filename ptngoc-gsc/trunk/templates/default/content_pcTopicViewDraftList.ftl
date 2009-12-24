<#assign tabs = [
	{"url":page.content.urlPcHome, "title":language.getMessage('msg.pcHome')},
	{"url":page.content.urlCreateTopic, "title":language.getMessage('msg.pcCreateTopic')},
	{"selected":true, "title":language.getMessage('msg.pcDraftTopics')+" ("+page.content.numDraftTopics+")"},
	{"url":page.content.urlPublishedTopics, "title":language.getMessage('msg.pcPublishedTopics')+" ("+page.content.numPublishedTopics+")"}
] />
<@MAKE_TABS tabs = tabs />
<@CONTENT_BLOCK colorScheme=THEME_HIGHLIGHTED_BLOCK>
	<table border="0" cellpadding="4" cellspacing="1" width="100%" class="tblListView">
	<thead>
		<tr>
			<th colspan="3">${language.getMessage('msg.actions')}</th>
			<th colspan="2">${language.getMessage('msg.topic.title')}</th>
			<th>${language.getMessage('msg.topic.creationTime')}</th>
			<th>${language.getMessage('msg.topic.lastupdateTime')}</th>			
		</tr>
	</thead>
	<tbody>
		<#list page.content.draftTopics as topic>
			<tr class="row${topic_index%2}">
				<td width="50" align="center"><a href="${topic.urlEdit}">${language.getMessage('msg.edit')}</a></td>
				<td width="50" align="center"><a href="${topic.urlPublish}">${language.getMessage('msg.publish')}</a></td>
				<td width="50" align="center"><a href="${topic.urlDelete}">${language.getMessage('msg.delete')}</a></td>
				<td width="48" align="center">
					<#if topic.isLocked()>
						<img border="0" alt="Locked" title="Locked" src="images/action_lock.png"/>
					</#if>
					<img border="0" alt="${topic.type?html}" title="${topic.type?html}"
						src="images/topic_type_${topic.type?html}.png"/>
				</td>
				<td>
					<a href="${topic.urlEdit}"><#if topic.title??>${topic.title?html}</#if></a>
				</td>
				<td align="center" width="150">${topic.creationTime}</td>
				<td align="center" width="150">${topic.lastupdateTime}</td>
			</tr>
		</#list>
	</tbody>
	</table>
</@CONTENT_BLOCK>
