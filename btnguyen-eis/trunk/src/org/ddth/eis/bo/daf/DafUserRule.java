package org.ddth.eis.bo.daf;

import java.io.Serializable;

import org.ddth.daf.UserRule;

public class DafUserRule extends UserRule implements Serializable {

    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = 4089280534634313386L;

    /**
     * Constructs a new DafUserRule object.
     */
    protected DafUserRule() {
    }

    /**
     * Constructs a new DafUserRule object.
     * 
     * @param user DafUser
     * @param permission DafPermission
     * @param isGlobal boolean
     */
    public DafUserRule(DafUser user, DafPermission permission, boolean isGlobal) {
        super(user, permission, isGlobal);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public DafUser getUser() {
        return (DafUser)super.getUser();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public DafPermission getPermission() {
        return (DafPermission)super.getPermission();
    }
}
