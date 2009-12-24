package com.greenstorm.gsc.util;

import java.net.URL;

import org.ddth.txcode.Configuration;
import org.ddth.txcode.DocumentTree;
import org.ddth.txcode.TXCodeEngine;

public abstract class AbstractTXCodeParser implements TXCodeParser {

    private Configuration config;

    /**
     * Constructs a new XhtmlTXCodeParser object.
     */
    public AbstractTXCodeParser() throws Exception {
        URL url = getClass().getResource("txcode.config.xml");
        config = Configuration.getConfiguration(url);
    }

    /**
     * Retrieves the Configuration instance.
     * 
     * @return Configuration
     */
    protected Configuration getConfig() {
        return config;
    }

    /**
     * {@inheritDoc}
     */
    public String parsePlain(String input) {
        DocumentTree docTree = TXCodeEngine.parse(input, getConfig());
        return docTree.toString();
    }
}
