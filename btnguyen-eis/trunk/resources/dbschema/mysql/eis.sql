# Database schema for EIS
# Database type: MySQL 5.x

# Note: for MySQL, please use DELETE TABLE IF EXISTS and CREATE TABLE IF NOT EXISTS whenever possible!

# Run the following queries as root
#DROP DATABASE IF EXISTS eis;
#CREATE DATABASE eis CHARACTER SET = utf8 COLLATE = utf8_general_ci;
#GRANT ALL PRIVILEGES ON eis.* TO 'eis'@'localhost' IDENTIFIED BY 'eis';

# Drop existing tables
DROP TABLE IF EXISTS eis_skill_inventory;
DROP TABLE IF EXISTS eis_skill_item;
DROP TABLE IF EXISTS eis_skill_category;

DROP TABLE IF EXISTS eis_app_config;
DROP TABLE IF EXISTS daf_user_profile;
DROP TABLE IF EXISTS daf_role;
DROP TABLE IF EXISTS daf_group_rule;
DROP TABLE IF EXISTS daf_user_rule;
DROP TABLE IF EXISTS daf_permission;
DROP TABLE IF EXISTS daf_group;
DROP TABLE IF EXISTS daf_user;

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
INSERT INTO eis_app_config (config_domain, config_key, config_string) VALUES ('SITE', 'NAME', 'EIS');
INSERT INTO eis_app_config (config_domain, config_key, config_string) VALUES ('SITE', 'TITLE', 'EIS - Enterprise Intranet Suite');
INSERT INTO eis_app_config (config_domain, config_key, config_string) VALUES ('SITE', 'KEYWORDS', 'EIS, Enterprise, Intranet');
INSERT INTO eis_app_config (config_domain, config_key, config_string) VALUES ('SITE', 'DESCRIPTION', 'EIS - Enterprise Intranet Suite');
INSERT INTO eis_app_config (config_domain, config_key, config_string) VALUES ('SITE', 'SLOGAN', 'Too lazy to think of a slogan');

CREATE TABLE daf_permission (
	pmodule							VARCHAR(32)			NOT NULL,
	paction							VARCHAR(64)			NOT NULL,
	pdescription					VARCHAR(255),
	ppassport_class_name			VARCHAR(255)		NOT NULL,
	PRIMARY KEY (pmodule, paction)
) Engine = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci;
INSERT INTO daf_permission (pmodule, paction, pdescription, ppassport_class_name)
VALUES ('EIS', 'STAFF', 'Staff-level permission.', 'org.ddth.daf.impl.BooleanPassport');
INSERT INTO daf_permission (pmodule, paction, pdescription, ppassport_class_name)
VALUES ('EIS', 'HR', 'HR-level permission.', 'org.ddth.daf.impl.BooleanPassport');
INSERT INTO daf_permission (pmodule, paction, pdescription, ppassport_class_name)
VALUES ('EIS', 'MANAGER', 'Manager-level permission.', 'org.ddth.daf.impl.BooleanPassport');
INSERT INTO daf_permission (pmodule, paction, pdescription, ppassport_class_name)
VALUES ('EIS', 'IT', 'IT-level permission.', 'org.ddth.daf.impl.BooleanPassport');

CREATE TABLE daf_user (
	uid								INTEGER				AUTO_INCREMENT,
	ulogin_name						VARCHAR(64)			NOT NULL,
		UNIQUE INDEX (ulogin_name),
	upassword						VARCHAR(64)			NOT NULL,
	uemail							VARCHAR(64)			NOT NULL,
		INDEX (uemail),
	uregister_timestamp				INTEGER				NOT NULL DEFAULT 0,
		INDEX (uregister_timestamp),
	ulast_update_timestamp			INTEGER				NOT NULL DEFAULT 0,
		INDEX (ulast_update_timestamp),
	utitle							VARCHAR(16),
	ufirst_name						VARCHAR(32),
	umid_name						VARCHAR(64),
	ulast_name						VARCHAR(32),
	usex							SMALLINT			NOT NULL DEFAULT 0,
	udob_day						SMALLINT			NOT NULL DEFAULT 0,
	udob_month						SMALLINT			NOT NULL DEFAULT 0,
	udob_year						SMALLINT			NOT NULL DEFAULT 0,
	utime_zone_id					VARCHAR(64),
	PRIMARY KEY (uid)
) Engine = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci;

CREATE TABLE daf_user_profile (
	uid								INTEGER				NOT NULL,
	upmodule						VARCHAR(32)			NOT NULL,
		INDEX (upmodule),
	upname							VARCHAR(64)			NOT NULL,
	updata							MEDIUMBLOB,
	PRIMARY KEY (uid, upmodule, upname)
) Engine = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci;

CREATE TABLE daf_group (
	gid								INTEGER				AUTO_INCREMENT,
	gis_god							CHAR(1)				NOT NULL DEFAULT 'N',
	gname							VARCHAR(64)			NOT NULL,
	gdescription					VARCHAR(255),
	gprefix							VARCHAR(255),
	gsuffix							VARCHAR(255),
	PRIMARY KEY (gid)
) Engine = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci;

CREATE TABLE daf_role (
	uid								INTEGER				NOT NULL,
		INDEX (uid),
		FOREIGN KEY (uid) REFERENCES daf_user(uid) ON DELETE CASCADE,
	gid								INTEGER				NOT NULL,
		INDEX (gid),
		FOREIGN KEY (gid) REFERENCES daf_group(gid) ON DELETE CASCADE,
	PRIMARY KEY (uid, gid)
) Engine = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci;

CREATE TABLE daf_group_rule (
	gid								INTEGER				NOT NULL,
		INDEX (gid),
		FOREIGN KEY (gid) REFERENCES daf_group(gid) ON DELETE CASCADE,
	pmodule							VARCHAR(32)			NOT NULL,
	paction							VARCHAR(64)			NOT NULL,
		INDEX (pmodule, paction),
		FOREIGN KEY (pmodule, paction)
			REFERENCES daf_permission(pmodule, paction) ON DELETE CASCADE,
	is_global						CHAR(1)				NOT NULL DEFAULT 'N',
	PRIMARY KEY (gid, pmodule, paction)
) Engine = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci;

CREATE TABLE daf_user_rule (
	uid								INTEGER				NOT NULL,
		INDEX (uid),
		FOREIGN KEY (uid) REFERENCES daf_user(uid) ON DELETE CASCADE,
	pmodule							VARCHAR(32)			NOT NULL,
	paction							VARCHAR(64)			NOT NULL,
		INDEX (pmodule, paction),
		FOREIGN KEY (pmodule, paction)
			REFERENCES daf_permission(pmodule, paction) ON DELETE CASCADE,
	is_global						CHAR(1)				NOT NULL DEFAULT 'N',
	PRIMARY KEY (uid, pmodule, paction)
) Engine = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci;

	-- Default administrator account: loginname="admin"/password="password"
INSERT INTO daf_user (ulogin_name, upassword, uemail, uregister_timestamp, ulast_update_timestamp)
VALUES ('admin', '5f4dcc3b5aa765d61d8327deb882cf99', 'admin@localhost', UNIX_TIMESTAMP(), UNIX_TIMESTAMP());

	-- Default user groups 
INSERT INTO daf_group (gis_god, gname, gdescription, gprefix, gsuffix)
VALUES (1, 'Administrator', 'God administrator who has full privileges.', '<font color="#FF0000">', '</font>');

INSERT INTO daf_group (gis_god, gname, gdescription, gprefix, gsuffix)
VALUES (0, 'Staff', 'Normal staff.', '', '');

INSERT INTO daf_group (gis_god, gname, gdescription, gprefix, gsuffix)
VALUES (0, 'HR', 'HR staff.', '', '');

INSERT INTO daf_group (gis_god, gname, gdescription, gprefix, gsuffix)
VALUES (0, 'Manager', 'Manager staff.', '', '');

INSERT INTO daf_group (gis_god, gname, gdescription, gprefix, gsuffix)
VALUES (0, 'IT', 'IT staff.', '', '');

INSERT INTO daf_group (gis_god, gname, gdescription, gprefix, gsuffix)
VALUES (0, 'Guest', 'Guest user who normally has read-only privileges.', '', '');

INSERT INTO daf_group_rule (gid, pmodule, paction, is_global) VALUES (2, 'EIS', 'STAFF', 'Y');
INSERT INTO daf_group_rule (gid, pmodule, paction, is_global) VALUES (3, 'EIS', 'HR', 'Y');
INSERT INTO daf_group_rule (gid, pmodule, paction, is_global) VALUES (4, 'EIS', 'MANAGER', 'Y');
INSERT INTO daf_group_rule (gid, pmodule, paction, is_global) VALUES (5, 'EIS', 'IT', 'Y');

-- Administrator account - Administrator group
INSERT INTO daf_role (gid, uid) VALUES(1, 1);
