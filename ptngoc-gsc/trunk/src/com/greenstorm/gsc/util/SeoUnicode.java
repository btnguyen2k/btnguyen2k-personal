package com.greenstorm.gsc.util;

/**
 * Unicode SEO.
 * 
 * @author Thanh Ba Nguyen <btnguyen2k@gmail.com>
 * @copyright See README.TXT file for more details
 * @version 1.0
 */
public interface SeoUnicode {
    /**
     * SEO an unicode string for URL.
     * 
     * @param uStr String
     * @return String
     */
    public String textToUrl(String uStr);

    /**
     * SEO an unicode string for browser's title.
     * 
     * @param uStr String
     * @return String
     */
    public String textToTitle(String uStr);
}
