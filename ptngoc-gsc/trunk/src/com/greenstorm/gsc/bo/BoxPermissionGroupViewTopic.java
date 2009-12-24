package com.greenstorm.gsc.bo;

import java.io.Serializable;

public class BoxPermissionGroupViewTopic implements Serializable {

    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = -3252378484982092566L;

    private int boxId;

    private Object groupId;

    public BoxPermissionGroupViewTopic() {
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
