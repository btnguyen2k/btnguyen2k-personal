package org.ddth.eis.controller.home;

import org.ddth.eis.EisConstants;
import org.ddth.eis.controller.BaseController;
import org.ddth.panda.UrlCreator;
import org.springframework.web.servlet.ModelAndView;
import org.springframework.web.servlet.View;
import org.springframework.web.servlet.view.RedirectView;

/**
 * Controller for action: login
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class LogoutController extends BaseController {

    /**
     * {@inheritDoc}
     */
    @Override
    protected String getViewName() {
        return null;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected ModelAndView execute() {
        removeSessionAttribute(EisConstants.SESSION_CURRENT_USERNAME);
        UrlCreator urlCreator = getUrlCreator();
        String uri = urlCreator.createUri(EisConstants.MODULE_HOME, EisConstants.ACTION_HOME_INDEX);
        View view = new RedirectView(uri);
        return new ModelAndView(view);
    }
}
