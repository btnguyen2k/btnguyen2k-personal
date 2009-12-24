package com.greenstorm.gsc.bo;

import java.io.Serializable;
import java.util.Set;

import org.ddth.txbb.bo.TopicContent;

public class Topic implements Serializable {

    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = 801237416861941794L;

    public final static String TYPE_QUESTION = "question";

    public final static String TYPE_ARTICLE = "article";

    public final static String TYPE_INFORMATION = "information";

    private int id;

    private Integer memberId;

    private String title;

    private String type;

    private int creationTimestamp, lastpostTimestamp, lastupdateTimestamp;

    private int numViews, numPosts;

    private boolean isPublished, isLocked;

    // private List<TopicContent> topicContents
    private Set<TopicContent> topicContents;

    /**
     * Constructs a new Topic object.
     */
    public Topic() {
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public Integer getMemberId() {
        return memberId;
    }

    public void setMemberId(Integer memberId) {
        this.memberId = memberId;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public int getCreationTimestamp() {
        return creationTimestamp;
    }

    public void setCreationTimestamp(int creationTimestamp) {
        this.creationTimestamp = creationTimestamp;
    }

    public int getLastpostTimestamp() {
        return lastpostTimestamp;
    }

    public void setLastpostTimestamp(int lastpostTimestamp) {
        this.lastpostTimestamp = lastpostTimestamp;
    }

    public int getLastupdateTimestamp() {
        return lastupdateTimestamp;
    }

    public void setLastupdateTimestamp(int lastupdateTimestamp) {
        this.lastupdateTimestamp = lastupdateTimestamp;
    }

    public int getNumViews() {
        return numViews;
    }

    public void setNumViews(int numViews) {
        this.numViews = numViews;
    }

    public int getNumPosts() {
        return numPosts;
    }

    public void setNumPosts(int numPosts) {
        this.numPosts = numPosts;
    }

    public boolean isPublished() {
        return isPublished;
    }

    public boolean getIsPublished() {
        return isPublished;
    }

    public void setPublished(boolean isPublished) {
        this.isPublished = isPublished;
    }

    public void setIsPublished(boolean isPublished) {
        this.isPublished = isPublished;
    }

    public boolean isLocked() {
        return isLocked;
    }

    public boolean getIsLocked() {
        return isLocked;
    }

    public void setLocked(boolean isLocked) {
        this.isLocked = isLocked;
    }

    public void setIsLocked(boolean isLocked) {
        this.isLocked = isLocked;
    }

    public Set<TopicContent> getTopicContents() {
        return topicContents;
    }

    public void setTopicContents(Set<TopicContent> topicContents) {
        this.topicContents = topicContents;
    }
}
