package org.ddth.eis.model;

import org.ddth.panda.daf.DafUser;

public class DafUserModel {

    private DafUser dafUser;

    private DafUserModel(DafUser dafUser) {
        this.dafUser = dafUser;
    }

    public static DafUserModel getInstance(DafUser dafUser) {
        return new DafUserModel(dafUser);
    }

    private String displayName;

    public String getDisplayName() {
        if ( displayName == null ) {
            if ( dafUser.getTitle() != null ) {
                displayName = dafUser.getTitle() + " ";
            } else {
                displayName = "";
            }
        }
        displayName += dafUser.getFirstName();
        if ( dafUser.getMiddleName() != null ) {
            displayName += " " + dafUser.getMiddleName();
        }
        displayName += " " + dafUser.getLastName();
        return displayName;
    }
}
