<@CONTENT_BLOCK title="${language.getMessage('msg.information')}">
	<p align="center" class="informationMessage">
		<#list application.informationMessages as msg>
			${msg}<br/>
		</#list>
		<#if page.header.redirectUrl?? && page.header.redirectUrl!="">
			${language.getMessage('msg.transition', page.header.redirectUrl)}
		</#if>
	</p>
</@CONTENT_BLOCK>
