package com.greenstorm.gsc.model;

import com.greenstorm.gsc.bo.TopicContent;

/**
 * A data model bean that encapsulates a TopicContent object and delegates
 * selected method calls to this object.
 * 
 * @author Thanh Ba Nguyen
 */
public class TopicContentModel {
    private TopicContent topicContent;

    public TopicContentModel(TopicContent topicContent) {
        this.topicContent = topicContent;
    }

    public int getId() {
        return topicContent.getId();
    }

    public int getTopicId() {
        return topicContent.getTopicId();
    }

    public int getOrder() {
        return topicContent.getOrder();
    }

    public String getContent() {
        return topicContent.getContent();
    }
}
