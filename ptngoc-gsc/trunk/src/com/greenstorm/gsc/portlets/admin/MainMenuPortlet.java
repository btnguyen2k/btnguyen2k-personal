package com.greenstorm.gsc.portlets.admin;

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

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.TxbbLangConstants;
import com.greenstorm.gsc.TxbbPermissionConstants;
import com.greenstorm.gsc.portlets.BaseTxbbPortlet;

public class MainMenuPortlet extends BaseTxbbPortlet implements
        BorderedPortlet, SingletonPortlet, MenuPortlet, TitledPortlet {

    private final static String MENU_ICON_ADMIN_HOME = "admin_home";

    private final static String MENU_ICON_LOGOUT = "logout";

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
     */
    public boolean isVisible() throws Exception {
        PandaPortalApplication app = getApplication();
        return app.hasPermission(TxbbPermissionConstants.PERMISSION_ACCESS_ADMIN_CP);
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

        url = urlCreator.createUri(module, TxbbConstants.ACTION_ADMIN_HOME);
        menuItem =
                new MenuItem(lang.getMessage(TxbbLangConstants.MSG_ADMIN_HOME),
                        url);
        menuItem.setIcon(MENU_ICON_ADMIN_HOME);
        menuItem.setHighlighted(true);
        result.add(menuItem);

        url =
                urlCreator.createUri(module,
                        TxbbConstants.ACTION_ADMIN_LIST_BOXES);
        menuItem =
                new MenuItem(
                        lang.getMessage(TxbbLangConstants.MSG_BOX_MANAGEMENT),
                        url);
        menuItem.setBulleted(true);
        // menuItem.setIcon(MENU_ICON_ADMIN_HOME);
        result.add(menuItem);

        url = urlCreator.createUri(module, TxbbConstants.ACTION_LOGOUT);
        menuItem =
                new MenuItem(lang.getMessage(TxbbLangConstants.MSG_LOGOUT), url);
        menuItem.setIcon(MENU_ICON_LOGOUT);
        result.add(menuItem);

        // MenuItem
        return result;
    }

    /**
     * {@inheritDoc}
     */
    public String getTitle() {
        Language lang = getLanguage();
        return lang.getMessage(TxbbLangConstants.MSG_ADMIN_MENU);
    }
}
