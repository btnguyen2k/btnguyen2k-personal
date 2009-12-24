package com.greenstorm.gsc.util;

import com.ibm.icu.text.Normalizer;

/**
 * TCVN6909 Unicode SEO.
 * 
 * @author Thanh Ba Nguyen <btnguyen2k@gmail.com>
 * @copyright See README.TXT file for more details
 * @version 1.0
 */
public class TCVN6909SeoUnicode implements SeoUnicode {

    /**
     * {@inheritDoc}
     */
    public String textToTitle(String str) {
        if ( str == null ) {
            return "";
        }
        // fix problem with Đ and đ
        // also remove characters that are not in TCVN6909
        str =
                Normalizer.decompose(str.replace("đ", "d").replace("Đ", "D"),
                        false).replaceAll("[\u0100-\uffff]+", "");
        return str.trim().replaceAll("\\s+", " ");
    }

    /**
     * {@inheritDoc}
     */
    public String textToUrl(String str) {
        if ( str == null ) {
            return "";
        }

        // fix problem with Đ and đ
        // also remove characters that are not in TCVN6909
        str =
                Normalizer.decompose(str.replace("đ", "d").replace("Đ", "D"),
                        false).replaceAll("[\u0100-\uffff]+", "");

        return str.replaceAll("\\W+", "-").replaceAll("^-+", "").replaceAll(
                "-+$", "").replaceAll("-+", "-");
    }
}
