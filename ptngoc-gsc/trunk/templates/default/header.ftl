<#setting locale="en_US"/><?xml version="1.0" encoding="${page.header.charset}"?>
<!DOCTYPE html
	PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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

	<script language="javascript" type="text/javascript" src="js/browser_detect.js"></script>
	<link href="css/site.css" type="text/css" rel="stylesheet"/>
	<style type="text/css">
		<#include "css/color_scheme_txbb.css">
	</style>
	<link href="css/tooltip.css" type="text/css" rel="stylesheet"/>
	<!--
	<script language="javascript" type="text/javascript" src="js/jquery-1.2.6.min.js"></script>
	-->
	<script language="javascript" type="text/javascript" src="js/cssFixForIE6onXP.js"></script>
	
	<!--Include YUI Loader: -->
	<script language="javascript" type="text/javascript" src="yui-2.7.0/yuiloader/yuiloader-min.js"></script>
	
	<!--Use YUI Loader to bring in your other dependencies: -->
	<script language="javascript" type="text/javascript">
	/* <![CDATA[ */
	// Instantiate and configure YUI Loader: 
	(function() {
		var loader = new YAHOO.util.YUILoader({
			base: "yui-2.7.0/",
			require: ["button"],
			loadOptional: false,
			combine: false,
			filter: "MIN",
			allowRollup: false,
			onSuccess: function() {
				//you can make use of all requested YUI modules 
				//here.
			},
			onFailure: function() {
				alert('Error while loading YUI!');
			}
		});
		// Load the files using the insert() method. 
		loader.insert();   
	})();
	/* ]]> */
	</script>  
	<link type="text/css" rel="stylesheet" href="scah/styles/shCore.css"/>
	<link type="text/css" rel="stylesheet" href="scah/styles/shThemeDefault.css"/>
</head>
<body>
<div id="tooltipBody"></div><img id="tooltipPointer" src="images/tooltip_arrow.gif" alt="Tooltip Arrow"/>
<script language="javascript" type="text/javascript" src="js/tooltip.js"></script>
<script language="javascript" type="text/javascript">
/* <![CDATA[ */
/*
function styleForms() {
	//styleButtons();
	//styleTextboxes();
}

function styleTextboxes() {
	$("input").filter(function() {
		return (this.type=='text' || this.type=='password') && !this.readOnly && !this.disabled;
	})
	.addClass('textbox')
	.focus(function() {
		this.className = 'textbox focused';
	})
	.blur(function() {
		this.className = 'textbox';
	});
	
	$("input").filter(function() {
		return (this.type=='text' || this.type=='password') && (this.readOnly || this.disabled);
	})
	.addClass('textbox').addClass('disabled');
	
	$("textarea").filter(function() {
		return !this.readOnly && !this.disabled;
	})
	.addClass('textbox')
	.focus(function() {
		this.className = 'textbox focused';
	})
	.blur(function() {
		this.className = 'textbox';
	});
	
	$("textarea").filter(function() {
		return this.readOnly || this.disabled;
	})
	.addClass('textbox').addClass('disabled');
}

function styleButtons() {
	$("input").filter(function() {
		return this.type=='button' || this.type=='reset';
	})
	.addClass('button')
	.focus(function() {
		this.className = 'button focused';
	})
	.blur(function() {
		this.className = 'button';
	});
	
	$("input").filter(function() {
		return this.type=='submit';
	})
	.addClass('button').addClass('submit')
	.focus(function() {
		this.className = 'button submit focused';
	})
	.blur(function() {
		this.className = 'button submit';
	});
}

$(document).ready(function() {
	styleForms();
});
*/
/* ]]> */
</script>
<table border="0" width="${PAGE_WIDTH}" cellspacing="0" cellpadding="0" align="center">
<tr><td>
<!-- Header -->
<#if page.content.topContent??
		&& page.content.topContent?is_sequence
		&& page.content.topContent?size gt 0>
	<@RENDER_PORTLET_LIST portlets=page.content.topContent/>
</#if>
<!-- /Header -->
</td></tr></table>
<div class="separator"/>