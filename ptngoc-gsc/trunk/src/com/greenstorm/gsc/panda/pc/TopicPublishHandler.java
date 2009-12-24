package com.greenstorm.gsc.panda.pc;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

import org.ddth.fileupload.SubmittedForm;
import org.ddth.mls.Language;
import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.daf.DafUser;
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
import org.ddth.txbb.panda.pc.BasePcActionHandler;
import org.ddth.webtemplate.datamodel.DMList;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.TxbbLangConstants;
import com.greenstorm.gsc.TxbbPermissionConstants;
import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.Topic;
import com.greenstorm.gsc.bo.TxbbManager;
import com.greenstorm.gsc.model.BoxModel;
import com.greenstorm.gsc.model.TopicModel;

public class TopicPublishHandler extends BasePcActionHandler {

    private final static String URL_PARAM_TOPIC_ID = "id";

    private final static String MODEL_TOPIC = "topic";

    private final static String MODEL_BOX_LIST = "boxList";

    private final static String FORM_FIELD_PUBLISHED_BOXES = "publishedBoxes";

    private Topic topic;

    /**
     * {@inheritDoc}
     */
    @Override
    protected ControlForward execute() throws Exception {
        PandaPortalApplication app = getApp();
        DafUser dafUser = app.getCurrentUser();
        int dafUserId = ((Integer)dafUser.getId()).intValue();

        int topicId =
                app.getHttpRequestParams().getUrlParamAsInt(URL_PARAM_TOPIC_ID);
        TxbbManager txbbMan =
                app.getBundleManager().getService(TxbbManager.class);
        topic = txbbMan.getTopic(topicId);

        Language lang = getLanguage();

        if ( topic == null ) {
            app.addErrorMessage(lang.getMessage(
                    TxbbLangConstants.ERROR_TOPIC_NOT_FOUND, topicId));
        } else if ( topic.getMemberId() != dafUserId ) {
            topic = null;
            app.addErrorMessage(lang.getMessage(
                    TxbbLangConstants.ERROR_CAN_NOT_PUBLISH_UNOWNED_TOPIC,
                    topicId));
        } else if ( topic.isPublished() ) {
            topic = null;
            app.addErrorMessage(lang.getMessage(
                    TxbbLangConstants.ERROR_TOPIC_ALREADY_PUBLISHED, topicId));
        } else {
            ModuleDescriptor md = app.getModule(getModuleName());
            SubmittedForm form = getSubmittedForm();
            UrlCreator urlCreator = app.getUrlCreator();
            if ( form != null && doPublishTopic(form) ) {
                String message =
                        lang.getMessage(
                                TxbbLangConstants.MSG_PC_TOPIC_PUBLISH_SUCCESSFUL,
                                StringUtils.escapeHtml(topic.getTitle()));
                String action = TxbbConstants.ACTION_PC_VIEW_DRAFT_TOPICS;
                TransitionRecord transition =
                        TransitionRecord.createInformationTransitionRecord(message);
                app.addTransition(transition);
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
                form.setCancelAction(urlCreator.createUri(md.getUrlMapping(),
                        TxbbConstants.ACTION_PC_VIEW_DRAFT_TOPICS));
            }
        }
        populateDataModels();
        return new ViewControlForward(getModule(), getAction());
    }

    private boolean doPublishTopic(SubmittedForm form) throws Exception {
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
        // which boxes the topic is published to
        int[] publishedBoxIds =
                form.getMultivalueAttributeAsInts(FORM_FIELD_PUBLISHED_BOXES);
        if ( publishedBoxIds == null ) {
            publishedBoxIds = new int[0];
        }
        Collection<Box> publishedBoxes = new ArrayList<Box>();
        for ( int boxId : publishedBoxIds ) {
            Box b = txbbMan.getBox(boxId);
            if ( b != null ) {
                if ( app.hasPermission(
                        TxbbPermissionConstants.PERMISSION_PUBLISH_TOPIC, b) ) {
                    publishedBoxes.add(b);
                } else {
                    app.addErrorMessage(lang.getMessage(
                            TxbbLangConstants.ERROR_NO_PERMISSION_PUBLISH_TOPIC_TO_BOX,
                            b.getId()));
                }
            }
        }
        if ( app.hasErrorMessage() ) {
            return false;
        }

        topic.setPublished(true);
        txbbMan.publishTopic(topic, publishedBoxes);
        return true;
    }

    /**
     * {@inheritDoc}
     */
    protected void modelPageContentCustom(DMMap pageContent) throws Exception {
        super.modelPageContentCustom(pageContent);
        PandaPortalApplication app = getApp();
        DafUser currentUser = app.getCurrentUser();

        TxbbManager fm = app.getBundleManager().getService(TxbbManager.class);
        DMList modelBoxList = new DMList(MODEL_BOX_LIST);
        Box[] boxTree = fm.getBoxTree();
        for ( Box box : boxTree ) {
            if ( box.canView(currentUser) )
                modelBoxList.addChild(BoxModel.getInstance(box));
        }
        pageContent.addChild(modelBoxList);

        if ( topic != null ) {
            pageContent.addChild(MODEL_TOPIC, TopicModel.getInstance(topic));
        }
    }
}
