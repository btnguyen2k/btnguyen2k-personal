package org.ddth.eis.controller;

import java.util.Map;

import javax.servlet.http.HttpServletRequest;

import org.ddth.fileupload.SubmittedForm;
import org.springframework.web.servlet.ModelAndView;

public abstract class BaseFormController extends BaseController implements IFormController {

    private final static String MODEL_PAGE_FORM = "form";

    private SubmittedForm       form;

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
    protected abstract ModelAndView getFormSubmissionSuccessfulModelAndView();

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
