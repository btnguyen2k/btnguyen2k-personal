<#assign tabs = [
	{"url":page.content.urlPcHome, "title":language.getMessage('msg.pcHome')},
	{"url":page.content.urlCreateTopic, "title":language.getMessage('msg.pcCreateTopic')},
	{"url":page.content.urlDraftTopics, "title":language.getMessage('msg.pcDraftTopics')+" ("+page.content.numDraftTopics+")"},
	{"selected":true, "title":language.getMessage('msg.pcPublishedTopics')+" ("+page.content.numPublishedTopics+")"}
] />
<@MAKE_TABS tabs = tabs />
<@CONTENT_BLOCK colorScheme=THEME_HIGHLIGHTED_BLOCK>
	<#assign tableView = page.content.publishedTopics />
	<#if tableView.hasPaging()>
		<@CONTENT_BLOCK_FREESTYLE colorScheme=THEME_NORMAL_BLOCK>
			<@BLOCK_CONTENT>
				<@PAGINATION_TABLE table=tableView/>
			</@BLOCK_CONTENT>
		</@CONTENT_BLOCK_FREESTYLE>
	</#if>
	<table border="0" cellpadding="4" cellspacing="1" width="100%" class="tblListView">
	<thead>
		<tr>
			<th colspan="2">${language.getMessage('msg.actions')}</th>
			<th colspan="2">${language.getMessage('msg.topic.title')}</th>
			<th>${language.getMessage('msg.topic.creationTime')}</th>
			<th>${language.getMessage('msg.topic.lastupdateTime')}</th>			
		</tr>
	</thead>
	<tbody>
		<#list tableView.rows as topic>
			<tr class="row${topic_index%2}">
				<td width="50" align="center"><a href="${topic.urlUnpublish}">${language.getMessage('msg.unpublish')}</a></td>
				<td width="50" align="center">
					<#if topic.isLocked()><a href="${topic.urlUnlock}">${language.getMessage('msg.unlock')}</a>
					<#else><a href="${topic.urlLock}">${language.getMessage('msg.lock')}</a></#if>
				</td>
				<td width="48" align="center">
					<#if topic.isLocked()>
						<img border="0" alt="Locked" title="Locked" src="images/action_lock.png"/>
					</#if>
					<img border="0" alt="${topic.type?html}" title="${topic.type?html}"
						src="images/topic_type_${topic.type?html}.png"/>
				</td>
				<td>
					<#if topic.title??>${topic.title?html}</#if>
				</td>
				<td align="center" width="150">${topic.creationTime}</td>
				<td align="center" width="150">${topic.lastupdateTime}</td>
			</tr>
		</#list>
	</tbody>
	</table>
	<#if tableView.hasPaging()>
		<@CONTENT_BLOCK_FREESTYLE colorScheme=THEME_NORMAL_BLOCK>
			<@BLOCK_CONTENT>
				<@PAGINATION_TABLE table=tableView/>
			</@BLOCK_CONTENT>
		</@CONTENT_BLOCK_FREESTYLE>
	</#if>
</@CONTENT_BLOCK>
