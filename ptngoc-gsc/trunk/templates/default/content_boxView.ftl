<#if !page.content.box??>
	<@CONTENT_BLOCK title=language.getMessage('msg.error')>
		${language.getMessage('msg.nodata')}
	</@CONTENT_BLOCK>
<#else>
	<#assign box = page.content.box />
	<@CONTENT_BLOCK title=box.title?html>
		<#assign tableView = page.content.topics />
		<#if tableView.rows?size gt 0>
			<#if tableView.hasPaging()>
				<@PAGINATION_TABLE table=tableView/>
			</#if>
			<#list tableView.rows as topic>
				<#if topic_index == 0>
					<div 
						<#if page.content.recentCommentedTopics??>
							<#assign _height=26+10+23*page.content.recentCommentedTopics?size/>
							style="height: ${_height}px"
						</#if>
					>
						<#if page.content.recentCommentedTopics??>
							<div class="inlineBox">
								<h1>${language.getMessage('msg.recentlyCommented')}</h1>
								<div class="content-box">
									<ul class="sidemenu">
										<#list page.content.recentCommentedTopics as commentedTopic>
											<li><a href="${topic.getUrlViewCommentsViaBox(box)}" title="${topic.title?html}">${commentedTopic.getTitleExcerpt(20)?html}</a></li>
										</#list>
									</ul>
								</div>
							</div>
						</#if>
						<h3><a href="${topic.getUrlViewViaBox(box)}" title="${topic.title?html}"
							style="padding-left: 20px; background: url(images/topic_type_${topic.type}.png) no-repeat left center;">${topic.getTitleExcerpt(64)?html}</a></h3>			
						<p>
							${topic.getContentExcerpt(512)?html}
						</p>
					</div>
					<div class="post-footer align-right">
						<a href="${topic.getUrlViewViaBox(box)}" class="readmore">${language.getMessage('msg.topic.readMore')}</a>
						<#if topic.numComments gt 0>
							<a href="${topic.getUrlViewCommentsViaBox(box)}" class="comments">${topic.numComments?c}
							<#if topic.numComments gt 1>${language.getMessage('msg.comments')}<#else>${language.getMessage('msg.comment')}</#if></a>
						</#if>
						<span class="date">${topic.lastupdateTime}</span>
					</div>
				<#else>
					<h3><a href="${topic.getUrlViewViaBox(box)}" title="${topic.title?html}"
						style="padding-left: 20px; background: url(images/topic_type_${topic.type}.png) no-repeat left center;">${topic.getTitleExcerpt(64)?html}</a></h3>
					<p>
						${topic.getContentExcerpt(256)?html}
					</p>
					<div class="post-footer align-right">
						<a href="${topic.getUrlViewViaBox(box)}" class="readmore">${language.getMessage('msg.topic.readMore')}</a>
						<#if topic.numComments gt 0>
							<a href="${topic.getUrlViewCommentsViaBox(box)}" class="comments">${topic.numComments?c}
							<#if topic.numComments gt 1>${language.getMessage('msg.comments')}<#else>${language.getMessage('msg.comment')}</#if></a>
						</#if>
						<span class="date">${topic.lastupdateTime}</span>
					</div>
				</#if>
			</#list>
			<#if tableView.hasPaging()>
				<@PAGINATION_TABLE table=tableView/>
			</#if>
		<#else>
			${language.getMessage('msg.nodata')}
		</#if>
	</@CONTENT_BLOCK>
</#if>
