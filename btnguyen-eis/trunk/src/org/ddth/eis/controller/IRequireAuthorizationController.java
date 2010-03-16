package org.ddth.eis.controller;

/**
 * Controller implements this interface to indicate that access to the controller requires
 * authorization.
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public interface IRequireAuthorizationController {
    /**
     * Checks if the current user can access this controler.
     * 
     * @return boolean
     */
    public boolean canAccess();
}
