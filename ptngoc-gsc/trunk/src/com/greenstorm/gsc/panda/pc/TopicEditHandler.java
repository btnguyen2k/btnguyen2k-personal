package com.greenstorm.gsc.panda.pc;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.Iterator;
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
import org.ddth.panda.web.UrlCreator;
import org.ddth.panda.web.form.FormValidatingReporter;
import org.ddth.panda.web.impl.controlforward.UrlRedirectControlForward;
import org.ddth.txbb.panda.pc.BasePcActionHandler;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.TxbbLangConstants;
import com.greenstorm.gsc.bo.Topic;
import com.greenstorm.gsc.bo.TopicContent;
import com.greenstorm.gsc.bo.GscManager;
import com.greenstorm.gsc.model.TopicModel;

public class TopicEditHandler extends BasePcActionHandler {

    private final static String URL_PARAM_TOPIC_ID = "id";

    private final static String FORM_FIELD_TOPIC_TYPE = "topicType";

    private final static String FORM_FIELD_TOPIC_TITLE = "topicTitle";

    private final static String FORM_FIELD_TOPIC_CONTENT = "topicContent";

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
                    TxbbLangConstants.ERROR_TOPIC_NOT_FOUND, topicId));
        } else if ( topic.getMemberId() != dafUserId ) {
            app.addErrorMessage(lang.getMessage(
                    TxbbLangConstants.ERROR_CAN_NOT_EDIT_UNOWNED_TOPIC, topicId));
        } else if ( topic.isPublished() ) {
            app.addErrorMessage(lang.getMessage(
                    TxbbLangConstants.ERROR_CAN_NOT_EDIT_PUBLISHED_TOPIC,
                    topicId));
        } else {
            ModuleDescriptor md = app.getModule(getModuleName());
            SubmittedForm form = getSubmittedForm();
            UrlCreator urlCreator = app.getUrlCreator();
            if ( form != null && doEditTopic(form) ) {
                String message =
                        lang.getMessage(TxbbLangConstants.MSG_PC_TOPIC_EDIT_SUCCESSFUL);
                String action = TxbbConstants.ACTION_PC_EDIT_TOPIC;
                TransitionRecord transition =
                        TransitionRecord.createInformationTransitionRecord(message);
                app.addTransition(transition);
                Map<String, String> params = new HashMap<String, String>();
                params.put(PandaPortalConstants.URL_PARAM_TRANSITION_ID,
                        transition.getId());
                params.put("id", String.valueOf(topic.getId()));
                String url =
                        urlCreator.createUri(md.getUrlMapping(), action, null,
                                params);
                return new UrlRedirectControlForward(url);
            }
            if ( form == null ) {
                form = initializeAssociatedForm();
                form.setAttribute(FORM_FIELD_TOPIC_TYPE, topic.getType());
                form.setAttribute(FORM_FIELD_TOPIC_TITLE, topic.getTitle());
                Collection<TopicContent> contents = topic.getTopicContents();
                if ( contents != null && contents.size() > 0 ) {
                    Iterator<TopicContent> it = contents.iterator();
                    form.setAttribute(FORM_FIELD_TOPIC_CONTENT, it.next()
                            .getContent());
                }
            }
            if ( form != null ) {
                form.setCancelAction(urlCreator.createUri(md.getUrlMapping(),
                        TxbbConstants.ACTION_PC_VIEW_DRAFT_TOPICS));
            }
        }
        populateDataModels();
        return new ViewControlForward(getModule(), getAction());
    }

    private boolean doEditTopic(SubmittedForm form) throws Exception {
        PandaPortalApplication app = getApp();
        Language lang = getLanguage();
        FormValidatingReporter reporter =
                new CommonFormValidatingReporter(lang, form);
        validateForm(reporter);
        if ( form.hasErrorMessage() ) {
            return false;
        }

        String type = form.getAttribute(FORM_FIELD_TOPIC_TYPE);
        String title = form.getAttribute(FORM_FIELD_TOPIC_TITLE);
        String content = form.getAttribute(FORM_FIELD_TOPIC_CONTENT);
        topic.setType(type);
        topic.setTitle(title);
        Collection<TopicContent> contents = topic.getTopicContents();
        TopicContent topicContent = null;
        if ( contents == null || contents.size() == 0 ) {
            topicContent = new TopicContent();
            if ( contents == null ) {
                contents = new ArrayList<TopicContent>();
            }
            contents.add(topicContent);
        } else {
            topicContent = contents.iterator().next();
        }
        topicContent.setTopicId(topic.getId());
        topicContent.setOrder(0);
        topicContent.setContent(content);

        GscManager txbbMan =
                app.getBundleManager().getService(GscManager.class);
        topic = txbbMan.updateTopic(topic);

        return true;
    }

    /**
     * {@inheritDoc}
     */
    protected void modelPageContentCustom(DMMap pageContent) throws Exception {
        super.modelPageContentCustom(pageContent);
        // PandaPortalApplication app = getApp();

        if ( topic != null ) {
            pageContent.addChild(MODEL_TOPIC, TopicModel.getInstance(topic));
        }
    }
}
