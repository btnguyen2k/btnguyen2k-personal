# Database schema for EIS - Skill Inventory
# Database type: MySQL 5.x
# Require: eis.sql

# Note: for MySQL, please use DELETE TABLE IF EXISTS and CREATE TABLE IF NOT EXISTS whenever possible!

DROP TABLE IF EXISTS eis_skill_inventory;
DROP TABLE IF EXISTS eis_skill_item;
DROP TABLE IF EXISTS eis_skill_category;

CREATE TABLE IF NOT EXISTS eis_skill_category (
	scid								INTEGER				AUTO_INCREMENT,
	scname								VARCHAR(64)			NOT NULL DEFAULT '',
	PRIMARY KEY(scid)
) Engine = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci;

CREATE TABLE IF NOT EXISTS eis_skill_item (
	siid								INTEGER				AUTO_INCREMENT,
	sicategory_id						INTEGER,
		INDEX (sicategory_id),
		FOREIGN KEY (sicategory_id) REFERENCES eis_skill_category(scid) ON DELETE CASCADE,
	siname								VARCHAR(64)			NOT NULL DEFAULT '',
	PRIMARY KEY (siid)
) Engine = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci;

CREATE TABLE IF NOT EXISTS eis_skill_inventory (
	siuser_id							INTEGER,
	siitem_id							INTEGER,
		INDEX (siitem_id),
	silevel								INTEGER				NOT NULL DEFAULT 0,
		INDEX (silevel),
	sinum_months_exp					INTEGER				NOT NULL DEFAULT 0,
		INDEX (sinum_months_exp),
	PRIMARY KEY (siuser_id,siitem_id),
	FOREIGN KEY (siuser_id) REFERENCES daf_user(uid) ON DELETE CASCADE,
	FOREIGN KEY (siitem_id) REFERENCES eis_skill_item(siid) ON DELETE CASCADE
) Engine = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci;
