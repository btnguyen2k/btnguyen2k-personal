package com.greenstorm.gsc.bo;

import java.io.Serializable;

public class BoxPermissionGroupPublishTopic implements Serializable {

    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = -2710906488291173937L;

    private int boxId;

    private Object groupId;

    public BoxPermissionGroupPublishTopic() {
    }

    public int getBoxId() {
        return boxId;
    }

    public void setBoxId(int boxId) {
        this.boxId = boxId;
    }

    public Integer getGroupId() {
        return (Integer)groupId;
    }

    public void setGroupId(Object groupId) {
        this.groupId = groupId;
    }
}
