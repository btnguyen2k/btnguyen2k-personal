package org.ddth.eis.controller;

/**
 * Controller implements this interface to indicate that access to this controller require
 * authentication.
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public interface IRequireAuthenticationController {
    /**
     * Gets the login URL to redirect to.
     * 
     * @return String
     */
    public String getLoginUrl();
}
