package com.greenstorm.gsc.bo;

import java.io.Serializable;

public class Publishing implements Serializable {

    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = -3293873970876300745L;

    private int boxId;

    private int topicId;

    private int timestamp;

    public Publishing() {
    }

    public int getBoxId() {
        return boxId;
    }

    public void setBoxId(int boxId) {
        this.boxId = boxId;
    }

    public int getTopicId() {
        return topicId;
    }

    public void setTopicId(int topicId) {
        this.topicId = topicId;
    }

    public int getTimestamp() {
        return timestamp;
    }

    public void setTimestamp(int timestamp) {
        this.timestamp = timestamp;
    }
}
