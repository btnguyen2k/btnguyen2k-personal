package com.greenstorm.gsc.model;

import org.ddth.panda.core.ApplicationRepository;
import org.ddth.panda.portal.PandaPortalApplication;

import com.greenstorm.gsc.bo.Card;

public class CardModel {
    private Card card;

    /**
     * Gets an instance of CardModel.
     * 
     * @param card Card
     * @return CardModel
     */
    public static CardModel getInstance(Card card) {
        PandaPortalApplication app =
                (PandaPortalApplication)ApplicationRepository.getCurrentApp();
        String cardId = card.getId();
        CardModel result = app.getAttribute(cardId, CardModel.class);
        if ( result == null ) {
            result = new CardModel(card);
        } else {
            result.card = card;
            result.invalidateCache();
        }
        return result;
    }

    protected CardModel(Card card) {
        this.card = card;
    }

    protected void invalidateCache() {
        // TODO
    }

    public String getId() {
        return card.getId();
    }
}
