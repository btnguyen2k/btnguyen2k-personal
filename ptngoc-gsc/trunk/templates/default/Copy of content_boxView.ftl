<#if !page.content.box??>
	<h2>${language.getMessage('msg.nodata')}</h3>
<#else>
	HEHEHE
</#if>



<#if page.content.box??>
	<#assign tabs = [{"url":page.content.urlHome, "title":language.getMessage('msg.home')}] />
	<#list page.content.boxTree as box>
		<#if box.canView()>
			<#if box.id==page.content.box.id>
				<#assign tabs = tabs+[{"title":box.title?html, "selected":true}] />
			<#else>
				<#assign tabs = tabs+[{"title":box.title?html, "url":box.urlView}] />
			</#if>
		</#if>
	</#list>
<#else>
	<#assign tabs = [{"selected":true, "title":language.getMessage('msg.home')}] />
	<#list page.content.boxTree as box>
		<#if box.canView()>
			<#assign tabs = tabs+[{"title":box.title?html, "url":box.urlView}] />
		</#if>
	</#list>
</#if>
<@MAKE_TABS tabs = tabs />
<@CONTENT_BLOCK colorScheme=THEME_HIGHLIGHTED_BLOCK>
	<#if !page.content.box??>
		${language.getMessage('msg.nodata')}
	<#else>
	<table border="0" cellpadding="4" cellspacing="1" width="100%">
	<tr>
		<#include "column_left.ftl"/>
		<td valign="top">
			<#list [page.content.box] as box>
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
	</table
	</#if>
</@CONTENT_BLOCK>
