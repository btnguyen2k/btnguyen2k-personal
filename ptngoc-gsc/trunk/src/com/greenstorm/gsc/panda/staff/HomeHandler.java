package com.greenstorm.gsc.panda.staff;

import org.ddth.panda.core.ControlForward;
import org.ddth.panda.core.impl.controlforward.ViewControlForward;

public class HomeHandler extends BaseStaffActionHandler {

    /**
     * {@inheritDoc}
     */
    @Override
    protected ControlForward execute() throws Exception {
        populateDataModels();
        return new ViewControlForward(getModule(), getAction());
    }
}
