package org.ddth.eis.bo.skillinventory;

/**
 * @author Thanh Ba Nguyen &lt;btnguyen2k@gmail.com&gt;
 */
public class SkillItem {
    private int    id, categoryId;
    private String name;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getCategoryId() {
        return categoryId;
    }

    public void setCategoryId(int categoryId) {
        this.categoryId = categoryId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
