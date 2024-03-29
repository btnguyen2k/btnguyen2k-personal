package org.ddth.eis.controller;

import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Properties;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import javax.xml.parsers.ParserConfigurationException;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.ddth.daf.AuthorityAgent;
import org.ddth.daf.AuthorizationAgent;
import org.ddth.daf.Passport;
import org.ddth.daf.Resource;
import org.ddth.eis.EisAppConfigConstants;
import org.ddth.eis.EisConstants;
import org.ddth.eis.bo.appconfig.AppConfig;
import org.ddth.eis.bo.appconfig.AppConfigManager;
import org.ddth.eis.model.DafUserModel;
import org.ddth.mls.Language;
import org.ddth.mls.LanguageFactory;
import org.ddth.mls.utils.MlsException;
import org.ddth.panda.PandaConstants;
import org.ddth.panda.UrlCreator;
import org.ddth.panda.daf.DafDataManager;
import org.ddth.panda.daf.DafPermission;
import org.ddth.panda.daf.DafUser;
import org.ddth.webtemplate.Template;
import org.ddth.webtemplate.TemplateFactory;
import org.ddth.webtemplate.utils.WebTemplateException;
import org.ddth.xconfig.ArrayXNode;
import org.ddth.xconfig.XConfig;
import org.ddth.xconfig.XNode;
import org.springframework.web.servlet.ModelAndView;
import org.springframework.web.servlet.mvc.AbstractController;
import org.springframework.web.servlet.view.RedirectView;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

public abstract class BaseController extends AbstractController {

    private final static Log LOGGER = LogFactory.getLog(BaseController.class);

    private HttpServletRequest httpRequest;

    private HttpServletResponse httpResponse;

    private ModelAndView mav;

    private DafPermission accessPermission;

    protected final static String MODEL_LANGUAGE = "language";

    protected final static String MODEL_CURRENT_USER = "currentUser";

    protected final static String MODEL_APP_VERSION_INFO = "appVersionInfo";

    protected final static String MODEL_PAGE = "page";
    protected final static String MODEL_PAGE_NAME = "name";
    protected final static String MODEL_PAGE_TITLE = "title";
    protected final static String MODEL_PAGE_KEYWORDS = "keywords";
    protected final static String MODEL_PAGE_DESCRIPTION = "description";
    protected final static String MODEL_PAGE_SLOGAN = "slogan";
    protected final static String MODEL_PAGE_COPYRIGHT = "copyright";
    protected final static String MODEL_PAGE_TOP_MENU = "topMenu";
    protected final static String MODEL_PAGE_SIDE_MENU = "sideMenu";

    private static XConfig appMenuConfig;

    /* for IRequireAuthenticationController */
    /**
     * Gets the login URL to redirect to.
     * 
     * @return String
     */
    public String getLoginUrl() {
        UrlCreator urlCreator = getUrlCreator();
        String uri = urlCreator.createUri(EisConstants.MODULE_HOME, EisConstants.ACTION_HOME_LOGIN);
        return uri;
    }

    /* for IRequireAuthenticationController */

    /* for IRequireAuthorizationController */
    /**
     * Checks if the current user can access this controler.
     * 
     * @return boolean
     */
    public boolean canAccess() {
        return accessPermission != null ? hasPermission(accessPermission) : true;
    }

    public void setAccessPermission(DafPermission accessPermission) {
        this.accessPermission = accessPermission;
    }

    public DafPermission getAccessPermission() {
        return this.accessPermission;
    }

    /* for IRequireAuthorizationController */

    /**
     * Gets a bean from Spring's application context.
     * 
     * @param <T>
     * @param name
     *            String
     * @param clazz
     *            Class
     * @return T
     */
    protected <T> T getBean(String name, Class<T> clazz) {
        return getApplicationContext().getBean(name, clazz);
    }

    /**
     * Gets the AppConfigManager instance.
     * 
     * @return AppConfigManager
     */
    protected AppConfigManager getAppConfigManager() {
        return getBean(EisConstants.BEAN_BO_APPCONFIG_MANAGER, AppConfigManager.class);
    }

    /**
     * Gets the UrlCreator instance.
     * 
     * @return UrlCreator
     */
    protected UrlCreator getUrlCreator() {
        return getBean(EisConstants.BEAN_URL_CREATOR, UrlCreator.class);
    }

    /**
     * Gets the currently logged in user.
     * 
     * @return DafUser
     */
    protected DafUser getCurrentUser() {
        String username = getSessionAttribute(EisConstants.SESSION_CURRENT_USERNAME, String.class);
        DafDataManager dafDm = getBean(EisConstants.BEAN_BO_DAF_MANAGER, DafDataManager.class);
        return username != null ? dafDm.getUser(username) : null;
    }

    /**
     * Gets the ModelAndView object.
     * 
     * @return ModelAndView
     */
    protected ModelAndView getModelAndView() {
        return this.mav;
    }

    /**
     * Gets the current Http request object.
     * 
     * @return HttpServletRequest
     */
    protected HttpServletRequest getRequest() {
        return this.httpRequest;
    }

    /**
     * Gets the current Http response object.
     * 
     * @return HttpServletResponse
     */
    protected HttpServletResponse getResponse() {
        return this.httpResponse;
    }

    /**
     * Gets the Http Session object.
     * 
     * @return httpSession
     */
    protected HttpSession getSession() {
        return getRequest().getSession(true);
    }

    /**
     * Gets a Http Session attribute.
     * 
     * @param attrName
     *            String
     * @return Object
     */
    protected Object getSessionAttribute(String attrName) {
        HttpSession session = getSession();
        return session.getAttribute(attrName);
    }

    /**
     * Gets a Http Session attribute.
     * 
     * @param <T>
     * @param attrName
     *            String
     * @param clazz
     *            Class<T>
     * @return T
     */
    @SuppressWarnings("unchecked")
    protected <T> T getSessionAttribute(String attrName, Class<T> clazz) {
        HttpSession session = getSession();
        Object obj = session.getAttribute(attrName);
        return obj != null ? (T) obj : null;
    }

    /**
     * Removes a Http Session attribute.
     * 
     * @param attrName
     *            String
     */
    protected void removeSessionAttribute(String attrName) {
        HttpSession session = getSession();
        session.removeAttribute(attrName);
    }

    /**
     * Gets the LanguageFactory instance.
     * 
     * @return LanguageFactory
     */
    protected LanguageFactory getLanguageFactory() {
        return getBean(EisConstants.BEAN_LANGUAGE_FACTORY, LanguageFactory.class);
    }

    /**
     * Gets the TemplateFactory instance.
     * 
     * @return TemplateFactory
     */
    protected TemplateFactory getTemplateFactory() {
        return getBean(EisConstants.BEAN_TEMPLATE_FACTORY, TemplateFactory.class);
    }

    /**
     * Gets the language object.
     * 
     * @return Language
     */
    protected Language getLanguage() {
        HttpSession session = getSession();
        Language language = (Language) session.getAttribute(PandaConstants.SESSION_LANGUAGE);
        if ( language == null ) {
            LanguageFactory lf = getLanguageFactory();
            try {
                language = lf.getLanguage(PandaConstants.DEFAULT_LANGUAGE_NAME);
            } catch ( MlsException e ) {
                LOGGER.error("Can not retrieve language pack ["
                        + PandaConstants.DEFAULT_LANGUAGE_NAME + "]", e);
                return null;
            }
            session.setAttribute(PandaConstants.SESSION_LANGUAGE, language);
            session.setAttribute(PandaConstants.SESSION_LANGUAGE_NAME, language.getName());
        }
        return language;
    }

    private void initLanguage() {
        HttpSession session = getSession();
        Language language = (Language) session.getAttribute(PandaConstants.SESSION_LANGUAGE);
        if ( language == null ) {
            LanguageFactory lf = getLanguageFactory();
            try {
                language = lf.getLanguage(PandaConstants.DEFAULT_LANGUAGE_NAME);
            } catch ( MlsException e ) {
                LOGGER.error("Can not retrieve language pack ["
                        + PandaConstants.DEFAULT_LANGUAGE_NAME + "]", e);
            }
            session.setAttribute(PandaConstants.SESSION_LANGUAGE, language);
            session.setAttribute(PandaConstants.SESSION_LANGUAGE_NAME, language.getName());
        }
    }

    private void initTemplate() {
        HttpSession session = getSession();
        Template template = (Template) session.getAttribute(PandaConstants.SESSION_TEMPLATE);
        if ( template == null ) {
            TemplateFactory tf = getTemplateFactory();
            try {
                template = tf.getTemplate(PandaConstants.DEFAULT_TEMPLATE_NAME);
            } catch ( WebTemplateException e ) {
                LOGGER.error("Can not retrieve template pack ["
                        + PandaConstants.DEFAULT_TEMPLATE_NAME + "]", e);
            }
            session.setAttribute(PandaConstants.SESSION_TEMPLATE, template);
            session.setAttribute(PandaConstants.SESSION_TEMPLATE_NAME, template.getName());
        }
    }

    /**
     * Sub-class must implement this method to return the view name.
     * 
     * @return String
     */
    protected abstract String getViewName();

    /**
     * Is this controller associated with a form.
     * 
     * @return boolean
     */
    protected boolean hasAssociatedForm() {
        return this instanceof IFormController;
    }

    private Properties appExternalConfigs;

    /**
     * Top level model: Sets the AppVersionInfo model.
     * 
     * @param mav
     *            ModelAndView
     */
    protected void modelAppVersionInfo(ModelAndView mav) {
        final String EXTERNAL_CONFIG_FILE = "/WEB-INF/eis-config.properties";
        final String PROP_VERSION_STRING = "version.string";
        final String PROP_BUILD_NUMBER = "build.number";
        if ( appExternalConfigs == null ) {
            appExternalConfigs = new Properties();
            InputStream is = getServletContext().getResourceAsStream(EXTERNAL_CONFIG_FILE);
            try {
                appExternalConfigs.load(is);
            } catch ( IOException e ) {
                throw new RuntimeException(e);
            } finally {
                try {
                    is.close();
                } catch ( IOException e ) {
                }
            }
        }
        String versionString = appExternalConfigs.getProperty(PROP_VERSION_STRING) + " Build "
                + appExternalConfigs.getProperty(PROP_BUILD_NUMBER);
        mav.addObject(MODEL_APP_VERSION_INFO, versionString);
    }

    /**
     * Top level model: Sets the Language model.
     * 
     * @param mav
     *            ModelAndView
     */
    protected void modelLanguage(ModelAndView mav) {
        mav.addObject(MODEL_LANGUAGE, getLanguage());
    }

    /**
     * Top level model: Sets the CurrentUser model.
     * 
     * @param mav
     *            ModelAndView
     */
    protected void modelCurrentUser(ModelAndView mav) {
        mav.addObject(MODEL_CURRENT_USER, DafUserModel.getInstance(getCurrentUser()));
    }

    /**
     * Top level model: Sets the Page model.
     * 
     * @param mav
     *            ModelAndView
     */
    protected void modelPage(ModelAndView mav) {
        Map<String, Object> modelPage = new HashMap<String, Object>();
        mav.addObject(MODEL_PAGE, modelPage);
        modelPageTopMenu(modelPage);
        modelPageSideMenu(modelPage);
        modelPageName(modelPage);
        modelPageTitle(modelPage);
        modelPageKeywords(modelPage);
        modelPageDescription(modelPage);
        modelPageSlogan(modelPage);
        modelPageCopyright(modelPage);
        modelPageContent(modelPage);
    }

    /**
     * Checks if the current user has a specific permission.
     * 
     * @param permission
     *            DafPermission
     * @return boolean
     */
    protected boolean hasPermission(DafPermission permission) {
        DafUser currentUser = getCurrentUser();
        if ( currentUser == null ) {
            return false;
        }
        AuthorizationAgent aa = getBean(EisConstants.BEAN_DAF_AUTHORIZATION_AGENT,
                                        AuthorizationAgent.class);
        // if ( currentUser != null ) {
        if ( aa.isInGodGroup(currentUser) ) {
            return true;
        }
        Collection<? extends Passport> passports = aa
                .getAuthority(currentUser, permission, AuthorityAgent.RETURN_ALL_PASSPORT);
        return passports != null && passports.size() > 0;
        // } else {
        // DafDataManager dafDm = getDafDataManager();
        // return permissionChecker.getPassport(dafDm.getGroup(PandaPortalConstants.GROUP_GUEST),
        // permission) != null;
        // }
    }

    /**
     * Checks if the current user has a specific permission against a specific resource.
     * 
     * @param permission
     *            DafPermission
     * @param resource
     *            Resource
     * @return boolean
     */
    protected boolean hasPermission(DafPermission permission, Resource resource) {
        DafUser currentUser = getCurrentUser();
        if ( currentUser == null ) {
            return false;
        }
        AuthorizationAgent aa = getBean(EisConstants.BEAN_DAF_AUTHORIZATION_AGENT,
                                        AuthorizationAgent.class);
        // if ( currentUser != null ) {
        if ( aa.isInGodGroup(currentUser) ) {
            return true;
        }
        Collection<? extends Passport> passports = aa
                .getAuthority(currentUser, permission, resource, AuthorityAgent.RETURN_ALL_PASSPORT);
        return passports != null && passports.size() > 0;
        // } else {
        // DafDataManager dafDm = getDafDataManager();
        // return permissionChecker.getPassport(dafDm.getGroup(PandaPortalConstants.GROUP_GUEST),
        // permission, resource) != null;
        // }
    }

    private XConfig loadMenuConfig() throws IOException, SAXException, ParserConfigurationException {
        InputStream is = getServletContext().getResourceAsStream("/WEB-INF/app-menu.xml");
        return new XConfig(new InputSource(is));
    }

    private List<Map<String, ?>> getAppMenuModel() {
        final String XPATH_MENU = "/app-menu/menu";
        final String NODE_TITLE = "title";
        final String NODE_LINK = "link";
        final String NODE_PERMISSION = "permission";
        final String ATTR_TITLE = "title";
        final String ATTR_URL = "url";
        final String ATTR_MENU_ITEMS = "menuItems";

        if ( appMenuConfig == null ) {
            try {
                appMenuConfig = loadMenuConfig();
            } catch ( Exception e ) {
                LOGGER.error("Can not load application menu configuration!", e);
            }
        }
        if ( appMenuConfig == null ) {
            return null;
        }
        List<Map<String, ?>> modelMenu = new ArrayList<Map<String, ?>>();
        XNode[] nodes = appMenuConfig.getConfig(XPATH_MENU);
        Language lang = getLanguage();
        UrlCreator urlCreator = getUrlCreator();
        for ( XNode node : nodes ) {
            String strPermission = node.getAtrribute(NODE_PERMISSION);
            if ( strPermission != null ) {
                String[] tokens = strPermission.split(":");
                if ( tokens.length != 2 ) {
                    // something wrong...
                    LOGGER.warn("Invalid permission: " + strPermission);
                    continue;
                }
                DafPermission permission = DafPermission.createInstance(tokens[0], tokens[1]);
                if ( !hasPermission(permission) ) {
                    continue;
                }
            }

            Map<String, Object> menuEntry = new HashMap<String, Object>();
            modelMenu.add(menuEntry);
            String title = node.getAtrribute(NODE_TITLE);
            if ( title.startsWith("lang:") ) {
                title = lang.getMessage(title.substring("lang:".length()));
            }
            menuEntry.put(ATTR_TITLE, title);

            String url = node.getAtrribute(NODE_LINK);
            if ( url != null ) {
                if ( url.startsWith("action:") ) {
                    String[] tokens = url.split(":");
                    String module = tokens.length > 1 ? tokens[1] : null;
                    String action = tokens.length > 2 ? tokens[2] : null;
                    url = urlCreator.createUri(module, action);
                } else if ( url.startsWith("url:") ) {
                    url = url.substring("url:".length());
                }
                menuEntry.put(ATTR_URL, url);
            }
            List<Map<String, ?>> menuItems = getMenuItems(node);
            if ( menuItems != null && menuItems.size() > 0 ) {
                menuEntry.put(ATTR_MENU_ITEMS, menuItems);
            }
        }
        return modelMenu;
    }

    private List<Map<String, ?>> getMenuItems(XNode root) {
        final String NODE_TITLE = "title";
        final String NODE_LINK = "link";
        final String NODE_PERMISSION = "permission";
        final String ATTR_TITLE = "title";
        final String ATTR_URL = "url";

        if ( !(root instanceof ArrayXNode) ) {
            return null;
        }

        Language lang = getLanguage();
        UrlCreator urlCreator = getUrlCreator();

        List<Map<String, ?>> menuItems = new ArrayList<Map<String, ?>>();
        ArrayXNode aNode = (ArrayXNode) root;
        XNode[] children = aNode.getNodeValue();
        for ( XNode child : children ) {
            String strPermission = child.getAtrribute(NODE_PERMISSION);
            if ( strPermission != null ) {
                String[] tokens = strPermission.split(":");
                if ( tokens.length != 2 ) {
                    // something wrong...
                    LOGGER.warn("Invalid permission: " + strPermission);
                    continue;
                }
                DafPermission permission = DafPermission.createInstance(tokens[0], tokens[1]);
                if ( !hasPermission(permission) ) {
                    continue;
                }
            }

            Map<String, Object> menuEntry = new HashMap<String, Object>();
            menuItems.add(menuEntry);
            String title = child.getAtrribute(NODE_TITLE);
            if ( title.startsWith("lang:") ) {
                title = lang.getMessage(title.substring("lang:".length()));
            }
            menuEntry.put(ATTR_TITLE, title);

            String url = child.getAtrribute(NODE_LINK);
            if ( url != null ) {
                if ( url.startsWith("action:") ) {
                    String[] tokens = url.split(":");
                    String module = tokens.length > 1 ? tokens[1] : null;
                    String action = tokens.length > 2 ? tokens[2] : null;
                    url = urlCreator.createUri(module, action);
                } else if ( url.startsWith("url:") ) {
                    url = url.substring("url:".length());
                }
                menuEntry.put(ATTR_URL, url);
            }
        }
        return menuItems;
    }

    /**
     * Models the page's top menu bar.
     * 
     * @param modelPage
     *            Map<String, Object>
     */
    protected void modelPageTopMenu(Map<String, Object> modelPage) {
        List<Map<String, ?>> modelMenu = getAppMenuModel();
        modelPage.put(MODEL_PAGE_TOP_MENU, modelMenu);
    }

    /**
     * Models the page's side menu.
     * 
     * @param modelPage
     *            Map<String, Object>
     */
    protected void modelPageSideMenu(Map<String, Object> modelPage) {
        List<Map<String, ?>> modelMenu = getAppMenuModel();
        modelPage.put(MODEL_PAGE_SIDE_MENU, modelMenu);
    }

    /**
     * Models the page's content. Subclass overrides this methods to model its own page content.
     * 
     * @param modelPage
     *            Map<String, Object>
     */
    protected void modelPageContent(Map<String, Object> modelPage) {
        // do nothing
    }

    /**
     * Models the page's name.
     * 
     * @param modelPage
     *            Map<String, Object>
     */
    protected void modelPageName(Map<String, Object> modelPage) {
        AppConfigManager acp = getAppConfigManager();
        AppConfig config = acp.loadConfig(EisAppConfigConstants.CONFIG_SITE_NAME);
        if ( config != null ) {
            modelPage.put(MODEL_PAGE_NAME, config.getStringValue());
        } else {
            modelPage.put(MODEL_PAGE_NAME, "");
        }
    }

    /**
     * Models the page's slogan.
     * 
     * @param modelPage
     *            Map<String, Object>
     */
    protected void modelPageSlogan(Map<String, Object> modelPage) {
        AppConfigManager acp = getAppConfigManager();
        AppConfig config = acp.loadConfig(EisAppConfigConstants.CONFIG_SITE_SLOGAN);
        if ( config != null ) {
            modelPage.put(MODEL_PAGE_SLOGAN, config.getStringValue());
        } else {
            modelPage.put(MODEL_PAGE_SLOGAN, "");
        }
    }

    /**
     * Models the page's copyright message.
     * 
     * @param modelPage
     *            Map<String, Object>
     */
    protected void modelPageCopyright(Map<String, Object> modelPage) {
        AppConfigManager acp = getAppConfigManager();
        AppConfig config = acp.loadConfig(EisAppConfigConstants.CONFIG_SITE_COPYRIGHT);
        if ( config != null ) {
            modelPage.put(MODEL_PAGE_COPYRIGHT, config.getStringValue());
        } else {
            modelPage.put(MODEL_PAGE_COPYRIGHT, "");
        }
    }

    /**
     * Models the page's title.
     * 
     * @param modelPage
     *            Map<String, Object>
     */
    protected void modelPageTitle(Map<String, Object> modelPage) {
        AppConfigManager acp = getAppConfigManager();
        AppConfig config = acp.loadConfig(EisAppConfigConstants.CONFIG_SITE_TITLE);
        if ( config != null ) {
            modelPage.put(MODEL_PAGE_TITLE, config.getStringValue());
        } else {
            modelPage.put(MODEL_PAGE_TITLE, "");
        }
    }

    /**
     * Models the page's keywords.
     * 
     * @param modelPage
     *            Map<String, Object>
     */
    protected void modelPageKeywords(Map<String, Object> modelPage) {
        AppConfigManager acp = getAppConfigManager();
        AppConfig config = acp.loadConfig(EisAppConfigConstants.CONFIG_SITE_KEYWORDS);
        if ( config != null ) {
            modelPage.put(MODEL_PAGE_KEYWORDS, config.getStringValue());
        } else {
            modelPage.put(MODEL_PAGE_KEYWORDS, "");
        }
    }

    /**
     * Models the page's description.
     * 
     * @param modelPage
     *            Map<String, Object>
     */
    protected void modelPageDescription(Map<String, Object> modelPage) {
        AppConfigManager acp = getAppConfigManager();
        AppConfig config = acp.loadConfig(EisAppConfigConstants.CONFIG_SITE_DESCRIPTION);
        if ( config != null ) {
            modelPage.put(MODEL_PAGE_DESCRIPTION, config.getStringValue());
        } else {
            modelPage.put(MODEL_PAGE_DESCRIPTION, "");
        }
    }

    /**
     * Calls this methods to start populating models.
     */
    protected void modelController() {
        modelController(getModelAndView());
    }

    /**
     * Calls this methods to start populating models.
     * 
     * @param mav
     *            ModelAndView
     */
    protected void modelController(ModelAndView mav) {
        modelCurrentUser(mav);
        modelLanguage(mav);
        modelAppVersionInfo(mav);
        modelPage(mav);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected ModelAndView handleRequestInternal(HttpServletRequest request,
            HttpServletResponse response) throws Exception {
        this.httpRequest = request;
        this.httpResponse = response;
        if ( this instanceof IRequireAuthenticationController ) {
            DafUser user = getCurrentUser();
            if ( user == null ) {
                removeSessionAttribute(EisConstants.SESSION_CURRENT_USERNAME);
                IRequireAuthenticationController controller = (IRequireAuthenticationController) this;
                String url = controller.getLoginUrl();
                assert url != null;
                return new ModelAndView(new RedirectView(url));
            }
        }
        if ( this instanceof IRequireAuthorizationController ) {
            IRequireAuthorizationController controller = (IRequireAuthorizationController) this;
            if ( !controller.canAccess() ) {
                // TODO refine!
                // current behavior: return HTTP Error 403
                response.sendError(403);
                return null;
            }
        }
        initLanguage();
        initTemplate();
        this.mav = execute();
        if ( this.mav == null ) {
            this.mav = new ModelAndView(getViewName());
            modelController();
        }
        return this.mav;
    }

    /**
     * Sub-class overrides this method to perform additional processing.
     * 
     * @return ModelAndView sub-class returns non-null ModelAndView to override the ModelAndView
     *         created by this base controller
     * @throws Exception
     */
    protected ModelAndView execute() throws Exception {
        return null;
    }
}
