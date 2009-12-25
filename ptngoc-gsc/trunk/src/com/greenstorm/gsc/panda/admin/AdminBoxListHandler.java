package com.greenstorm.gsc.panda.admin;

import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.txbb.panda.admin.BaseAdminBoxHandler;
import org.ddth.webtemplate.datamodel.DMList;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.GscManager;
import com.greenstorm.gsc.model.CardModel;

public class AdminBoxListHandler extends BaseAdminBoxHandler {

    private final static String MODEL_BOX_LIST = "boxList";

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
        DMList modelBoxList = new DMList(MODEL_BOX_LIST);
        Box[] boxTree = fm.getBoxTree();
        for ( Box box : boxTree ) {
            modelBoxList.addChild(CardModel.getInstance(box));
        }
        pageContent.addChild(modelBoxList);
    }
}
