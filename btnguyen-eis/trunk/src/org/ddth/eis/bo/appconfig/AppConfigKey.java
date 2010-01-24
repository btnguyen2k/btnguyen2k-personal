package org.ddth.eis.bo.appconfig;

import java.io.Serializable;

/**
 * <p>
 * Represents a key (which distinguishes a key with others) of an application
 * configuration.
 * </p>
 * <p>
 * A configuration key is a combination of domain (a.k.a. namespace, to
 * distinguish an application with others) and a key (to distinguish
 * configurations among an application).
 * </p>
 * 
 * @author Thanh Ba Nguyen <btnguyen2k@gmail.com>
 */
public class AppConfigKey implements Serializable {

	/**
	 * Auto-generated serial version UID.
	 */
	private static final long serialVersionUID = 7698324645906210382L;

	protected String domain;

	protected String key;

	/**
	 * Constructs a new AppConfigKey object.
	 */
	public AppConfigKey() {
	}

	/**
	 * Constructs a new AppConfigKey object.
	 * 
	 * @param domain
	 *            String
	 * @param key
	 *            String
	 */
	public AppConfigKey(String domain, String key) {
		this.domain = domain;
		this.key = key;
	}

	/**
	 * Retrieves domain value.
	 * 
	 * @return String
	 */
	public String getDomain() {
		return this.domain;
	}

	/**
	 * Sets domain value.
	 * 
	 * @param domain
	 *            String
	 */
	public void setDomain(String domain) {
		this.domain = domain;
	}

	/**
	 * Retrieves key value.
	 * 
	 * @return String
	 */
	public String getKey() {
		return this.key;
	}

	/**
	 * Sets key value.
	 * 
	 * @param key
	 *            String
	 */
	public void setKey(String key) {
		this.key = key;
	}

	/**
	 * {@inheritDoc}
	 */
	@Override
	public boolean equals(Object o) {
		if (o == null || !(o instanceof AppConfigKey)) {
			return false;
		}
		AppConfigKey c = (AppConfigKey) o;
		return equalsDomain(c) && equalsKey(c);
	}

	private boolean equalsDomain(AppConfigKey ack) {
		return this.domain == null ? ack.domain == null : this.domain
				.equals(ack.domain);
	}

	private boolean equalsKey(AppConfigKey ack) {
		return this.key == null ? ack.key == null : this.key.equals(ack.key);
	}

	/**
	 * {@inheritDoc}
	 */
	@Override
	public int hashCode() {
		int hashCode = 0;
		hashCode ^= this.domain != null ? this.domain.hashCode() : 0;
		hashCode ^= this.key != null ? this.key.hashCode() : 0;
		return hashCode;
	}

	/**
	 * Convenient method to create a new AppConfigKey object.
	 * 
	 * @param domain
	 *            String
	 * @param key
	 *            String
	 * @return AppConfigKey
	 */
	public static AppConfigKey createAppConfigKey(String domain, String key) {
		return new AppConfigKey(domain, key);
	}
}
