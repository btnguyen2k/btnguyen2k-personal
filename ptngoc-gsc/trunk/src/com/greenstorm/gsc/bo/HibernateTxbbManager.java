package com.greenstorm.gsc.bo;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.ddth.common.HibernateSessionFactory;
import org.ddth.panda.core.daf.DafUser;
import org.ddth.panda.utils.SystemUtils;
import org.ddth.txbb.bo.Box;
import org.ddth.txbb.bo.Member;
import org.ddth.txbb.bo.Post;
import org.ddth.txbb.bo.Publishing;
import org.ddth.txbb.bo.Topic;
import org.ddth.txbb.bo.TxbbManager;
import org.hibernate.HibernateException;
import org.hibernate.Query;
import org.hibernate.Session;

public class HibernateTxbbManager implements TxbbManager {

    private final static String ENTITY_BOX = Box.class.getName();

    private final static String ENTITY_MEMBER = Member.class.getName();

    private final static String ENTITY_TOPIC = Topic.class.getName();

    private final static String ENTITY_PUBLISHING = Publishing.class.getName();

    private final static String ENTITY_POST = Post.class.getName();

    private HibernateSessionFactory hsf;

    private Map<Object, Box> cacheAllBoxesMap;

    private Box[] cacheAllBoxes;

    private Box[] cacheBoxTree;

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

    private void invalidateBoxCaches() {
        cacheAllBoxesMap = null;
        cacheAllBoxes = null;
        cacheBoxTree = null;
    }

    /**
     * {@inheritDoc}
     */
    public Member createMember(Member member) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            session.save(ENTITY_MEMBER, member);
            return member;
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
    public Member getMember(DafUser dafUser) {
        if ( dafUser == null ) {
            return null;
        }
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            Member member =
                    (Member)session.get(ENTITY_MEMBER,
                            (Serializable)dafUser.getId());
            if ( member == null ) {
                member = new Member(dafUser);
                member = createMember(member);
            }
            return member;
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
    public Member updateMember(Member member) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            session.update(ENTITY_MEMBER, member);
            return member;
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
    public int countDraftTopics(DafUser user) {
        if ( user == null ) {
            return 0;
        }
        String hql =
                "SELECT count(T.id) FROM " + ENTITY_TOPIC
                        + " T WHERE T.isPublished=false AND T.memberId="
                        + user.getId();
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            Number result = (Number)session.createQuery(hql).uniqueResult();
            return result != null
                    ? result.intValue() : 0;
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
    public int countPublishedTopics(DafUser user) {
        if ( user == null ) {
            return 0;
        }
        String hql =
                "SELECT count(T.id) FROM " + ENTITY_TOPIC
                        + " T WHERE T.isPublished=true AND T.memberId="
                        + user.getId();
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            Number result = (Number)session.createQuery(hql).uniqueResult();
            return result != null
                    ? result.intValue() : 0;
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
    public int countPublishedTopicsInBoxes(Collection<Box> boxes) {
        if ( boxes == null || boxes.size() == 0 ) {
            return 0;
        }
        String hql =
                "SELECT count(P.topicId) FROM " + ENTITY_PUBLISHING
                        + " P WHERE P.boxId IN (:boxIds)";
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            Collection<Integer> boxIds = new ArrayList<Integer>();
            for ( Box b : boxes ) {
                boxIds.add(b.getId());
            }
            Number result =
                    (Number)session.createQuery(hql).setParameterList("boxIds",
                            boxIds).setCacheable(true).uniqueResult();
            return result != null
                    ? result.intValue() : 0;
        } catch ( HibernateException e ) {
            hasError = true;
            throw e;
        } finally {
            releaseHibernateSession(session, hasError);
        }
    }

    /**
     * Creates a topic publishing.
     * 
     * @param topicPublish Publishing
     * @return
     */
    public Publishing createPublishing(Publishing topicPublish) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            topicPublish.setTimestamp(SystemUtils.getCurrentTimestamp());
            session.save(ENTITY_PUBLISHING, topicPublish);
            return topicPublish;
        } catch ( HibernateException e ) {
            hasError = true;
            throw e;
        } finally {
            releaseHibernateSession(session, hasError);
        }
    }

    /**
     * Deletes a topic publishing.
     * 
     * @param topicPublish Publishing
     */
    public void deletePublishing(Publishing topicPublish) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            session.delete(ENTITY_PUBLISHING, topicPublish);
        } catch ( HibernateException e ) {
            hasError = true;
            throw e;
        } finally {
            releaseHibernateSession(session, hasError);
        }
    }

    /**
     * Gets all publishing entries for a topic.
     * 
     * @param topic Topic
     * @return Publishing[]
     */
    public Publishing[] getTopicPublishings(Topic topic) {
        if ( topic == null ) {
            return new Publishing[0];
        }
        String hql =
                "FROM " + ENTITY_PUBLISHING + " WHERE topicId=" + topic.getId();
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            List<?> result = session.createQuery(hql).setCacheable(true).list();
            return result.toArray(new Publishing[0]);
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
    public Topic createTopic(Topic topic) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            topic.setLastpostTimestamp(0);
            topic.setCreationTimestamp(SystemUtils.getCurrentTimestamp());
            topic.setLastupdateTimestamp(SystemUtils.getCurrentTimestamp());
            session.save(ENTITY_TOPIC, topic);
            return topic;
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
    public void deleteTopic(Topic topic) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            session.delete(ENTITY_TOPIC, topic);
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
    public Topic[] getDraftTopics(DafUser user) {
        if ( user == null ) {
            return new Topic[0];
        }
        String hql =
                "SELECT T.id FROM " + ENTITY_TOPIC
                        + " T WHERE T.isPublished=false AND T.memberId="
                        + user.getId() + " ORDER BY lastupdateTimestamp DESC";
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            List<Topic> result = new ArrayList<Topic>();
            List<?> ids = session.createQuery(hql).setCacheable(true).list();
            for ( Object id : ids ) {
                Topic t = getTopic((Integer)id);
                if ( t != null ) {
                    result.add(t);
                }
            }
            return result.toArray(new Topic[0]);
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
    public Topic[] getPublishedTopics(DafUser user, int page, int pageSize) {
        if ( user == null ) {
            return new Topic[0];
        }
        if ( page < 1 ) {
            page = 1;
        }
        if ( pageSize < 1 ) {
            pageSize = 1;
        }
        String hql =
                "SELECT T.id FROM " + ENTITY_TOPIC
                        + " T WHERE T.isPublished=true AND T.memberId="
                        + user.getId() + " ORDER BY lastpostTimestamp DESC";
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            List<Topic> result = new ArrayList<Topic>();
            List<?> ids =
                    session.createQuery(hql)
                            .setFirstResult((page - 1) * pageSize)
                            .setMaxResults(pageSize)
                            .setCacheable(true)
                            .list();
            for ( Object id : ids ) {
                Topic t = getTopic((Integer)id);
                if ( t != null ) {
                    result.add(t);
                }
            }
            return result.toArray(new Topic[0]);
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
    public Topic[] getPublishedTopicsForBoxes(Collection<Box> boxes, int page,
            int pageSize) {
        if ( boxes == null || boxes.size() == 0 ) {
            return new Topic[0];
        }
        if ( page < 1 ) {
            page = 1;
        }
        if ( pageSize < 1 ) {
            pageSize = 1;
        }
        String hql =
                "SELECT P.topicId FROM "
                        + ENTITY_PUBLISHING
                        + " P WHERE P.boxId IN (:boxIds) ORDER BY timestamp DESC";
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            Collection<Integer> boxIds = new ArrayList<Integer>();
            for ( Box b : boxes ) {
                boxIds.add(b.getId());
            }
            List<Topic> result = new ArrayList<Topic>();
            List<?> ids =
                    session.createQuery(hql)
                            .setParameterList("boxIds", boxIds)
                            .setFirstResult((page - 1) * pageSize)
                            .setMaxResults(pageSize)
                            .setCacheable(true)
                            .list();
            for ( Object id : ids ) {
                Topic t = getTopic((Integer)id);
                if ( t != null ) {
                    result.add(t);
                }
            }
            return result.toArray(new Topic[0]);
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
    public Topic getTopic(int topicId) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            return (Topic)session.get(ENTITY_TOPIC, topicId);
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
    public Topic[] getRecentCommentedTopics(int num, Collection<Box> boxes) {
        boolean boxFilter = boxes != null;
        String HSQL =
                "SELECT T.id FROM "
                        + ENTITY_TOPIC
                        + " T, "
                        + ENTITY_PUBLISHING
                        + " P WHERE P.topicId = T.id AND T.isPublished=true AND T.numPosts>0";
        String WHERE_BOX_FILTER = " AND P.boxId IN (:boxes)";
        if ( boxFilter ) {
            HSQL += WHERE_BOX_FILTER;
        }
        String ORDER_BY = " ORDER BY T.lastpostTimestamp DESC";

        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            Query query = session.createQuery(HSQL + ORDER_BY);
            if ( boxFilter ) {
                Collection<Integer> boxIds = new ArrayList<Integer>();
                // in case the collection is empty, means no box is viewable
                boxIds.add(-1);
                for ( Box b : boxes ) {
                    boxIds.add(b.getId());
                }
                query.setParameterList("boxes", boxIds);
            }
            List<?> topicIds =
                    query.setMaxResults(num).setCacheable(true).list();
            Collection<Topic> result = new ArrayList<Topic>();
            for ( Object topicId : topicIds ) {
                Topic topic = getTopic((Integer)topicId);
                if ( topic != null ) {
                    result.add(topic);
                }
            }
            return result.toArray(new Topic[0]);
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
    public Topic[] getRecentPublishedTopics(int num, Collection<Box> boxes) {
        boolean boxFilter = boxes != null;
        String HSQL =
                "SELECT T.id FROM " + ENTITY_TOPIC + " T, " + ENTITY_PUBLISHING
                        + " P WHERE P.topicId = T.id AND T.isPublished=true";
        String WHERE_BOX_FILTER = " AND P.boxId IN (:boxes)";
        if ( boxFilter ) {
            HSQL += WHERE_BOX_FILTER;
        }
        String ORDER_BY = " ORDER BY P.timestamp DESC";

        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            Query query = session.createQuery(HSQL + ORDER_BY);
            if ( boxFilter ) {
                Collection<Integer> boxIds = new ArrayList<Integer>();
                // in case the collection is empty, means no box is viewable
                boxIds.add(-1);
                for ( Box b : boxes ) {
                    boxIds.add(b.getId());
                }
                query.setParameterList("boxes", boxIds);
            }
            List<?> topicIds =
                    query.setMaxResults(num).setCacheable(true).list();
            Collection<Topic> result = new ArrayList<Topic>();
            for ( Object topicId : topicIds ) {
                Topic topic = getTopic((Integer)topicId);
                if ( topic != null ) {
                    result.add(topic);
                }
            }
            return result.toArray(new Topic[0]);
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
    public Topic lockTopic(Topic topic) {
        topic.setLocked(true);
        return updateTopic(topic);
    }

    /**
     * {@inheritDoc}
     */
    public Topic unlockTopic(Topic topic) {
        topic.setLocked(false);
        return updateTopic(topic);
    }

    /**
     * {@inheritDoc}
     */
    public Topic publishTopic(Topic topic, Collection<Box> boxes) {
        topic.setPublished(true);
        if ( boxes != null ) {
            for ( Box b : boxes ) {
                Publishing p = new Publishing();
                p.setBoxId(b.getId());
                p.setTopicId(topic.getId());
                createPublishing(p);
            }
        }
        return updateTopic(topic);
    }

    /**
     * {@inheritDoc}
     */
    public Topic unpublishTopic(Topic topic) {
        topic.setPublished(false);
        Publishing[] publishing = getTopicPublishings(topic);
        for ( Publishing p : publishing ) {
            deletePublishing(p);
        }
        return updateTopic(topic);
    }

    /**
     * {@inheritDoc}
     */
    public Topic updateTopic(Topic topic) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            topic.setLastupdateTimestamp(SystemUtils.getCurrentTimestamp());
            session.update(ENTITY_TOPIC, topic);
            return topic;
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
    public int countBoxes() {
        String hql = "SELECT count(*) FROM " + ENTITY_BOX;
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            Number result = (Number)session.createQuery(hql).uniqueResult();
            return result != null
                    ? result.intValue() : 0;
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
    public int countPosts() {
        String hql = "SELECT count(*) FROM " + ENTITY_POST;
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            Number result = (Number)session.createQuery(hql).uniqueResult();
            return result != null
                    ? result.intValue() : 0;
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
    public int countTopics() {
        String hql = "SELECT count(*) FROM " + ENTITY_TOPIC;
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            Number result = (Number)session.createQuery(hql).uniqueResult();
            return result != null
                    ? result.intValue() : 0;
        } catch ( HibernateException e ) {
            hasError = true;
            throw e;
        } finally {
            releaseHibernateSession(session, hasError);
        }
    }

    /**
     * Gets all available boxes.
     * 
     * @return Box[]
     * @throws Exception
     */
    private Box[] getAllBoxes() {
        final String hql =
                "SELECT id FROM " + ENTITY_BOX + " ORDER BY parentId, position";
        if ( cacheAllBoxes == null ) {
            Session session = getHibernateSession();
            boolean hasError = false;
            try {
                List<?> idList =
                        session.createQuery(hql).setCacheable(true).list();
                List<Box> result = new ArrayList<Box>();
                for ( Object id : idList ) {
                    Box b = _getBox((Integer)id);
                    if ( b != null ) {
                        result.add(b);
                    }
                }
                cacheAllBoxes = result.toArray(new Box[0]);
            } catch ( HibernateException e ) {
                hasError = true;
                throw e;
            } finally {
                releaseHibernateSession(session, hasError);
            }
        }
        return cacheAllBoxes;
    }

    /**
     * Gets all available boxes as a map.
     * 
     * @return Map<Object, Box>
     * @throws Exception
     */
    private Map<Object, Box> getAllBoxesMap() {
        if ( cacheAllBoxesMap == null ) {
            Box[] allBoxes = getAllBoxes();
            cacheAllBoxesMap = new HashMap<Object, Box>();
            for ( Box box : allBoxes ) {
                cacheAllBoxesMap.put(box.getId(), box);
            }
        }
        return cacheAllBoxesMap;
    }

    /**
     * Builds the box tree.
     * 
     * @throws Exception
     */
    private void buildBoxTree() {
        Map<Object, Box> boxMap = getAllBoxesMap();
        Box[] allBoxes = boxMap.values().toArray(new Box[0]);
        List<Box> result = new ArrayList<Box>();
        for ( Box box : allBoxes ) {
            Integer parentId = box.getParentId();
            if ( parentId != null ) {
                Box parent = boxMap.get(parentId);
                if ( parent != null ) {
                    parent.addChild(box);
                }
            } else {
                result.add(box);
            }
        }
        Collections.sort(result, new Comparator<Box>() {
            public int compare(Box b1, Box b2) {
                return b1.getPosition() - b2.getPosition();
            }
        });
        cacheBoxTree = result.toArray(new Box[0]);
    }

    /**
     * {@inheritDoc}
     */
    public Box createBox(Box box) {
        invalidateBoxCaches();

        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            session.save(ENTITY_BOX, box);
            return box;
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
    public void deleteBox(Box box) {
        invalidateBoxCaches();

        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            session.delete(ENTITY_BOX, box);
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
    synchronized public Box[] getBoxTree() {
        buildBoxTree();
        return cacheBoxTree;
    }

    /**
     * {@inheritDoc}
     */
    synchronized public Box getBox(int id) {
        buildBoxTree();
        Map<Object, Box> boxMap = getAllBoxesMap();
        return boxMap.get(id);
    }

    /**
     * {@inheritDoc}
     */
    public Box[] getPublishedBoxes(Topic topic) {
        if ( topic == null || !topic.isPublished() ) {
            return new Box[0];
        }
        final String hql =
                "SELECT boxId FROM " + ENTITY_PUBLISHING + " WHERE topicId="
                        + topic.getId();
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            List<?> idList = session.createQuery(hql).setCacheable(true).list();
            List<Box> result = new ArrayList<Box>();
            for ( Object id : idList ) {
                Box b = getBox((Integer)id);
                if ( b != null ) {
                    result.add(b);
                }
            }
            return result.toArray(new Box[0]);
        } catch ( HibernateException e ) {
            hasError = true;
            throw e;
        } finally {
            releaseHibernateSession(session, hasError);
        }
    }

    /**
     * Gets a box by id.
     * 
     * @param id int
     * @return Box
     * @throws Exception
     */
    private Box _getBox(int id) {
        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            return (Box)session.get(ENTITY_BOX, id);
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
    public Box updateBox(Box box) {
        invalidateBoxCaches();

        Session session = getHibernateSession();
        boolean hasError = false;
        try {
            session.update(ENTITY_BOX, box);
            return box;
        } catch ( HibernateException e ) {
            hasError = true;
            throw e;
        } finally {
            releaseHibernateSession(session, hasError);
        }
    }
}
