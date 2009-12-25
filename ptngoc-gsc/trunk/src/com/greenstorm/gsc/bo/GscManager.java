package com.greenstorm.gsc.bo;

public interface GscManager {

    /**
     * Creates a new customer account.
     * 
     * @param Customer customer
     * @return Customer
     */
    public Customer createCustomer(Customer customer);

    /**
     * Gets a customer account.
     * 
     * @param int customerId
     * @return Customer
     */
    public Customer getCustomer(int customerId);

    /**
     * Updates an existing customer account.
     * 
     * @param customer Customer
     * @return Customer
     */
    public Customer updateCustomer(Customer customer);

    /**
     * Deletes a customer account.
     * 
     * @param customer Customer
     */
    public void deleteCustomer(Customer customer);
}
