package com.greenstorm.gsc.panda;

import org.ddth.panda.core.ControlForward;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.module.admin.PortalAdminConstants;
import org.ddth.panda.portal.module.panda.LastAccessUrl;
import org.ddth.panda.portal.module.panda.ModuleActionHandler;
import org.ddth.panda.web.UrlCreator;
import org.ddth.panda.web.impl.controlforward.UrlRedirectControlForward;

public class DefaultHandler extends ModuleActionHandler implements
        LastAccessUrl {
    /**
     * {@inheritDoc}
     */
    @Override
    protected ControlForward execute() throws Exception {
        PandaPortalApplication app = getApp();
        UrlCreator urlCreator = app.getUrlCreator();

        String urlIndex =
                urlCreator.createUri(getModule(),
                        PortalAdminConstants.DEFAULT_ACTION);
        return new UrlRedirectControlForward(urlIndex);
    }
}
