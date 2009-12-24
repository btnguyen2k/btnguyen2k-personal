package com.greenstorm.gsc.panda.admin;

import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.txbb.panda.admin.BaseAdminBoxHandler;
import org.ddth.webtemplate.datamodel.DMList;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.TxbbManager;
import com.greenstorm.gsc.model.BoxModel;

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

        TxbbManager fm = app.getBundleManager().getService(TxbbManager.class);
        DMList modelBoxList = new DMList(MODEL_BOX_LIST);
        Box[] boxTree = fm.getBoxTree();
        for ( Box box : boxTree ) {
            modelBoxList.addChild(BoxModel.getInstance(box));
        }
        pageContent.addChild(modelBoxList);
    }
}
