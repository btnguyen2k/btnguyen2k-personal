package org.ddth.eis.bo.daf;

import java.io.Serializable;
import java.lang.reflect.Constructor;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.ddth.daf.Group;
import org.ddth.daf.GroupRule;
import org.ddth.daf.Passport;
import org.ddth.daf.Permission;
import org.ddth.daf.Resource;
import org.ddth.daf.User;
import org.ddth.daf.UserProfile;
import org.ddth.daf.UserRule;
import org.ddth.daf.UserProfile.Id;
import org.ddth.daf.utils.DafException;
import org.hibernate.Session;
import org.hibernate.SessionFactory;

/**
 * Hibernate-implementation of {@link DafDataManager}.
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class HibernateDafDataManager implements DafDataManager {

    private final static Log    LOGGER                                = LogFactory
                                                                              .getLog(HibernateDafDataManager.class);

    private final static String ENTITY_USER                           = DafUser.class.getName();

    private final static String ENTITY_USER_RULE                      = DafUserRule.class.getName();

    private final static String ENTITY_USER_PROFILE                   = DafUserProfile.class
                                                                              .getName();

    private final static String ENTITY_GROUP                          = DafGroup.class.getName();

    private final static String ENTITY_GROUP_RULE                     = DafGroupRule.class
                                                                              .getName();

    private final static String ENTITY_PERMISSION                     = DafPermission.class
                                                                              .getName();

    private final static String HQL_SELECT_ALL_GROUP_IDS              = "SELECT id FROM "
                                                                              + ENTITY_GROUP
                                                                              + " ORDER BY id";

    private final static String HQL_SELECT_ALL_PERMISSIONS            = "FROM "
                                                                              + ENTITY_PERMISSION
                                                                              + " ORDER BY domain, action";

    private final static String HQL_SELECT_ALL_USER_PROFILES          = "FROM "
                                                                              + ENTITY_USER_PROFILE
                                                                              + " WHERE id.userId=:userId"
                                                                              + " ORDER BY domain, action";

    private final static String HQL_SELECT_ALL_RULES_FOR_GROUP        = "FROM "
                                                                              + ENTITY_GROUP_RULE
                                                                              + " WHERE group=:group";

    private final static String HQL_GET_NEWEST_USER_ID                = "SELECT max(id) FROM "
                                                                              + ENTITY_USER;

    private final static String HQL_SELECT_PERMISSION                 = "FROM "
                                                                              + ENTITY_PERMISSION
                                                                              + " WHERE domain=:domain AND action=:action";
    private final static String HQL_SELECT_PERMISSIONS_FROM_DOMAIN    = "FROM "
                                                                              + ENTITY_PERMISSION
                                                                              + " WHERE domain=:domain ORDER BY action";

    private final static String HQL_GET_USER_ID_BY_LOGIN_NAME         = "SELECT id FROM "
                                                                              + ENTITY_USER
                                                                              + " WHERE loginName=:loginName";

    private final static String HQL_GET_USER_ID_BY_EMAIL              = "SELECT id FROM "
                                                                              + ENTITY_USER
                                                                              + " WHERE email=:email";

    private final static String HQL_SELECT_USER_PROFILE_IDS_BY_DOMAIN = "SELECT id FROM "
                                                                              + ENTITY_USER_PROFILE
                                                                              + " WHERE id.userId=:userId AND id.domain=:domain"
                                                                              + " ORDER BY id.name";

    private final static String HQL_SELECT_USER_IDS_BY_FILTER         = "SELECT id FROM "
                                                                              + ENTITY_USER;

    private final static String HQL_DELETE_FROM_GROUP_RULE            = "DELETE FROM "
                                                                              + ENTITY_GROUP_RULE
                                                                              + " WHERE group=:group";

    private final static String HQL_DELETE_FROM_USER_RULE             = "DELETE FROM "
                                                                              + ENTITY_USER_RULE
                                                                              + " WHERE user=:user";

    private final static String HQL_SELECT_GROUP_RULE_BY_PERMISSION   = "FROM "
                                                                              + ENTITY_GROUP_RULE
                                                                              + " WHERE permission=:permission AND group=:group";

    private final static String HQL_COUNT_REGISTERED_ACCOUNTS         = "SELECT max(id) FROM "
                                                                              + ENTITY_USER;
    private final static String HQL_COUNT_USERS                       = "SELECT count(id) FROM "
                                                                              + ENTITY_USER;

    private final static String HQL_SELECT_USER_RULE_BY_PERMISSION    = "FROM "
                                                                              + ENTITY_USER_RULE
                                                                              + " WHERE permission=:permission AND user=:user";

    private final static String HQL_DELETE_ALL_USER_PROFILES          = "DELETE FROM "
                                                                              + ENTITY_USER_PROFILE
                                                                              + " WHERE id.userId=:userId";

    private final static String HQL_DELETE_FROM_USER_PROFILES         = "DELETE FROM "
                                                                              + ENTITY_USER_PROFILE
                                                                              + " WHERE id.userId=:userId AND id.domain=:domain";

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
    public DafUser assignUserToGroup(Object userId, Object groupId) throws DafException {
        return assignUserToGroup(getUser(userId), getGroup(groupId));
    }

    /**
     * {@inheritDoc}
     */
    public DafUser assignUserToGroup(User user, Group group) throws DafException {
        if ( user == null || group == null ) {
            LOGGER.warn("User and/or Group is null!");
            return null;
        }
        if ( !(user instanceof DafUser) ) {
            LOGGER.warn("User is not instance of " + DafUser.class.getName());
            return assignUserToGroup(getUser(user.getId()), group);
        }
        if ( !(group instanceof DafGroup) ) {
            LOGGER.warn("Group is not instance of " + DafGroup.class.getName());
            return assignUserToGroup(user, getGroup(group.getId()));
        }
        DafUser dafUser = (DafUser) user;
        DafGroup dafGroup = (DafGroup) group;
        dafUser.addRole(dafGroup);
        return updateUser(dafUser);
    }

    /**
     * {@inheritDoc}
     */
    public DafGroup createGroup(Group data) throws DafException {
        if ( data == null || !(data instanceof DafGroup) ) {
            throw new DafException("Argument is not an instance of " + DafGroup.class.getName());
        }
        Session session = getSession();
        try {
            session.save(ENTITY_GROUP, data);
            return (DafGroup) data;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafPermission createPermission(Permission data) throws DafException {
        if ( data == null || !(data instanceof DafPermission) ) {
            throw new DafException("Argument is not an instance of "
                    + DafPermission.class.getName());
        }
        Session session = getSession();
        try {
            session.save(ENTITY_PERMISSION, data);
            return (DafPermission) data;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafUser createUser(User data) throws DafException {
        if ( data == null || !(data instanceof DafUser) ) {
            throw new DafException("Argument is not an instance of " + DafUser.class.getName());
        }
        Session session = getSession();
        try {
            session.save(ENTITY_USER, data);
            return (DafUser) data;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafUserProfile createUserProfile(UserProfile data) throws DafException {
        if ( data == null || !(data instanceof DafUserProfile) ) {
            throw new DafException("Argument is not an instance of "
                    + DafUserProfile.class.getName());
        }
        Session session = getSession();
        try {
            session.saveOrUpdate(ENTITY_USER_PROFILE, data);
            return (DafUserProfile) data;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public void deleteGroup(Object groupId) throws DafException {
        deleteGroup(getGroup(groupId));
    }

    /**
     * {@inheritDoc}
     */
    public void deleteGroup(Group group) throws DafException {
        if ( group == null ) {
            return;
        }
        if ( !(group instanceof DafGroup) ) {
            LOGGER.warn("Group is not instance of " + DafGroup.class.getName());
            deleteGroup(getGroup(group.getId()));
        }
        Session session = getSession();
        try {
            session.delete(ENTITY_GROUP, group);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public void deleteUser(Object userId) throws DafException {
        deleteUser(getUser(userId));
    }

    /**
     * {@inheritDoc}
     */
    public void deleteUser(User user) throws DafException {
        if ( user == null ) {
            return;
        }
        if ( !(user instanceof DafUser) ) {
            LOGGER.warn("User is not instance of " + DafUser.class.getName());
            deleteUser(getUser(user.getId()));
        }
        Session session = getSession();
        try {
            session.delete(ENTITY_USER, user);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends DafGroup> getAllGroups() throws DafException {
        Session session = getSession();
        try {
            Collection<DafGroup> result = new ArrayList<DafGroup>();
            Collection<?> ids = session.createQuery(HQL_SELECT_ALL_GROUP_IDS).setCacheable(true)
                    .list();
            if ( ids != null ) {
                for ( Object id : ids ) {
                    DafGroup gr = getGroup(id);
                    if ( gr != null ) {
                        result.add(gr);
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
    public Collection<? extends DafPermission> getAllPermissions() throws DafException {
        Session session = getSession();
        try {
            Collection<?> permissions = session.createQuery(HQL_SELECT_ALL_PERMISSIONS)
                    .setCacheable(true).list();
            Collection<DafPermission> result = new ArrayList<DafPermission>();
            for ( Object p : permissions ) {
                result.add((DafPermission) p);
            }
            return result;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends DafUserProfile> getAllUserProfiles(User user) throws DafException {
        Session session = getSession();
        try {
            Collection<?> userProfiles = session.createQuery(HQL_SELECT_ALL_USER_PROFILES)
                    .setParameter("userId", user.getId()).list();
            Collection<DafUserProfile> result = new ArrayList<DafUserProfile>();
            for ( Object p : userProfiles ) {
                result.add((DafUserProfile) p);
            }
            return result;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafGroup getGroup(Object groupId) throws DafException {
        if ( groupId == null ) {
            return null;
        }
        Session session = getSession();
        try {
            return (DafGroup) session.get(ENTITY_GROUP, (Serializable) groupId);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends DafGroupRule> getGroupRules(Group group) throws DafException {
        Session session = getSession();
        try {
            Collection<DafGroupRule> result = new ArrayList<DafGroupRule>();
            Collection<?> groupRules = session.createQuery(HQL_SELECT_ALL_RULES_FOR_GROUP)
                    .setParameter("group", group).list();
            for ( Object gr : groupRules ) {
                result.add((DafGroupRule) gr);
            }
            return result;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends DafGroup> getGroups(Object[] groupIds) throws DafException {
        Collection<DafGroup> result = new ArrayList<DafGroup>();
        for ( Object id : groupIds ) {
            DafGroup g = getGroup(id);
            if ( g != null ) {
                result.add(g);
            }
        }
        return result;
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends DafGroup> getGroups(Collection<Object> groupIds)
            throws DafException {
        return groupIds != null ? getGroups(groupIds.toArray()) : new ArrayList<DafGroup>();
    }

    /**
     * {@inheritDoc}
     */
    public DafUser getNewestUser() throws DafException {
        Session session = getSession();
        try {
            Object userId = session.createQuery(HQL_GET_NEWEST_USER_ID).setCacheable(true)
                    .uniqueResult();
            return getUser(userId);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafPermission getPermission(String module, String action) throws DafException {
        Session session = getSession();
        try {
            return (DafPermission) session.createQuery(HQL_SELECT_PERMISSION).setString("domain",
                                                                                        module)
                    .setString("action", action).setCacheable(true).uniqueResult();
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends DafPermission> getPermissions(String module) throws DafException {
        Session session = getSession();
        try {
            Collection<?> permissions = session.createQuery(HQL_SELECT_PERMISSIONS_FROM_DOMAIN)
                    .setString("domain", module).list();
            Collection<DafPermission> result = new ArrayList<DafPermission>();
            for ( Object p : permissions ) {
                result.add((DafPermission) p);
            }
            return result;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafUser getUser(Object userId) throws DafException {
        if ( userId == null ) {
            return null;
        }
        Session session = getSession();
        try {
            return (DafUser) session.get(ENTITY_USER, (Serializable) userId);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafUser getUser(String loginName) throws DafException {
        if ( loginName == null ) {
            return null;
        }
        // TODO normalizing login name?
        loginName = loginName.trim();
        Session session = getSession();
        try {

            Object id = session.createQuery(HQL_GET_USER_ID_BY_LOGIN_NAME).setString("loginName",
                                                                                     loginName)
                    .setCacheable(true).uniqueResult();
            return getUser(id);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafUser getUserByEmail(String email) throws DafException {
        if ( email == null ) {
            return null;
        }
        // TODO normalizing email?
        email = email.trim();

        Session session = getSession();
        try {
            Serializable id = (Serializable) session.createQuery(HQL_GET_USER_ID_BY_EMAIL)
                    .setString("email", email).setCacheable(true).uniqueResult();
            return getUser(id);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafUserProfile getUserProfile(Id id) throws DafException {
        if ( id == null ) {
            return null;
        }
        Session session = getSession();
        try {
            return (DafUserProfile) session.get(ENTITY_USER_PROFILE, id);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends DafUserProfile> getUserProfiles(User user, String module)
            throws DafException {
        Session session = getSession();
        try {
            Collection<?> ids = session.createQuery(HQL_SELECT_USER_PROFILE_IDS_BY_DOMAIN)
                    .setParameter("userId", user.getId()).setParameter("domain", module).list();
            Collection<DafUserProfile> result = new ArrayList<DafUserProfile>();
            for ( Object id : ids ) {
                DafUserProfile up = getUserProfile((Id) id);
                if ( up != null ) {
                    result.add(up);
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
    public List<? extends DafUser> getUsers(DafUserFilter filter) throws DafException {
        Session session = getSession();
        try {
            String sql = HQL_SELECT_USER_IDS_BY_FILTER + buildWhereUserFilter(filter)
                    + buildSortingUserFilter(filter);
            List<?> ids = session.createQuery(sql).setFirstResult(filter.getPageOffset())
                    .setMaxResults(filter.getPageSize()).setCacheable(true).list();
            List<DafUser> result = new ArrayList<DafUser>();
            for ( Object id : ids ) {
                DafUser user = getUser(id);
                if ( user != null ) {
                    result.add(user);
                }
            }
            return result;
        } finally {
            closeSession(session);
        }
    }

    private String buildSortingUserFilter(DafUserFilter filter) {
        StringBuffer sb = new StringBuffer();
        if ( filter.hasSortingCriteria() ) {
            sb.append(" order by ");
            for ( DafSortingCriteria dafSr : filter.getSortingCriteria() ) {
                switch ( dafSr.getColumn() ) {
                    case DafDataManager.SORT_DOB: {
                        if ( dafSr.isSortDescending() ) {
                            sb.append("dobYear desc, dobMonth desc, dobDay desc");
                        } else {
                            sb.append("dobYear, dobMonth, dobDay");
                        }
                        break;
                    }
                    case DafDataManager.SORT_FIRST_NAME: {
                        if ( dafSr.isSortDescending() ) {
                            sb.append("firstName desc");
                        } else {
                            sb.append("firstName");
                        }
                        break;
                    }
                    case DafDataManager.SORT_LAST_NAME: {
                        if ( dafSr.isSortDescending() ) {
                            sb.append("lastName desc");
                        } else {
                            sb.append("lastName");
                        }
                        break;
                    }
                    case DafDataManager.SORT_LAST_UPDATE_TIME: {
                        if ( dafSr.isSortDescending() ) {
                            sb.append("lastUpdateTimestamp desc");
                        } else {
                            sb.append("lastUpdateTimestamp");
                        }
                        break;
                    }
                    case DafDataManager.SORT_LOGIN_NAME: {
                        if ( dafSr.isSortDescending() ) {
                            sb.append("loginName desc");
                        } else {
                            sb.append("loginName");
                        }
                        break;
                    }
                    case DafDataManager.SORT_REGISTER_TIME: {
                        if ( dafSr.isSortDescending() ) {
                            sb.append("registerTimestamp desc");
                        } else {
                            sb.append("registerTimestamp");
                        }
                        break;
                    }
                    default: {
                        if ( dafSr.isSortDescending() ) {
                            sb.append("id desc");
                        } else {
                            sb.append("id");
                        }
                        break;
                    }
                }
                sb.append(",");
            }
            sb.setLength(sb.length() - 1);
            sb.append(" ");
        }
        return sb.toString();
    }

    private String buildWhereUserFilter(DafUserFilter filter) {
        StringBuffer sb = new StringBuffer();
        if ( filter.hasWhereCriteria() ) {
            sb.append(" where ");
            sb.append(filter.getWhereCriteria());
            sb.append(" ");
        }
        return sb.toString();
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends DafUser> getUsers(Object[] userIds) throws DafException {
        Collection<DafUser> result = new ArrayList<DafUser>();
        for ( Object id : userIds ) {
            DafUser u = getUser(id);
            if ( u != null ) {
                result.add(u);
            }
        }
        return result;
    }

    /**
     * {@inheritDoc}
     */
    public Collection<? extends DafUser> getUsers(String[] loginNames) throws DafException {
        Collection<DafUser> result = new ArrayList<DafUser>();
        for ( String loginName : loginNames ) {
            DafUser u = getUser(loginName);
            if ( u != null ) {
                result.add(u);
            }
        }
        return result;
    }

    /**
     * {@inheritDoc}
     */
    public DafUser removeUserFromGroup(Object userId, Object groupId) throws DafException {
        return removeUserFromGroup(getUser(userId), getGroup(groupId));
    }

    /**
     * {@inheritDoc}
     */
    public DafUser removeUserFromGroup(User user, Group group) throws DafException {
        if ( user == null || group == null ) {
            LOGGER.warn("User and/or Group is null!");
            return null;
        }
        if ( !(user instanceof DafUser) ) {
            LOGGER.warn("User is not instance of " + DafUser.class.getName());
            return assignUserToGroup(getUser(user.getId()), group);
        }
        if ( !(group instanceof DafGroup) ) {
            LOGGER.warn("Group is not instance of " + DafGroup.class.getName());
            return assignUserToGroup(user, getGroup(group.getId()));
        }
        DafUser dafUser = (DafUser) user;
        DafGroup dafGroup = (DafGroup) group;
        dafUser.removeRole(dafGroup);
        return updateUser(dafUser);
    }

    /**
     * {@inheritDoc}
     */
    public DafGroup setGroupPermissions(Object groupId, Collection<? extends GroupRule> rules)
            throws DafException {
        return setGroupPermissions(getGroup(groupId), rules);
    }

    /**
     * {@inheritDoc}
     */
    public DafGroup setGroupPermissions(Group group, Collection<? extends GroupRule> rules)
            throws DafException {
        if ( !(group instanceof DafGroup) ) {
            LOGGER.warn("Group is not instance of " + DafGroup.class.getName());
            return setGroupPermissions(getGroup(group.getId()), rules);
        }
        Session session = getSession();
        try {
            session.createQuery(HQL_DELETE_FROM_GROUP_RULE).setParameter("group", group)
                    .executeUpdate();
            for ( GroupRule gr : rules ) {
                if ( gr instanceof DafGroupRule ) {
                    if ( group.getId().equals(gr.getGroup().getId()) ) {
                        session.saveOrUpdate(ENTITY_GROUP_RULE, gr);
                    }
                } else {
                    LOGGER.warn("Object is not an instance of " + DafGroupRule.class.getName());
                }
            }
            return (DafGroup) group;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafUser setUserPermissions(Object userId, Collection<? extends UserRule> rules)
            throws DafException {
        return setUserPermissions(getUser(userId), rules);
    }

    /**
     * {@inheritDoc}
     */
    public DafUser setUserPermissions(User user, Collection<? extends UserRule> rules)
            throws DafException {
        if ( !(user instanceof DafUser) ) {
            LOGGER.warn("User is not instance of " + DafUser.class.getName());
            return setUserPermissions(getUser(user.getId()), rules);
        }
        Session session = getSession();
        try {
            session.createQuery(HQL_DELETE_FROM_USER_RULE).setParameter("user", user)
                    .executeUpdate();
            for ( UserRule ur : rules ) {
                if ( ur instanceof DafUserRule ) {
                    if ( user.getId().equals(ur.getUser().getId()) ) {
                        session.saveOrUpdate(ENTITY_USER_RULE, ur);
                    }
                } else {
                    LOGGER.warn("Object is not an instance of " + DafUserRule.class.getName());
                }
            }
            return (DafUser) user;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafGroup updateGroup(Group group) throws DafException {
        if ( group == null || !(group instanceof DafGroup) ) {
            throw new DafException("Argument is not an instance of " + DafGroup.class.getName());
        }
        Session session = getSession();
        try {
            session.update(ENTITY_GROUP, group);
            return (DafGroup) group;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafUser updateUser(User user) throws DafException {
        if ( user == null || !(user instanceof DafUser) ) {
            throw new DafException("Argument is not an instance of " + DafUser.class.getName());
        }
        Session session = getSession();
        try {
            session.update(ENTITY_USER, user);
            return (DafUser) user;
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public DafUserProfile updateUserProfile(UserProfile userProfile) throws DafException {
        if ( userProfile == null || !(userProfile instanceof DafUserProfile) ) {
            throw new DafException("Argument is not an instance of "
                    + DafUserProfile.class.getName());
        }
        Session session = getSession();
        try {
            session.update(ENTITY_USER_PROFILE, userProfile);
            return (DafUserProfile) userProfile;
        } finally {
            closeSession(session);
        }
    }

    private Method getMethod(Class<?> clazz, String methodName, Class<?>[] paramTypes) {
        try {
            return clazz.getMethod(methodName, paramTypes);
        } catch ( SecurityException e ) {
            throw e;
        } catch ( NoSuchMethodException e ) {
            return null;
        }
    }

    private <T> Constructor<T> getConstructor(Class<T> clazz, Class<?>[] paramTypes) {
        try {
            return clazz.getConstructor(paramTypes);
        } catch ( SecurityException e ) {
            throw e;
        } catch ( NoSuchMethodException e ) {
            return null;
        }
    }

    /**
     * {@inheritDoc}
     */
    public Passport getGroupPassport(Group group, Permission permission) throws DafException {
        Session session = getSession();
        try {
            List<?> result = session.createQuery(HQL_SELECT_GROUP_RULE_BY_PERMISSION)
                    .setParameter("permission", permission).setParameter("group", group)
                    .setCacheable(true).list();
            if ( result == null ) {
                return null;
            }

            Iterator<?> it = result.iterator();
            GroupRule groupRule = (GroupRule) (it.hasNext() ? it.next() : null);
            if ( groupRule == null ) {
                return null;
            }
            String ruleClassName = permission.getPassportClassName();
            Class<?> c = Class.forName(ruleClassName);
            Constructor<?> co = getConstructor(c, null);
            if ( co == null ) {
                throw new DafException("No constructor found for class [" + ruleClassName + "]!");
            }
            Passport p = (Passport) co.newInstance();

            Method m;
            Class<?>[] paramTypes;

            paramTypes = new Class<?>[] { Group.class };
            m = getMethod(c, "setGroup", paramTypes);
            if ( m == null ) {
                throw new DafException("No method setGroup(" + paramTypes[0].getName()
                        + ") found for class [" + ruleClassName + "]!");
            }
            m.invoke(p, group);

            paramTypes = new Class<?>[] { Permission.class };
            m = getMethod(c, "setPermission", paramTypes);
            if ( m == null ) {
                throw new DafException("No method setPermission(" + paramTypes[0].getName()
                        + ") found for class [" + ruleClassName + "]!");
            }
            m.invoke(p, permission);

            paramTypes = new Class<?>[] { boolean.class };
            m = getMethod(c, "setGlobal", paramTypes);
            if ( m == null ) {
                m = getMethod(c, "setIsGlobal", paramTypes);
            }
            if ( m == null ) {
                paramTypes = new Class<?>[] { Boolean.class };
                m = getMethod(c, "setGlobal", paramTypes);
            }
            if ( m == null ) {
                m = getMethod(c, "setIsGlobal", paramTypes);
            }
            if ( m == null ) {
                throw new DafException("No method setGlobal(" + boolean.class.getName()
                        + ") found for class [" + ruleClassName + "]!");
            }
            m.invoke(p, groupRule.isGlobal());

            return p;
        } catch ( Exception e ) {
            throw DafException.asDafException(e);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Passport getGroupPassport(Group group, Permission permission, Resource resource)
            throws DafException {
        Passport passport = getGroupPassport(group, permission);
        if ( passport != null ) {
            return passport.isGlobal() ? passport
                    : (resource.authorizePassport(passport) ? passport : null);
        }
        return null;
    }

    /**
     * {@inheritDoc}
     */
    public Number getNumRegisteredAccounts() throws DafException {
        Session session = getSession();
        try {
            return (Number) session.createQuery(HQL_COUNT_REGISTERED_ACCOUNTS).uniqueResult();
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Number getNumUsers() throws DafException {
        Session session = getSession();
        try {
            return (Number) session.createQuery(HQL_COUNT_USERS).uniqueResult();
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Passport getUserPassport(User user, Permission permission) throws DafException {
        Session session = getSession();
        try {
            List<?> result = session.createQuery(HQL_SELECT_USER_RULE_BY_PERMISSION)
                    .setParameter("permission", permission).setParameter("user", user)
                    .setCacheable(true).list();
            if ( result == null ) {
                return null;
            }

            Iterator<?> it = result.iterator();
            UserRule userRule = (UserRule) (it.hasNext() ? it.next() : null);
            if ( userRule == null ) {
                return null;
            }
            String ruleClassName = permission.getPassportClassName();
            Class<?> c = Class.forName(ruleClassName);
            Constructor<?> co = getConstructor(c, null);
            if ( co == null ) {
                throw new DafException("No constructor found for class [" + ruleClassName + "]!");
            }
            Passport p = (Passport) co.newInstance();

            Method m;
            Class<?>[] paramTypes;

            paramTypes = new Class<?>[] { User.class };
            m = getMethod(c, "setUser", paramTypes);
            if ( m == null ) {
                throw new DafException("No method setUser(" + paramTypes[0].getName()
                        + ") found for class [" + ruleClassName + "]!");
            }
            m.invoke(p, user);

            paramTypes = new Class<?>[] { Permission.class };
            m = getMethod(c, "setPermission", paramTypes);
            if ( m == null ) {
                throw new DafException("No method setPermission(" + paramTypes[0].getName()
                        + ") found for class [" + ruleClassName + "]!");
            }
            m.invoke(p, permission);

            paramTypes = new Class<?>[] { boolean.class };
            m = getMethod(c, "setGlobal", paramTypes);
            if ( m == null ) {
                m = getMethod(c, "setIsGlobal", paramTypes);
            }
            if ( m == null ) {
                paramTypes = new Class<?>[] { Boolean.class };
                m = getMethod(c, "setGlobal", paramTypes);
            }
            if ( m == null ) {
                m = getMethod(c, "setIsGlobal", paramTypes);
            }
            if ( m == null ) {
                throw new DafException("No method setGlobal(" + boolean.class.getName()
                        + ") found for class [" + ruleClassName + "]!");
            }
            m.invoke(p, userRule.isGlobal());

            return p;
        } catch ( Exception e ) {
            throw DafException.asDafException(e);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public Passport getUserPassport(User user, Permission permission, Resource resource)
            throws DafException {
        Passport passport = getUserPassport(user, permission);
        if ( passport != null ) {
            return passport.isGlobal() ? passport
                    : (resource.authorizePassport(passport) ? passport : null);
        }
        return null;
    }

    /**
     * {@inheritDoc}
     */
    public void deleteAllUserProfiles(User user) throws DafException {
        Session session = getSession();
        try {
            session.createQuery(HQL_DELETE_ALL_USER_PROFILES).setParameter("userId", user.getId())
                    .executeUpdate();
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public void deleteUserProfile(UserProfile userProfile) throws DafException {
        if ( userProfile == null ) {
            return;
        }
        if ( !(userProfile instanceof DafUserProfile) ) {
            LOGGER.warn("UserProfile is not instance of " + DafUserProfile.class.getName());
            deleteUserProfile(getUserProfile(userProfile.getId()));
        }
        Session session = getSession();
        try {
            session.delete(ENTITY_USER_PROFILE, userProfile);
        } finally {
            closeSession(session);
        }
    }

    /**
     * {@inheritDoc}
     */
    public void deleteUserProfiles(User user, String module) throws DafException {
        Session session = getSession();
        try {
            session.createQuery(HQL_DELETE_FROM_USER_PROFILES).setParameter("userId", user.getId())
                    .setParameter("domain", module).executeUpdate();
        } finally {
            closeSession(session);
        }
    }
}
