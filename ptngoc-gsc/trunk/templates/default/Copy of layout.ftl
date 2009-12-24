<#setting locale="en_US"/><#include "constants.ftl"><#include "library.ftl"><#include "txbbeditor.ftl">
<!DOCTYPE html
	PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
-->
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="application/xhtml+xml; charset=${page.header.charset}"/>
	<meta name="Keywords" content="${page.header.keywords?html}"/>
	<meta name="Description" content="${page.header.description?html}"/>
	<meta name="Robots" content="index, noimageindex, follow"/>
	<meta name="Revisit-After" content="2 days"/>
	<meta name="Rating" content="General"/>
	<#if page.header.redirectUrl?? && page.header.redirectUrl!="">
		<meta http-equiv="refresh" content="3; url=${page.header.redirectUrl}"/>
	</#if>
	<title>${page.header.title?html}</title>
	<script language="JavaScript" type="text/javascript">
	/* <![CDATA[ */
	var timing = new Date();
	_startTime = timing.getTime();
	/* ]]> */
	</script>
	<link rel="stylesheet" href="css/Refresh.css" type="text/css" />
	<link rel="stylesheet" href="css/txbb.css" type="text/css" />
</head>

<body>
<!-- wrap starts here -->
<div id="wrap">
	<!--header -->
	<div id="header">			     
		<h1 id="logo-text">${page.header.title?html}</h1>		
		<h2 id="slogan">${page.header.description?html}</h2>
		<!--
		<form class="search" method="post" action="#">
			<p>
				<input class="textbox" type="text" name="search_query" value="" />
				<input class="button" type="submit" name="Submit" value="Search" />
			</p>
		</form>
		-->				
	</div>
	
	<!-- menu -->	
	<div id="menu">
		<ul>
			<li id="current"><a href="index.html">Home</a></li>
			<li><a href="index.html">Archives</a></li>
			<li><a href="index.html">Downloads</a></li>
			<li><a href="index.html">Services</a></li>
			<li><a href="index.html">Support</a></li>
			<li><a href="index.html">About</a></li>		
		</ul>
	</div>
	
	<#if page.content.topContent??
			&& page.content.topContent?is_sequence
			&& page.content.topContent?size gt 0>
		<div id="content-wrap">
			<@RENDER_PORTLET_LIST portlets=page.content.topContent/>
		</div>
	</#if>

	<!-- content-wrap starts here -->
	<div id="content-wrap">
		<#if page.content.leftColumn?? 
				&& page.content.leftColumn?is_sequence
				&& page.content.leftColumn?size gt 0>
			<#include "column_left.ftl">
		</#if>

		<div id="main">
			<#if MAIN_CONTENT_FILE??>
				<#include MAIN_CONTENT_FILE>
			<#else>
				<#include "column_center.ftl">
			</#if>			
		</div>
	</div>

	<div id="footer">
		<#if page.content.bottomContent??
				&& page.content.bottomContent?is_sequence
				&& page.content.bottomContent?size gt 0>
			<@RENDER_PORTLET_LIST portlets=page.content.bottomContent/>
		</#if>
		<!--
			<p>
			&copy; 2006 <strong>Your Company</strong> | 
			Design by: <a href="http://www.styleshout.com/">styleshout</a> | 
			Valid <a href="http://validator.w3.org/check?uri=referer">XHTML</a> | 
			<a href="http://jigsaw.w3.org/css-validator/check/referer">CSS</a>
			
   		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			
			<a href="index.html">Home</a>&nbsp;|&nbsp;
   		<a href="index.html">Sitemap</a>&nbsp;|&nbsp;
	   	<a href="index.html">RSS Feed</a>
   		</p>
		-->	
	</div>	
</div>
</body>
</html>

<#--
<!-- body -->
<table border="0" width="${PAGE_WIDTH}" cellspacing="0" cellpadding="0" align="center">
<tr>
	<#if false && page.content.leftColumn?? 
			&& page.content.leftColumn?is_sequence
			&& page.content.leftColumn?size gt 0>
		<#include "column_left.ftl">
	</#if>	
	<td width="100%" valign="top">
		<#if !(DISABLE_EIW_BLOCKS?? && DISABLE_EIW_BLOCKS)>
			<#if application.hasErrorMessage()>
				<table border="0" cellpadding="0" cellspacing="0" width="40%" align="center">
				<tr>
					<td>
						<@CONTENT_BLOCK colorScheme=THEME_ERROR title=language.getMessage('msg.error')>
							<table border="0" cellpadding="4" cellspacing="0" align="center">
							<tr>
								<td valign="top"><img border="0" alt="Error" src="images/dialog-error.png" /></td>
								<td valign="top">
									<#list application.errorMessages as msg>
										<span class="errorMessage">${msg?replace("\n","<br/>")}</span><br/>
									</#list>
									<br/>
									<small><a href="javascript:history.go(-1);">${language.getMessage('msg.goBack')}</a></small>
								</td>
							</tr>
							</table>
						</@CONTENT_BLOCK>
					</td>
				</tr>
				</table>
				<div class="separator"></div>
			</#if>
			<#if application.hasWarningMessage()>
				<table border="0" cellpadding="0" cellspacing="0" width="40%" align="center">
				<tr>
					<td>
						<@CONTENT_BLOCK colorScheme=THEME_WARNING title=language.getMessage('msg.warning')>
							<table border="0" cellpadding="4" cellspacing="0" align="center">
							<tr>
								<td valign="top"><img border="0" alt="Warning" src="images/dialog-warning.png" /></td>
								<td valign="top">
									<#list application.warningMessages as msg>
										<span class="warningMessage">${msg?replace("\n","<br/>")}</span><br/>
									</#list>
								</td>
							</tr>
							</table>
						</@CONTENT_BLOCK>
					</td>
				</tr>
				</table>
				<div class="separator"></div>
			</#if>
			<#if application.hasInformationMessage()>
				<table border="0" cellpadding="0" cellspacing="0" width="40%" align="center">
				<tr>
					<td>
						<@CONTENT_BLOCK colorScheme=THEME_INFORMATION title=language.getMessage('msg.information')>
							<table border="0" cellpadding="4" cellspacing="0" align="center">
							<tr>
								<td valign="top"><img border="0" alt="Information" src="images/dialog-information.png" /></td>
								<td valign="top">
									<#list application.informationMessages as msg>
										<span class="infoMessage">${msg?replace("\n","<br/>")}</span><br/>
									</#list>
								</td>
							</tr>
							</table>
						</@CONTENT_BLOCK>
					</td>
				</tr>
				</table>
				<div class="separator"></div>
			</#if>
		</#if>
		<#if MAIN_CONTENT_FILE??>
			<#include MAIN_CONTENT_FILE>
		<#else>
			<#include "column_center.ftl">
		</#if>		
	</td>
	<#if false && page.content.rightColumn?? 
			&& page.content.rightColumn?is_sequence
			&& page.content.rightColumn?size gt 0>
		<#include "column_right.ftl">
	</#if>
</tr>
</table>
<!-- /body -->
<#include "footer.ftl">
-->