<@CONTENT_BLOCK title="${language.getMessage('msg.staffLogin')}">
	<#assign _form=page.form/>
	<form name="${_form.name?html}" action="${_form.action}" method="post">
		<#if _form.hasErrorMessage()>
			<p class="errorMessage">
				<#list _form.errorMessages as msg>
					${msg}<br />
				</#list>
			</p>
		</#if>
		<p>
			<label>${language.getMessage('msg.loginName')}</label>
			<input name="loginname" type="text" size="32" />
			<label>${language.getMessage('msg.password')}</label>
			<input name="password" type="password" size="32" />
			<br />
			<br />
			<input class="button" type="submit" value="${language.getMessage('msg.login')}"/>
		</p>
	</form>
</@CONTENT_BLOCK>
