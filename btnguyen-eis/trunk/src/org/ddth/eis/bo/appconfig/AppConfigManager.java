package org.ddth.eis.bo.appconfig;

/**
 * Manager interface that provides APIs to manipulate application
 * configurations.
 * 
 * @author Thanh Ba Nguyen <btnguyen2k@gmail.com>
 */
public interface AppConfigManager {

	/**
	 * Initializing method.
	 */
	public void init();

	/**
	 * Clean-up method.
	 */
	public void destroy();

	/**
	 * Deletes a configuration.
	 * 
	 * @param config
	 *            AppConfig
	 */
	public void deleteConfig(AppConfig config);

	/**
	 * Deletes a configuration.
	 * 
	 * @param configKey
	 *            AppConfigKey
	 */
	public void deleteConfig(AppConfigKey configKey);

	/**
	 * Deletes a configuration.
	 * 
	 * @param domain
	 *            String
	 * @param key
	 *            String
	 */
	public void deleteConfig(String domain, String key);

	/**
	 * Retrieves all configuration objects.
	 * 
	 * @return AppConfig[]
	 */
	public AppConfig[] loadAllConfigs();

	/**
	 * Retrieves all configuration objects within a domain.
	 * 
	 * @param domain
	 *            String
	 * @return AppConfig[]
	 */
	public AppConfig[] loadConfigs(String domain);

	/**
	 * Retrieves a configuration object.
	 * 
	 * @param configKey
	 *            AppConfigKey
	 * @return the retrieved configuration object, or null if not found
	 */
	public AppConfig loadConfig(AppConfigKey configKey);

	/**
	 * Retrieves a configuration object.
	 * 
	 * @param String
	 *            domain
	 * @param String
	 *            key
	 * @return the retreived configuration object, or null if not found
	 */
	public AppConfig loadConfig(String domain, String key);

	/**
	 * Saves an application configuration.
	 * 
	 * @param config
	 *            AppConfig
	 */
	public void saveConfig(AppConfig config);
}
