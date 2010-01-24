# Database schema for EIS
# Database type: MySQL 5.x

# Note: for MySQL, please use DELETE TABLE IF EXISTS and CREATE TABLE IF NOT EXISTS whenever possible!

# Run the following queries as root
#DROP DATABASE eis;
#CREATE DATABASE eis CHARACTER SET = utf8 COLLATE = utf8_general_ci;
#GRANT ALL PRIVILEGES ON eis.* TO 'eis'@'localhost' IDENTIFIED BY 'eis';

# Drop existing tables
DROP TABLE IF EXISTS eis_app_config;

# Create tables
CREATE TABLE IF NOT EXISTS eis_app_config (
	config_domain					VARCHAR(32),
	config_key						VARCHAR(64),
	config_long						BIGINT,
	config_double					DOUBLE,
	config_string					MEDIUMTEXT,
	config_boolean					CHAR(1),
	config_date						TIMESTAMP,
	config_binary					MEDIUMBLOB,
	INDEX (config_key),
	PRIMARY KEY (config_domain, config_key)
) Engine = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci;
INSERT INTO eis_app_config (config_domain, config_key, config_string) VALUES ('SITE', 'NAME', 'Knob');
INSERT INTO eis_app_config (config_domain, config_key, config_string) VALUES ('SITE', 'TITLE', 'Knob');
INSERT INTO eis_app_config (config_domain, config_key, config_string) VALUES ('SITE', 'KEYWORDS', 'Knob, CMS, Content Management System');
INSERT INTO eis_app_config (config_domain, config_key, config_string) VALUES ('SITE', 'DESCRIPTION', 'Knob - Content Management System');
