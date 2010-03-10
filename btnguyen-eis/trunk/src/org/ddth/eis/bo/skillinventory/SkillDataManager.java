package org.ddth.eis.bo.skillinventory;

import java.util.Collection;

import org.ddth.eis.bo.daf.DafUser;

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
}
