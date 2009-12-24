<#if page.content.box??>
	<@MAKE_TABS tabs = [
		{"url":page.content.urlAdminHome, "title":language.getMessage('msg.adminHome')},
		{"url":page.content.urlListBoxes, "title":language.getMessage('msg.boxManagement')},
		{"selected":true, "title":language.getMessage('msg.editBox') + ' | ' + page.content.box.title?html}
	]/>
	<@CONTENT_BLOCK colorScheme=THEME_HIGHLIGHTED_BLOCK>
		<#assign form=page.form>
		<form method="post" action="${form.action}" name="${form.name}">
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
				<@BLOCK_TITLE title=language.getMessage('msg.box.name') />
				<@BLOCK_CONTENT>
					<table border="0" cellpadding="4" cellspacing="1" align="center" class="tblListView" width="100%">
					<tr>
						<td align="right" class="row1" width="40%">${language.getMessage('msg.parent')}</td>
						<td class="row0" width="60%">
							<select name="parentId">
								<option value="0"></option>
								<#if form.getAttribute('parentId')??>
									<#assign parentId = form.getAttributeAsInt('parentId')>
								<#else>
									<#assign parentId = 0>
								</#if>
								<#list page.content.boxList as box>
									<option value="${box.id}" <#if box.id==parentId>selected="selected"</#if>>${box.title?html}</option>
								</#list>
							</select>
						</td>
					</tr>
					<tr>
						<td align="right" class="row1" width="40%">${language.getMessage('msg.title')} *</td>
						<td class="row0" width="60%"><input type="text" name="title"
							value="<#if form.getAttribute('title')??>${form.getAttribute('title')?html}</#if>"
							title="${language.getMessage('msg.title')}"
							style="width: 99%" maxlength="255"/></td>
					</tr>
					<tr>
						<td align="right" class="row1" width="40%">${language.getMessage('msg.box.outerDescription')}</td>
						<td class="row0" width="60%">
							<textarea style="width: 99%;" rows="10" cols="60" name="outerDescription"
								><#if form.getAttribute('outerDescription')??>${form.getAttribute('outerDescription')?html}</#if></textarea>
						</td>
					</tr>
					<tr>
						<td align="right" class="row1" width="40%">${language.getMessage('msg.box.innerDescription')}</td>
						<td class="row0" width="60%">
							<textarea style="width: 99%;" rows="10" cols="60" name="innerDescription"
								><#if form.getAttribute('innerDescription')??>${form.getAttribute('innerDescription')?html}</#if></textarea>
						</td>
					</tr>
					</table>
				</@BLOCK_CONTENT>
				
				<@BLOCK_TITLE title=language.getMessage('msg.box.permissions') />
				<@BLOCK_CONTENT>
					<script language="javascript" type="text/javascript">
					/* <![CDATA[ */
					function checkAllCols(formName, permission) {
						<#list page.content.userGroups as userGroup>
							<#if !userGroup.isGod()>
								eval('document.'+formName+'.group'+${userGroup.id?c}+'_'+permission+'.checked = true');
							</#if>
						</#list>
					}
					function uncheckAllCols(formName, permission) {
						<#list page.content.userGroups as userGroup>
							<#if !userGroup.isGod()>
								eval('document.'+formName+'.group'+${userGroup.id?c}+'_'+permission+'.checked = false');
							</#if>
						</#list>
					}
					function syncOff(formName, elMainToOff, elSubToOff) {
						var el1 = eval('document.'+formName+'.'+elMainToOff);
						var el2 = eval('document.'+formName+'.'+elSubToOff);
						if ( !el1.checked ) {
							el2.checked = false;
						}
					}
					function syncOn(formName, elMainToOn, elSubToOn) {
						var el1 = eval('document.'+formName+'.'+elMainToOn);
						var el2 = eval('document.'+formName+'.'+elSubToOn);
						if ( el1.checked ) {
							el2.checked = true;
						}
					}
					/* ]]> */
					</script>
					<table border="0" cellpadding="4" cellspacing="1" align="center" class="tblListView" width="100%">
					<thead>
						<tr>
							<td width="60%" align="right">${language.getMessage('msg.userGroup')}</td>
							<td width="20%" align="center">${language.getMessage('msg.box.permission.viewTopic')}</td>
							<td width="20%" align="center">${language.getMessage('msg.box.permission.publishTopic')}</td>
						</tr>
					</thead>
					<tfoot>
						<tr>
							<td width="60%">&nbsp;</td>
							<td width="20%" align="center">
								<input type="button" value="+" class="button small"
									onclick="checkAllCols(this.form.name, 'viewTopic');" />
								<input type="button" value="-" class="button small"
									onclick="uncheckAllCols(this.form.name, 'viewTopic'); uncheckAllCols(this.form.name, 'publishTopic');" />
							</td>
							<td width="20%" align="center">
								<input type="button" value="+" class="button small"
									onclick="checkAllCols(this.form.name, 'publishTopic'); checkAllCols(this.form.name, 'viewTopic');" />
								<input type="button" value="-" class="button small"
									onclick="uncheckAllCols(this.form.name, 'publishTopic');" />
							</td>
						</tr>
					</tfoot>
					<tbody>
						<#list page.content.userGroups as userGroup>
							<tr>
								<#assign _viewTopic = 'group'+userGroup.id+'_viewTopic' />
								<#assign _publishTopic = 'group'+userGroup.id+'_publishTopic' />
								<td width="60%" align="right" class="row0">${userGroup.name}</td>
								<td width="20%" align="center" class="row0">
									<input type="checkbox" value="1" name="${_viewTopic}"
										onclick="syncOff(this.form.name, '${_viewTopic}', '${_publishTopic}');"
										<#if userGroup.isGod()>disabled="disabled"</#if>
										<#if userGroup.isGod() || form.getAttributeAsBoolean(_viewTopic)>checked="checked"</#if> /></td>
								<td width="20%" align="center" class="row0">
									<input type="checkbox" value="1" name="${_publishTopic}"
										onclick="syncOn(this.form.name, '${_publishTopic}', '${_viewTopic}');"
										<#if userGroup.isGod()>disabled="disabled"</#if>
										<#if userGroup.isGod() || form.getAttributeAsBoolean(_publishTopic)>checked="checked"</#if> /></td>
							</tr>
						</#list>
					</tbody>
					</table>
				</@BLOCK_CONTENT>
				
				<@BLOCK_HIGHLIGHTED>
					<input type="submit" style="width: 96px" value="${language.getMessage('msg.ok')}"/>
					<input type="button" style="width: 96px" value="${language.getMessage('msg.cancel')}"
						onclick="javascript: location.href='${form.cancelAction}';"/>
				</@BLOCK_HIGHLIGHTED>
			</@CONTENT_BLOCK_FREESTYLE>	
		</form>
	</@CONTENT_BLOCK>
</#if>