package com.greenstorm.gsc.model;

import org.ddth.panda.core.ApplicationRepository;
import org.ddth.panda.portal.PandaPortalApplication;

import com.greenstorm.gsc.bo.Customer;

public class CustomerModel {
    private Customer customer;

    /**
     * Gets an instance of CustomerModel.
     * 
     * @param customer Customer
     * @return CustomerModel
     */
    public static CustomerModel getInstance(Customer customer) {
        PandaPortalApplication app =
                (PandaPortalApplication)ApplicationRepository.getCurrentApp();
        int customerId = customer.getId();
        CustomerModel result =
                app.getAttribute(String.valueOf(customerId),
                        CustomerModel.class);
        if ( result == null ) {
            result = new CustomerModel(customer);
        } else {
            result.customer = customer;
            result.invalidateCache();
        }
        return result;
    }

    protected CustomerModel(Customer customer) {
        this.customer = customer;
    }

    protected void invalidateCache() {
        // TODO
    }

    public int getId() {
        return customer.getId();
    }
}
