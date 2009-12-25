package com.greenstorm.gsc.panda.admin;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;

import org.ddth.fileupload.SubmittedForm;
import org.ddth.mls.Language;
import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.daf.DafDataManager;
import org.ddth.panda.core.daf.DafGroup;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.PandaPortalConstants;
import org.ddth.panda.portal.model.DafGroupModel;
import org.ddth.panda.portal.module.ModuleDescriptor;
import org.ddth.panda.portal.utils.CommonFormValidatingReporter;
import org.ddth.panda.portal.utils.TransitionRecord;
import org.ddth.panda.utils.StringUtils;
import org.ddth.panda.web.UrlCreator;
import org.ddth.panda.web.form.FormValidatingReporter;
import org.ddth.panda.web.impl.controlforward.UrlRedirectControlForward;
import org.ddth.txbb.panda.admin.BaseAdminBoxHandler;
import org.ddth.webtemplate.datamodel.DMList;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.GscConstants;
import com.greenstorm.gsc.GscLangConstants;
import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.BoxPermissionGroupPublishTopic;
import com.greenstorm.gsc.bo.BoxPermissionGroupViewTopic;
import com.greenstorm.gsc.bo.GscManager;
import com.greenstorm.gsc.model.CardModel;

public class AdminBoxEditHandler extends BaseAdminBoxHandler {

    private final static String URL_PARAM_BOX_ID = "id";

    private final static String MODEL_BOX = "box";

    private Box box;

    private final static String MODEL_BOX_LIST = "boxList";

    private final static String MODEL_USER_GROUPS = "userGroups";

    private final static String FORM_FIELD_PARENT_ID = "parentId";

    private final static String FORM_FIELD_TITLE = "title";

    private final static String FORM_FIELD_OUTER_DESCRIPTION =
            "outerDescription";

    private final static String FORM_FIELD_INNER_DESCRIPTION =
            "innerDescription";

    private final static String FORM_FIELD_PERMISSION_PREFIX = "group";

    private final static String FORM_FIELD_PERMISSION_SUFFIX_VIEW_TOPIC =
            "_viewTopic";

    private final static String FORM_FIELD_PERMISSION_SUFFIX_PUBLISH_TOPIC =
            "_publishTopic";

    /**
     * {@inheritDoc}
     */
    @Override
    protected ControlForward execute() throws Exception {
        PandaPortalApplication app = getApp();
        int boxId =
                app.getHttpRequestParams().getUrlParamAsInt(URL_PARAM_BOX_ID);
        GscManager txbbMan =
                app.getBundleManager().getService(GscManager.class);
        box = txbbMan.getBox(boxId);

        Language lang = getLanguage();

        if ( box == null ) {
            app.addErrorMessage(lang.getMessage(
                    GscLangConstants.ERROR_BOX_NOT_FOUND, boxId));
        } else {
            SubmittedForm form = getSubmittedForm();
            UrlCreator urlCreator = app.getUrlCreator();
            if ( form != null && doEditBox(form) ) {
                String message =
                        lang.getMessage(
                                GscLangConstants.MSG_BOX_EDIT_SUCCESSFUL,
                                StringUtils.escapeHtml(box.getTitle()));
                String action = GscConstants.ACTION_ADMIN_LIST_BOXES;
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
                form.setAttribute(FORM_FIELD_INNER_DESCRIPTION,
                        box.getInnerDescription());
                form.setAttribute(FORM_FIELD_OUTER_DESCRIPTION,
                        box.getOuterDescription());
                form.setAttribute(FORM_FIELD_PARENT_ID,
                        box.getParentId() != null
                                ? box.getParentId() : 0);
                form.setAttribute(FORM_FIELD_TITLE, box.getTitle());

                Set<BoxPermissionGroupViewTopic> permissionViewTopic =
                        box.getPermissionViewTopic();
                for ( BoxPermissionGroupViewTopic bpgvp : permissionViewTopic ) {
                    int groupId = bpgvp.getGroupId();
                    String key =
                            FORM_FIELD_PERMISSION_PREFIX + groupId
                                    + FORM_FIELD_PERMISSION_SUFFIX_VIEW_TOPIC;
                    form.setAttribute(key, true);
                }

                Set<BoxPermissionGroupPublishTopic> permissionPublishTopic =
                        box.getPermissionPublishTopic();
                for ( BoxPermissionGroupPublishTopic bppvp : permissionPublishTopic ) {
                    int groupId = bppvp.getGroupId();
                    String key =
                            FORM_FIELD_PERMISSION_PREFIX
                                    + groupId
                                    + FORM_FIELD_PERMISSION_SUFFIX_PUBLISH_TOPIC;
                    form.setAttribute(key, true);
                }
            }
            if ( form != null ) {
                ModuleDescriptor md = app.getModule(getModuleName());
                form.setCancelAction(urlCreator.createUri(md.getUrlMapping(),
                        GscConstants.ACTION_ADMIN_LIST_BOXES));
            }
        }
        populateDataModels();
        return new ViewControlForward(getModule(), getAction());
    }

    private boolean doEditBox(SubmittedForm form) throws Exception {
        PandaPortalApplication app = getApp();
        Language lang = getLanguage();
        FormValidatingReporter reporter =
                new CommonFormValidatingReporter(lang, form);
        validateForm(reporter);
        if ( form.hasErrorMessage() ) {
            return false;
        }

        int parentId = form.getAttributeAsInt(FORM_FIELD_PARENT_ID);
        String title = form.getAttribute(FORM_FIELD_TITLE);
        String outerDesc = form.getAttribute(FORM_FIELD_OUTER_DESCRIPTION);
        String innerDesc = form.getAttribute(FORM_FIELD_INNER_DESCRIPTION);

        DafDataManager dafDm = app.getDafDataManager();
        Collection<? extends DafGroup> groups = dafDm.getAllGroups();
        Set<BoxPermissionGroupViewTopic> permissionViewTopic =
                box.getPermissionViewTopic();
        Set<BoxPermissionGroupPublishTopic> permissionPublishTopic =
                box.getPermissionPublishTopic();
        permissionViewTopic.clear();
        permissionPublishTopic.clear();
        for ( DafGroup group : groups ) {
            String key = FORM_FIELD_PERMISSION_PREFIX + group.getId();
            if ( form.getAttributeAsBoolean(key
                    + FORM_FIELD_PERMISSION_SUFFIX_PUBLISH_TOPIC) ) {
                BoxPermissionGroupPublishTopic p =
                        new BoxPermissionGroupPublishTopic();
                p.setGroupId(group.getId());
                p.setBoxId(box.getId());
                permissionPublishTopic.add(p);
            }
            if ( form.getAttributeAsBoolean(key
                    + FORM_FIELD_PERMISSION_SUFFIX_VIEW_TOPIC) ) {
                BoxPermissionGroupViewTopic p =
                        new BoxPermissionGroupViewTopic();
                p.setGroupId(group.getId());
                p.setBoxId(box.getId());
                permissionViewTopic.add(p);
            }
        }

        // Box box = new Box();
        box.setParentId(parentId > 0
                ? parentId : null);
        box.setTitle(title);
        box.setInnerDescription(innerDesc);
        box.setOuterDescription(outerDesc);
        // box.setPermissionPublishTopic(permissionPublishTopic);
        // box.setPermissionViewTopic(permissionViewTopic);

        GscManager txbbMan =
                app.getBundleManager().getService(GscManager.class);
        txbbMan.updateBox(box);
        return true;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected void modelPageContentCustom(DMMap pageContent) throws Exception {
        super.modelPageContentCustom(pageContent);
        PandaPortalApplication app = getApp();

        if ( box != null ) {
            pageContent.addChild(MODEL_BOX, CardModel.getInstance(box));
        }

        GscManager fm = app.getBundleManager().getService(GscManager.class);
        DMList modelBoxList = new DMList(MODEL_BOX_LIST);
        Box[] boxTree = fm.getBoxTree();
        for ( Box box : boxTree ) {
            if ( !box.getId().equals(this.box.getId()) ) {
                // do not include the current box from the list to avoid
                // circular loop
                modelBoxList.addChild(CardModel.getInstance(box));
            }
        }
        pageContent.addChild(modelBoxList);

        DafDataManager dafDm = app.getDafDataManager();
        Collection<? extends DafGroup> groups = dafDm.getAllGroups();
        DMList modelUserGroups = new DMList(MODEL_USER_GROUPS);
        for ( DafGroup group : groups ) {
            modelUserGroups.addChild(new DafGroupModel(group));
        }
        pageContent.addChild(modelUserGroups);
    }
}
