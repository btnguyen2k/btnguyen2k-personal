package com.greenstorm.gsc.panda.staff;

import javax.servlet.ServletException;

import org.ddth.fileupload.SubmittedForm;
import org.ddth.mls.Language;
import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.daf.DafDataManager;
import org.ddth.panda.core.daf.DafUser;
import org.ddth.panda.core.impl.controlforward.ActionControlForward;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.PandaPortalConstants;
import org.ddth.panda.portal.utils.CommonFormValidatingReporter;
import org.ddth.panda.portal.utils.TransitionRecord;
import org.ddth.panda.utils.StringUtils;
import org.ddth.panda.web.UrlCreator;
import org.ddth.panda.web.form.FormValidatingReporter;

import com.greenstorm.gsc.GscConstants;
import com.greenstorm.gsc.GscLangConstants;
import com.greenstorm.gsc.panda.BaseActionHandler;

public class LoginHandler extends BaseActionHandler {

    private final static String FORM_FIELD_LOGIN_NAME = "loginname";

    private final static String FORM_FIELD_PASSWORD = "password";

    /**
     * {@inheritDoc}
     */
    @Override
    protected ControlForward execute() throws Exception {
        PandaPortalApplication app = getApp();

        SubmittedForm form = getSubmittedForm();
        if ( form != null && doLogin(form) ) {
            String urlLastPage =
                    app.getHttpSessionAttribute(
                            PandaPortalConstants.SESSION_LAST_ACCESS_URL,
                            String.class);
            if ( StringUtils.isEmpty(urlLastPage) ) {
                UrlCreator urlCreator = app.getUrlCreator();
                urlLastPage =
                        urlCreator.createUri(getModule(),
                                GscConstants.ACTION_STAFF);
            }
            Language lang = getLanguage();
            String message =
                    lang.getMessage(GscLangConstants.MSG_LOGIN_SUCCESFUL);
            TransitionRecord transition =
                    TransitionRecord.createInformationTransitionRecord(message,
                            urlLastPage, null);
            app.setTransition(transition);
            return new ActionControlForward(getModule(),
                    PandaPortalConstants.ACTION_INFORMATION);
        }
        populateDataModels();
        return new ViewControlForward(getModule(), getAction());
    }

    private boolean doLogin(SubmittedForm form) throws Exception {
        PandaPortalApplication app = getApp();
        Language lang = getLanguage();
        FormValidatingReporter reporter =
                new CommonFormValidatingReporter(lang, form);
        validateForm(reporter);
        if ( form.hasErrorMessage() ) {
            return false;
        }

        DafDataManager dafDm = app.getDafDataManager();
        if ( dafDm == null ) {
            throw new ServletException("Can not find DafDataManager instance!");
        }
        String loginName = form.getAttribute(FORM_FIELD_LOGIN_NAME);
        String password = form.getAttribute(FORM_FIELD_PASSWORD);
        DafUser user = dafDm.getUser(loginName);
        if ( user == null || !user.authenticate(password) ) {
            String errorMsg =
                    lang.getMessage(GscLangConstants.ERROR_LOGIN_FAILED);
            form.addErrorMessage(errorMsg);
            return false;
        }
        app.login(user);
        return true;
    }
}
