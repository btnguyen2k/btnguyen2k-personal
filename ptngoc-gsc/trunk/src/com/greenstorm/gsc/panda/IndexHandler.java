package com.greenstorm.gsc.panda;

import java.util.Collection;

import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.txbb.panda.BaseActionHandler;
import org.ddth.webtemplate.datamodel.DMList;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.Topic;
import com.greenstorm.gsc.bo.GscManager;
import com.greenstorm.gsc.model.TopicModel;
import com.greenstorm.gsc.util.TxbbHelper;

/* Dashboard */
public class IndexHandler extends BaseActionHandler {

    private final static String MODEL_RECENT_PUBLISHED_TOPICS =
            "recentPublishedTopics";

    private final static String MODEL_RECENT_COMMENTED_TOPICS =
            "recentCommentedTopics";

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

        GscManager fm = app.getBundleManager().getService(GscManager.class);

        Collection<Box> viewableBoxes = TxbbHelper.getViewableBoxes();

        // recent published topics
        Topic[] recentPublishedTopics =
                fm.getRecentPublishedTopics(
                        TxbbConstants.NUM_RECENT_PUBLISHED_TOPICS_ALL_BOXES,
                        viewableBoxes);
        DMList modelRecentPublishedTopics =
                new DMList(MODEL_RECENT_PUBLISHED_TOPICS);
        for ( Topic topic : recentPublishedTopics ) {
            modelRecentPublishedTopics.addChild(TopicModel.getInstance(topic));
        }
        pageContent.addChild(modelRecentPublishedTopics);

        // recent commented topics
        Topic[] recentCommentedTopics =
                fm.getRecentCommentedTopics(
                        TxbbConstants.NUM_RECENT_COMMENTED_TOPICS,
                        viewableBoxes);
        DMList modelRecentCommentedTopics =
                new DMList(MODEL_RECENT_COMMENTED_TOPICS);
        for ( Topic topic : recentCommentedTopics ) {
            modelRecentCommentedTopics.addChild(TopicModel.getInstance(topic));
        }
        pageContent.addChild(modelRecentCommentedTopics);
    }
}
