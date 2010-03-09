package org.ddth.eis.controller.home;

import org.ddth.eis.EisConstants;
import org.ddth.eis.controller.BaseController;
import org.ddth.eis.controller.IRequireAuthenticationController;

public class IndexController extends BaseController implements IRequireAuthenticationController {

    private final static String VIEW_NAME = EisConstants.MODULE_HOME + "."
                                                  + EisConstants.ACTION_INDEX;

    /**
     * {@inheritDoc}
     */
    @Override
    protected String getViewName() {
        return VIEW_NAME;
    }
}
