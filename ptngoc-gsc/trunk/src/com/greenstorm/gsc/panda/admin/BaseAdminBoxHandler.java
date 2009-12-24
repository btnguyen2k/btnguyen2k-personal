package com.greenstorm.gsc.panda.admin;

import java.util.Collection;

import org.ddth.panda.core.daf.DafPermission;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.web.UrlCreator;
import org.ddth.txbb.panda.admin.BaseAdminActionHandler;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.TxbbPermissionConstants;

public abstract class BaseAdminBoxHandler extends BaseAdminActionHandler {

    private final static String MODEL_URL_LIST_BOXES = "urlListBoxes";

    private final static String MODEL_URL_CREATE_BOX = "urlCreateBox";

    /**
     * {@inheritDoc}
     */
    protected Collection<DafPermission> getRequiredPermissions() {
        Collection<DafPermission> requiredPermissions =
                super.getRequiredPermissions();
        requiredPermissions.add(TxbbPermissionConstants.PERMISSION_MANAGE_BOXES);
        return requiredPermissions;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected void modelPageContentCustom(DMMap pageContent) throws Exception {
        super.modelPageContentCustom(pageContent);
        PandaPortalApplication app = getApp();
        UrlCreator urlCreator = app.getUrlCreator();

        String urlCreateBox =
                urlCreator.createUri(getModule(),
                        TxbbConstants.ACTION_ADMIN_CREATE_BOX);
        pageContent.addChild(MODEL_URL_CREATE_BOX, urlCreateBox);

        String urlBoxList =
                urlCreator.createUri(getModule(),
                        TxbbConstants.ACTION_ADMIN_LIST_BOXES);
        pageContent.addChild(MODEL_URL_LIST_BOXES, urlBoxList);
    }
}
