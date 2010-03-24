package org.ddth.eis.bo.appconfig;

import java.util.ArrayList;
import java.util.List;

import org.hibernate.Session;
import org.hibernate.SessionFactory;

/**
 * Hibernate-implementation of AppConfigManager.
 * 
 * @author Thanh Ba Nguyen <btnguyen2k@gmail.com>
 * 
 */
public class HibernateAppConfigManager implements AppConfigManager {

    private final static String ENTITY_APP_CONFIG            = AppConfig.class.getName();

    private static final String HQL_SELECT_CONFIGS_BY_DOMAIN = "FROM "
                                                                     + ENTITY_APP_CONFIG
                                                                     + " WHERE configKey.domain = :domain";

    private static final String HQL_SELECT_ALL_CONFIGS       = "FROM "
                                                                     + ENTITY_APP_CONFIG
                                                                     + " ORDER BY configKey.domain, configKey.key";

    private SessionFactory      sessionFactory;

    public SessionFactory getSessionFactory() {
        return this.sessionFactory;
    }

    public void setSessionFactory(SessionFactory sessionFactory) {
        this.sessionFactory = sessionFactory;
    }

    private Session getSession() {
        SessionFactory sessionFactory = getSessionFactory();
        Session session = null;// sessionFactory.getCurrentSession();
        if ( session == null ) {
            session = sessionFactory.openSession();
        }
        return session;
    }

    private void closeSession(Session session) {
        session.flush();
        session.close();
    }

    /**
     * {@inheritDoc}
     */
    public void destroy() {
        // do nothing
    }

    /**
     * {@inheritDoc}
     */
    public void init() {
        // do nothing
    }

    /**
     * {@inheritDoc}
     */
    public void deleteConfig(AppConfig config) {
        Session session = getSession();
        try {
            session.delete(ENTITY_APP_CONFIG, config);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public void deleteConfig(AppConfigKey configKey) {
        deleteConfig(loadConfig(configKey));
    }

    /**
     * {@inheritDoc}
     */
    public void deleteConfig(String domain, String key) {
        deleteConfig(loadConfig(domain, key));
    }

    /**
     * {@inheritDoc}
     */
    public AppConfig[] loadAllConfigs() {
        Session session = getSession();
        try {
            List<?> hResult = session.createQuery(HQL_SELECT_ALL_CONFIGS).setCacheable(true).list();
            List<AppConfig> result = new ArrayList<AppConfig>();
            if ( hResult != null ) {
                for ( Object o : hResult ) {
                    result.add((AppConfig) o);
                }
            }
            return result.toArray(new AppConfig[0]);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public AppConfig loadConfig(AppConfigKey configKey) {
        Session session = getSession();
        try {
            return (AppConfig) session.get(ENTITY_APP_CONFIG, configKey);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public AppConfig loadConfig(String domain, String key) {
        return loadConfig(AppConfigKey.createAppConfigKey(domain, key));
    }

    /**
     * {@inheritDoc}
     */
    public AppConfig[] loadConfigs(String domain) {
        Session session = getSession();
        try {
            List<?> hResult = session.createQuery(HQL_SELECT_CONFIGS_BY_DOMAIN)
                    .setParameter("domain", domain).setCacheable(true).list();
            List<AppConfig> result = new ArrayList<AppConfig>();
            if ( hResult != null ) {
                for ( Object o : hResult ) {
                    result.add((AppConfig) o);
                }
            }
            return result.toArray(new AppConfig[0]);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public void saveConfig(AppConfig config) {
        Session session = getSession();
        try {
            session.saveOrUpdate(ENTITY_APP_CONFIG, config);
        } finally {
            closeSession(session);
        }
    }
}
