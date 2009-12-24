package com.greenstorm.gsc.bo;

import java.io.Serializable;

public class BoxModerateMember implements Serializable {

    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = 6061139371348264496L;

    private int boxId;

    private Object memberId;

    public BoxModerateMember() {
    }

    public int getBoxId() {
        return boxId;
    }

    public void setBoxId(int boxId) {
        this.boxId = boxId;
    }

    public Integer getMemberId() {
        return (Integer)memberId;
    }

    public void setMemberId(Object memberId) {
        this.memberId = memberId;
    }
}
