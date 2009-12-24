package com.greenstorm.gsc.bo;

import java.io.Serializable;

public class TopicContent implements Serializable {

    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = 4170763908792665701L;

    private int id;

    private int topicId;

    private int order;

    private String content;

    /**
     * Constructs a new TopicContent object.
     */
    public TopicContent() {
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getTopicId() {
        return topicId;
    }

    public void setTopicId(int topicId) {
        this.topicId = topicId;
    }

    public int getOrder() {
        return order;
    }

    public void setOrder(int order) {
        this.order = order;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }
}
