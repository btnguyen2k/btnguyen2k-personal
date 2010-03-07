package org.ddth.eis.controller;

import javax.servlet.http.HttpServletRequest;

import org.ddth.fileupload.SubmittedForm;

public interface IFormController {

    /**
     * Checks is the request is a form submission.
     * 
     * @param request
     *            HttpServletRequest
     * @return boolean
     */
    public boolean isFormSubmission(HttpServletRequest request);

    /**
     * Initializes the associated form.
     * 
     * @return SubmittedForm
     */
    public SubmittedForm initAssociatedForm();

    /**
     * Populates the submitted form.
     * 
     * @param form
     *            SubmittedForm
     * @param request
     *            HttpServletRequest
     */
    public void populateSubmittedForm(SubmittedForm form, HttpServletRequest request);

    /**
     * Processes the submitted form.
     * 
     * @param form
     *            SubmittedForm
     * @return boolean true if the process completes successfully
     */
    public boolean processFormSubmission(SubmittedForm form);

    /**
     * Validates the submitted form.
     * 
     * @param form
     *            SubmittedForm
     * @return boolean false if the validation fails.
     */
    public boolean validateSubmittedForm(SubmittedForm form);
}
