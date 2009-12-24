package com.greenstorm.gsc.util.txcode.tag;

import java.util.Map;

import org.ddth.txcode.TagConfig;
import org.ddth.txcode.TokenOpenTag;
import org.ddth.txcode.Tokenizer;

public class Scah extends org.ddth.txcode.tag.Scah {

    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = -775579493984998506L;

    /**
     * Constructs a new Scah object.
     * 
     * @param tagConfig TagConfig
     */
    public Scah(TagConfig tagConfig) {
        super(tagConfig);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void parse(TokenOpenTag openTag, Tokenizer tokenizer) {
        super.parse(openTag, tokenizer);
        Map<String, String> attrs = openTag.getTagParameters();
        String codeLanguage = attrs.get("");
        String title = attrs.get("TITLE");
        String className = "brush:";
        if ( codeLanguage != null && codeLanguage.trim().length() != 0 ) {
            className += codeLanguage.trim();
        } else {
            className += "plain";
        }
        if ( title != null && title.trim().length() != 0 ) {
            className +=
                    "; light: false; ruler: true; gutter: true; toolbar: true";
        } else {
            className += "; light: true; ruler: false";
        }
        addHtmlParam("class", className);
    }
}
