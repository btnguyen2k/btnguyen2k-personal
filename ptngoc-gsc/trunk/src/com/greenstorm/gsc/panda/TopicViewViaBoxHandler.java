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

import com.greenstorm.gsc.GscConstants;
import com.greenstorm.gsc.GscLangConstants;
import com.greenstorm.gsc.GscPermissionConstants;
import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.Topic;
import com.greenstorm.gsc.bo.GscManager;
import com.greenstorm.gsc.model.CardModel;
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
        GscManager txbbMan =
                app.getBundleManager().getService(GscManager.class);
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
                        GscPermissionConstants.PERMISSION_VIEW_BOX, box) ) {
            // no permission to view would be the same as "not found"
            box = null;
            app.addErrorMessage(lang.getMessage(
                    GscLangConstants.ERROR_BOX_NOT_FOUND, boxId));
        } else if ( topic == null || !containBox(publishedBoxes, box) ) {
            // no topic or no publishing would be the same as "not found"
            topic = null;
            app.addErrorMessage(lang.getMessage(
                    GscLangConstants.ERROR_TOPIC_NOT_FOUND, topicId));
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

        GscManager txbbMan =
                app.getBundleManager().getService(GscManager.class);

        if ( box != null ) {
            // viewable boxes
            Collection<Box> viewableBoxes = new ArrayList<Box>();
            viewableBoxes.add(box);
            for ( Box child : box.getChildren() ) {
                if ( app.hasPermission(
                        GscPermissionConstants.PERMISSION_VIEW_BOX, child) ) {
                    viewableBoxes.add(child);
                }
            }

            pageContent.addChild(MODEL_BOX, CardModel.getInstance(box));

            // recent commented topics
            Topic[] recentCommentedTopics =
                    txbbMan.getRecentCommentedTopics(
                            GscConstants.NUM_RECENT_COMMENTED_TOPICS,
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
                    + GscConstants.TITLE_SEPARATOR + box.getTitle());
        }
    }
}
