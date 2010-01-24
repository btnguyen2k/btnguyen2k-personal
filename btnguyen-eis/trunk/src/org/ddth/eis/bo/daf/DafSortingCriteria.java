package org.ddth.eis.bo.daf;

public class DafSortingCriteria {

    private int column;

    private boolean descending;

    /**
     * Constructs a new DafSortingCriteria object.
     * 
     * @param column int
     */
    public DafSortingCriteria(int column) {
        this(column, false);
    }

    /**
     * Constructs a new DafSortingCriteria object.
     * 
     * @param column int
     * @param descending boolean
     */
    public DafSortingCriteria(int column, boolean descending) {
        setColumn(column);
        setDescending(descending);
    }

    public int getColumn() {
        return column;
    }

    public void setColumn(int column) {
        this.column = column;
    }

    public boolean isSortDescending() {
        return descending;
    }

    public void setDescending(boolean descending) {
        this.descending = descending;
    }
}
