package com.greenstorm.gsc.util;

import com.ibm.icu.text.Normalizer;

/**
 * Default Unicode SEO.
 * 
 * @author Thanh Ba Nguyen <btnguyen2k@gmail.com>
 * @copyright See README.TXT file for more details
 * @version 1.0
 */
public class DefaultSeoUnicode implements SeoUnicode {

    /**
     * {@inheritDoc}
     */
    public String textToTitle(String str) {
        if ( str == null ) {
            return "";
        }
        str = Normalizer.decompose(str, false);
        return str.trim().replaceAll("\\s+", " ");
    }

    /**
     * {@inheritDoc}
     */
    public String textToUrl(String str) {
        if ( str == null ) {
            return "";
        }
        str = Normalizer.decompose(str, false);
        return str.replaceAll("\\W+", "-").replaceAll("^-+", "").replaceAll(
                "-+$", "").replaceAll("-+", "-");
    }
}
