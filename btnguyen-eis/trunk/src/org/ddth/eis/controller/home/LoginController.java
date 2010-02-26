package org.ddth.eis.controller.home;

import org.ddth.eis.EisConstants;
import org.ddth.eis.controller.BaseController;

/**
 * Controller for action: login
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class LoginController extends BaseController {

    private final static String VIEW_NAME = EisConstants.MODULE_HOME + "."
                                                  + EisConstants.ACTION_INDEX;

    /**
     * {@inheritDoc}
     */
    protected void execute() throws Exception {
        modelController();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected String getViewName() {
        return VIEW_NAME;
    }
}
