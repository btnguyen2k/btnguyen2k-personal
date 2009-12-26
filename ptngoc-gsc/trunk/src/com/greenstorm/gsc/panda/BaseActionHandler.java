package com.greenstorm.gsc.panda;

import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.module.panda.LastAccessUrl;
import org.ddth.panda.web.UrlCreator;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.GscConstants;
import com.greenstorm.gsc.bo.GscManager;

public abstract class BaseActionHandler extends
        org.ddth.panda.portal.module.panda.BaseActionHandler implements
        LastAccessUrl {

    public final static String MODEL_URL_HOME = "urlHome";

    public final static String MODEL_URL_LOGOUT = "urlLogin";

    public final static String MODEL_URL_LOGIN = "urlLogout";

    public final static String MODEL_BOX_TREE = "boxTree";

    // /**
    // * {@inheritDoc}
    // */
    // public boolean checkAuthorization() throws Exception {
    // PandaPortalApplication app = getApp();
    // if ( app.isGod() ) {
    // return true;
    // }
    //
    // Collection<DafPermission> requiredPermissions =
    // getRequiredPermissions();
    // if ( requiredPermissions != null ) {
    // for ( DafPermission permission : requiredPermissions ) {
    // if ( !app.hasPermission(permission) ) {
    // return false;
    // }
    // }
    // }
    //
    // return true;
    // }

    // /**
    // * {@inheritDoc}
    // *
    // * @throws Exception
    // */
    // @Override
    // public ControlForward getLoginControlForward() throws Exception {
    // PandaPortalApplication app = getApp();
    // ModuleManager mm = app.getModuleManager();
    // ModuleDescriptor md = mm.getModule(ModuleManager.MODULE_PORTAL_CORE);
    // UrlCreator urlCreator = app.getUrlCreator();
    // Language lang = getLanguage();
    // String msg = lang.getMessage(TxbbLangConstants.MSG_LOGIN_MESSAGE);
    // TransitionRecord transition =
    // TransitionRecord.createWarningTransitionRecord(msg);
    // app.addTransition(transition);
    // Map<String, String> params = new HashMap<String, String>();
    // params.put(PandaPortalConstants.URL_PARAM_TRANSITION_ID,
    // transition.getId());
    // String urlLogin =
    // urlCreator.createUri(md.getUrlMapping(),
    // PandaPortalConstants.ACTION_LOGIN, null, params);
    // return new UrlRedirectControlForward(urlLogin);
    // }

    /**
     * {@inheritDoc}
     */
    @Override
    protected void modelPageContentCustom(DMMap pageContent) throws Exception {
        super.modelPageContentCustom(pageContent);
        PandaPortalApplication app = getApp();
        UrlCreator urlCreator = app.getUrlCreator();

        String url =
                urlCreator.createUri(getModule(), GscConstants.ACTION_HOME);
        pageContent.addChild(MODEL_URL_HOME, url);

        url = urlCreator.createUri(getModule(), GscConstants.ACTION_LOGIN);
        pageContent.addChild(MODEL_URL_LOGIN, url);

        url = urlCreator.createUri(getModule(), GscConstants.ACTION_LOGOUT);
        pageContent.addChild(MODEL_URL_LOGOUT, url);

        GscManager txbbMan =
                app.getBundleManager().getService(GscManager.class);

        /*
         * // box tree DMList modelBoxTree = new DMList(MODEL_BOX_TREE); Box[]
         * boxTree = txbbMan.getBoxTree(); for ( Box box : boxTree ) { if (
         * app.hasPermission(GscPermissionConstants.PERMISSION_VIEW_BOX, box) )
         * { modelBoxTree.addChild(CardModel.getInstance(box)); } }
         * pageContent.addChild(modelBoxTree);
         */
    }
}
