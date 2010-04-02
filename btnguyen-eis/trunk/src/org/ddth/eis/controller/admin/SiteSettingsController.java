package org.ddth.eis.controller.admin;

import java.util.Map;

import javax.servlet.http.HttpServletRequest;

import org.ddth.eis.EisAppConfigConstants;
import org.ddth.eis.EisConstants;
import org.ddth.eis.bo.appconfig.AppConfig;
import org.ddth.eis.bo.appconfig.AppConfigManager;
import org.ddth.eis.controller.BaseFormController;
import org.ddth.eis.controller.IRequireAuthenticationController;
import org.ddth.eis.controller.IRequireAuthorizationController;
import org.ddth.fileupload.SubmittedForm;
import org.ddth.fileupload.impl.SubmittedFormImpl;

public class SiteSettingsController extends BaseFormController implements
        IRequireAuthenticationController, IRequireAuthorizationController {

    private final static String FORM_NAME = "frmAdminSiteSettings";

    private final static String VIEW_NAME = EisConstants.MODULE_ADMIN + "."
            + EisConstants.ACTION_ADMIN_SITE_SETTINGS;

    private final static String FORM_FIELD_SITE_NAME = "siteName";
    private final static String FORM_FIELD_SITE_TITLE = "siteTitle";
    private final static String FORM_FIELD_SITE_KEYWORDS = "siteKeywords";
    private final static String FORM_FIELD_SITE_DESCRIPTION = "siteDescription";
    private final static String FORM_FIELD_SITE_SLOGAN = "siteSlogan";
    private final static String FORM_FIELD_SITE_COPYRIGHT = "siteCopyright";

    /**
     * {@inheritDoc}
     */
    @Override
    protected String getFormSubmissionSuccessfulViewName() {
        return VIEW_NAME;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected String getViewName() {
        return VIEW_NAME;
    }

    /**
     * {@inheritDoc}
     */
    public SubmittedForm initAssociatedForm() {
        SubmittedForm form = new SubmittedFormImpl(FORM_NAME);
        form.setAction("");

        AppConfigManager acm = getAppConfigManager();
        AppConfig appConfig;

        appConfig = acm.loadConfig(EisAppConfigConstants.CONFIG_SITE_COPYRIGHT);
        form.setAttribute(FORM_FIELD_SITE_COPYRIGHT, appConfig != null ? appConfig.getStringValue()
                : "");

        appConfig = acm.loadConfig(EisAppConfigConstants.CONFIG_SITE_DESCRIPTION);
        form.setAttribute(FORM_FIELD_SITE_DESCRIPTION, appConfig != null ? appConfig
                .getStringValue() : "");

        appConfig = acm.loadConfig(EisAppConfigConstants.CONFIG_SITE_KEYWORDS);
        form.setAttribute(FORM_FIELD_SITE_KEYWORDS, appConfig != null ? appConfig.getStringValue()
                : "");

        appConfig = acm.loadConfig(EisAppConfigConstants.CONFIG_SITE_NAME);
        form
                .setAttribute(FORM_FIELD_SITE_NAME, appConfig != null ? appConfig.getStringValue()
                        : "");

        appConfig = acm.loadConfig(EisAppConfigConstants.CONFIG_SITE_SLOGAN);
        form.setAttribute(FORM_FIELD_SITE_SLOGAN, appConfig != null ? appConfig.getStringValue()
                : "");

        appConfig = acm.loadConfig(EisAppConfigConstants.CONFIG_SITE_TITLE);
        form.setAttribute(FORM_FIELD_SITE_TITLE, appConfig != null ? appConfig.getStringValue()
                : "");

        return form;
    }

    /**
     * {@inheritDoc}
     */
    protected void modelPageContent(Map<String, Object> modelPage) {
        super.modelPageContent(modelPage);
    }

    /**
     * {@inheritDoc}
     */
    public void populateSubmittedForm(SubmittedForm form, HttpServletRequest request) {
        form.setAttribute(FORM_FIELD_SITE_COPYRIGHT, request
                .getParameter(FORM_FIELD_SITE_COPYRIGHT));
        form.setAttribute(FORM_FIELD_SITE_DESCRIPTION, request
                .getParameter(FORM_FIELD_SITE_DESCRIPTION));
        form.setAttribute(FORM_FIELD_SITE_KEYWORDS, request.getParameter(FORM_FIELD_SITE_KEYWORDS));
        form.setAttribute(FORM_FIELD_SITE_NAME, request.getParameter(FORM_FIELD_SITE_NAME));
        form.setAttribute(FORM_FIELD_SITE_SLOGAN, request.getParameter(FORM_FIELD_SITE_SLOGAN));
        form.setAttribute(FORM_FIELD_SITE_TITLE, request.getParameter(FORM_FIELD_SITE_TITLE));
    }

    /**
     * {@inheritDoc}
     */
    public boolean processFormSubmission(SubmittedForm form) {
        AppConfigManager acm = getAppConfigManager();
        AppConfig appConfig;
        
        appConfig = new AppConfig();
        appConfig.setKey(EisAppConfigConstants.CONFIG_SITE_COPYRIGHT);
        appConfig.setStringValue(form.getAttribute(FORM_FIELD_SITE_COPYRIGHT));
        acm.saveConfig(appConfig);

        appConfig = new AppConfig();
        appConfig.setKey(EisAppConfigConstants.CONFIG_SITE_DESCRIPTION);
        appConfig.setStringValue(form.getAttribute(FORM_FIELD_SITE_DESCRIPTION));
        acm.saveConfig(appConfig);

        appConfig = new AppConfig();
        appConfig.setKey(EisAppConfigConstants.CONFIG_SITE_KEYWORDS);
        appConfig.setStringValue(form.getAttribute(FORM_FIELD_SITE_KEYWORDS));
        acm.saveConfig(appConfig);

        appConfig = new AppConfig();
        appConfig.setKey(EisAppConfigConstants.CONFIG_SITE_NAME);
        appConfig.setStringValue(form.getAttribute(FORM_FIELD_SITE_NAME));
        acm.saveConfig(appConfig);

        appConfig = new AppConfig();
        appConfig.setKey(EisAppConfigConstants.CONFIG_SITE_SLOGAN);
        appConfig.setStringValue(form.getAttribute(FORM_FIELD_SITE_SLOGAN));
        acm.saveConfig(appConfig);

        appConfig = new AppConfig();
        appConfig.setKey(EisAppConfigConstants.CONFIG_SITE_TITLE);
        appConfig.setStringValue(form.getAttribute(FORM_FIELD_SITE_TITLE));
        acm.saveConfig(appConfig);

        return true;
    }
}
