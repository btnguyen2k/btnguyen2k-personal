package org.ddth.eis.controller.home;

import java.util.HashMap;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;

import org.ddth.eis.EisConstants;
import org.ddth.eis.controller.BaseFormController;
import org.ddth.fileupload.SubmittedForm;
import org.ddth.fileupload.impl.SubmittedFormImpl;
import org.ddth.panda.UrlCreator;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.web.servlet.ModelAndView;

/**
 * Controller for action: login
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class LoginController extends BaseFormController {

    private final static String VIEW_NAME             = EisConstants.MODULE_HOME + "."
                                                              + EisConstants.ACTION_LOGIN;

    private final static String VIEW_LOGIN_DONE       = EisConstants.MODULE_HOME + "."
                                                              + EisConstants.ACTION_LOGIN
                                                              + ".successful";

    private final static String FORM_FIELD_LOGIN_NAME = "loginName";
    private final static String FORM_FIELD_PASSWORD   = "password";

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
    protected ModelAndView getFormSubmissionSuccessfulModelAndView() {
        Map<String, String> modelPage = new HashMap<String, String>();
        UrlCreator urlCreator = getUrlCreator();
        String transitUrl = urlCreator.createUri(EisConstants.MODULE_HOME,
                                                 EisConstants.ACTION_INDEX);
        modelPage.put(MODEL_PAGE_TRANSIT_URL, transitUrl);
        ModelAndView mav = new ModelAndView(VIEW_LOGIN_DONE, MODEL_PAGE, modelPage);
        return mav;
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
        return am.authenticate(authentication) != null;
    }
}
