package com.greenstorm.gsc.model;

import org.ddth.panda.core.ApplicationRepository;
import org.ddth.panda.portal.PandaPortalApplication;

import com.greenstorm.gsc.bo.Invoice;

public class InvoiceModel {
    private Invoice invoice;

    /**
     * Gets an instance of InvoiceModel.
     * 
     * @param invoice Invoice
     * @return InvoiceModel
     */
    public static InvoiceModel getInstance(Invoice invoice) {
        PandaPortalApplication app =
                (PandaPortalApplication)ApplicationRepository.getCurrentApp();
        int invoiceId = invoice.getId();
        InvoiceModel result =
                app.getAttribute(String.valueOf(invoiceId), InvoiceModel.class);
        if ( result == null ) {
            result = new InvoiceModel(invoice);
        } else {
            result.invoice = invoice;
            result.invalidateCache();
        }
        return result;
    }

    protected InvoiceModel(Invoice invoice) {
        this.invoice = invoice;
    }

    protected void invalidateCache() {
        // TODO
    }

    public int getId() {
        return invoice.getId();
    }
}
