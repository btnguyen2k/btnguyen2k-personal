package org.ddth.eis.bo.skillinventory;

import java.io.Serializable;

import org.ddth.eis.bo.daf.DafUser;

/**
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class SkillInventory implements Serializable {
    /**
     * Autog-generated serrial version UID.
     */
    private static final long serialVersionUID = -2474094414059627717L;

    private DafUser           user;
    private SkillItem         skillItem;
    private int               userId, skillItemId, level;

    public int getUserId() {
        return userId;
    }

    public void setUserId(int userId) {
        this.userId = userId;
    }

    public int getSkillItemId() {
        return skillItemId;
    }

    public void setSkillItemId(int skillItemId) {
        this.skillItemId = skillItemId;
    }

    public int getLevel() {
        return level;
    }

    public void setLevel(int level) {
        this.level = level;
    }

    public DafUser getUser() {
        return user;
    }

    public void setUser(DafUser user) {
        this.user = user;
    }

    public SkillItem getSkillItem() {
        return skillItem;
    }

    public void setSkillItem(SkillItem skillItem) {
        this.skillItem = skillItem;
    }
}
