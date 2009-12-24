package com.greenstorm.gsc.bo;

import java.util.Collection;
import java.util.Comparator;
import java.util.Set;
import java.util.TreeSet;

import org.ddth.daf.Group;
import org.ddth.daf.Passport;
import org.ddth.daf.Permission;
import org.ddth.daf.Resource;
import org.ddth.daf.User;
import org.ddth.txbb.bo.Box;
import org.ddth.txbb.bo.BoxModerateGroup;
import org.ddth.txbb.bo.BoxModerateMember;
import org.ddth.txbb.bo.BoxPermissionGroupPublishTopic;
import org.ddth.txbb.bo.BoxPermissionGroupViewTopic;

import com.greenstorm.gsc.TxbbPermissionConstants;

public class Box implements Resource {
    private int id;

    // private boolean isDeleted;

    private Integer parentId;

    private int position;

    private String iconNewPost, iconNoNewPost;

    private Integer lastTopicId;

    // private int type;

    private String title;

    private String outerDescription, innerDescription;

    private Set<Box> children = new TreeSet<Box>(new Comparator<Box>() {
        public int compare(Box b1, Box b2) {
            return b1.position - b2.position;
        }
    });

    private Set<BoxModerateGroup> groupModerating;

    private Set<BoxModerateMember> memberModerating;

    private Set<BoxPermissionGroupPublishTopic> permissionPublishTopic;

    private Set<BoxPermissionGroupViewTopic> permissionViewTopic;

    /**
     * Constructs a new Box object.
     */
    public Box() {
    }

    /**
     * Checks if this box has any child box.
     * 
     * @return boolean
     */
    public boolean hasChildren() {
        return children.size() > 0;
    }

    /**
     * Gets number of children.
     * 
     * @return int
     */
    public int getNumChildren() {
        return children.size();
    }

    /**
     * Gets all children.
     * 
     * @return Set<Box>
     */
    public Set<Box> getChildren() {
        return children;
    }

    /**
     * Adds a child.
     * 
     * @param box Box
     * @return Box the added child if successful, null otherwise
     */
    public Box addChild(Box box) {
        if ( canAddChild(box) ) {
            children.add(box);
            box.setParentId(id);
            return box;
        } else {
            return null;
        }
    }

    /**
     * Removes a child.
     * 
     * @param box Box
     * @return Box the removed child if successful, null otherwise
     */
    public Box removeChild(Box box) {
        if ( canRemoveChild(box) ) {
            children.remove(box);
            box.setParentId(null);
            return box;
        } else {
            return null;
        }
    }

    /**
     * Checks if a child is add-able.
     * 
     * @param box Box
     * @return boolean
     */
    protected boolean canAddChild(Box box) {
        return box != null
                && (box.getParentId() == null || box.getParentId().equals(id));
    }

    /**
     * Checks if a child is remove-able.
     * 
     * @param box Box
     * @return boolean
     */
    protected boolean canRemoveChild(Box box) {
        return box != null && children.contains(box);
    }

    public Integer getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    // public boolean isDeleted() {
    // return isDeleted;
    // }

    // public void setDeleted(boolean isDeleted) {
    // this.isDeleted = isDeleted;
    // }

    // public boolean getIsDeleted() {
    // return isDeleted;
    // }

    // public void setIsDeleted(boolean isDeleted) {
    // this.isDeleted = isDeleted;
    // }

    /**
     * @return the parentId
     */
    public Integer getParentId() {
        return parentId;
    }

    /**
     * @param parentId the parentId to set
     */
    public void setParentId(Integer parentId) {
        this.parentId = parentId;
    }

    /**
     * @return the position
     */
    public int getPosition() {
        return position;
    }

    /**
     * @param position the position to set
     */
    public void setPosition(int position) {
        this.position = position;
    }

    /**
     * @return the iconNewPost
     */
    public String getIconNewPost() {
        return iconNewPost;
    }

    /**
     * @param iconNewPost the iconNewPost to set
     */
    public void setIconNewPost(String iconNewPost) {
        this.iconNewPost = iconNewPost;
    }

    /**
     * @return the iconNoNewPost
     */
    public String getIconNoNewPost() {
        return iconNoNewPost;
    }

    /**
     * @param iconNoNewPost the iconNoNewPost to set
     */
    public void setIconNoNewPost(String iconNoNewPost) {
        this.iconNoNewPost = iconNoNewPost;
    }

    /**
     * @return the lastTopicId
     */
    public Integer getLastTopicId() {
        return lastTopicId;
    }

    /**
     * @param lastTopicId the lastTopicId to set
     */
    public void setLastTopicId(Integer lastTopicId) {
        this.lastTopicId = lastTopicId;
    }

    // public int getType() {
    // return type;
    // }

    // public void setType(int type) {
    // this.type = type;
    // }

    /**
     * @return the title
     */
    public String getTitle() {
        return title;
    }

    /**
     * @param title the title to set
     */
    public void setTitle(String title) {
        this.title = title;
    }

    /**
     * @return the outerDescription
     */
    public String getOuterDescription() {
        return outerDescription;
    }

    /**
     * @param outerDescription the outerDescription to set
     */
    public void setOuterDescription(String outerDescription) {
        this.outerDescription = outerDescription;
    }

    /**
     * @return the innerDescription
     */
    public String getInnerDescription() {
        return innerDescription;
    }

    /**
     * @param innerDescription the innerDescription to set
     */
    public void setInnerDescription(String innerDescription) {
        this.innerDescription = innerDescription;
    }

    public Set<BoxModerateGroup> getGroupModerating() {
        return groupModerating;
    }

    public void setGroupModerating(Set<BoxModerateGroup> groupModerating) {
        this.groupModerating = groupModerating;
    }

    public Set<BoxModerateMember> getMemberModerating() {
        return memberModerating;
    }

    public void setMemberModerating(Set<BoxModerateMember> memberModerating) {
        this.memberModerating = memberModerating;
    }

    public Set<BoxPermissionGroupPublishTopic> getPermissionPublishTopic() {
        return permissionPublishTopic;
    }

    public void setPermissionPublishTopic(
            Set<BoxPermissionGroupPublishTopic> permissionPublishTopic) {
        this.permissionPublishTopic = permissionPublishTopic;
    }

    public Set<BoxPermissionGroupViewTopic> getPermissionViewTopic() {
        return permissionViewTopic;
    }

    public void setPermissionViewTopic(
            Set<BoxPermissionGroupViewTopic> permissionViewTopic) {
        this.permissionViewTopic = permissionViewTopic;
    }

    /**
     * {@inheritDoc}
     */
    public boolean authorizePassport(Passport passport) {
        if ( passport == null )
            return false;
        Permission permission = passport.getPermission();
        if ( permission == null )
            return false;

        if ( permission.equals(TxbbPermissionConstants.PERMISSION_VIEW_BOX) ) {
            return canView(passport.getGroup()) || canView(passport.getUser());
        }

        if ( permission.equals(TxbbPermissionConstants.PERMISSION_PUBLISH_TOPIC) ) {
            return canPublish(passport.getGroup())
                    || canPublish(passport.getUser());
        }

        return false;
    }

    /**
     * Checks if a group can view this box.
     * 
     * @param group Group
     * @return boolean
     */
    public boolean canView(Group group) {
        if ( group == null || permissionViewTopic == null ) {
            return false;
        }
        if ( group.isGod() ) {
            return true;
        }
        for ( BoxPermissionGroupViewTopic bpgvt : permissionViewTopic ) {
            if ( bpgvt.getGroupId().equals(group.getId()) ) {
                return true;
            }
        }
        return false;
    }

    /**
     * Checks if a user can publish topics in this box.
     * 
     * @param user User
     * @return boolean
     */
    public boolean canPublish(User user) {
        if ( user == null ) {
            return false;
        }
        Collection<? extends Group> groups = user.getRoles();
        if ( groups != null ) {
            for ( Group g : groups ) {
                if ( canPublish(g) ) {
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * Checks if a group can publish topics in this box.
     * 
     * @param group Group
     * @return boolean
     */
    public boolean canPublish(Group group) {
        if ( group == null || permissionPublishTopic == null ) {
            return false;
        }
        if ( group.isGod() ) {
            return true;
        }
        for ( BoxPermissionGroupPublishTopic bpgpt : permissionPublishTopic ) {
            if ( canView(group) && bpgpt.getGroupId().equals(group.getId()) ) {
                return true;
            }
        }
        return false;
    }

    /**
     * Checks if a user can view this box.
     * 
     * @param user User
     * @return boolean
     */
    public boolean canView(User user) {
        if ( user == null ) {
            return false;
        }
        Collection<? extends Group> groups = user.getRoles();
        if ( groups != null ) {
            for ( Group g : groups ) {
                if ( canView(g) ) {
                    return true;
                }
            }
        }
        return false;
    }
}
