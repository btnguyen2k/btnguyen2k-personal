<#--
<#assign tabs = [{"selected":true, "title":language.getMessage('msg.home')}] />
<#list page.content.boxTree as box>
	<#if box.canView()>
		<#assign tabs = tabs+[{"title":box.title?html, "url":box.urlView}] />
	</#if>
</#list>
<@MAKE_TABS tabs = tabs />
-->
<#list page.content.boxTree as box>
	<#if box.canView()>
		<@CONTENT_BLOCK title=box.title?html>
			<#if box.lastTopic??>
				<#assign topic=box.lastTopic/>
				<h3><a href="${topic.getUrlViewViaBox(box)}" title="${topic.title?html}"
					style="padding-left: 20px; background: url(images/topic_type_${topic.type}.png) no-repeat left center;">${topic.getTitleExcerpt(64)?html}</a></h3>
				<p>
					${topic.contentExcerpt?html}
				</p>
				<div class="post-footer align-right">
					<a href="${topic.getUrlViewViaBox(box)}" class="readmore">Read more</a>
					<span class="date">Oct 01, 2006</span>
					<hr style="border-top: 1px solid #f2f2f2;" />
					<b><a href="${box.urlView}">${box.title?html}</a></b>:
					<#list box.children as child>
						<#if child.canView()>
							<a href="${child.urlView}">${child.title?html}</a> |
						</#if>
					</#list>
				</div>
				<p class="content-box" style="float: right; vertical-align: top;">
					hehehe
				</p>
			</#if>
		</@CONTENT_BLOCK>
	</#if>
</#list>

<#--
	<table border="0" cellpadding="4" cellspacing="1" width="100%">
	<tr>
		<td valign="top">
			<#list page.content.boxTree as box>
				<#if box.canView()>
					<@CONTENT_BLOCK colorScheme=THEME_NORMAL_BLOCK title=box.title?html>
						<#if box.lastTopic??>
							<#assign topic=box.lastTopic/>
							<img border="0" alt="" src="images/topic_type_${topic.type}.png"/>
							<span class="topic_title"><a href="${topic.getUrlViewViaBox(box)}" title="${topic.title?html}">${topic.getTitleExcerpt(64)?html}</a></span>
							<div class="separator"></div>
							${topic.contentExcerpt?html}
						</#if>
						<div class="separator"></div>
						<@HORIZONTAL_LINE/>
						<span class="block_footer">
							<b><a href="${box.urlView}">${box.title?html}</a></b>:
							<#list box.children as child>
								<#if child.canView()>
									<a href="${child.urlView}">${child.title?html}</a> |
								</#if>
							</#list>
						</span>
						<table width="200px" align="right" border="1" cellpadding="0" cellspacing="0">
						<tr><td>
							<@CONTENT_BLOCK colorScheme=THEME_HIGHLIGHTED_BLOCK title=language.getMessage('msg.recentlyPublished')>
								<#if page.content.recentPublishedTopics?? && page.content.recentPublishedTopics?size gt 0>
									<table border="0" cellpadding="2" cellspacing="4">
									<#list page.content.recentPublishedTopics as topic>
									<tr>
										<td valign="top"><img border="0" alt="" src="images/topic_type_${topic.type}.png"/></td>
										<td valign="top"><b><a href="${topic.urlView}" title="${topic.title?html}">${topic.getTitleExcerpt(20)?html}</a></b></td>
									</tr>
									</#list>
									</table>
								<#else>
									<i>${language.getMessage('msg.nodata')}</i>
								</#if>
							</@CONTENT_BLOCK>
						</td></tr>
						</table>
					</@CONTENT_BLOCK>
					<#if box_has_next>
						<div class="separator"></div>
					</#if>
				</#if>
			</#list>
		</td>
		<td width="200" valign="top">
			<@CONTENT_BLOCK colorScheme=THEME_HIGHLIGHTED_BLOCK title=language.getMessage('msg.recentlyPublished')>
				<#if page.content.recentPublishedTopics?? && page.content.recentPublishedTopics?size gt 0>
					<table border="0" cellpadding="2" cellspacing="4">
					<#list page.content.recentPublishedTopics as topic>
					<tr>
						<td valign="top"><img border="0" alt="" src="images/topic_type_${topic.type}.png"/></td>
						<td valign="top"><b><a href="${topic.urlView}" title="${topic.title?html}">${topic.getTitleExcerpt(20)?html}</a></b></td>
					</tr>
					</#list>
					</table>
				<#else>
					<i>${language.getMessage('msg.nodata')}</i>
				</#if>
			</@CONTENT_BLOCK>
			<div class="separator"></div>
			<@CONTENT_BLOCK colorScheme=THEME_NORMAL_BLOCK title=language.getMessage('msg.recentlyCommented')>
				<#if page.content.recentCommentedTopics?? && page.content.recentCommentedTopics?size gt 0>
					<table border="0" cellpadding="2" cellspacing="4">
					<#list page.content.recentCommentedTopics as topic>
					<tr>
						<td valign="top"><img border="0" alt="" src="images/topic_type_${topic.type}.png"/></td>
						<td valign="top"><i><a href="${topic.urlView}" title="${topic.title?html}">${topic.getTitleExcerpt(20)?html}</i></b></td>
					</tr>
					</#list>
					</table>
				<#else>
					<i>${language.getMessage('msg.nodata')}</i>
				</#if>
			</@CONTENT_BLOCK>
		</td>
	</tr>
	</table>
-->