package com.greenstorm.gsc.panda.admin;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;

import org.ddth.mls.Language;
import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.daf.DafPermission;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.PandaPortalConstants;
import org.ddth.panda.portal.module.ModuleDescriptor;
import org.ddth.panda.portal.module.ModuleManager;
import org.ddth.panda.portal.module.panda.BaseActionHandler;
import org.ddth.panda.portal.module.panda.LastAccessUrl;
import org.ddth.panda.portal.module.panda.RequireAuthorization;
import org.ddth.panda.portal.module.panda.RequireLoggedin;
import org.ddth.panda.portal.utils.TransitionRecord;
import org.ddth.panda.web.UrlCreator;
import org.ddth.panda.web.impl.controlforward.UrlRedirectControlForward;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.GscConstants;
import com.greenstorm.gsc.GscLangConstants;
import com.greenstorm.gsc.GscPermissionConstants;

public abstract class BaseAdminActionHandler extends BaseActionHandler
        implements RequireLoggedin, RequireAuthorization, LastAccessUrl {

    public final static String MODEL_URL_ADMIN_HOME = "urlAdminHome";

    /**
     * {@inheritDoc}
     */
    protected Collection<DafPermission> getRequiredPermissions() {
        Collection<DafPermission> requiredPermissions =
                super.getRequiredPermissions();
        requiredPermissions.add(GscPermissionConstants.PERMISSION_ACCESS_ADMIN_CP);
        return requiredPermissions;
    }

    /**
     * {@inheritDoc}
     */
    public boolean checkAuthorization() throws Exception {
        PandaPortalApplication app = getApp();
        if ( app.isGod() ) {
            return true;
        }

        Collection<DafPermission> requiredPermissions =
                getRequiredPermissions();
        if ( requiredPermissions != null ) {
            for ( DafPermission permission : requiredPermissions ) {
                if ( !app.hasPermission(permission) ) {
                    return false;
                }
            }
        }

        return true;
    }

    /**
     * {@inheritDoc}
     * 
     * @throws Exception
     */
    @Override
    public ControlForward getLoginControlForward() throws Exception {
        PandaPortalApplication app = getApp();
        ModuleManager mm = app.getModuleManager();
        ModuleDescriptor md = mm.getModule(ModuleManager.MODULE_PORTAL_CORE);
        UrlCreator urlCreator = app.getUrlCreator();
        Language lang = getLanguage();
        String msg = lang.getMessage(GscLangConstants.MSG_LOGIN_MESSAGE);
        TransitionRecord transition =
                TransitionRecord.createWarningTransitionRecord(msg);
        app.addTransition(transition);
        Map<String, String> params = new HashMap<String, String>();
        params.put(PandaPortalConstants.URL_PARAM_TRANSITION_ID,
                transition.getId());
        String urlLogin =
                urlCreator.createUri(md.getUrlMapping(),
                        PandaPortalConstants.ACTION_LOGIN, null, params);
        return new UrlRedirectControlForward(urlLogin);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected void modelPageContentCustom(DMMap pageContent) throws Exception {
        super.modelPageContentCustom(pageContent);
        PandaPortalApplication app = getApp();
        UrlCreator urlCreator = app.getUrlCreator();

        String url =
                urlCreator.createUri(getModule(),
                        GscConstants.ACTION_ADMIN_HOME);
        pageContent.addChild(MODEL_URL_ADMIN_HOME, url);
    }
}
