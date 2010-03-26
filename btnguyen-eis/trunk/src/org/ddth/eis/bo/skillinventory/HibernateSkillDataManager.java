package org.ddth.eis.bo.skillinventory;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashSet;
import java.util.List;

import org.ddth.panda.daf.DafUser;
import org.hibernate.Query;
import org.hibernate.Session;
import org.hibernate.SessionFactory;

/**
 * Hibernate-implementation of SkillDataManager.
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class HibernateSkillDataManager implements SkillDataManager {

    private final static String ENTITY_SKILL_CATEGORY = SkillCategory.class.getName();

    private final static String ENTITY_SKILL_ITEM = SkillItem.class.getName();

    private final static String ENTITY_SKILL_INVENTORY = SkillInventory.class.getName();

    private SessionFactory sessionFactory;

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
    public SkillCategory getSkillCategory(int id) {
        Session session = getSession();
        try {
            return (SkillCategory) session.get(ENTITY_SKILL_CATEGORY, id);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public SkillItem getSkillItem(int id) {
        Session session = getSession();
        try {
            return (SkillItem) session.get(ENTITY_SKILL_ITEM, id);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public void deleteSkillCategory(SkillCategory skillCategory) {
        Session session = getSession();
        try {
            session.delete(ENTITY_SKILL_CATEGORY, skillCategory);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public SkillCategory updateSkillCategory(SkillCategory skillCategory) {
        Session session = getSession();
        try {
            session.update(ENTITY_SKILL_CATEGORY, skillCategory);
            return skillCategory;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public SkillCategory createSkillCategory(SkillCategory skillCategory) {
        Session session = getSession();
        try {
            session.save(ENTITY_SKILL_CATEGORY, skillCategory);
            return skillCategory;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends SkillCategory> getAllSkillCategories() {
        final String HQL_SELECT_ALL_CATEGORY_IDS = "SELECT id FROM " + ENTITY_SKILL_CATEGORY
                + " ORDER BY name";

        Session session = getSession();
        try {
            Collection<SkillCategory> result = new ArrayList<SkillCategory>();
            Collection<?> ids = session.createQuery(HQL_SELECT_ALL_CATEGORY_IDS).setCacheable(true)
                    .list();
            if ( ids != null ) {
                for ( Object id : ids ) {
                    SkillCategory cat = getSkillCategory((Integer) id);
                    if ( cat != null ) {
                        result.add(cat);
                    }
                }
            }
            return result;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public void deleteSkillInventory(SkillInventory skillInventory) {
        Session session = getSession();
        try {
            session.delete(ENTITY_SKILL_INVENTORY, skillInventory);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends SkillInventory> getSkillInventories(DafUser user) {
        if ( user == null ) {
            return null;
        }
        final String HQL_SELECT_SKILL_INVENTORIES = "FROM " + ENTITY_SKILL_INVENTORY
                + " WHERE user=:user";

        Session session = getSession();
        try {
            Collection<SkillInventory> result = new ArrayList<SkillInventory>();
            Collection<?> temp = session.createQuery(HQL_SELECT_SKILL_INVENTORIES)
                    .setParameter("user", user).setCacheable(true).list();
            if ( temp != null ) {
                for ( Object obj : temp ) {
                    result.add((SkillInventory) obj);
                }
            }
            return result;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public SkillInventory createSkillInventory(SkillInventory skillInventory) {
        Session session = getSession();
        try {
            session.save(ENTITY_SKILL_INVENTORY, skillInventory);
            return skillInventory;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public SkillInventory updateSkillInventory(SkillInventory skillInventory) {
        Session session = getSession();
        try {
            session.update(ENTITY_SKILL_INVENTORY, skillInventory);
            return skillInventory;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends DafUser> searchSkill(Collection<? extends DafUser> searchWithin,
            SkillItem skillItem, String skillLevelOperator, int skillLevel) {
        return searchSkill(searchWithin, skillItem, skillLevelOperator, skillLevel, null, -1);
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends DafUser> searchSkill(Collection<? extends DafUser> searchWithin,
            SkillItem skillItem, String skillLevelOperator, int skillLevel,
            String monthsExpOperator, int monthsExp) {
        String HQL = "SELECT SI.user FROM " + ENTITY_SKILL_INVENTORY
                + " SI WHERE SI.skillItem=:skillItem AND SI.level" + skillLevelOperator
                + skillLevel;
        if ( monthsExpOperator != null ) {
            HQL += " AND SI.numMonthsExp" + monthsExpOperator + monthsExp;
        }
        if ( searchWithin != null ) {
            if ( searchWithin.size() == 0 ) {
                return new ArrayList<DafUser>();
            }
            HQL += " AND SI.user IN (:users)";
        }
        Session session = getSession();
        try {
            Query query = session.createQuery(HQL).setCacheable(true).setParameter("skillItem",
                                                                                   skillItem);
            if ( searchWithin != null ) {
                query = query.setParameterList("users", searchWithin);
            }
            List<?> hresult = query.list();
            Collection<DafUser> result = new HashSet<DafUser>();
            if ( result != null ) {
                for ( Object obj : hresult ) {
                    result.add((DafUser) obj);
                }
            }
            return result;
        } finally {
            closeSession(session);
        }
    }
}
