package com.greenstorm.gsc.panda;

import java.util.ArrayList;
import java.util.Collection;

import org.ddth.ehconfig.AppConfig;
import org.ddth.mls.Language;
import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.PandaPortalConfigConstants;
import org.ddth.txbb.panda.BaseActionHandler;
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

public class TopicViewViaBoxHandler extends BaseActionHandler {

    private final static String MODEL_BOX = "box";

    private final static String MODEL_TOPIC = "topic";

    private final static int VIRTUAL_PARAM_TOPIC_ID = 2;

    private final static String URL_PARAM_BOX_ID = "box";

    // private final static String MODEL_RECENT_PUBLISHED_TOPICS =
    // "recentPublishedTopics";

    private final static String MODEL_RECENT_COMMENTED_TOPICS =
            "recentCommentedTopics";

    private Box box;

    private Topic topic;

    /**
     * {@inheritDoc}
     */
    @Override
    protected ControlForward execute() throws Exception {
        PandaPortalApplication app = getApp();
        TxbbManager txbbMan =
                app.getBundleManager().getService(TxbbManager.class);
        Language lang = getLanguage();

        int boxId =
                app.getHttpRequestParams().getUrlParamAsInt(URL_PARAM_BOX_ID);
        box = txbbMan.getBox(boxId);

        int topicId =
                app.getHttpRequestParams().getVirtualPathParamAsInt(
                        VIRTUAL_PARAM_TOPIC_ID);
        topic = txbbMan.getTopic(topicId);
        Box[] publishedBoxes = topic != null
                ? txbbMan.getPublishedBoxes(topic) : null;

        if ( box == null
                || !app.hasPermission(
                        TxbbPermissionConstants.PERMISSION_VIEW_BOX, box) ) {
            // no permission to view would be the same as "not found"
            box = null;
            app.addErrorMessage(lang.getMessage(
                    TxbbLangConstants.ERROR_BOX_NOT_FOUND, boxId));
        } else if ( topic == null || !containBox(publishedBoxes, box) ) {
            // no topic or no publishing would be the same as "not found"
            topic = null;
            app.addErrorMessage(lang.getMessage(
                    TxbbLangConstants.ERROR_TOPIC_NOT_FOUND, topicId));
        }

        populateDataModels();
        return new ViewControlForward(getModule(), getAction());
    }

    private boolean containBox(Box[] list, Box box) {
        if ( list == null || list.length == 0 || box == null ) {
            return false;
        }
        for ( Box b : list ) {
            if ( b.getId() == box.getId() ) {
                return true;
            }
        }
        return false;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected void modelPageContentCustom(DMMap pageContent) throws Exception {
        super.modelPageContentCustom(pageContent);
        PandaPortalApplication app = getApp();

        TxbbManager txbbMan =
                app.getBundleManager().getService(TxbbManager.class);

        if ( box != null ) {
            // viewable boxes
            Collection<Box> viewableBoxes = new ArrayList<Box>();
            viewableBoxes.add(box);
            for ( Box child : box.getChildren() ) {
                if ( app.hasPermission(
                        TxbbPermissionConstants.PERMISSION_VIEW_BOX, child) ) {
                    viewableBoxes.add(child);
                }
            }

            pageContent.addChild(MODEL_BOX, BoxModel.getInstance(box));

            // recent commented topics
            Topic[] recentCommentedTopics =
                    txbbMan.getRecentCommentedTopics(
                            TxbbConstants.NUM_RECENT_COMMENTED_TOPICS,
                            viewableBoxes);
            DMList modelRecentCommentedTopics =
                    new DMList(MODEL_RECENT_COMMENTED_TOPICS);
            for ( Topic topic : recentCommentedTopics ) {
                modelRecentCommentedTopics.addChild(TopicModel.getInstance(topic));
            }
            pageContent.addChild(modelRecentCommentedTopics);
        }

        if ( topic != null ) {
            pageContent.addChild(MODEL_TOPIC, TopicModel.getInstance(topic));
        }
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected void modelPageHeaderTitle(DMMap pageHeader) throws Exception {
        if ( box == null ) {
            super.modelPageHeaderTitle(pageHeader);
        } else {
            AppConfig config =
                    getApp().getAppConfig(
                            PandaPortalConfigConstants.CONFIG_SITE_TITLE);
            String siteTitle = config != null
                    ? config.getConfigValue() : "";
            pageHeader.addChild(MODEL_PAGE_HEADER_TITLE, siteTitle
                    + TxbbConstants.TITLE_SEPARATOR + box.getTitle());
        }
    }
}
