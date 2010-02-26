package org.ddth.eis.controller;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.ddth.eis.EisConstants;
import org.ddth.panda.UrlCreator;
import org.springframework.beans.BeansException;
import org.springframework.context.ApplicationContext;
import org.springframework.context.ApplicationContextAware;
import org.springframework.web.servlet.ModelAndView;
import org.springframework.web.servlet.View;
import org.springframework.web.servlet.mvc.Controller;
import org.springframework.web.servlet.view.RedirectView;

/**
 * Global-default controller. This controller redirects to the "index" page.
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class ReturnHomeController implements Controller, ApplicationContextAware {

    private UrlCreator urlCreator;

    /**
     * {@inheritDoc}
     */
    public ModelAndView handleRequest(HttpServletRequest request, HttpServletResponse response)
            throws Exception {
        String uri = urlCreator.createUri(EisConstants.MODULE_HOME, EisConstants.ACTION_INDEX);
        View view = new RedirectView(uri);
        return new ModelAndView(view);
    }

    /**
     * {@inheritDoc}
     */
    public void setApplicationContext(ApplicationContext context) throws BeansException {
        this.urlCreator = context.getBean(EisConstants.BEAN_URL_CREATOR, UrlCreator.class);
    }
}
