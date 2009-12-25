package com.greenstorm.gsc.bo;

/**
 * Represents a card.
 * 
 * @author btnguyen2k@gmail.com
 * 
 */
public class Customer {
    private int id;

    private Card card, refCard;

    private String name;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public Card getRefCard() {
        return refCard;
    }

    public void setRefCard(Card refCard) {
        this.refCard = refCard;
    }

    public Card getCard() {
        return card;
    }

    public void setCard(Card card) {
        this.card = card;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
