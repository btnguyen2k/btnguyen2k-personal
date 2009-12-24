package com.greenstorm.gsc.panda.pc;

import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.txbb.panda.pc.BasePcActionHandler;
import org.ddth.webtemplate.datamodel.DMList;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.bo.Topic;
import com.greenstorm.gsc.bo.TxbbManager;
import com.greenstorm.gsc.model.TopicModel;

public class TopicViewDraftListHandler extends BasePcActionHandler {

    private final static String MODEL_DRAFT_TOPICS = "draftTopics";

    /**
     * {@inheritDoc}
     */
    @Override
    protected ControlForward execute() throws Exception {
        populateDataModels();
        return new ViewControlForward(getModule(), getAction());
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected void modelPageContentCustom(DMMap pageContent) throws Exception {
        super.modelPageContentCustom(pageContent);
        PandaPortalApplication app = getApp();

        TxbbManager fm = app.getBundleManager().getService(TxbbManager.class);
        Topic[] draftTopics = fm.getDraftTopics(app.getCurrentUser());
        DMList modelDraftTopics = new DMList(MODEL_DRAFT_TOPICS);
        for ( Topic topic : draftTopics ) {
            modelDraftTopics.addChild(TopicModel.getInstance(topic));
        }
        pageContent.addChild(modelDraftTopics);
    }
}
