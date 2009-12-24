package com.greenstorm.gsc.bo;

import java.util.Collection;

import org.ddth.panda.core.daf.DafUser;
import org.ddth.txbb.bo.Box;
import org.ddth.txbb.bo.Member;
import org.ddth.txbb.bo.Topic;

public interface TxbbManager {

    /**
     * Creates a new member account.
     * 
     * @param Member member
     * @return Member
     */
    public Member createMember(Member member);

    /**
     * Gets a member account.
     * 
     * @param dafUser DafUder
     * @return Member
     */
    public Member getMember(DafUser dafUser);

    /**
     * Updates an existing member account.
     * 
     * @param member Member
     * @return Member
     */
    public Member updateMember(Member member);

    /**
     * Counts number of draft topics of a user.
     * 
     * @param user DafUser
     * @return int
     */
    public int countDraftTopics(DafUser user);

    /**
     * Counts number of published topics of a user.
     * 
     * @param user DafUser
     * @return int
     */
    public int countPublishedTopics(DafUser user);

    /**
     * Counts number of published topics in a collection of boxes.
     * 
     * @param boxes Collection<Box>
     * @return int
     */
    public int countPublishedTopicsInBoxes(Collection<Box> boxes);

    /**
     * Creates a new topic.
     * 
     * @param topic Topic
     * @return Topic
     */
    public Topic createTopic(Topic topic);

    /**
     * Deletes a topic.
     * 
     * @param topic Topic
     */
    public void deleteTopic(Topic topic);

    /**
     * Gets all draft topics of a user.
     * 
     * @param user DafUser
     * @return Topic[]
     */
    public Topic[] getDraftTopics(DafUser user);

    /**
     * Gets list of published topics of a user.
     * 
     * @param user DafUser
     * @param page int only retreive topics within this page
     * @param pageSize int size of a page
     * @return Topic[]
     */
    public Topic[] getPublishedTopics(DafUser user, int page, int pageSize);

    /**
     * Gets list of published topics in a collection of boxes.
     * 
     * @param boxes Collection<Box>
     * @param page int only retreive topics within this page
     * @param pageSize int size of a page
     * @return Topic[]
     */
    public Topic[] getPublishedTopicsForBoxes(Collection<Box> boxes, int page,
            int pageSize);

    /**
     * Gets a topic by id.
     * 
     * @param topicId int
     * @return Topic
     */
    public Topic getTopic(int topicId);

    /**
     * Gets last n commented topics.
     * 
     * @param num int number of topics to retrieve
     * @param Collection<Box> only retrieve topics published in these boxes
     * @return Topic[]
     */
    public Topic[] getRecentCommentedTopics(int num, Collection<Box> boxes);

    /**
     * Gets last n published topics.
     * 
     * @param num int number of topics to retrieve
     * @param Collection<Box> only retrieve topics published in these boxes
     * @return Topic[]
     */
    public Topic[] getRecentPublishedTopics(int num, Collection<Box> boxes);

    /**
     * Locks a topic.
     * 
     * @param topic Topic the topic to lock
     * @return Topic
     */
    public Topic lockTopic(Topic topic);

    /**
     * Unlocks a topic.
     * 
     * @param topic Topic the topic to unlock
     * @return Topic
     */
    public Topic unlockTopic(Topic topic);

    /**
     * Publishes a topic.
     * 
     * @param topic Topic the topic to publish
     * @param boxes list of boxes to publish to
     * @return Topic
     */
    public Topic publishTopic(Topic topic, Collection<Box> boxes);

    /**
     * Unpublishes a topic.
     * 
     * @param topic Topic the topic to unpublish
     * @return Topic
     */
    public Topic unpublishTopic(Topic topic);

    /**
     * Updates an existing topic.
     * 
     * @param topic Topic
     * @return Topic
     */
    public Topic updateTopic(Topic topic);

    /**
     * Counts number of boxes.
     * 
     * @return int
     */
    public int countBoxes();

    /**
     * Counts number of posts.
     * 
     * @return int
     */
    public int countPosts();

    /**
     * Counts number of topics.
     * 
     * @return int
     */
    public int countTopics();

    /**
     * Creates a new box.
     * 
     * @param box Box
     * @return Box
     */
    public Box createBox(Box box);

    /**
     * Delets a box.
     * 
     * @param box Box
     */
    public void deleteBox(Box box);

    /**
     * Gets a box by id.
     * 
     * @param id int
     * @return Box
     */
    public Box getBox(int id);

    /**
     * Gets all available boxes in a tree-like hierarchy.
     * 
     * @return Box[]
     */
    public Box[] getBoxTree();

    /**
     * Gets published boxes for a topic.
     * 
     * @param topic Topic
     * @return Box[]
     */
    public Box[] getPublishedBoxes(Topic topic);

    /**
     * Updates an existing box.
     * 
     * @param box Box
     * @return Box
     */
    public Box updateBox(Box box);
}
