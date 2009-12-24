<table border="0" width="${PAGE_WIDTH}" cellspacing="0" cellpadding="0" align="center">
<tr><td>
	<div class="separator" />
		<p align="right">
			<script language="JavaScript" type="text/javascript">
			/* <![CDATA[ */
			var timing=new Date();
			_endTime=timing.getTime();
			document.write("Page load <b>"+(_endTime-_startTime)/1000+"s</b> - ");
			/* ]]> */
			</script>
			<a onclick="scrollTo(0,0);" href="javascript:;">Go top</a> ||
			<a onclick="print();" href="javascript:;">Print page</a> ||
			<a onclick="location.reload();" href="javascript:;">Reload</a> ||
		</p>
	<div class="separator" />
	<@HORIZONTAL_LINE />
	<#if page.content.bottomContent??
			&& page.content.bottomContent?is_sequence
			&& page.content.bottomContent?size gt 0>
		<@RENDER_PORTLET_LIST portlets=page.content.bottomContent/>		
	</#if>
	<center>
		Template based on <a href="http://tango.freedesktop.org/Tango_Icon_Library" target="_blank">Tango</a>
		and <a href="http://www.famfamfam.com/lab/icons/silk/" target="_blank">Silk Icons</a>.
		<br/>
		<a href="http://validator.w3.org/check?uri=referer" target="_blank">
			<img src="images/valid-xhtml10-blue.gif" border="0"
				alt="Valid XHTML 1.0 Transitional" height="31" width="88" />
		</a>
		<a href="http://jigsaw.w3.org/css-validator/" target="_blank">
    		<img style="border:0;width:88px;height:31px"
        		src="images/vcss-blue.gif"
        		alt="Valid CSS!" />
		</a>
		<a href="http://www.freemarker.org" target="_blank">
			<img src="images/freemarker_poweredby_sq_simple.png"
				border="0" alt="Powered by FreeMarker" />
		</a>
		<a href="http://www.java.com?cid=2436" target="_blank">
			<img border="0" width="100" height="45" alt="Java Get Powered"
			src="http://java.com/im/get_powered_sm.jpg" />
		</a>
	</center>
</td></tr></table>
	<#if _SCAH??>
		<script type="text/javascript" language="javascript" src="scah/scripts/shCoreTxbb.js"></script>
		<script type="text/javascript" language="javascript" src="scah/shCoreTxbb.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushBash.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushCpp.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushCSharp.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushCss.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushDelphi.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushJava.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushJScript.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushPerl.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushPhp.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushPython.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushRuby.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushSql.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushVb.js"></script>
		<script type="text/javascript" language="javascript" src="scah/scripts/shBrushXml.js"></script>
	</#if>
</body>
</html>