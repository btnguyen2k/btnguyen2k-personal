package com.greenstorm.gsc.util;

/**
 * TXBB Code Parser.
 * 
 * @author Thanh Ba Nguyen <btnguyen2k@gmail.com>
 * @copyright See README.TXT file for more details
 * @version 1.0
 */
public interface TXCodeParser {
    /**
     * Parses a TXBB-tag string to HTML/XHTML.
     * 
     * @param input String
     * @return String
     */
    public String parse(String input);

    /**
     * Parse a TXBB-tag string and return the plain text.
     * 
     * @param input String
     * @return String
     */
    public String parsePlain(String input);
}
