package com.greenstorm.gsc.panda;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.ddth.ehconfig.AppConfig;
import org.ddth.mls.Language;
import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.PandaPortalConfigConstants;
import org.ddth.panda.portal.model.DMTableView;
import org.ddth.panda.web.UrlCreator;
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

/* Dashboard */
public class BoxViewHandler extends BaseActionHandler {

    private final static String MODEL_BOX = "box";

    private final static int VIRTUAL_PARAM_BOX_ID = 2;

    // private final static String MODEL_RECENT_PUBLISHED_TOPICS =
    // "recentPublishedTopics";

    private final static String MODEL_RECENT_COMMENTED_TOPICS =
            "recentCommentedTopics";

    private final static String MODEL_TOPICS = "topics";

    private final static String URL_PARAM_PAGE = "p";

    private final static int TOPICS_PER_PAGE = 12;

    private Box box;

    /**
     * {@inheritDoc}
     */
    @Override
    protected ControlForward execute() throws Exception {
        PandaPortalApplication app = getApp();
        int boxId =
                app.getHttpRequestParams().getVirtualPathParamAsInt(
                        VIRTUAL_PARAM_BOX_ID);
        GscManager txbbMan =
                app.getBundleManager().getService(GscManager.class);
        box = txbbMan.getBox(boxId);
        Language lang = getLanguage();

        if ( box == null
                || !app.hasPermission(
                        GscPermissionConstants.PERMISSION_VIEW_BOX, box) ) {
            // no permission to view would be the same as "not found"
            box = null;
            app.addErrorMessage(lang.getMessage(
                    GscLangConstants.ERROR_BOX_NOT_FOUND, boxId));
        }
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

            pageContent.addChild(new TableTopics(MODEL_TOPICS));

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
    }

    private class TableTopics extends DMTableView {
        private int currentPage;

        private int numPages;

        private UrlCreator urlCreator;

        private CardModel boxModel = CardModel.getInstance(box);

        public TableTopics(String modelName) throws Exception {
            super(modelName);
            PandaPortalApplication app = getApp();
            urlCreator = app.getUrlCreator();

            GscManager txbbMan =
                    app.getBundleManager().getService(GscManager.class);

            Collection<Box> viewableBoxes = new ArrayList<Box>();
            viewableBoxes.add(box);
            for ( Box child : box.getChildren() ) {
                if ( app.hasPermission(
                        GscPermissionConstants.PERMISSION_VIEW_BOX, child) ) {
                    viewableBoxes.add(child);
                }
            }

            int numRecords = txbbMan.countPublishedTopicsInBoxes(viewableBoxes);
            numPages = numRecords / TOPICS_PER_PAGE;
            if ( numPages < 1 ) {
                numPages = 1;
            }
            currentPage =
                    app.getHttpRequestParams().getUrlParamAsInt(URL_PARAM_PAGE);
            if ( currentPage < 1 ) {
                currentPage = 1;
            }
            if ( currentPage > numPages ) {
                currentPage = numPages;
            }

            Topic[] topics =
                    txbbMan.getPublishedTopicsForBoxes(viewableBoxes,
                            currentPage, TOPICS_PER_PAGE);
            for ( Topic topic : topics ) {
                addRow(TopicModel.getInstance(topic));
            }
        }

        /**
         * {@inheritDoc}
         */
        public boolean hasPaging() {
            return true;
        }

        /**
         * {@inheritDoc}
         */
        public int getPageSize() {
            return TOPICS_PER_PAGE;
        }

        /**
         * {@inheritDoc}
         */
        public int getNumPages() {
            return numPages;
        }

        /**
         * {@inheritDoc}
         */
        public int getCurrentPage() {
            return currentPage;
        }

        private DMList pagination;

        /**
         * {@inheritDoc}
         */
        public Collection<Object> getPagination() {
            if ( pagination == null ) {
                pagination = new DMList("");
                int[] pages =
                        new int[]{ currentPage - 9, currentPage - 5,
                            currentPage - 3, currentPage - 2, currentPage - 1,
                            currentPage, currentPage + 1, currentPage + 2,
                            currentPage + 3, currentPage + 5, currentPage + 9 };
                for ( int pageNum : pages ) {
                    if ( pageNum < 1 || pageNum > numPages ) {
                        continue;
                    }
                    DMMap modelPage = new DMMap("");
                    modelPage.addChild("page", pageNum);
                    modelPage.addChild("url", getUrlForPage(pageNum));
                    pagination.addChild(modelPage);
                }
            }
            return pagination.asJavaObject();
        }

        private Map<Integer, String> cacheUrls = new HashMap<Integer, String>();

        /**
         * {@inheritDoc}
         */
        public String getUrlForPage(int pageNum) {
            String url = cacheUrls.get(pageNum);
            if ( url == null ) {
                List<String> vpParams = new ArrayList<String>();
                vpParams.add(String.valueOf(boxModel.getId()));
                vpParams.add(boxModel.getTitleForUrl());
                Map<String, String> urlParams = new HashMap<String, String>();
                urlParams.put(URL_PARAM_PAGE, String.valueOf(pageNum));
                url =
                        urlCreator.createUri(getModule(),
                                GscConstants.ACTION_VIEW_BOX,
                                vpParams.toArray(new String[0]), urlParams);
                cacheUrls.put(pageNum, url);
            }
            return url;
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
