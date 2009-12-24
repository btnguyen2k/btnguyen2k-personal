package com.greenstorm.gsc.panda.pc;

import java.util.HashMap;
import java.util.Map;

import org.ddth.mls.Language;
import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.daf.DafUser;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.PandaPortalConstants;
import org.ddth.panda.portal.module.ModuleDescriptor;
import org.ddth.panda.portal.module.ModuleManager;
import org.ddth.panda.portal.module.panda.BaseActionHandler;
import org.ddth.panda.portal.module.panda.LastAccessUrl;
import org.ddth.panda.portal.module.panda.RequireLoggedin;
import org.ddth.panda.portal.utils.TransitionRecord;
import org.ddth.panda.web.UrlCreator;
import org.ddth.panda.web.impl.controlforward.UrlRedirectControlForward;
import org.ddth.webtemplate.datamodel.DMMap;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.TxbbLangConstants;
import com.greenstorm.gsc.bo.TxbbManager;

public abstract class BasePcActionHandler extends BaseActionHandler implements
        RequireLoggedin, LastAccessUrl {

    public final static String MODEL_NUM_DRAFT_TOPICS = "numDraftTopics";

    public final static String MODEL_NUM_PUBLISHED_TOPICS =
            "numPublishedTopics";

    public final static String MODEL_URL_PC_HOME = "urlPcHome";

    public final static String MODEL_URL_PC_CREATE_TOPIC = "urlCreateTopic";

    public final static String MODEL_URL_PC_DRAFT_TOPICS = "urlDraftTopics";

    public final static String MODEL_URL_PC_PUBLISHED_TOPICS =
            "urlPublishedTopics";

    // /**
    // * {@inheritDoc}
    // */
    // protected Collection<DafPermission> getRequiredPermissions() {
    // Collection<DafPermission> requiredPermissions =
    // super.getRequiredPermissions();
    // requiredPermissions.add(TxbbPermissionConstants.PERMISSION_ACCESS_ADMIN_CP);
    // return requiredPermissions;
    // }
    //
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
        String msg = lang.getMessage(TxbbLangConstants.MSG_LOGIN_MESSAGE);
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
                urlCreator.createUri(getModule(), TxbbConstants.ACTION_PC_INDEX);
        pageContent.addChild(MODEL_URL_PC_HOME, url);

        url =
                urlCreator.createUri(getModule(),
                        TxbbConstants.ACTION_PC_CREATE_TOPIC);
        pageContent.addChild(MODEL_URL_PC_CREATE_TOPIC, url);

        url =
                urlCreator.createUri(getModule(),
                        TxbbConstants.ACTION_PC_VIEW_DRAFT_TOPICS);
        pageContent.addChild(MODEL_URL_PC_DRAFT_TOPICS, url);

        url =
                urlCreator.createUri(getModule(),
                        TxbbConstants.ACTION_PC_VIEW_PUBLISHED_TOPICS);
        pageContent.addChild(MODEL_URL_PC_PUBLISHED_TOPICS, url);

        TxbbManager txbbMan =
                app.getBundleManager().getService(TxbbManager.class);
        DafUser user = app.getCurrentUser();
        pageContent.addChild(MODEL_NUM_DRAFT_TOPICS,
                txbbMan.countDraftTopics(user));
        pageContent.addChild(MODEL_NUM_PUBLISHED_TOPICS,
                txbbMan.countPublishedTopics(user));
    }
}
