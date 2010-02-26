package org.ddth.eis.controller;

import java.util.HashMap;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.ddth.eis.EisAppConfigConstants;
import org.ddth.eis.EisConstants;
import org.ddth.eis.bo.appconfig.AppConfig;
import org.ddth.eis.bo.appconfig.AppConfigManager;
import org.ddth.mls.Language;
import org.ddth.mls.LanguageFactory;
import org.ddth.mls.utils.MlsException;
import org.ddth.panda.PandaConstants;
import org.ddth.webtemplate.Template;
import org.ddth.webtemplate.TemplateFactory;
import org.ddth.webtemplate.utils.WebTemplateException;
import org.springframework.web.servlet.ModelAndView;
import org.springframework.web.servlet.mvc.AbstractController;

public abstract class BaseController extends AbstractController {

    private final static Log    LOGGER                 = LogFactory.getLog(BaseController.class);

    private HttpServletRequest  httpRequest;

    private HttpServletResponse httpResponse;

    private ModelAndView        mav;

    public final static String  MODEL_LANGUAGE         = "language";

    public final static String  MODEL_PAGE             = "page";

    public final static String  MODEL_PAGE_NAME        = "name";

    public final static String  MODEL_PAGE_TITLE       = "title";

    public final static String  MODEL_PAGE_KEYWORDS    = "keywords";

    public final static String  MODEL_PAGE_DESCRIPTION = "description";

    /**
     * Gets a bean
     * 
     * @param <T>
     * @param name
     *            String
     * @param clazz
     *            Class<T>
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
     * Subclass must implement this method to return the view name.
     * 
     * @return String
     */
    protected abstract String getViewName();

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
     * Top level model: Sets the Page model.
     * 
     * @param mav
     *            ModelAndView
     */
    protected void modelPage(ModelAndView mav) {
        Map<String, Object> modelPage = new HashMap<String, Object>();
        mav.addObject(MODEL_PAGE, modelPage);
        modelPageName(modelPage);
        modelPageTitle(modelPage);
        modelPageKeywords(modelPage);
        modelPageDescription(modelPage);
        modelPageContent(modelPage);
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
        AppConfig config = acp.loadConfig(EisAppConfigConstants.CONFIG_PAGE_NAME);
        if ( config != null ) {
            modelPage.put(MODEL_PAGE_NAME, config.getStringValue());
        } else {
            modelPage.put(MODEL_PAGE_NAME, "");
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
        AppConfig config = acp.loadConfig(EisAppConfigConstants.CONFIG_PAGE_TITLE);
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
        AppConfig config = acp.loadConfig(EisAppConfigConstants.CONFIG_PAGE_KEYWORDS);
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
        AppConfig config = acp.loadConfig(EisAppConfigConstants.CONFIG_PAGE_DESCRIPTION);
        if ( config != null ) {
            modelPage.put(MODEL_PAGE_DESCRIPTION, config.getStringValue());
        } else {
            modelPage.put(MODEL_PAGE_DESCRIPTION, "");
        }
    }

    /**
     * Subclass calls this methods to start populating models.
     * 
     * @param mav
     *            ModelAndView
     */
    protected void modelController() {
        modelLanguage(getModelAndView());
        modelPage(getModelAndView());
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected ModelAndView handleRequestInternal(HttpServletRequest request,
            HttpServletResponse response) throws Exception {
        this.httpRequest = request;
        this.httpResponse = response;
        initLanguage();
        initTemplate();
        this.mav = new ModelAndView(getViewName());
        execute();
        return mav;
    }

    /**
     * Sub-class overrides this method to actually execute the controller.
     * 
     * @throws Exception
     */
    protected abstract void execute() throws Exception;
}
