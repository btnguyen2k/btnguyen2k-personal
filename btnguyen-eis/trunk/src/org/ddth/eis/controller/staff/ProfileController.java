package org.ddth.eis.controller.staff;

import java.util.Map;

import javax.servlet.http.HttpServletRequest;

import org.ddth.eis.EisConstants;
import org.ddth.eis.controller.BaseFormController;
import org.ddth.eis.controller.IRequireAuthenticationController;
import org.ddth.eis.controller.IRequireAuthorizationController;
import org.ddth.fileupload.SubmittedForm;
import org.ddth.fileupload.impl.SubmittedFormImpl;
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
        // SkillDataManager sdm = getBean(EisConstants.BEAN_BO_SKILL_MANAGER,
        // SkillDataManager.class);
        // Collection<? extends SkillCategory> skillCategories = sdm.getAllSkillCategories();
        // for ( SkillCategory skillCategory : skillCategories ) {
        // for ( SkillItem skillItem : skillCategory.getSkillItems() ) {
        // String fieldName = FORM_FIELD_SKILL_ITEM_LEVEL + "_" + skillItem.getId();
        // Object fieldValue = request.getParameter(fieldName);
        // if ( fieldValue != null ) {
        // form.setAttribute(fieldName, fieldValue);
        // }
        // fieldName = FORM_FIELD_SKILL_ITEM_NUM_MONTHS_EXP + "_" + skillItem.getId();
        // fieldValue = request.getParameter(fieldName);
        // if ( fieldValue != null ) {
        // form.setAttribute(fieldName, fieldValue);
        // }
        // }
        // }
    }

    /**
     * {@inheritDoc}
     */
    public boolean processFormSubmission(SubmittedForm form) {
        return false;
        // SkillDataManager sdm = getBean(EisConstants.BEAN_BO_SKILL_MANAGER,
        // SkillDataManager.class);
        // DafUser user = getCurrentUser();
        // Collection<? extends SkillInventory> mySkillInventories = sdm.getSkillInventories(user);
        // for ( SkillInventory skillInventory : mySkillInventories ) {
        // sdm.deleteSkillInventory(skillInventory);
        // }
        //
        // Collection<? extends SkillCategory> skillCategories = sdm.getAllSkillCategories();
        // for ( SkillCategory skillCategory : skillCategories ) {
        // for ( SkillItem skillItem : skillCategory.getSkillItems() ) {
        // String fieldName = FORM_FIELD_SKILL_ITEM_LEVEL + "_" + skillItem.getId();
        // int fieldValue = form.getAttributeAsInt(fieldName);
        // if ( 1 <= fieldValue && fieldValue <= 9 ) {
        // SkillInventory skillInventory = new SkillInventory();
        // skillInventory.setSkillItem(skillItem);
        // skillInventory.setUser(user);
        // skillInventory.setLevel(fieldValue);
        // fieldName = FORM_FIELD_SKILL_ITEM_NUM_MONTHS_EXP + "_" + skillItem.getId();
        // fieldValue = form.getAttributeAsInt(fieldName);
        // skillInventory.setNumMonthsExp(fieldValue);
        // sdm.createSkillInventory(skillInventory);
        // }
        // }
        // }
        // DafDataManager dafDm = getBean(EisConstants.BEAN_BO_DAF_MANAGER, DafDataManager.class);
        // UserProfile.Id userProfileId = new Id(user.getId(), EisConstants.APP_DOMAIN,
        // EisConstants.UPK_KEY_LAST_SKILL_UPDATE_TIMESTAMP);
        // try {
        // DafUserProfile userProfile = dafDm.getUserProfile(userProfileId);
        // if ( userProfile == null ) {
        // userProfile = new DafUserProfile(userProfileId, null);
        // dafDm.createUserProfile(userProfile);
        // }
        // userProfile.setValue(System.currentTimeMillis() / 1000);
        // dafDm.updateUserProfile(userProfile);
        // } catch ( DafException e ) {
        // throw new RuntimeException(e);
        // }
        // return true;
    }
}
