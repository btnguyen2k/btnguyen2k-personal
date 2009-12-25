package com.greenstorm.gsc.panda.admin;

import java.util.HashMap;
import java.util.Map;

import org.ddth.mls.Language;
import org.ddth.panda.core.ControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.PandaPortalConstants;
import org.ddth.panda.portal.module.ModuleDescriptor;
import org.ddth.panda.portal.utils.TransitionRecord;
import org.ddth.panda.utils.StringUtils;
import org.ddth.panda.web.UrlCreator;
import org.ddth.panda.web.impl.controlforward.UrlRedirectControlForward;
import org.ddth.txbb.panda.admin.BaseAdminBoxHandler;

import com.greenstorm.gsc.GscConstants;
import com.greenstorm.gsc.GscLangConstants;
import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.GscManager;

public class AdminBoxMoveDownHandler extends BaseAdminBoxHandler {

    private final static String URL_PARAM_BOX_ID = "id";

    /**
     * {@inheritDoc}
     */
    @Override
    protected ControlForward execute() throws Exception {
        PandaPortalApplication app = getApp();
        int boxId =
                app.getHttpRequestParams().getUrlParamAsInt(URL_PARAM_BOX_ID);
        GscManager txbbMan =
                app.getBundleManager().getService(GscManager.class);
        Box box = txbbMan.getBox(boxId);

        Language lang = getLanguage();
        UrlCreator urlCreator = app.getUrlCreator();
        String action = GscConstants.ACTION_ADMIN_LIST_BOXES;
        TransitionRecord transition;

        if ( box != null ) {
            Box parent = box.getParentId() != null
                    ? txbbMan.getBox(box.getParentId()) : null;
            if ( parent == null ) {
                moveDownBox(txbbMan.getBoxTree(), box);
            } else {
                moveDownBox(parent.getChildren().toArray(new Box[0]), box);
            }
            String message =
                    lang.getMessage(
                            GscLangConstants.MSG_BOX_REORDER_SUCCESSFUL,
                            StringUtils.escapeHtml(box.getTitle()));
            transition =
                    TransitionRecord.createInformationTransitionRecord(message);
            app.addTransition(transition);
        } else {
            String message =
                    lang.getMessage(GscLangConstants.ERROR_BOX_NOT_FOUND,
                            boxId);
            transition = TransitionRecord.createErrorTransitionRecord(message);
            app.addTransition(transition);
        }

        ModuleDescriptor md = app.getModule(getModuleName());
        Map<String, String> params = new HashMap<String, String>();
        params.put(PandaPortalConstants.URL_PARAM_TRANSITION_ID,
                transition.getId());
        String url =
                urlCreator.createUri(md.getUrlMapping(), action, null, params);
        return new UrlRedirectControlForward(url);
    }

    private void moveDownBox(Box[] boxList, Box box) throws Exception {
        PandaPortalApplication app = getApp();
        GscManager txbbMan =
                app.getBundleManager().getService(GscManager.class);
        Box current = null;
        for ( Box next : boxList ) {
            if ( current != null && current.getId().equals(box.getId()) ) {
                next.setPosition(box.getPosition());
                box.setPosition(box.getPosition() + 1);
                txbbMan.updateBox(next);
                txbbMan.updateBox(box);
                return;
            } else {
                current = next;
            }
        }
    }
}
