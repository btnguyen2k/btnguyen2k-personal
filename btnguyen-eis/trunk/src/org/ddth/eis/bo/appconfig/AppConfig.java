package org.ddth.eis.bo.appconfig;

/*
 * See the included README file(s) for Copyright notice and License information.
 */

import java.io.Serializable;
import java.util.Date;

/**
 * <p>
 * This class represents an application configuration.
 * </p>
 * <p>
 * A configuration can be retrieved as a number, a string, a boolean value or a
 * binary data array.
 * </p>
 * 
 * @author Thanh Ba Nguyen <btnguyen2k@gmail.com>
 */
public class AppConfig implements Serializable {

    /**
     * Auto-generated serial version UID.
     */
    private static final long serialVersionUID = 3975896072694397071L;

    private AppConfigKey key;

    private Long longValue;

    private Double doubleValue;

    private String stringValue;

    private Date dateValue;

    private Boolean booleanValue;

    private byte[] binaryValue;

    public AppConfigKey getKey() {
        return this.key;
    }

    public void setKey(AppConfigKey key) {
        this.key = key;
    }

    public Long getLongValue() {
        return this.longValue;
    }

    public void setLongValue(Long longValue) {
        this.longValue = longValue;
    }

    public void setLongValue(long longValue) {
        this.longValue = longValue;
    }

    public Integer getIntValue() {
        return this.longValue != null
                ? this.longValue.intValue() : null;
    }

    public void setIntValue(Integer intValue) {
        this.longValue = intValue != null
                ? intValue.longValue() : null;
    }

    public void setIntValue(int intValue) {
        this.longValue = (long)intValue;
    }

    public Double getDoubleValue() {
        return this.doubleValue;
    }

    public void setDoubleValue(Double doubleValue) {
        this.doubleValue = doubleValue;
    }

    public void setDoubleValue(double doubleValue) {
        this.doubleValue = doubleValue;
    }

    public Float getFloatValue() {
        return this.doubleValue != null
                ? this.doubleValue.floatValue() : null;
    }

    public void setFloatValue(Float floatValue) {
        this.doubleValue = floatValue != null
                ? floatValue.doubleValue() : null;
    }

    public void setFloatValue(float floatValue) {
        this.doubleValue = (double)floatValue;
    }

    public Date getDateValue() {
        return this.dateValue;
    }

    public void setDateValue(Date dateValue) {
        this.dateValue = dateValue;
    }

    public String getStringValue() {
        return this.stringValue;
    }

    public void setStringValue(String stringValue) {
        this.stringValue = stringValue;
    }

    public Boolean getBooleanValue() {
        return this.booleanValue;
    }

    public void setBooleanValue(Boolean booleanValue) {
        this.booleanValue = booleanValue;
    }
    
    public void setBooleanValue(boolean booleanValue) {
        this.booleanValue = booleanValue;
    }

    public byte[] getBinaryValue() {
        return this.binaryValue;
    }

    public void setBinaryValue(byte[] binaryValue) {
        this.binaryValue = binaryValue;
    }
}
