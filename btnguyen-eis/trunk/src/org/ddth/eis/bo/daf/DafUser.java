package org.ddth.eis.bo.daf;

import java.io.Serializable;
import java.util.Calendar;
import java.util.Date;
import java.util.Set;
import java.util.TimeZone;

import org.ddth.daf.Group;
import org.ddth.daf.impl.UserImpl;

public class DafUser extends UserImpl implements Serializable {
	/**
	 * Auto-generated serial version UID.
	 */
	private static final long serialVersionUID = 3852998155072618466L;

	public final static int SEX_UNKNOWN = 0;

	public final static int SEX_MALE = 1;

	public final static int SEX_FEMALE = 2;

	private String email;

	private int registerTimestamp;

	private int lastUpdateTimestamp;

	private String firstName, midName, lastName;

	private int sex;

	private int dobDay, dobMonth, dobYear;

	private String timeZoneId;

	/**
	 * Constructs a new DafUser object.
	 */
	public DafUser() {
		// int currentTimestamp = SystemUtils.getCurrentTimestamp();
		// setRegisterTimestamp(currentTimestamp);
		// setLastUpdateTimestamp(currentTimestamp);
	}

	/*
	 * protected DafUser(Object userId, String loginName, String password,
	 * String email, int activationCode) { setId(userId);
	 * setLoginName(loginName); setPassword(password); setEmail(email);
	 * setActivationCode(activationCode); }
	 */

	private boolean verifyRoles(Set<?> groups) {
		if (groups != null) {
			for (Object g : groups) {
				if (!(g instanceof DafGroup))
					return false;
			}
		}
		return true;
	}

	/**
	 * {@inheritDoc}
	 */
	@SuppressWarnings("unchecked")
	@Override
	public Set<? extends DafGroup> getRoles() {
		return (Set<? extends DafGroup>) super.getRoles();
	}

	/**
	 * {@inheritDoc}
	 */
	@Override
	synchronized public void setRoles(Set<? extends Group> groups) {
		if (verifyRoles(groups)) {
			super.setRoles(groups);
		} else {
			throw new IllegalArgumentException(
					"Argument is not of type Set<? extends DafGroup>.");
		}
	}

	/**
	 * {@inheritDoc}
	 */
	@Override
	synchronized public void addRole(Group group) {
		if (!(group instanceof DafGroup)) {
			throw new IllegalArgumentException(
					"Argument is not of type DafGroup.");
		}
		super.addRole(group);
	}

	/**
	 * Gets email address.
	 * 
	 * @return String
	 */
	public String getEmail() {
		return this.email;
	}

	/**
	 * Sets email address.
	 * 
	 * @param email
	 *            String
	 */
	public void setEmail(String email) {
		this.email = email;
	}

	/**
	 * Gets first name.
	 * 
	 * @return String
	 */
	public String getFirstName() {
		return this.firstName;
	}

	/**
	 * Sets first name.
	 * 
	 * @param firstName
	 *            String
	 */
	public void setFirstName(String firstName) {
		this.firstName = firstName;
	}

	/**
	 * Gets middle name.
	 * 
	 * @return String
	 */
	public String getMiddleName() {
		return this.midName;
	}

	/**
	 * Sets middle name.
	 * 
	 * @param midName
	 *            String
	 */
	public void setMiddleName(String midName) {
		this.midName = midName;
	}

	/**
	 * Gets last name.
	 * 
	 * @return String
	 */
	public String getLastName() {
		return this.lastName;
	}

	/**
	 * Sets last name.
	 * 
	 * @param lastName
	 *            String
	 */
	public void setLastName(String lastName) {
		this.lastName = lastName;
	}

	/**
	 * Gets account's register timestamp (UNIX timestamp).
	 * 
	 * @return int
	 */
	public int getRegisterTimestamp() {
		return this.registerTimestamp;
	}

	/**
	 * Sets account's register timestamp (UNIX timestamp).
	 * 
	 * @param timestamp
	 *            int
	 */
	public void setRegisterTimestamp(int timestamp) {
		this.registerTimestamp = timestamp;
	}

	/**
	 * Gets account's last update timestamp (UNIX timestamp).
	 * 
	 * @return int
	 */
	public int getLastUpdateTimestamp() {
		return this.lastUpdateTimestamp;
	}

	/**
	 * Sets account's last update timestamp (UNIX timestamp).
	 * 
	 * @param timestamp
	 *            int
	 */
	public void setLastUpdateTimestamp(int timestamp) {
		this.lastUpdateTimestamp = timestamp;
	}

	/**
	 * Gets user's sex attribute.
	 * 
	 * @return int
	 */
	public int getSex() {
		return this.sex;
	}

	/**
	 * Sets user's sex attribute.
	 * 
	 * @param sex
	 *            int
	 */
	public void setSex(int sex) {
		this.sex = (sex == SEX_MALE || sex == SEX_FEMALE) ? sex : SEX_UNKNOWN;
	}

	/**
	 * Gets day component of user's DoB.
	 * 
	 * @return int
	 */
	public int getDobDay() {
		return this.dobDay;
	}

	/**
	 * Sets day component of user's DoB.
	 * 
	 * @param day
	 *            int
	 */
	public void setDobDay(int day) {
		this.dobDay = (day >= 1 && day <= 31) ? day : 1;
	}

	/**
	 * Gets month component of user's DoB.
	 * 
	 * @return int
	 */
	public int getDobMonth() {
		return this.dobMonth;
	}

	/**
	 * Sets month component of user's DoB.
	 * 
	 * @param month
	 *            int
	 */
	public void setDobMonth(int month) {
		this.dobMonth = (month >= 1 && month <= 12) ? month : 1;
	}

	/**
	 * Gets year component of user's DoB.
	 * 
	 * @return int
	 */
	public int getDobYear() {
		return this.dobYear;
	}

	/**
	 * Sets year component of user's DoB.
	 * 
	 * @param year
	 *            int
	 */
	public void setDobYear(int year) {
		this.dobYear = (year >= 1 && year <= 9999) ? year : 1970;
	}

	/**
	 * Gets user's DoB.
	 * 
	 * @return Date
	 */
	public Date getDob() {
		Calendar c = Calendar.getInstance();
		c.set(Calendar.DAY_OF_MONTH, dobDay);
		c.set(Calendar.MONTH, dobMonth - 1);
		c.set(Calendar.YEAR, dobYear);
		return c.getTime();
	}

	/**
	 * Sets user's DoB
	 * 
	 * @param day
	 *            int
	 * @param month
	 *            int
	 * @param year
	 *            int
	 */
	public void setDob(int day, int month, int year) {
		setDobDay(day);
		setDobMonth(month);
		setDobYear(year);
	}

	/**
	 * Sets user's DoB
	 * 
	 * @param dob
	 *            Date
	 */
	public void setDob(Date dob) {
		Calendar c = Calendar.getInstance();
		c.setTime(dob);
		setDobDay(c.get(Calendar.DAY_OF_MONTH));
		setDobMonth(c.get(Calendar.MONTH) + 1);
		setDobMonth(c.get(Calendar.YEAR));
	}

	/**
	 * Gets time zone id.
	 * 
	 * @return String
	 */
	public String getTimeZoneId() {
		return timeZoneId;
	}

	/**
	 * Set time zone id.
	 * 
	 * @param timeZoneId
	 *            String
	 */
	public void setTimeZoneId(String timeZoneId) {
		this.timeZoneId = timeZoneId;
		this.timeZone = null;
	}

	private TimeZone timeZone;

	/**
	 * Gets time zone.
	 * 
	 * @return TimeZone
	 */
	public TimeZone getTimeZone() {
		if (timeZone == null) {
			try {
				timeZone = TimeZone.getTimeZone(timeZoneId);
			} catch (Exception e) {
				timeZone = TimeZone.getTimeZone("UTC");
			}
		}
		return timeZone;
	}

	/**
	 * Gets time zone raw offset.
	 * 
	 * @return int time zone raw offset in seconds
	 */
	public int getTimeZoneRawOffset() {
		return getTimeZone().getRawOffset();
	}
}
