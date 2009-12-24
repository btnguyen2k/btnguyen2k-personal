<#assign _counter = 0/>
<#list page.content.boxTree as box>
	<@CONTENT_BLOCK title=box.title?html url=box.urlView>
		<#if box.lastTopic??>
			<#assign topic=box.lastTopic />
			<#if _counter == 0>
				<div 
					<#if page.content.recentPublishedTopics??>
						<#assign _height=26+10+23*page.content.recentPublishedTopics?size/>
						style="height: ${_height}px"
					</#if>
				>
					<#if page.content.recentPublishedTopics??>
						<div class="inlineBox">
							<h1>${language.getMessage('msg.recentlyPublished')}</h1>
							<div class="content-box">
								<ul class="sidemenu">
									<#list page.content.recentPublishedTopics as publishedTopic>
										<li><a href="${topic.urlView}" title="${topic.title?html}">${publishedTopic.getTitleExcerpt(20)?html}</a></li>
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
					<#if box.recentPublishedTopics?? && box.recentPublishedTopics?size gt 0>
						<h3>${language.getMessage('msg.recentlyPublished')}</h3>
						<ul>
							<#list box.recentPublishedTopics as topic>
								<#if topic_index != 0>
									<#-- ignore the first item as it has already been displayed -->
									<li><a href="${topic.getUrlViewViaBox(box)}">${topic.title?html}</a></li>
								</#if>
							</#list>
						</ul>
					</#if>
				</div>
				<div class="post-footer align-right">
					<a href="${topic.getUrlViewViaBox(box)}" class="readmore">${language.getMessage('msg.topic.readMore')}</a>
					<#if topic.numComments gt 0>
						<a href="${topic.getUrlViewCommentsViaBox(box)}" class="comments">${topic.numComments?c}
						<#if topic.numComments gt 1>${language.getMessage('msg.comments')}<#else>${language.getMessage('msg.comment')}</#if></a>
					</#if>
					<span class="date">${topic.lastupdateTime}</span>
					<hr style="border-top: 1px solid #f2f2f2;" />
					<b><a href="${box.urlView}">${box.title?html}</a></b>:
					<#list box.children as child>
						<#if child.canView()>
							<a href="${child.urlView}">${child.title?html}</a> |
						</#if>
					</#list>
				</div>
			<#else>
				<h3><a href="${topic.getUrlViewViaBox(box)}" title="${topic.title?html}"
					style="padding-left: 20px; background: url(images/topic_type_${topic.type}.png) no-repeat left center;">${topic.getTitleExcerpt(64)?html}</a></h3>
				<p>
					${topic.getContentExcerpt(256)?html}
				</p>
				<#if box.recentPublishedTopics?? && box.recentPublishedTopics?size gt 0>
					<h3>${language.getMessage('msg.recentlyPublished')}</h3>
					<ul>
						<#list box.recentPublishedTopics as topic>
							<#if topic_index != 0>
								<#-- ignore the first item as it has already been displayed -->
								<li><a href="${topic.getUrlViewViaBox(box)}">${topic.title?html}</a></li>
							</#if>
						</#list>
					</ul>
				</#if>				
				<div class="post-footer align-right">
					<a href="${topic.getUrlViewViaBox(box)}" class="readmore">${language.getMessage('msg.topic.readMore')}</a>
					<#if topic.numComments gt 0>
						<a href="${topic.getUrlViewCommentsViaBox(box)}" class="comments">${topic.numComments?c}
						<#if topic.numComments gt 1>${language.getMessage('msg.comments')}<#else>${language.getMessage('msg.comment')}</#if></a>
					</#if>
					<span class="date">${topic.lastupdateTime}</span>
					<hr style="border-top: 1px solid #f2f2f2;" />
					<b><a href="${box.urlView}">${box.title?html}</a></b>:
					<#list box.children as child>
						<#if child.canView()>
							<a href="${child.urlView}">${child.title?html}</a> |
						</#if>
					</#list>
				</div>
			</#if>
			<#assign _counter = _counter+1 />
		</#if>
	</@CONTENT_BLOCK>
</#list>
