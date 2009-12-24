package com.greenstorm.gsc.panda.pc;

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
import org.ddth.panda.web.UrlCreator;
import org.ddth.panda.web.form.FormValidatingReporter;
import org.ddth.panda.web.impl.controlforward.UrlRedirectControlForward;
import org.ddth.txbb.panda.pc.BasePcActionHandler;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.TxbbLangConstants;
import com.greenstorm.gsc.bo.Topic;
import com.greenstorm.gsc.bo.TxbbManager;

public class TopicCreateHandler extends BasePcActionHandler {

    private final static String FORM_FIELD_TOPIC_TYPE = "topicType";

    /**
     * {@inheritDoc}
     */
    @Override
    protected ControlForward execute() throws Exception {
        PandaPortalApplication app = getApp();
        ModuleDescriptor md = app.getModule(getModuleName());
        SubmittedForm form = getSubmittedForm();
        UrlCreator urlCreator = app.getUrlCreator();
        Topic topic;
        if ( form != null && (topic = doCreateTopic(form)) != null ) {
            Language lang = getLanguage();
            String message =
                    lang.getMessage(TxbbLangConstants.MSG_PC_TOPIC_CREATE_SUCCESSFUL);
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
        }
        if ( form != null ) {
            form.setCancelAction(urlCreator.createUri(md.getUrlMapping(),
                    TxbbConstants.ACTION_PC_INDEX));
        }
        populateDataModels();
        return new ViewControlForward(getModule(), getAction());
    }

    private Topic doCreateTopic(SubmittedForm form) throws Exception {
        PandaPortalApplication app = getApp();
        Language lang = getLanguage();
        FormValidatingReporter reporter =
                new CommonFormValidatingReporter(lang, form);
        validateForm(reporter);
        if ( form.hasErrorMessage() ) {
            return null;
        }

        String type = form.getAttribute(FORM_FIELD_TOPIC_TYPE);
        Topic topic = new Topic();
        topic.setType(type);
        topic.setMemberId((Integer)app.getCurrentUser().getId());

        TxbbManager txbbMan =
                app.getBundleManager().getService(TxbbManager.class);
        topic = txbbMan.createTopic(topic);

        return topic;
    }
}
