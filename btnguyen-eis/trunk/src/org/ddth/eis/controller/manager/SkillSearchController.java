package org.ddth.eis.controller.manager;

import java.util.Collection;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;

import org.ddth.eis.EisConstants;
import org.ddth.eis.bo.skillinventory.SkillCategory;
import org.ddth.eis.bo.skillinventory.SkillDataManager;
import org.ddth.eis.controller.BaseFormController;
import org.ddth.eis.controller.IRequireAuthenticationController;
import org.ddth.eis.controller.IRequireAuthorizationController;
import org.ddth.fileupload.SubmittedForm;
import org.ddth.fileupload.impl.SubmittedFormImpl;

public class SkillSearchController extends BaseFormController implements
        IRequireAuthenticationController, IRequireAuthorizationController {

    private final static String FORM_NAME = "frmManagerSkillSearch";

    private final static String VIEW_NAME = EisConstants.MODULE_MANAGER + "."
            + EisConstants.ACTION_MANAGER_SKILL_SEARCH;

    private final static String MODEL_PAGE_SKILL_CATEGORIES = "skillCategories";

    private final static String FORM_FIELD_QUERY_SKILL = "querySkill_";
    private final static String FORM_FIELD_QUERY_SKILL_OPERATOR = "querySkillOperator_";
    private final static String FORM_FIELD_QUERY_SKILL_LEVEL = "querySkillLevel_";
    private final static String FORM_FIELD_QUERY_MONTHS_EXP_OPERATOR = "queryMonthsExpOperator_";
    private final static String FORM_FIELD_QUERY_MONTHS_EXP = "queryMonthsExp_";

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
        Collection<? extends SkillCategory> skillCategories = sdm.getAllSkillCategories();
        modelPage.put(MODEL_PAGE_SKILL_CATEGORIES, skillCategories);
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
        String query = null;
        for ( int i = 1; i < 100; i++ ) {
            String field = FORM_FIELD_QUERY_SKILL + i;
            int skillId = form.getAttributeAsInt(field);
            if ( skillId > 0 ) {
                String temp = "SKILL_" + i;
                field = FORM_FIELD_QUERY_SKILL_OPERATOR + i;
                temp += " " + form.getAttribute(field) + " ";
                field = FORM_FIELD_QUERY_SKILL_LEVEL + i;
                temp += form.getAttributeAsInt(field);
                if ( query == null ) {
                    query = temp;
                } else {
                    query = query + " AND (" + temp + ")";
                }
            }
        }
        return true;
    }
}
