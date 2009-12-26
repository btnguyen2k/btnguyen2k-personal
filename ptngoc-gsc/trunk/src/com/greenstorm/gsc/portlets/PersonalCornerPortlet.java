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

public class PersonalCornerPortlet extends BaseTxbbPortlet implements
        BorderedPortlet, SingletonPortlet, MenuPortlet, TitledPortlet {

    /**
     * Constructs a new MainMenuPortlet object.
     * 
     * @param portletDescriptor PortletDescriptor
     */
    public PersonalCornerPortlet(PortletDescriptor portletDescriptor) {
        super(portletDescriptor);
    }

    /**
     * {@inheritDoc}
     */
    public boolean isVisible() throws Exception {
        PandaPortalApplication app = getApplication();
        return app.getCurrentUser() != null;
    }

    /**
     * {@inheritDoc}
     * 
     * @throws Exception
     */
    public List<MenuItem> getMenuItems() {
        Language lang = getLanguage();
        List<MenuItem> result = new ArrayList<MenuItem>();
        PandaPortalApplication app = getApplication();
        UrlCreator urlCreator = app.getUrlCreator();
        String module = getPortletDescriptor().getModule();
        ModuleDescriptor md = getModule(module);
        module = md.getUrlMapping();
        String url;
        MenuItem menuItem;

        url = urlCreator.createUri(module, GscConstants.ACTION_PC_INDEX);
        menuItem =
                new MenuItem(lang.getMessage(GscLangConstants.MSG_PC_HOME), url);
        menuItem.setHighlighted(true);
        result.add(menuItem);

        return result;
    }

    /**
     * {@inheritDoc}
     */
    public String getTitle() {
        Language lang = getLanguage();
        return lang.getMessage(GscLangConstants.MSG_PC);
    }
}
