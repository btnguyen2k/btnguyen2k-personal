package com.greenstorm.gsc.panda;

import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.bo.GscManager;

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
    }
}
