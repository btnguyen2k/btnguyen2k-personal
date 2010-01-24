package org.ddth.eis.bo.daf;

import java.io.UnsupportedEncodingException;

import org.ddth.daf.UserProfile;

public class DafUserProfile implements UserProfile {

    private Id id;

    private byte[] data;

    /**
     * Constructs a new DafUserProfile object.
     */
    public DafUserProfile() {
    }

    /**
     * Constructs a new DafUserProfile object.
     * 
     * @param id
     * @param data
     */
    public DafUserProfile(Id id, byte[] data) {
        setId(id);
        setData(data);
    }

    public Id getId() {
        return id;
    }

    public void setId(Id id) {
        this.id = id;
    }

    public byte[] getData() {
        return data;
    }

    public void setData(byte[] data) {
        this.data = data;
    }

    public void setValue(String data) {
        this.data = data != null
                ? data.getBytes() : null;
    }

    public void setValue(String data, String encoding)
            throws UnsupportedEncodingException {
        this.data = data != null
                ? data.getBytes(encoding) : null;
    }

    public void setValue(boolean value) {
        setValue(value
                ? "TRUE" : "FALSE");
    }

    public void setValue(double value) {
        setValue(Double.toString(value));
    }

    public void setValue(float value) {
        setValue(Float.toString(value));
    }

    public void setValue(int value) {
        setValue(Integer.toString(value));
    }

    public void setValue(long value) {
        setValue(Long.toString(value));
    }

    /**
     * Gets value as a string.
     * 
     * @return String
     */
    public String getValue() {
        return new String(this.data);
    }

    /**
     * Gets value as a string with specified encoding
     * 
     * @param encoding String
     * @return String
     * @throws UnsupportedEncodingException
     */
    public String getValue(String encoding) throws UnsupportedEncodingException {
        return new String(this.data, encoding);
    }

    /**
     * Gets value as boolean.
     * 
     * @return boolean
     */
    public boolean getValueAsBoolean() {
        String value = getValue();
        if ( value.equalsIgnoreCase("YES") || value.equalsIgnoreCase("Y")
                || value.equalsIgnoreCase("TRUE")
                || value.equalsIgnoreCase("T") )
            return true;
        return false;
    }

    /**
     * Gets value as double.
     * 
     * @return double
     */
    public double getValueAsDouble() {
        return Double.parseDouble(getValue());
    }

    /**
     * Gets value as float.
     * 
     * @return float
     */
    public float getValueAsFloat() {
        return Float.parseFloat(getValue());
    }

    /**
     * Gets value as int.
     * 
     * @return int
     */
    public int getConfigValueAsInt() {
        return Integer.parseInt(getValue());
    }

    /**
     * Gets value as long
     * 
     * @return long
     */
    public long getValueAsLong() {
        return Long.parseLong(getValue());
    }
}
