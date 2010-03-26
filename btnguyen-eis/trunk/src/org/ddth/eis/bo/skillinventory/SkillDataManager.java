package org.ddth.eis.bo.skillinventory;

import java.util.Collection;

import org.ddth.panda.daf.DafUser;

public interface SkillDataManager {
    /**
     * Deletes a skill category.
     * 
     * @param skillCategory
     *            SkillCategory
     */
    public void deleteSkillCategory(SkillCategory skillCategory);

    /**
     * Gets a skill category by id.
     * 
     * @param id
     *            int
     * @return SkillCategory
     */
    public SkillCategory getSkillCategory(int id);

    /**
     * Gets a skill item by id.
     * 
     * @param id
     *            int
     * @return SkillItem
     */
    public SkillItem getSkillItem(int id);

    /**
     * Gets all available skill categories.
     * 
     * @return Collection<? extends SkillCategory>
     */
    public Collection<? extends SkillCategory> getAllSkillCategories();

    /**
     * Creates a skill inventory entry.
     * 
     * @param skillInventory
     *            SkillInventory
     * @return SkillInventory
     */
    public SkillInventory createSkillInventory(SkillInventory skillInventory);

    /**
     * Deletes a skill inventory entry.
     * 
     * @param skillInventory
     *            SkillInventory
     */
    public void deleteSkillInventory(SkillInventory skillInventory);

    /**
     * Gets skill inventory info of a user.
     * 
     * @param user
     *            DafUser
     * @return Collection<? extends SkillInventory>
     */
    public Collection<? extends SkillInventory> getSkillInventories(DafUser user);

    /**
     * Finds users that has specific skill.
     * 
     * @param searchWithin
     *            Collection<? extends DafUser> find within only this collection of users, null to
     *            search all users
     * @param skillItem
     *            SkillItem
     * @param skillILevelOperator
     *            String one of "=", "<", ">", "<=", ">="
     * @param skillLevel
     *            int
     * @return Collection<? extends DafUser>
     */
    public Collection<? extends DafUser> searchSkill(Collection<? extends DafUser> searchWithin,
            SkillItem skillItem, String skillILevelOperator, int skillLevel);

    /**
     * Finds users that has specific skill and number of months experienced.
     * 
     * @param searchWithin
     *            Collection<? extends DafUser> find within only this collection of users, null to
     *            search all users
     * @param skillItem
     *            SkillItem
     * @param skillLevelOperator
     *            String one of "=", "<", ">", "<=", ">="
     * @param skillLevel
     *            int
     * @param monthsExpOperator
     *            String one of "=", "<", ">", "<=", ">="
     * @param monthsExp
     *            int
     * @return Collection<? extends DafUser>
     */
    public Collection<? extends DafUser> searchSkill(Collection<? extends DafUser> searchWithin,
            SkillItem skillItem, String skillLevelOperator, int skillLevel,
            String monthsExpOperator, int monthsExp);
}
