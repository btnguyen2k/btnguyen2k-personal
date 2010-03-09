package org.ddth.eis;

/**
 * EIS constants.
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class EisConstants {

    public final static int    GROUP_ID_ADMINISTRATOR         = 1;
    public final static int    GROUP_ID_STAFF                 = 2;
    public final static int    GROUP_ID_GUEST                 = 3;

    public final static String SESSION_CURRENT_USERNAME       = "EIS_USERNAME";

    public final static String BEAN_HIBERNATE_SESSION_FACTORY = "sessionFactory";
    public final static String BEAN_LANGUAGE_FACTORY          = "languageFactory";
    public final static String BEAN_TEMPLATE_FACTORY          = "templateFactory";
    public final static String BEAN_AUTHENTICATION_MANAGER    = "authenticationManager";
    public final static String BEAN_URL_CREATOR               = "urlCreator";

    public final static String BEAN_BO_APPCONFIG_MANAGER      = "managerAppConfig";
    public final static String BEAN_BO_DAF_MANAGER            = "managerDaf";

    public final static String MODULE_HOME                    = "home";
    public final static String ACTION_INDEX                   = "index";
    public final static String ACTION_LOGIN                   = "login";
    public final static String ACTION_LOGOUT                  = "logout";
}
