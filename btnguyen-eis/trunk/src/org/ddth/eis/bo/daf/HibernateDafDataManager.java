package org.ddth.eis.bo.daf;

import java.util.Collection;
import java.util.List;

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

/**
 * Hibernate-implementation of {@link DafDataManager}.
 * 
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class HibernateDafDataManager implements DafDataManager {

	public DafUser assignUserToGroup(Object userId, Object groupId)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUser assignUserToGroup(User user, Group group)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafGroup createGroup(Group data) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafPermission createPermission(Permission data) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUser createUser(User data) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUserProfile createUserProfile(UserProfile data)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public void deleteGroup(Object groupId) throws DafException {
		// TODO Auto-generated method stub

	}

	public void deleteGroup(Group group) throws DafException {
		// TODO Auto-generated method stub

	}

	public void deleteUser(Object userId) throws DafException {
		// TODO Auto-generated method stub

	}

	public void deleteUser(User user) throws DafException {
		// TODO Auto-generated method stub

	}

	public Collection<? extends DafGroup> getAllGroups() throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Collection<? extends DafPermission> getAllPermissions()
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Collection<? extends DafUserProfile> getAllUserProfiles(User user)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafGroup getGroup(Object groupId) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Collection<? extends DafGroupRule> getGroupRules(Group group)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Collection<? extends DafGroup> getGroups(Object[] groupIds)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Collection<? extends DafGroup> getGroups(Collection<Object> groupIds)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUser getNewestUser() throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafPermission getPermission(String module, String action)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Collection<? extends DafPermission> getPermissions(String module)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUser getUser(Object userId) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUser getUser(String loginName) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUser getUserByEmail(String email) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUserProfile getUserProfile(Id id) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Collection<? extends DafUserProfile> getUserProfiles(User user,
			String module) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public List<? extends DafUser> getUsers(DafUserFilter filter)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Collection<? extends DafUser> getUsers(Object[] userIds)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Collection<? extends DafUser> getUsers(String[] loginNames)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUser removeUserFromGroup(Object userId, Object groupId)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUser removeUserFromGroup(User user, Group group)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafGroup setGroupPermissions(Object groupId,
			Collection<? extends GroupRule> rules) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafGroup setGroupPermissions(Group group,
			Collection<? extends GroupRule> rules) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUser setUserPermissions(Object userId,
			Collection<? extends UserRule> rules) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUser setUserPermissions(User user,
			Collection<? extends UserRule> rules) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafGroup updateGroup(Group group) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUser updateUser(User user) throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public DafUserProfile updateUserProfile(UserProfile userProfile)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Passport getGroupPassport(Group arg0, Permission arg1)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Passport getGroupPassport(Group arg0, Permission arg1, Resource arg2)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Number getNumRegisteredAccounts() throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Number getNumUsers() throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Passport getUserPassport(User arg0, Permission arg1)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public Passport getUserPassport(User arg0, Permission arg1, Resource arg2)
			throws DafException {
		// TODO Auto-generated method stub
		return null;
	}

	public void deleteAllUserProfiles(User arg0) throws DafException {
		// TODO Auto-generated method stub

	}

	public void deleteUserProfile(UserProfile arg0) throws DafException {
		// TODO Auto-generated method stub

	}

	public void deleteUserProfiles(User arg0, String arg1) throws DafException {
		// TODO Auto-generated method stub

	}
}