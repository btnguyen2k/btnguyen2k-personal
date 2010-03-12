package org.ddth.eis;

import org.ddth.daf.impl.BooleanPassport;
import org.ddth.eis.bo.daf.DafPermission;

public class EisPermissionConstants {
    public final static DafPermission PERMISSION_STAFF   = new DafPermission(
                                                                 EisConstants.APP_DOMAIN, "STAFF",
                                                                 "Staff-level permission.",
                                                                 BooleanPassport.class.getName());
    public final static DafPermission PERMISSION_HR      = new DafPermission(
                                                                 EisConstants.APP_DOMAIN, "HR",
                                                                 "HR-level permission.",
                                                                 BooleanPassport.class.getName());
    public final static DafPermission PERMISSION_MANAGER = new DafPermission(
                                                                 EisConstants.APP_DOMAIN,
                                                                 "MANAGER",
                                                                 "Manager-level permission.",
                                                                 BooleanPassport.class.getName());
    public final static DafPermission PERMISSION_IT      = new DafPermission(
                                                                 EisConstants.APP_DOMAIN, "IT",
                                                                 "IT-level permission.",
                                                                 BooleanPassport.class.getName());
}
