package com.greenstorm.gsc.panda.pc;

import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.txbb.panda.pc.BasePcActionHandler;
import org.ddth.webtemplate.datamodel.DMList;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.GscPermissionConstants;
import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.GscManager;
import com.greenstorm.gsc.model.CardModel;

public class IndexHandler extends BasePcActionHandler {

    private final static String MODEL_BOX_TREE = "boxTree";

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
        DMList modelBoxTree = new DMList(MODEL_BOX_TREE);
        Box[] boxTree = fm.getBoxTree();
        for ( Box box : boxTree ) {
            if ( app.hasPermission(GscPermissionConstants.PERMISSION_VIEW_BOX,
                    box) ) {
                modelBoxTree.addChild(CardModel.getInstance(box));
            }
        }
        pageContent.addChild(modelBoxTree);
    }
}
