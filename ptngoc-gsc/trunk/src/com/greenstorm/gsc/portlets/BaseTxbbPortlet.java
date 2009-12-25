package com.greenstorm.gsc.portlets;

import org.ddth.panda.portal.module.ModuleDescriptor;
import org.ddth.panda.portal.module.PortletDescriptor;
import org.ddth.panda.portal.portlet.PortalPortlet;

import com.greenstorm.gsc.GscConstants;

public abstract class BaseTxbbPortlet extends PortalPortlet {

    /**
     * Constructs a new BaseTxbbPortlet
     * 
     * @param portletDescriptor PortletDescriptor
     */
    public BaseTxbbPortlet(PortletDescriptor portletDescriptor) {
        super(portletDescriptor);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public ModuleDescriptor getMyModule() {
        return getModule(GscConstants.MODULE_NAME);
    }
}
