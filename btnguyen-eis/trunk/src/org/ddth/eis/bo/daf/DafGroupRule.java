package org.ddth.eis.bo.daf;

import java.io.Serializable;

import org.ddth.daf.GroupRule;

public class DafGroupRule extends GroupRule implements Serializable {
    /**
     * Auto-generated serial version UID
     */
    private static final long serialVersionUID = 8952631675553175828L;

    /**
     * Constructs a new DafGroupRule object.
     */
    public DafGroupRule() {
    }

    /**
     * Constructs a new DafGroupRule object.
     * 
     * @param group DafGroup
     * @param permission DafPermission
     * @param isGlobal boolean
     */
    public DafGroupRule(DafGroup group, DafPermission permission,
            boolean isGlobal) {
        super(group, permission, isGlobal);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public DafGroup getGroup() {
        return (DafGroup)super.getGroup();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public DafPermission getPermission() {
        return (DafPermission)super.getPermission();
    }
}
