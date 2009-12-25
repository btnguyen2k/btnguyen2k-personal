package com.greenstorm.gsc.panda.admin;

import org.ddth.logging.LogManager;
import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.daf.DafDataManager;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.txbb.panda.admin.BaseAdminActionHandler;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.bo.GscManager;

public class AdminIndexHandler extends BaseAdminActionHandler {

    final private static String MODEL_NUM_BOXES = "numBoxes";

    final private static String MODEL_NUM_TOPICS = "numTopics";

    final private static String MODEL_NUM_POSTS = "numPosts";

    final private static String MODEL_NUM_MEMBERS = "numMembers";

    final private static String MODEL_NUM_REG_ACCOUNTS = "numRegAccounts";

    final private static String MODEL_NUM_ONLINES = "numOnlines";

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
        pageContent.addChild(MODEL_NUM_BOXES, fm.countBoxes());
        pageContent.addChild(MODEL_NUM_TOPICS, fm.countTopics());
        pageContent.addChild(MODEL_NUM_POSTS, fm.countPosts());

        DafDataManager dafDm = app.getDafDataManager();
        pageContent.addChild(MODEL_NUM_MEMBERS, dafDm.getNumUsers());
        pageContent.addChild(MODEL_NUM_REG_ACCOUNTS,
                dafDm.getNumRegisteredAccounts());
        pageContent.addChild(MODEL_NUM_POSTS, fm.countPosts());

        LogManager lm = app.getLogManager();
        pageContent.addChild(MODEL_NUM_ONLINES,
                lm.getNumOnlineSessions(30 * 60));
    }
}
