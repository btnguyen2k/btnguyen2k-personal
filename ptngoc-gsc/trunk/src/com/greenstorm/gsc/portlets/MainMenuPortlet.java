package com.greenstorm.gsc.portlets;

import java.util.ArrayList;
import java.util.List;

import org.ddth.mls.Language;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.module.ModuleDescriptor;
import org.ddth.panda.portal.module.PortletDescriptor;
import org.ddth.panda.portal.portlet.BorderedPortlet;
import org.ddth.panda.portal.portlet.MenuPortlet;
import org.ddth.panda.portal.portlet.SingletonPortlet;
import org.ddth.panda.portal.portlet.TitledPortlet;
import org.ddth.panda.portal.portlet.menu.MenuItem;
import org.ddth.panda.web.UrlCreator;

import com.greenstorm.gsc.GscConstants;
import com.greenstorm.gsc.GscLangConstants;

public class MainMenuPortlet extends BaseTxbbPortlet implements
        BorderedPortlet, SingletonPortlet, MenuPortlet, TitledPortlet {

    /**
     * Constructs a new MainMenuPortlet object.
     * 
     * @param portletDescriptor PortletDescriptor
     */
    public MainMenuPortlet(PortletDescriptor portletDescriptor) {
        super(portletDescriptor);
    }

    /**
     * {@inheritDoc}
     * 
     * @throws Exception
     */
    public List<MenuItem> getMenuItems() throws Exception {
        Language lang = getLanguage();
        List<MenuItem> result = new ArrayList<MenuItem>();
        PandaPortalApplication app = getApplication();
        UrlCreator urlCreator = app.getUrlCreator();
        String module = getPortletDescriptor().getModule();
        ModuleDescriptor md = getModule(module);
        module = md.getUrlMapping();
        String url;
        MenuItem menuItem;

        url = urlCreator.createUri(module, GscConstants.ACTION_HOME);
        menuItem =
                new MenuItem(lang.getMessage(GscLangConstants.MSG_HOME), url);
        menuItem.setHighlighted(true);
        result.add(menuItem);

        /*
         * GscManager fm = app.getBundleManager().getService(GscManager.class);
         * Box[] boxTree = fm.getBoxTree(); for ( Box box : boxTree ) {
         * CardModel bm = CardModel.getInstance(box); url = bm.getUrlView(); if
         * ( url != null ) { menuItem = new MenuItem(bm.getTitle(), url);
         * menuItem.setBulleted(true); result.add(menuItem); } }
         */

        return result;
    }

    /**
     * {@inheritDoc}
     */
    public String getTitle() {
        Language lang = getLanguage();
        return lang.getMessage(GscLangConstants.MSG_MAIN_MENU);
    }
}
