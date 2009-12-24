package com.greenstorm.gsc.panda.admin;

import java.util.HashMap;
import java.util.Map;

import org.ddth.fileupload.SubmittedForm;
import org.ddth.mls.Language;
import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.PandaPortalConstants;
import org.ddth.panda.portal.module.ModuleDescriptor;
import org.ddth.panda.portal.utils.CommonFormValidatingReporter;
import org.ddth.panda.portal.utils.TransitionRecord;
import org.ddth.panda.utils.StringUtils;
import org.ddth.panda.web.UrlCreator;
import org.ddth.panda.web.form.FormValidatingReporter;
import org.ddth.panda.web.impl.controlforward.UrlRedirectControlForward;
import org.ddth.txbb.panda.admin.BaseAdminBoxHandler;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.TxbbLangConstants;
import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.TxbbManager;
import com.greenstorm.gsc.model.BoxModel;

public class AdminBoxDeleteHandler extends BaseAdminBoxHandler {

    private final static String URL_PARAM_BOX_ID = "id";

    private final static String MODEL_BOX = "box";

    private Box box;

    /**
     * {@inheritDoc}
     */
    @Override
    protected ControlForward execute() throws Exception {
        PandaPortalApplication app = getApp();
        int boxId =
                app.getHttpRequestParams().getUrlParamAsInt(URL_PARAM_BOX_ID);
        TxbbManager txbbMan =
                app.getBundleManager().getService(TxbbManager.class);
        box = txbbMan.getBox(boxId);

        Language lang = getLanguage();

        if ( box == null ) {
            app.addErrorMessage(lang.getMessage(
                    TxbbLangConstants.ERROR_BOX_NOT_FOUND, boxId));
        } else if ( box.hasChildren() ) {
            app.addErrorMessage(lang.getMessage(
                    TxbbLangConstants.ERROR_CAN_NOT_DELETE_BOX_HAS_CHILD,
                    StringUtils.escapeHtml(box.getTitle())));
            box = null;
        } else {
            app.addWarningMessage(lang.getMessage(
                    TxbbLangConstants.MSG_BOX_DELETE_CONFIRMATION,
                    StringUtils.escapeHtml(box.getTitle())));

            SubmittedForm form = getSubmittedForm();
            UrlCreator urlCreator = app.getUrlCreator();
            if ( form != null && doDeleteBox(form) ) {
                String message =
                        lang.getMessage(
                                TxbbLangConstants.MSG_BOX_DELETE_SUCCESSFUL,
                                StringUtils.escapeHtml(box.getTitle()));
                String action = TxbbConstants.ACTION_ADMIN_LIST_BOXES;
                TransitionRecord transition =
                        TransitionRecord.createInformationTransitionRecord(message);
                app.addTransition(transition);
                ModuleDescriptor md = app.getModule(getModuleName());
                Map<String, String> params = new HashMap<String, String>();
                params.put(PandaPortalConstants.URL_PARAM_TRANSITION_ID,
                        transition.getId());
                String url =
                        urlCreator.createUri(md.getUrlMapping(), action, null,
                                params);
                return new UrlRedirectControlForward(url);
            }
            if ( form == null ) {
                form = initializeAssociatedForm();
            }
            if ( form != null ) {
                ModuleDescriptor md = app.getModule(getModuleName());
                form.setCancelAction(urlCreator.createUri(md.getUrlMapping(),
                        TxbbConstants.ACTION_ADMIN_LIST_BOXES));
            }
        }
        populateDataModels();
        return new ViewControlForward(getModule(), getAction());
    }

    private boolean doDeleteBox(SubmittedForm form) throws Exception {
        PandaPortalApplication app = getApp();
        Language lang = getLanguage();
        FormValidatingReporter reporter =
                new CommonFormValidatingReporter(lang, form);
        validateForm(reporter);
        if ( form.hasErrorMessage() ) {
            return false;
        }
        TxbbManager txbbMan =
                app.getBundleManager().getService(TxbbManager.class);
        txbbMan.deleteBox(box);
        return true;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected void modelPageContentCustom(DMMap pageContent) throws Exception {
        super.modelPageContentCustom(pageContent);

        if ( box != null ) {
            pageContent.addChild(MODEL_BOX, BoxModel.getInstance(box));
        }
    }
}
