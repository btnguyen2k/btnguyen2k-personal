package com.greenstorm.gsc.osgi;

import java.util.Properties;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.ddth.common.HibernateSessionFactory;
import org.ddth.panda.core.ApplicationRepository;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.osgi.AbstractActivator;
import org.ddth.panda.portal.utils.PandaPortalAppPreFinishHook;
import org.ddth.panda.utils.ServletUtils;
import org.ddth.txbb.osgi.Activator;
import org.hibernate.Session;
import org.osgi.framework.BundleContext;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.bo.HibernateGscManager;
import com.greenstorm.gsc.bo.GscManager;

public class Activator extends AbstractActivator {

    private final static String HIBERNATE_CONFIG_FILE =
            "/hibernate/hibernate.properties";

    private BundleContext context;

    private static Log LOGGER = LogFactory.getLog(Activator.class);

    /**
     * {@inheritDoc}
     */
    protected String getHibernateConfigFile() {
        return HIBERNATE_CONFIG_FILE;
    }

    /**
     * {@inheritDoc}
     */
    protected String getModuleName() {
        return TxbbConstants.MODULE_NAME;
    }

    /**
     * {@inheritDoc}
     */
    public void start(BundleContext context) throws Exception {
        this.context = context;
        super.start(context);

        registerTxbbManager();
    }

    protected void registerTxbbManager() throws Exception {
        HibernateGscManager hfm = new HibernateGscManager();
        hfm.setHibernateSessionFactory(new AppHibernateSessionFactory(
                getHibernateSessionFactory()));
        hfm.init();
        context.registerService(GscManager.class.getName(), hfm,
                new Properties());
        ServletUtils.setContextAttribute(
                TxbbConstants.SERVLET_CTX_ATTR_TXBB_MANAGER, hfm);
    }

    private class AppHibernateSessionFactory implements HibernateSessionFactory {

        private final static String APP_ATTR_KEY = "TXBB_HIBERNATE_SESSION";

        private HibernateSessionFactory hsf;

        public AppHibernateSessionFactory(HibernateSessionFactory hsf) {
            this.hsf = hsf;
        }

        private PandaPortalApplication getApp() {
            return (PandaPortalApplication)ApplicationRepository.getCurrentApp();
        }

        /**
         * {@inheritDoc}
         */
        public Session getHibernateSession() throws Exception {
            return getHibernateSession(true);
        }

        /**
         * {@inheritDoc}
         */
        public Session getHibernateSession(boolean startTransaction)
                throws Exception {
            PandaPortalApplication app = getApp();
            Session session = app.getAttribute(APP_ATTR_KEY, Session.class);
            if ( session == null ) {
                session = hsf.getHibernateSession(startTransaction);
                app.addPreFinishHook(new PandaPortalAppPreFinishHookImpl(
                        session));
                app.setAttribute(APP_ATTR_KEY, session);
            }
            return session;
        }

        private class PandaPortalAppPreFinishHookImpl implements
                PandaPortalAppPreFinishHook {
            private Session session;

            public PandaPortalAppPreFinishHookImpl(Session session) {
                this.session = session;
            }

            public void execute(PandaPortalApplication app, boolean hasException) {
                try {
                    hsf.releaseHibernateSession(session, !hasException);
                } catch ( Exception e ) {
                    LOGGER.error(e.getMessage(), e);
                }
            }
        }

        /**
         * {@inheritDoc}
         */
        public void releaseHibernateSession(Session session) throws Exception {
            // EMPTY
        }

        /**
         * {@inheritDoc}
         */
        public void releaseHibernateSession(Session session, boolean hasError)
                throws Exception {
            // EMPTY
        }
    }
}
