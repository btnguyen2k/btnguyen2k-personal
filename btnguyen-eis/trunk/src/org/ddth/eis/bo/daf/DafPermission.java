package org.ddth.eis.bo.daf;

import java.io.Serializable;

import org.ddth.daf.impl.PermissionImpl;

public class DafPermission extends PermissionImpl implements Serializable {
    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = 2843335464279911719L;

    /**
     * Constructs a new DafPermission object.
     */
    public DafPermission() {
    }

    /**
     * Constructs a new DafPermission object.
     * 
     * @param module String
     * @param action String
     * @param description String
     * @param passportClassName String
     */
    public DafPermission(String module, String action, String description,
            String passportClassName) {
        super(module, action, passportClassName, description);
    }

    /**
     * Alias of {@link #getDomain()}
     * 
     * @return String
     */
    public String getModule() {
        return getDomain();
    }

    /**
     * Alias of {@link #setDomain(String)}
     * 
     * @return String
     */
    public void setModule(String module) {
        setDomain(module);
    }
}
