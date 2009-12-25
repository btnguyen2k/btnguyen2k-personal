package com.greenstorm.gsc.bo;

import org.ddth.common.HibernateSessionFactory;
import org.hibernate.HibernateException;
import org.hibernate.Session;

public class HibernateGscManager implements GscManager {

    private final static String ENTITY_CARD = Card.class.getName();

    private final static String ENTITY_CUSTOMER = Customer.class.getName();

    private final static String ENTITY_INVOICE = Invoice.class.getName();

    private HibernateSessionFactory hsf;

    public void setHibernateSessionFactory(HibernateSessionFactory hfs) {
        this.hsf = hfs;
    }

    public HibernateSessionFactory getHibernateSessionFactory() {
        return hsf;
    }

    public Session getHibernateSession() {
        try {
            return getHibernateSessionFactory().getHibernateSession(true);
        } catch ( Exception e ) {
            throw new RuntimeException(e);
        }
    }

    public void releaseHibernateSession(Session session, boolean hasError) {
        try {
            getHibernateSessionFactory().releaseHibernateSession(session,
                    !hasError);
        } catch ( Exception e ) {
            throw new RuntimeException(e);
        }
    }

    public void init() {
    }

    public void destroy() {
    }

    /**
     * {@inheritDoc}
     */
    public Customer createCustomer(Customer customer) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            session.save(ENTITY_CUSTOMER, customer);
            return customer;
        } catch ( HibernateException e ) {
            hasError = true;
            throw e;
        } finally {
            releaseHibernateSession(session, hasError);
        }
    }

    /**
     * {@inheritDoc}
     */
    public void deleteCustomer(Customer customer) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            session.delete(ENTITY_CUSTOMER, customer);
        } catch ( HibernateException e ) {
            hasError = true;
            throw e;
        } finally {
            releaseHibernateSession(session, hasError);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Customer getCustomer(int customerId) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            return (Customer)session.get(ENTITY_CUSTOMER, customerId);
        } catch ( HibernateException e ) {
            hasError = true;
            throw e;
        } finally {
            releaseHibernateSession(session, hasError);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Customer updateCustomer(Customer customer) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            session.update(ENTITY_CUSTOMER, customer);
            return customer;
        } catch ( HibernateException e ) {
            hasError = true;
            throw e;
        } finally {
            releaseHibernateSession(session, hasError);
        }
    }
}
