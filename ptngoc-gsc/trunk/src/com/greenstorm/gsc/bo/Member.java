package com.greenstorm.gsc.bo;

import org.ddth.panda.core.daf.DafUser;

public class Member {
    private DafUser dafUser;

    private int numTopics, numPosts;

    private double txbbPoints;

    private int lastvisitTimestamp;

    private boolean viewSignature, viewAvatar;

    private String signature;

    /**
     * Constructs a new Member object.
     */
    public Member() {
    }

    /**
     * Constructs a new Member object from a DafUser object.
     * 
     * @param dafUser DafUser
     */
    public Member(DafUser dafUser) {
        setDafUser(dafUser);
    }

    public Integer getId() {
        return (Integer)dafUser.getId();
    }

    public void setId(Integer id) {
        // empty
    }

    public DafUser getDafUser() {
        return dafUser;
    }

    public void setDafUser(DafUser dafUser) {
        this.dafUser = dafUser;
    }

    public int getNumTopics() {
        return numTopics;
    }

    public void setNumTopics(int numTopics) {
        this.numTopics = numTopics;
    }

    public int getNumPosts() {
        return numPosts;
    }

    public void setNumPosts(int numPosts) {
        this.numPosts = numPosts;
    }

    public double getTxbbPoints() {
        return txbbPoints;
    }

    public void setTxbbPoints(double txbbPoints) {
        this.txbbPoints = txbbPoints;
    }

    public int getLastvisitTimestamp() {
        return lastvisitTimestamp;
    }

    public void setLastvisitTimestamp(int lastvisitTimestamp) {
        this.lastvisitTimestamp = lastvisitTimestamp;
    }

    public boolean isViewSignature() {
        return viewSignature;
    }

    public void setViewSignature(boolean viewSignature) {
        this.viewSignature = viewSignature;
    }

    public boolean isViewAvatar() {
        return viewAvatar;
    }

    public void setViewAvatar(boolean viewAvatar) {
        this.viewAvatar = viewAvatar;
    }

    public String getSignature() {
        return signature;
    }

    public void setSignature(String signature) {
        this.signature = signature;
    }
}
