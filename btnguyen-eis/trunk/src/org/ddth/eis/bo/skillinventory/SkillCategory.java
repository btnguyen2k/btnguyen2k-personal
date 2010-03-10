package org.ddth.eis.bo.skillinventory;

import java.util.Set;

/**
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class SkillCategory {
    private int            id;
    private String         name;
    private Set<SkillItem> skillItems;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Set<SkillItem> getSkillItems() {
        return skillItems;
    }

    public void setSkillItems(Set<SkillItem> skillItems) {
        this.skillItems = skillItems;
    }
}
