package org.ddth.eis.bo.daf;

import java.io.Serializable;

import org.ddth.daf.impl.RoleImpl;

public class DafRole extends RoleImpl implements Serializable {
    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = -61670212112736481L;

    /**
     * Constructs a new DafRole object.
     */
    protected DafRole() {
    }

    /**
     * Constructs a new DafRole object.
     * 
     * @param user DafUser
     * @param group DafGroup
     */
    public DafRole(DafUser user, DafGroup group) {
        super(group, user);
    }

    /**
     * Gets the associated group.
     * 
     * @return DafRole
     */
    public DafGroup getGroup() {
        return (DafGroup)super.getGroup();
    }

    /**
     * Sets the associated group.
     * 
     * @param group DafGroup
     */
    protected void setGroup(DafGroup group) {
        super.setGroup(group);
    }

    /**
     * Gets the associated user.
     * 
     * @return DafUser
     */
    public DafUser getUser() {
        return (DafUser)super.getUser();
    }

    /**
     * Sets the associated user.
     * 
     * @param user DafUser
     */
    protected void setUser(DafUser user) {
        super.setUser(user);
    }
}