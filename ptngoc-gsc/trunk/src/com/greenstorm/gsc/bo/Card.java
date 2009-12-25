package com.greenstorm.gsc.bo;

/**
 * Represents a GSC Card
 * 
 * @author btnguyen2k@gmail.com
 * 
 */
public class Card {
    private String id;

    private int issuedTimestamp;

    public String getId() {
        return this.id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public int getIssuedTimestamp() {
        return issuedTimestamp;
    }

    public void setIssuedTimestamp(int issuedTimestamp) {
        this.issuedTimestamp = issuedTimestamp;
    }
}
