package org.ddth.eis;

import org.ddth.eis.bo.appconfig.AppConfigKey;

/**
 * Application configuration constants for Eis.
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class EisAppConfigConstants {
    public final static AppConfigKey CONFIG_PAGE_NAME        = AppConfigKey
                                                                     .createAppConfigKey("SITE",
                                                                                         "NAME");

    public final static AppConfigKey CONFIG_PAGE_TITLE       = AppConfigKey
                                                                     .createAppConfigKey("SITE",
                                                                                         "TITLE");

    public final static AppConfigKey CONFIG_PAGE_KEYWORDS    = AppConfigKey
                                                                     .createAppConfigKey("SITE",
                                                                                         "KEYWORDS");

    public final static AppConfigKey CONFIG_PAGE_DESCRIPTION = AppConfigKey
                                                                     .createAppConfigKey("SITE",
                                                                                         "DESCRIPTION");
}
