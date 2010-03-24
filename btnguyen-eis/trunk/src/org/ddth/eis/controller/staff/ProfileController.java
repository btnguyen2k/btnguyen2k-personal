package org.ddth.eis.controller.staff;

import java.util.Map;

import javax.servlet.http.HttpServletRequest;

import org.ddth.eis.EisConstants;
import org.ddth.eis.controller.BaseFormController;
import org.ddth.eis.controller.IRequireAuthenticationController;
import org.ddth.eis.controller.IRequireAuthorizationController;
import org.ddth.fileupload.SubmittedForm;
import org.ddth.fileupload.impl.SubmittedFormImpl;
import org.ddth.panda.daf.DafDataManager;
import org.ddth.panda.daf.DafUser;

public class ProfileController extends BaseFormController implements
        IRequireAuthenticationController, IRequireAuthorizationController {

    private final static String FORM_NAME = "frmStaffProfile";

    private final static String VIEW_NAME = EisConstants.MODULE_STAFF + "."
            + EisConstants.ACTION_STAFF_PROFILE;

    private final static String FORM_FIELD_PROFILE_TITLE = "profileTitle";
    private final static String FORM_FIELD_PROFILE_FIRSTNAME = "profileFirstName";
    private final static String FORM_FIELD_PROFILE_MIDDLENAMES = "profileMiddleNames";
    private final static String FORM_FIELD_PROFILE_LASTNAME = "profileLastName";
    private final static String FORM_FIELD_PROFILE_DOB_DAY = "profileDobDay";
    private final static String FORM_FIELD_PROFILE_DOB_MONTH = "profileDobMonth";
    private final static String FORM_FIELD_PROFILE_DOB_YEAR = "profileDobYear";

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

        DafUser user = getCurrentUser();
        form.setAttribute(FORM_FIELD_PROFILE_TITLE, user.getTitle());
        form.setAttribute(FORM_FIELD_PROFILE_FIRSTNAME, user.getFirstName());
        form.setAttribute(FORM_FIELD_PROFILE_LASTNAME, user.getLastName());
        form.setAttribute(FORM_FIELD_PROFILE_MIDDLENAMES, user.getMiddleName());
        form.setAttribute(FORM_FIELD_PROFILE_DOB_DAY, user.getDobDay());
        form.setAttribute(FORM_FIELD_PROFILE_DOB_MONTH, user.getDobMonth());
        form.setAttribute(FORM_FIELD_PROFILE_DOB_YEAR, user.getDobYear());

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
        form.setAttribute(FORM_FIELD_PROFILE_TITLE, request.getParameter(FORM_FIELD_PROFILE_TITLE));
        form.setAttribute(FORM_FIELD_PROFILE_FIRSTNAME, request
                .getParameter(FORM_FIELD_PROFILE_FIRSTNAME));
        form.setAttribute(FORM_FIELD_PROFILE_MIDDLENAMES, request
                .getParameter(FORM_FIELD_PROFILE_MIDDLENAMES));
        form.setAttribute(FORM_FIELD_PROFILE_LASTNAME, request
                .getParameter(FORM_FIELD_PROFILE_LASTNAME));
        form.setAttribute(FORM_FIELD_PROFILE_DOB_DAY, request
                .getParameter(FORM_FIELD_PROFILE_DOB_DAY));
        form.setAttribute(FORM_FIELD_PROFILE_DOB_MONTH, request
                .getParameter(FORM_FIELD_PROFILE_DOB_MONTH));
        form.setAttribute(FORM_FIELD_PROFILE_DOB_YEAR, request
                .getParameter(FORM_FIELD_PROFILE_DOB_YEAR));
    }

    /**
     * {@inheritDoc}
     */
    public boolean processFormSubmission(SubmittedForm form) {
        DafUser user = getCurrentUser();
        user.setTitle(form.getAttribute(FORM_FIELD_PROFILE_TITLE));
        user.setFirstName(form.getAttribute(FORM_FIELD_PROFILE_FIRSTNAME));
        user.setMiddleName(form.getAttribute(FORM_FIELD_PROFILE_MIDDLENAMES));
        user.setLastName(form.getAttribute(FORM_FIELD_PROFILE_LASTNAME));
        int dobDay = form.getAttributeAsInt(FORM_FIELD_PROFILE_DOB_DAY);
        int dobMonth = form.getAttributeAsInt(FORM_FIELD_PROFILE_DOB_MONTH);
        int dobYear = form.getAttributeAsInt(FORM_FIELD_PROFILE_DOB_YEAR);
        user.setDob(dobDay, dobMonth, dobYear);
        user.setLastUpdateTimestamp((int) (System.currentTimeMillis() / 1000));
        DafDataManager dafDm = getBean(EisConstants.BEAN_BO_DAF_MANAGER, DafDataManager.class);
        dafDm.updateUser(user);
        return true;
    }
}
