package org.ddth.eis.controller;

import java.util.HashMap;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;

import org.ddth.fileupload.SubmittedForm;
import org.springframework.web.servlet.ModelAndView;

public abstract class BaseFormController extends BaseController implements IFormController {

    protected final static String MODEL_PAGE_TRANSITION_URL     = "transitionUrl";
    protected final static String MODEL_PAGE_TRANSITION_MESSAGE = "transitionMessage";

    private final static String   MODEL_PAGE_FORM               = "form";

    private SubmittedForm         form;

    /**
     * {@inheritDoc}
     */
    public boolean isFormSubmission(HttpServletRequest request) {
        return request.getMethod().equalsIgnoreCase(METHOD_POST);
    }

    /**
     * {@inheritDoc}
     */
    public boolean validateSubmittedForm(SubmittedForm form) {
        return true;
    }

    /**
     * Gets the ModelAndView for successful form submission.
     * 
     * @return ModelAndView
     */
    @SuppressWarnings("unchecked")
    protected ModelAndView getFormSubmissionSuccessfulModelAndView() {
        String viewName = getFormSubmissionSuccessfulViewName();
        if ( viewName == null ) {
            return null;
        }
        ModelAndView mav = new ModelAndView(viewName);
        modelController(mav);

        Map<String, Object> model = mav.getModel();
        Object modelPageObj = model.get(MODEL_PAGE);
        if ( modelPageObj == null ) {
            modelPageObj = new HashMap<String, Object>();
            model.put(MODEL_PAGE, modelPageObj);
        }
        Map<String, Object> modelPage = (Map<String, Object>) modelPageObj;
        String transitionUrl = getTransitionUrl();
        if ( transitionUrl != null ) {
            modelPage.put(MODEL_PAGE_TRANSITION_URL, transitionUrl);
            modelPage.put(MODEL_PAGE_TRANSITION_MESSAGE, getTransitionMessage());
        }

        return mav;
    }

    /**
     * Gets transition url. Sub-class overrides this method to provide its own transition url.
     * 
     * @return String
     */
    protected String getTransitionUrl() {
        return null;
    }

    /**
     * Gets transition message. Sub-class overrides this method to provide its own transition
     * message.
     * 
     * @return String
     */
    protected String getTransitionMessage() {
        return null;
    }

    /**
     * Gets name of the view for successful form submission.
     * 
     * @return String
     */
    protected abstract String getFormSubmissionSuccessfulViewName();

    /**
     * Models the page's content. Subclass overrides this methods to model its own page content.
     * 
     * @param modelPage
     *            Map<String, Object>
     */
    @Override
    protected void modelPageContent(Map<String, Object> modelPage) {
        super.modelPageContent(modelPage);
        modelPage.put(MODEL_PAGE_FORM, this.form);
    }

    /**
     * {@inheritDoc}
     */
    protected ModelAndView execute() {
        this.form = initAssociatedForm();
        if ( isFormSubmission(getRequest()) ) {
            populateSubmittedForm(form, getRequest());
            if ( validateSubmittedForm(form) && processFormSubmission(form) ) {
                ModelAndView mav = getFormSubmissionSuccessfulModelAndView();
                if ( mav != null ) {
                    return mav;
                }
            }
        }
        return null;
    }
}
