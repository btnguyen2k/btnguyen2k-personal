<#macro TXBB_EDITOR_TOOLBAR textareaName>
	<#assign _OPEN_TAG="{" />
	<#assign _CLOSE_TAG="}" />
	<img style="cursor: pointer; padding: 1px" onclick="_makeBold('${textareaName}');"
		alt="Bold" title="Bold" border="1" src="images/txbbeditor/text_bold.png" />
	<img style="cursor: pointer; padding: 1px" onclick="_makeItalic('${textareaName}');" 
		alt="Italic" title="Italic" border="1" src="images/txbbeditor/text_italic.png" />
	<img style="cursor: pointer; padding: 1px" onclick="_makeUnderline('${textareaName}');" 
		alt="Underline" title="Underline" border="1" src="images/txbbeditor/text_underline.png" />
	<img style="cursor: pointer; padding: 1px" onclick="_makeStrikeThrough('${textareaName}');" 
		alt="Strikethrough" title="Strikethrough" border="1" src="images/txbbeditor/text_strikethrough.png" />
	<img style="cursor: pointer; padding: 1px" onclick="_makeSuperscript('${textareaName}');" 
		alt="Superscript" title="Superscript" border="1" src="images/txbbeditor/text_superscript.png" />
	<img style="cursor: pointer; padding: 1px" onclick="_makeSubscript('${textareaName}');" 
		alt="Subscript" title="Subscript" border="1" src="images/txbbeditor/text_subscript.png" />
	&nbsp;
	<img style="cursor: pointer; padding: 1px" onclick="_makeQuote('${textareaName}');" 
		alt="Quote" title="Quote" border="1" src="images/txbbeditor/text_quote.png" />
	<img style="cursor: pointer; padding: 1px" onclick="_makeListBullets('${textareaName}');" 
		alt="List" title="List" border="1" src="images/txbbeditor/text_list_bullets.png" />
	<img style="cursor: pointer; padding: 1px" onclick="_makeListNumbers('${textareaName}');" 
		alt="List" title="List" border="1" src="images/txbbeditor/text_list_numbers.png" />
	&nbsp;
	<img style="cursor: pointer; padding: 1px" onclick="_makeLink('${textareaName}');" 
		alt="Link" title="Link" border="1" src="images/txbbeditor/link_add.png" />
	<img style="cursor: pointer; padding: 1px" onclick="_makeEmail('${textareaName}');" 
		alt="Email" title="Email" border="1" src="images/txbbeditor/email_add.png" />
	<img style="cursor: pointer; padding: 1px" onclick="_makeImg('${textareaName}');" 
		alt="Image" title="Image" border="1" src="images/txbbeditor/image_add.png" />
	<br />
	<select name="_txbbColor" onchange="if (this.options[this.selectedIndex].value != '') {_makeColor('${textareaName}', this.options[this.selectedIndex].value); this.selectedIndex=0;}">
		<option value=''>-- color --</option>
		<option style='color:#FF0000' value='FF0000'>FF0000</option>
		<option style='color:#00FF00' value='00FF00'>00FF00</option>
		<option style='color:#0000FF' value='0000FF'>0000FF</option>
		<option style='color:#808080' value='808080'>808080</option>
		<option style='color:#FF00FF' value='FF00FF'>FF00FF</option>
		<option style='color:#808000' value='808000'>808000</option>
		<option style='color:#800080' value='800080'>800080</option>
		<option style='color:#008080' value='008080'>008080</option>
	</select>
	<select name="_txbbSourceCode" onchange="if (this.selectedIndex != 0) {_makeSourceCode('${textareaName}', this.options[this.selectedIndex].value); this.selectedIndex=0;}">
		<option value=''>-- source code --</option>
		<option value='asm'>Assembly</option>
		<option value='c'>C/C++</option>
		<option value='pascal'>Delphi/Pascal</option>
		<option value='html'>HTML/JavaScript</option>
		<option value='java'>Java</option>
		<option value='perl'>Perl</option>
		<option value='php'>PHP</option>
		<option value='sql'>SQL</option>
		<option value='vb'>Visual Basic</option>
		<option value=''>-Others-</option>
	</select>
	<br/>
	<#nested />
	
	<script language="javascript" type="text/javascript">
	/* <![CDATA[ */
	function _createCaret() {
		if ( this.createTextRange && document.selection && document.selection.createRange ) {
			this.caretPos = document.selection.createRange().duplicate();
		}
	}
	function _createCaret2(el) {
		if ( el.createTextRange && document.selection && document.selection.createRange ) {
			el.caretPos = document.selection.createRange().duplicate();
		}
	}
	function _insertTag(textArea, openTag, tagData, closeTag) {
		if ( !document.all ) {
			//non-IE browsers
			if ( textArea.selectionStart != null ) {
				//Mozilla 1.3+, Netscape 7+
				curIndex = textArea.selectionStart;
				endIndex = textArea.selectionEnd;
				var suffix = textArea.value.substring(0, curIndex);
				var w = textArea.value.substring(curIndex, endIndex);
				var rest = textArea.value.substring(endIndex);
				
				//replace current text selection with new value
				textArea.value = suffix+openTag+w+closeTag+rest;
				
				//set net selection range so that new call to insertTXCode
				//will do as we would expected
				textArea.setSelectionRange(curIndex, curIndex+(openTag+w+closeTag).length);
	
				//change forcus to the textarea
				textArea.focus();
			} else {
				//old Mozilla, Netscape browsers
				textArea.value += openTag+tagData+closeTag;
			} //end if
		} else if (textArea.createTextRange && textArea.caretPos) {
			//IE 5+
			//replace current text selection with new value
			var caretPos = textArea.caretPos;
			caretPos.text = openTag+caretPos.text+closeTag;
			
			//move the caret to the begining of the selection
			caretPos.collapse(true);
			
			//change forcus to the textarea
			textArea.focus();
			
			//create new selection, be sure to call this after setting focus!
			_createCaret2(textArea);
		} else {
			//other browsers
			textArea.value += openTag+tagData+closeTag;
		}
	}
	function _makeBold(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{b}', '', '{/b}');
	}
	function _makeItalic(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{i}', '', '{/u}');
	}
	function _makeUnderline(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{u}', '', '{/u}');
	}
	function _makeStrikeThrough(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{s}', '', '{/s}');
	}
	function _makeSuperscript(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{sup}', '', '{/sup}');
	}
	function _makeSubscript(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{sub}', '', '{/sub}');
	}
	function _makeQuote(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{quote}', '', '{/quote}');
	}
	function _makeLink(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{url="http://domain.com"}', '', '{/url}');
	}
	function _makeEmail(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{email="name@domain.com"}', '', '{/email}');
	}
	function _makeImg(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{img="http://domain.com/file.gif"}', '', '');
	}
	function _makeListBullets(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{list}\n{*}-1- first item...\n{*}-2- second item...\n{*}-3- third item...\n{/list}', '', '');
	}
	function _makeListNumbers(elName) {
		var textArea = eval(elName);
		_insertTag(textArea, '{list type="1"}\n{*}-1- first item...\n{*}-2- second item...\n{*}-3- third item...\n{/list}', '', '');
	}
	function _makeColor(elName, color) {
		var textArea = eval(elName);
		_insertTag(textArea, '{color="'+color+'"}', '', '{/color}');
	}
	function _makeSourceCode(elName, sourceCode) {
		var textArea = eval(elName);
		if ( sourceCode != '' ) {
			_insertTag(textArea, '{code="'+sourceCode+'"}', '', '{/code}');
		} else {
			_insertTag(textArea, '{code}', '', '{/code}');
		}
	}
	
	var el = eval('${textareaName}');
	el.onkeyup=_createCaret;
	el.onclick=_createCaret;
	el.onselect=_createCaret;
	/* ]]> */
	</script>
</#macro>
