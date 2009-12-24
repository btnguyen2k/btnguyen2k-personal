/**
 * Add-on for Code Syntax Highlighter.
 * 
 * Copyright (C) 2009 Thanh Ba Nguyen - DDTH.ORG
 */
SyntaxHighlighter.config.tagName = 'code';
SyntaxHighlighter.config.stripBrs = true;

SyntaxHighlighter.config.clipboardSwf = 'scah/scripts/clipboard.swf';

SyntaxHighlighter.utils.addEvent(
		window, 
		'load', 
		function() {
	var elements = document
			.getElementsByTagName(SyntaxHighlighter.config.tagName);
	if (elements.length === 0) {
		return;
	}
	for ( var i = 0; i < elements.length; i++) {
		var target = elements[i];
		var params = SyntaxHighlighter.utils.parseParams(target.className);
		var languageName = params['scah'];
		if ( !languageName ) {
			continue;
		}
		target.className = 'brush:'+languageName;
		if ( target.title ) {
			//a little hack here: since custom class name is append to end of
			//SyntaxHighlighter's generated class name, we will take advantage
			//of that to 'insert' a title attribute
			target.className += '; ruler: true; title:\''+target.title+'\'';
		} else {
			target.className += '; light: true';
		}
	}
	SyntaxHighlighter.highlight();
	//SyntaxHighlighter.all();
});
