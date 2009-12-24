package com.greenstorm.gsc;

import java.lang.reflect.Field;
import java.text.MessageFormat;

import org.ddth.daf.impl.BooleanPassport;
import org.ddth.panda.core.daf.DafPermission;
import org.ddth.txbb.TxbbConstants;
import org.ddth.txbb.TxbbPermissionConstants;

public class TxbbPermissionConstants {

    public final static DafPermission PERMISSION_VIEW_BOX =
            new DafPermission(TxbbConstants.MODULE_KEY_NAME, "VIEW_BOX",
                    "Has permission to view topics in a box.",
                    BooleanPassport.class.getName());

    public final static DafPermission PERMISSION_PUBLISH_TOPIC =
            new DafPermission(TxbbConstants.MODULE_KEY_NAME, "PUBLISH_TOPIC",
                    "Has permission to publish topics.",
                    BooleanPassport.class.getName());

    public final static DafPermission PERMISSION_ACCESS_ADMIN_CP =
            new DafPermission(
                    TxbbConstants.MODULE_KEY_NAME,
                    "ACCESS_ADMINCP",
                    "Has permission to access TXBB Administration Control Panel.",
                    BooleanPassport.class.getName());

    public final static DafPermission PERMISSION_MANAGE_BOXES =
            new DafPermission(TxbbConstants.MODULE_KEY_NAME, "MANAGE_BOXES",
                    "Has permission to manage boxes.",
                    BooleanPassport.class.getName());

    public static void main(String[] argv) throws IllegalArgumentException,
            IllegalAccessException {
        Field[] fields = TxbbPermissionConstants.class.getDeclaredFields();
        for ( Field f : fields ) {
            if ( f.getType() == DafPermission.class ) {
                DafPermission permission = (DafPermission)f.get(null);
                String pattern =
                        "INSERT INTO daf_permission (pmodule, paction, pdescription, ppassport_class_name)"
                                + "\nVALUES ({0}, {1}, {2}, {3});\n";
                String sql =
                        MessageFormat.format(pattern, new Object[]{
                            "'" + permission.getModule() + "'",
                            "'" + permission.getAction() + "'",
                            "'" + permission.getDescription() + "'",
                            "'" + permission.getPassportClassName() + "'" });
                System.out.print(sql);
            }
        }
    }
}
