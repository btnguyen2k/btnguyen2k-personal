/**
 * Redirects to an URL.
 * @param url (string) the url to redirect to.
 */
function global_urlJumpPost(url) {
	var index = url.indexOf('#');
	if ( index >= 0 ) {
		var id = url.substr(index+1);
		var layer = global_getLayer("_"+id);
		if ( layer ) {
			location.hash = "#"+id
		} else {
			if ( url.indexOf("://") > 0 ) {
				location.href = url;
			} else {
				var forwardUrl = location.protocol+"//"+location.host;
				if ( url.charAt(0) == "/" ) {
					forwardUrl += url;
				} else {
					var temp = location.pathname.lastIndexOf('/');
					if ( temp < 0 ) {
						forwardUrl += '/'+url;
					} else {
						forwardUrl += location.pathname.substr(0, temp+1) + url;
					} 
				} 
				location.href = forwardUrl;
			} 
		} 
	} 
	return false;
} 

/**
 * Performs a check on login form.
 * @param loginname (string) the loginname to check.
 * @param password (string) the password to check.
 * @param errorEmptyLoginname (string) the error message to display when loginname is empty.
 * @param errorEmptyPassword (string) the error message to display when password is empty.
 * @param errorInvalidLoginname (string) the error message to display when loginname is invalid.
 * @return TRUE if the check has passed, FALSE if the check has failed.
 */
function global_checkLoginForm(loginname, password,
		errorEmptyLoginname, errorEmptyPassword, errorInvalidLoginname) {
	//check if loginname is not empty
	if ( !txbbForm_ValidateRequire(loginname) ) {
		alert(errorEmptyLoginname);
		return false;
	} 

	//check if loginname is an identifier
	if ( !txbbForm_ValidateIsIdentifer(loginname) ) {
		alert(errorInvalidLoginname);
		return false;
	} 

	//check if password is not empty
	if ( !txbbForm_ValidateRequire(password) ) {
		alert(errorEmptyPassword);
		return false;
	} 

	return true;
} 

/**
 * Finds and replace all sub strings in a string by a new string.
 * @param inputString the original string
 * @param fromString the sub string to be replaced
 * @param toString the new string to replace
 * @return the string after all replacements
 * Note: This function is taken from http://www.breakingpar.com/bkp/home.nsf/0/45A5696524D222C387256AFB0013D850
 */
function global_replaceSubstring(inputString, fromString, toString) {
	//Goes through the inputString and replaces every occurrence of fromString with toString
	var temp = inputString;
	if ( fromString == "" ) {
		return inputString;
	} 
	if ( toString.indexOf(fromString) == -1 ) {
		//if the string being replaced is not a part of the replacement string (normal situation)
		while ( temp.indexOf(fromString) != -1 ) {
			var toTheLeft = temp.substring(0, temp.indexOf(fromString));
			var toTheRight = temp.substring(temp.indexOf(fromString)+fromString.length, temp.length);
			temp = toTheLeft + toString + toTheRight;
		}
	} else {
		//string being replaced is part of replacement string (like "+" being replaced with "++") - prevent an infinite loop
		var midStrings = new Array("~", "`", "_", "^", "#");
		var midStringLen = 1;
		var midString = "";

		//find a string that doesn't exist in the inputString to be used
		//as an "inbetween" string
		while ( midString == "" ) {
			for ( var i=0; i < midStrings.length; i++ ) {
				var tempMidString = "";
				for ( var j=0; j < midStringLen; j++ ) { tempMidString += midStrings[i]; }
				if ( fromString.indexOf(tempMidString) == -1 ) {
					midString = tempMidString;
					i = midStrings.length + 1;
				} 
			} 
		} // Keep on going until we build an "inbetween" string that doesn't exist

		//now go through and do two replaces - first, replace the "fromString" with the "inbetween" string
		while ( temp.indexOf(fromString) != -1 ) {
			var toTheLeft = temp.substring(0, temp.indexOf(fromString));
			var toTheRight = temp.substring(temp.indexOf(fromString)+fromString.length, temp.length);
			temp = toTheLeft + midString + toTheRight;
		} 

		//next, replace the "inbetween" string with the "toString"
		while ( temp.indexOf(midString) != -1 ) {
			var toTheLeft = temp.substring(0, temp.indexOf(midString));
			var toTheRight = temp.substring(temp.indexOf(midString)+midString.length, temp.length);
			temp = toTheLeft + toString + toTheRight;
		} 
	} 
   return temp;
} 

/**
 * Returns a layer object by its name. Layer is a <div id="layerName"></div> block.
 * Note: requires browser_detect.js
 * @param layerName name of the layer to retrieve
 * @return the layer object
 */
function global_getLayer(layerName) {
	if ( is_nav6up ) {
		return document.getElementById(layerName);
	} else if ( is_nav4up ) {
		return eval('document.'+layerName+';');
	} else if ( is_ie5up ) {
		return eval('document.all.'+layerName+';');
	} else {
		return null;
	} 
} 

/**
 * Collapses a table row by its name. A table row is a <tr id="rowName"></tr> block.
 * @param rowName name of the row to collapse
 */
function global_collapseTableRow(rowName) {
	var layer = global_getLayer(rowName);
	if ( layer==null ) return;
	if ( is_ie5up || is_nav6up ) {
		layer.style.display = 'none';
	} 
} 

/**
 * Expands a collapsed a table row by its name. A table row is a <tr id="rowName"></tr> block.
 * @param rowName name of the collapsed row to expand
 */
function global_expandTableRow(rowName) {
	var layer = global_getLayer(rowName);
	if ( layer==null ) return;
	if ( is_ie5up || is_nav6up ) {
		layer.style.display = '';
	} 
} 

/**
 * Collapses a layer by its name. Layer is a <div id="layerName"></div> block.
 * @param layerName name of the layer to collapse
 */
function global_collapseLayer(layerName) {
	var layer = global_getLayer(layerName);
	if ( layer==null ) return;
	if ( is_ie5up || is_nav6up ) {
		layer.style.display = 'none';
	} 
} 

/**
 * Expands a collapsed a layer by its name. Layer is a <div id="layerName"></div> block.
 * @param layerName name of the collapsed layer to expand
 */
function global_expandLayer(layerName) {
	var layer = global_getLayer(layerName);
	if ( layer==null ) return;
	if ( is_ie5up || is_nav6up ) {
		layer.style.display = '';
	} 
} 

/**
 * Hides a layer by its name. Layer is a <div id="layerName"></div> block.
 * @param layerName name of the layer to hide
 */
function global_hideLayer(layerName) {
	var layer = global_getLayer(layerName);
	if ( layer==null ) return;
	if ( is_ie5up || is_nav6up ) {
		layer.style.visibility = 'hidden';
	} else if ( is_nav4up ) {
		layer.visibility = 'hide';
	} 
} 

/**
 * Shows a layer by its name. Layer is a <div id="layerName"></div> block.
 * @param layerName name of the layer to show
 */
function global_showLayer(layerName) {
	var layer = global_getLayer(layerName);
	if ( layer==null ) return;
	if ( is_ie5up || is_nav6up ) {
		layer.style.visibility = 'visible';
	} else if ( is_nav4up ) {
		layer.visibility = 'show';
	} 
} 