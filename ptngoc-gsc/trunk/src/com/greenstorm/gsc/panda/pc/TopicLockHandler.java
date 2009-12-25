package com.greenstorm.gsc.panda.pc;

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
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.GscConstants;
import com.greenstorm.gsc.GscLangConstants;
import com.greenstorm.gsc.bo.Topic;
import com.greenstorm.gsc.bo.GscManager;
import com.greenstorm.gsc.model.TopicModel;

public class TopicLockHandler extends BasePcActionHandler {

    private final static String URL_PARAM_TOPIC_ID = "id";

    private final static String MODEL_TOPIC = "topic";

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
        GscManager txbbMan =
                app.getBundleManager().getService(GscManager.class);
        topic = txbbMan.getTopic(topicId);

        Language lang = getLanguage();

        if ( topic == null ) {
            app.addErrorMessage(lang.getMessage(
                    GscLangConstants.ERROR_TOPIC_NOT_FOUND, topicId));
        } else if ( topic.getMemberId() != dafUserId ) {
            topic = null;
            app.addErrorMessage(lang.getMessage(
                    GscLangConstants.ERROR_CAN_NOT_LOCK_UNOWNED_TOPIC, topicId));
        } else if ( !topic.isPublished() ) {
            topic = null;
            app.addErrorMessage(lang.getMessage(
                    GscLangConstants.ERROR_TOPIC_NOT_PUBLISHED, topicId));
        } else {
            ModuleDescriptor md = app.getModule(getModuleName());
            SubmittedForm form = getSubmittedForm();
            UrlCreator urlCreator = app.getUrlCreator();
            if ( form != null && doUnpublishTopic(form) ) {
                String message =
                        lang.getMessage(
                                GscLangConstants.MSG_PC_TOPIC_LOCK_SUCCESSFUL,
                                StringUtils.escapeHtml(topic.getTitle()));
                String action = GscConstants.ACTION_PC_VIEW_PUBLISHED_TOPICS;
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
                        GscConstants.ACTION_PC_VIEW_PUBLISHED_TOPICS));
            }
        }
        populateDataModels();
        return new ViewControlForward(getModule(), getAction());
    }

    private boolean doUnpublishTopic(SubmittedForm form) throws Exception {
        PandaPortalApplication app = getApp();
        Language lang = getLanguage();
        FormValidatingReporter reporter =
                new CommonFormValidatingReporter(lang, form);
        validateForm(reporter);
        if ( form.hasErrorMessage() ) {
            return false;
        }

        GscManager txbbMan =
                app.getBundleManager().getService(GscManager.class);
        txbbMan.lockTopic(topic);
        return true;
    }

    /**
     * {@inheritDoc}
     */
    protected void modelPageContentCustom(DMMap pageContent) throws Exception {
        super.modelPageContentCustom(pageContent);

        if ( topic != null ) {
            pageContent.addChild(MODEL_TOPIC, TopicModel.getInstance(topic));
        }
    }
}
