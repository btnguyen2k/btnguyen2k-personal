package org.ddth.eis.bo.daf;

import java.util.Collection;
import java.util.List;

import org.ddth.daf.DataManipulator;
import org.ddth.daf.DataProvider;
import org.ddth.daf.Group;
import org.ddth.daf.GroupRule;
import org.ddth.daf.Permission;
import org.ddth.daf.User;
import org.ddth.daf.UserProfile;
import org.ddth.daf.UserRule;
import org.ddth.daf.UserProfile.Id;
import org.ddth.daf.utils.DafException;

public interface DafDataManager extends DataProvider, DataManipulator {

	public final static int SORT_NONE = 0;

	public final static int SORT_ID = 1;

	public final static int SORT_DEFAULT = SORT_ID;

	public final static int SORT_LOGIN_NAME = 2;

	public final static int SORT_REGISTER_TIME = 3;

	public final static int SORT_LAST_UPDATE_TIME = 4;

	public final static int SORT_FIRST_NAME = 5;

	public final static int SORT_LAST_NAME = 6;

	public final static int SORT_DOB = 7;

	/**
	 * {@inheritDoc}
	 */
	public Collection<? extends DafGroup> getAllGroups() throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public Collection<? extends DafPermission> getAllPermissions()
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafGroup getGroup(Object groupId) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public Collection<? extends DafGroupRule> getGroupRules(Group group)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public Collection<? extends DafGroup> getGroups(Object[] groupIds)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public Collection<? extends DafGroup> getGroups(Collection<Object> groupIds)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUser getNewestUser() throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafPermission getPermission(String module, String action)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public Collection<? extends DafPermission> getPermissions(String module)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUser getUser(Object userId) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUser getUser(String loginName) throws DafException;

	/**
	 * Retrieves a User by its email address.
	 * 
	 * @param email
	 *            String
	 * @return DafUser
	 * @throws DafException
	 */
	public DafUser getUserByEmail(String email) throws DafException;

	/**
	 * Retrieves list of users with filtering criteria.
	 * 
	 * @param filter
	 *            DafUserFilter
	 * @return List<? extends DafUser>
	 * @throws DafException
	 */
	public List<? extends DafUser> getUsers(DafUserFilter filter)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public Collection<? extends DafUser> getUsers(Object[] userIds)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public Collection<? extends DafUser> getUsers(String[] loginNames)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUser assignUserToGroup(Object userId, Object groupId)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUser assignUserToGroup(User user, Group group)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafGroup createGroup(Group data) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafPermission createPermission(Permission data) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUser createUser(User data) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public void deleteGroup(Object groupId) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public void deleteGroup(Group group) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public void deleteUser(Object userId) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public void deleteUser(User user) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUser removeUserFromGroup(Object userId, Object groupId)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUser removeUserFromGroup(User user, Group group)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafGroup setGroupPermissions(Object groupId,
			Collection<? extends GroupRule> rules) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafGroup setGroupPermissions(Group group,
			Collection<? extends GroupRule> rules) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUser setUserPermissions(Object userId,
			Collection<? extends UserRule> rules) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUser setUserPermissions(User user,
			Collection<? extends UserRule> rules) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafGroup updateGroup(Group group) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUser updateUser(User user) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUserProfile createUserProfile(UserProfile data)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUserProfile getUserProfile(Id id) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public Collection<? extends DafUserProfile> getAllUserProfiles(User user)
			throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public Collection<? extends DafUserProfile> getUserProfiles(User user,
			String module) throws DafException;

	/**
	 * {@inheritDoc}
	 */
	public DafUserProfile updateUserProfile(UserProfile userProfile)
			throws DafException;
}
