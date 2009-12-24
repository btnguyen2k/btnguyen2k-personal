package com.greenstorm.gsc.panda.pc;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.daf.DafUser;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.PandaPortalConstants;
import org.ddth.panda.portal.model.DMTableView;
import org.ddth.panda.web.UrlCreator;
import org.ddth.txbb.panda.pc.BasePcActionHandler;
import org.ddth.webtemplate.datamodel.DMList;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.bo.Topic;
import com.greenstorm.gsc.bo.TxbbManager;
import com.greenstorm.gsc.model.TopicModel;

public class TopicViewPublishedListHandler extends BasePcActionHandler {

    private final static String URL_PARAM_PAGE = "p";

    private final static String MODEL_PUBLISHED_TOPICS = "publishedTopics";

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
        pageContent.addChild(new TablePublishedTopics(MODEL_PUBLISHED_TOPICS));
    }

    private class TablePublishedTopics extends DMTableView {

        int currentPage;

        int numPages;

        UrlCreator urlCreator;

        public TablePublishedTopics(String modelName) throws Exception {
            super(modelName);
            PandaPortalApplication app = getApp();
            urlCreator = app.getUrlCreator();

            TxbbManager txbbMan =
                    app.getBundleManager().getService(TxbbManager.class);

            DafUser currentUser = app.getCurrentUser();
            int numRecords = txbbMan.countPublishedTopics(currentUser);
            numPages = numRecords / PandaPortalConstants.PAGE_SIZE;
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

            Topic[] publishedTopics =
                    txbbMan.getPublishedTopics(app.getCurrentUser(),
                            currentPage, PandaPortalConstants.PAGE_SIZE);
            for ( Topic topic : publishedTopics ) {
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
            return TxbbConstants.PAGE_SIZE;
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
                Map<String, String> urlParams = new HashMap<String, String>();
                for ( int pageNum : pages ) {
                    if ( pageNum < 1 || pageNum > numPages ) {
                        continue;
                    }
                    urlParams.put("page", String.valueOf(pageNum));
                    String url =
                            urlCreator.createUri(
                                    getModule(),
                                    TxbbConstants.ACTION_PC_VIEW_PUBLISHED_TOPICS,
                                    null, urlParams);
                    DMMap modelPage = new DMMap("");
                    modelPage.addChild("page", pageNum);
                    modelPage.addChild("url", url);
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
                Map<String, String> urlParams = new HashMap<String, String>();
                urlParams.put("page", String.valueOf(pageNum));
                url =
                        urlCreator.createUri(getModule(),
                                TxbbConstants.ACTION_PC_VIEW_PUBLISHED_TOPICS,
                                null, urlParams);
                cacheUrls.put(pageNum, url);
            }
            return url;
        }
    }
}
