package org.ddth.eis.controller.manager;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;

import org.ddth.eis.EisConstants;
import org.ddth.eis.bo.skillinventory.SkillCategory;
import org.ddth.eis.bo.skillinventory.SkillDataManager;
import org.ddth.eis.bo.skillinventory.SkillItem;
import org.ddth.eis.controller.BaseFormController;
import org.ddth.eis.controller.IRequireAuthenticationController;
import org.ddth.eis.controller.IRequireAuthorizationController;
import org.ddth.eis.model.DafUserModel;
import org.ddth.fileupload.SubmittedForm;
import org.ddth.fileupload.impl.SubmittedFormImpl;
import org.ddth.panda.daf.DafUser;

public class SkillSearchController extends BaseFormController implements
        IRequireAuthenticationController, IRequireAuthorizationController {

    private final static String FORM_NAME = "frmManagerSkillSearch";

    private final static String VIEW_NAME = EisConstants.MODULE_MANAGER + "."
            + EisConstants.ACTION_MANAGER_SKILL_SEARCH;

    private final static String MODEL_PAGE_SKILL_CATEGORIES = "skillCategories";
    private final static String MODEL_PAGE_SEARCH_RESULT = "searchResult";

    private final static String FORM_FIELD_QUERY_SKILL = "querySkill_";
    private final static String FORM_FIELD_QUERY_SKILL_OPERATOR = "querySkillOperator_";
    private final static String FORM_FIELD_QUERY_SKILL_LEVEL = "querySkillLevel_";
    private final static String FORM_FIELD_QUERY_MONTHS_EXP_OPERATOR = "queryMonthsExpOperator_";
    private final static String FORM_FIELD_QUERY_MONTHS_EXP = "queryMonthsExp_";

    private Collection<? extends DafUser> searchResult;

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

        return form;
    }

    /**
     * {@inheritDoc}
     */
    protected void modelPageContent(Map<String, Object> modelPage) {
        super.modelPageContent(modelPage);
        SkillDataManager sdm = getBean(EisConstants.BEAN_BO_SKILL_MANAGER, SkillDataManager.class);
        Collection<? extends SkillCategory> modelSkillCategories = sdm.getAllSkillCategories();
        modelPage.put(MODEL_PAGE_SKILL_CATEGORIES, modelSkillCategories);

        Collection<DafUserModel> modelSearchResult = new ArrayList<DafUserModel>();
        if ( this.searchResult != null ) {
            for ( DafUser user : this.searchResult ) {
                modelSearchResult.add(DafUserModel.getInstance(user));
            }
        }
        modelPage.put(MODEL_PAGE_SEARCH_RESULT, modelSearchResult);
    }

    /**
     * {@inheritDoc}
     */
    public void populateSubmittedForm(SubmittedForm form, HttpServletRequest request) {
        for ( int i = 1; i < 100; i++ ) {
            String field = FORM_FIELD_QUERY_SKILL + i;
            form.setAttribute(field, request.getParameter(field));

            field = FORM_FIELD_QUERY_SKILL_OPERATOR + i;
            form.setAttribute(field, request.getParameter(field));

            field = FORM_FIELD_QUERY_SKILL_LEVEL + i;
            form.setAttribute(field, request.getParameter(field));

            field = FORM_FIELD_QUERY_MONTHS_EXP_OPERATOR + i;
            form.setAttribute(field, request.getParameter(field));

            field = FORM_FIELD_QUERY_MONTHS_EXP + i;
            form.setAttribute(field, request.getParameter(field));
        }
    }

    /**
     * {@inheritDoc}
     */
    public boolean processFormSubmission(SubmittedForm form) {
        SkillDataManager skillDm = getBean(EisConstants.BEAN_BO_SKILL_MANAGER,
                                           SkillDataManager.class);
        for ( int i = 1; i < 100; i++ ) {
            String field = FORM_FIELD_QUERY_SKILL + i;
            int skillId = form.getAttributeAsInt(field);
            SkillItem skillItem = skillDm.getSkillItem(skillId);
            if ( skillItem != null ) {
                field = FORM_FIELD_QUERY_SKILL_OPERATOR + i;
                String skillLevelOperator = form.getAttribute(field);
                field = FORM_FIELD_QUERY_SKILL_LEVEL + i;
                int skillLevel = form.getAttributeAsInt(field);

                field = FORM_FIELD_QUERY_MONTHS_EXP_OPERATOR + i;
                String monthsExpOperator = form.getAttribute(field);
                field = FORM_FIELD_QUERY_MONTHS_EXP + i;
                int monthsExp = form.getAttributeAsInt(field);

                if ( monthsExpOperator != null && monthsExpOperator.length() > 0 ) {
                    this.searchResult = skillDm.searchSkill(this.searchResult, skillItem,
                                                            skillLevelOperator, skillLevel,
                                                            monthsExpOperator, monthsExp);
                } else {
                    this.searchResult = skillDm.searchSkill(this.searchResult, skillItem,
                                                            skillLevelOperator, skillLevel);
                }
            }
        }
        return true;
    }
}
