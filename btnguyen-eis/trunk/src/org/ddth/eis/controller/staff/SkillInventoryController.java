package org.ddth.eis.controller.staff;

import java.util.Collection;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;

import org.ddth.daf.UserProfile;
import org.ddth.daf.UserProfile.Id;
import org.ddth.daf.utils.DafException;
import org.ddth.eis.EisConstants;
import org.ddth.eis.bo.skillinventory.SkillCategory;
import org.ddth.eis.bo.skillinventory.SkillDataManager;
import org.ddth.eis.bo.skillinventory.SkillInventory;
import org.ddth.eis.bo.skillinventory.SkillItem;
import org.ddth.eis.controller.BaseFormController;
import org.ddth.eis.controller.IRequireAuthenticationController;
import org.ddth.eis.controller.IRequireAuthorizationController;
import org.ddth.fileupload.SubmittedForm;
import org.ddth.fileupload.impl.SubmittedFormImpl;
import org.ddth.panda.daf.DafDataManager;
import org.ddth.panda.daf.DafUser;
import org.ddth.panda.daf.DafUserProfile;

public class SkillInventoryController extends BaseFormController implements
        IRequireAuthenticationController, IRequireAuthorizationController {

    private final static String VIEW_NAME = EisConstants.MODULE_STAFF + "."
            + EisConstants.ACTION_STAFF_SKILL_INVENTORY;

    private final static String MODEL_PAGE_SKILL_CATEGORIES = "skillCategories";
    private final static String MODEL_PAGE_MY_SKILL_INVENTORIES = "mySkillInventories";

    private final static String FORM_FIELD_SKILL_ITEM_LEVEL = "skillItem_level";
    private final static String FORM_FIELD_SKILL_ITEM_NUM_MONTHS_EXP = "skillItem_numMonthsExp";

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
        SubmittedForm form = new SubmittedFormImpl("frmSkillInventory");
        form.setAction("");
        return form;
    }

    /**
     * {@inheritDoc}
     */
    protected void modelPageContent(Map<String, Object> modelPage) {
        super.modelPageContent(modelPage);
        SkillDataManager sdm = getBean(EisConstants.BEAN_BO_SKILL_MANAGER, SkillDataManager.class);
        Collection<? extends SkillCategory> skillCategories = sdm.getAllSkillCategories();
        modelPage.put(MODEL_PAGE_SKILL_CATEGORIES, skillCategories);
        DafUser user = getCurrentUser();
        Collection<? extends SkillInventory> mySkillInventories = sdm.getSkillInventories(user);
        modelPage.put(MODEL_PAGE_MY_SKILL_INVENTORIES, mySkillInventories);
    }

    /**
     * {@inheritDoc}
     */
    public void populateSubmittedForm(SubmittedForm form, HttpServletRequest request) {
        SkillDataManager sdm = getBean(EisConstants.BEAN_BO_SKILL_MANAGER, SkillDataManager.class);
        Collection<? extends SkillCategory> skillCategories = sdm.getAllSkillCategories();
        for ( SkillCategory skillCategory : skillCategories ) {
            for ( SkillItem skillItem : skillCategory.getSkillItems() ) {
                String fieldName = FORM_FIELD_SKILL_ITEM_LEVEL + "_" + skillItem.getId();
                Object fieldValue = request.getParameter(fieldName);
                if ( fieldValue != null ) {
                    form.setAttribute(fieldName, fieldValue);
                }
                fieldName = FORM_FIELD_SKILL_ITEM_NUM_MONTHS_EXP + "_" + skillItem.getId();
                fieldValue = request.getParameter(fieldName);
                if ( fieldValue != null ) {
                    form.setAttribute(fieldName, fieldValue);
                }
            }
        }
    }

    /**
     * {@inheritDoc}
     */
    public boolean processFormSubmission(SubmittedForm form) {
        SkillDataManager sdm = getBean(EisConstants.BEAN_BO_SKILL_MANAGER, SkillDataManager.class);
        DafUser user = getCurrentUser();
        Collection<? extends SkillInventory> mySkillInventories = sdm.getSkillInventories(user);
        for ( SkillInventory skillInventory : mySkillInventories ) {
            sdm.deleteSkillInventory(skillInventory);
        }

        Collection<? extends SkillCategory> skillCategories = sdm.getAllSkillCategories();
        for ( SkillCategory skillCategory : skillCategories ) {
            for ( SkillItem skillItem : skillCategory.getSkillItems() ) {
                String fieldName = FORM_FIELD_SKILL_ITEM_LEVEL + "_" + skillItem.getId();
                int fieldValue = form.getAttributeAsInt(fieldName);
                if ( 1 <= fieldValue && fieldValue <= 9 ) {
                    SkillInventory skillInventory = new SkillInventory();
                    skillInventory.setSkillItem(skillItem);
                    skillInventory.setUser(user);
                    skillInventory.setLevel(fieldValue);
                    fieldName = FORM_FIELD_SKILL_ITEM_NUM_MONTHS_EXP + "_" + skillItem.getId();
                    fieldValue = form.getAttributeAsInt(fieldName);
                    skillInventory.setNumMonthsExp(fieldValue);
                    sdm.createSkillInventory(skillInventory);
                }
            }
        }
        DafDataManager dafDm = getBean(EisConstants.BEAN_BO_DAF_MANAGER, DafDataManager.class);
        UserProfile.Id userProfileId = new Id(user.getId(), EisConstants.APP_DOMAIN,
                EisConstants.UPK_KEY_LAST_SKILL_UPDATE_TIMESTAMP);
        try {
            DafUserProfile userProfile = dafDm.getUserProfile(userProfileId);
            if ( userProfile == null ) {
                userProfile = new DafUserProfile(userProfileId, null);
                dafDm.createUserProfile(userProfile);
            }
            userProfile.setValue(System.currentTimeMillis() / 1000);
            dafDm.updateUserProfile(userProfile);
        } catch ( DafException e ) {
            throw new RuntimeException(e);
        }
        return true;
    }
}
