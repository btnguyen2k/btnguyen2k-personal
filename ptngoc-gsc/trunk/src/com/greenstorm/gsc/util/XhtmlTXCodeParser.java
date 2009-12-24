package com.greenstorm.gsc.util;

import org.ddth.txcode.DocumentTree;
import org.ddth.txcode.TXCodeEngine;
import org.ddth.txcode.XhtmlRenderer;

public class XhtmlTXCodeParser extends AbstractTXCodeParser {

    private XhtmlRenderer xhtmlRenderer;

    /**
     * Constructs a new XhtmlTXCodeParser object.
     */
    public XhtmlTXCodeParser() throws Exception {
        super();
        xhtmlRenderer = new XhtmlRenderer(getConfig());
    }

    /**
     * Retrieves the renderer instance.
     * 
     * @return XhtmlRenderer
     */
    protected XhtmlRenderer getRenderer() {
        return xhtmlRenderer;
    }

    /**
     * {@inheritDoc}
     */
    public String parse(String input) {
        if ( input == null ) {
            return "";
        }
        DocumentTree docTree = TXCodeEngine.parse(input, getConfig());
        return xhtmlRenderer.render(docTree);
    }
}
