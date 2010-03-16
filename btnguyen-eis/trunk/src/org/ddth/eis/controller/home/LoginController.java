package org.ddth.eis.controller.home;

import java.util.Calendar;
import java.util.TimeZone;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.ddth.daf.utils.DafException;
import org.ddth.eis.EisConstants;
import org.ddth.eis.EisLanguageConstants;
import org.ddth.eis.controller.BaseFormController;
import org.ddth.fileupload.SubmittedForm;
import org.ddth.fileupload.impl.SubmittedFormImpl;
import org.ddth.mls.Language;
import org.ddth.panda.UrlCreator;
import org.ddth.panda.daf.DafDataManager;
import org.ddth.panda.daf.DafUser;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.ProviderNotFoundException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;

/**
 * Controller for action: login
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class LoginController extends BaseFormController {

    private final Log LOGGER = LogFactory.getLog(LoginController.class);

    private final static String VIEW_NAME = EisConstants.MODULE_HOME + "."
            + EisConstants.ACTION_HOME_LOGIN;

    private final static String VIEW_LOGIN_DONE = EisConstants.MODULE_HOME + "."
            + EisConstants.ACTION_HOME_LOGIN + ".successful";

    private final static String FORM_FIELD_LOGIN_NAME = "loginName";
    private final static String FORM_FIELD_PASSWORD = "password";

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
    @Override
    protected String getFormSubmissionSuccessfulViewName() {
        return VIEW_LOGIN_DONE;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected String getTransitionUrl() {
        UrlCreator urlCreator = getUrlCreator();
        return urlCreator.createUri(EisConstants.MODULE_HOME, EisConstants.ACTION_HOME_INDEX);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected String getTransitionMessage() {
        Language lang = getLanguage();
        return lang.getMessage(EisLanguageConstants.MESSAGE_LOGIN_SUCCESSFUL);
    }

    /**
     * {@inheritDoc}
     */
    public SubmittedForm initAssociatedForm() {
        SubmittedForm form = new SubmittedFormImpl("frmLogin");
        form.setAction("");
        return form;
    }

    /**
     * {@inheritDoc}
     */
    public void populateSubmittedForm(SubmittedForm form, HttpServletRequest request) {
        String loginName = request.getParameter(FORM_FIELD_LOGIN_NAME);
        String password = request.getParameter(FORM_FIELD_PASSWORD);
        form.setAttribute(FORM_FIELD_LOGIN_NAME, loginName);
        form.setAttribute(FORM_FIELD_PASSWORD, password);
    }

    /**
     * {@inheritDoc}
     */
    public boolean processFormSubmission(SubmittedForm form) {
        String username = form.getAttribute(FORM_FIELD_LOGIN_NAME);
        String password = form.getAttribute(FORM_FIELD_PASSWORD);
        Authentication authentication = new UsernamePasswordAuthenticationToken(username, password);
        AuthenticationManager am = getBean(EisConstants.BEAN_AUTHENTICATION_MANAGER,
                                           AuthenticationManager.class);
        try {
            authentication = am.authenticate(authentication);
        } catch ( ProviderNotFoundException e ) {
            authentication = null;
        }
        if ( authentication != null ) {
            // create daf user account if not exists
            DafDataManager dafManager = getBean(EisConstants.BEAN_BO_DAF_MANAGER,
                                                DafDataManager.class);
            try {
                DafUser user = dafManager.getUser(username);
                if ( user == null ) {
                    user = new DafUser();
                    user.setLoginName(username);
                    user.setPassword("");
                    user.setEmail("");
                    int now = (int) (System.currentTimeMillis() / 1000);
                    user.setRegisterTimestamp(now);
                    user.setLastUpdateTimestamp(now);
                    TimeZone timeZone = Calendar.getInstance().getTimeZone();
                    user.setTimeZoneId(timeZone.getID());
                    user = dafManager.createUser(user);
                    user = dafManager.assignUserToGroup(user.getId(), EisConstants.GROUP_ID_STAFF);
                }
            } catch ( DafException e ) {
                LOGGER.fatal(e.getMessage(), e);
                throw new RuntimeException(e);
            }

            HttpSession session = getSession();
            session.setAttribute(EisConstants.SESSION_CURRENT_USERNAME, username);
        } else {
            Language lang = getLanguage();
            form.addErrorMessage(lang.getMessage(EisLanguageConstants.ERROR_LOGIN_FAILED));
        }
        return authentication != null;
    }
}
