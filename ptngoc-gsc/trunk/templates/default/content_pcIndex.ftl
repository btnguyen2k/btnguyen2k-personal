<#assign tabs = [
	{"selected":true, "title":language.getMessage('msg.pcHome')},
	{"url":page.content.urlCreateTopic, "title":language.getMessage('msg.pcCreateTopic')},
	{"url":page.content.urlDraftTopics, "title":language.getMessage('msg.pcDraftTopics')+" ("+page.content.numDraftTopics+")"},
	{"url":page.content.urlPublishedTopics, "title":language.getMessage('msg.pcPublishedTopics')+" ("+page.content.numPublishedTopics+")"}
] />
<@MAKE_TABS tabs = tabs />
<@CONTENT_BLOCK colorScheme=THEME_HIGHLIGHTED_BLOCK>
	
	<#--
	<table border="0" cellpadding="4" cellspacing="1" width="100%">
	<tr>
		<td colspan="2">
			${language.getMessage('msg.adminHome.intro')}
		</td>
	</tr>
	<tr>
		<td width="50%" valign="top">
			<table border="0" cellpadding="2" cellspacing="0" width="100%">
			<tr>
				<td style="font-weight: bold;" colspan="2">
					${language.getMessage('msg.overview')}
					<@HORIZONTAL_LINE/>
				</td>
			</tr>
			<tr>
				<td>${language.getMessage('msg.numBoxes')}</td>
				<td align="right">${page.content.numBoxes}</td>
			</tr>
			<tr>
				<td>${language.getMessage('msg.numTopics')}</td>
				<td align="right">${page.content.numTopics}</td>
			</tr>
			<tr>
				<td>${language.getMessage('msg.numPosts')}</td>
				<td align="right">${page.content.numPosts}</td>
			</tr>
			</table>
		</td>
		<td width="50%" valign="top">
			<table border="0" cellpadding="2" cellspacing="0" width="100%">
			<tr>
				<td style="font-weight: bold;" colspan="2">
					${language.getMessage('msg.memberStats')}
					<@HORIZONTAL_LINE/>
				</td>
			</tr>
			<tr>
				<td>${language.getMessage('msg.numMembers')}</td>
				<td align="right">${page.content.numMembers}</td>
			</tr>
			<tr>
				<td>${language.getMessage('msg.numRegAccounts')}</td>
				<td align="right">${page.content.numRegAccounts}</td>
			</tr>
			<tr>
				<td>${language.getMessage('msg.numOnlines')}</td>
				<td align="right">${page.content.numOnlines}</td>
			</tr>
			</table>
		</td>
	</tr>
	</table>
	-->
</@CONTENT_BLOCK>
