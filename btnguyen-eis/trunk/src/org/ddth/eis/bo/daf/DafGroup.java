package org.ddth.eis.bo.daf;

import java.io.Serializable;

import org.ddth.daf.impl.GroupImpl;

public class DafGroup extends GroupImpl implements Serializable {
    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = 5872939713249959141L;

    private String prefix, suffix;

    /**
     * Constructs a new DafGroup object.
     */
    public DafGroup() {
    }

    /**
     * Constructs a new DafGroup object.
     * 
     * @param isGod boolean
     * @param name String
     * @param description String
     * @param prefix String
     * @param suffix String
     */
    public DafGroup(boolean isGod, String name, String description,
            String prefix, String suffix) {
        this(null, isGod, name, description, prefix, suffix);
    }

    /**
     * Constructs a new DafGroup object with specific id.
     * 
     * @param id Object
     * @param isGod boolean
     * @param name String
     * @param description String
     * @param prefix String
     * @param suffix String
     */
    public DafGroup(Object id, boolean isGod, String name, String description,
            String prefix, String suffix) {
        setId(id);
        setIsGod(isGod);
        setName(name);
        setDescription(description);
        setPrefix(prefix);
        setSuffix(suffix);
    }

    // /**
    // * {@inheritDoc}
    // */
    // protected void setId(Object id) {
    // super.setId(id);
    // }
    //
    // /**
    // * Alias of {@link #isGod()}
    // */
    // public boolean getIsGod() {
    // return isGod();
    // }
    //
    // /**
    // * Alias of {@link #setIsGod(boolean)}
    // */
    // public void setIsGod(boolean isGod) {
    // markIsGod(isGod);
    // }
    //
    // /**
    // * {@inheritDoc}
    // */
    // public void markIsGod(boolean isGod) {
    // super.markIsGod(isGod);
    // }
    //
    // /**
    // * {@inheritDoc}
    // */
    // public void setDescription(String desc) {
    // super.setDescription(desc);
    // }
    //
    // /**
    // * {@inheritDoc}
    // */
    // public void setName(String name) {
    // super.setName(name);
    // }

    /**
     * Gets group's prefix.
     * 
     * @return String
     */
    public String getPrefix() {
        return this.prefix;
    }

    /**
     * Sets group's prefix.
     * 
     * @param prefix String
     */
    public void setPrefix(String prefix) {
        this.prefix = prefix;
    }

    /**
     * Gets group's suffix.
     * 
     * @return String
     */
    public String getSuffix() {
        return this.suffix;
    }

    /**
     * Sets group's suffix.
     * 
     * @param suffix
     */
    public void setSuffix(String suffix) {
        this.suffix = suffix;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean equals(Object o) {
        if ( !(o instanceof DafGroup) ) {
            return false;
        }

        DafGroup g = (DafGroup)o;
        return super.equals(g) && equalsPrefix(g) && equalsSuffix(g);
    }

    protected boolean equalsPrefix(DafGroup g) {
        return prefix != null
                ? prefix.equals(g.getPrefix()) : g.getPrefix() == null;
    }

    protected boolean equalsSuffix(DafGroup g) {
        return suffix != null
                ? suffix.equals(g.getSuffix()) : g.getSuffix() == null;
    }

    @Override
    public int hashCode() {
        int hashCode = super.hashCode();
        hashCode ^= prefix != null
                ? prefix.hashCode() : 0;
        hashCode ^= suffix != null
                ? suffix.hashCode() : 0;
        return hashCode;
    }
}
