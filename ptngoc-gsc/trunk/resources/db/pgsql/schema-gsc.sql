-- PandaPortal: Schema for GSC tables
-- DBMS: PostgreSQL - version: N/A
-- Prerequisite: N/A

-- Populate GSC application configurations
-- //Populate GSC application configurations

-- Populate GSC permissions
-- //Populate TXBB permissions

-- Populate GSC user groups
-- //Populate GSC user groups

-- Populate GSC group rules
-- //Populate TXBB group rules

---------- GSC Tables ----------
SELECT panda_drop_table_if_exists('gsc_card');
SELECT panda_drop_table_if_exists('gsc_customer');
SELECT panda_drop_table_if_exists('gsc_invoice');

CREATE TABLE gsc_card (
	cid							VARCHAR(64),
	cissued_timestamp			INTEGER				NOT NULL DEFAULT 0,
	PRIMARY KEY (cid)
);
CREATE INDEX gsc_card_issued_timestamp ON gsc_card(cissued_timestamp);

CREATE TABLE gsc_customer (
	cid							SERIAL,
	ccard_id					VARCHAR(64)			NOT NULL,
		FOREIGN KEY (ccard_id) REFERENCES gsc_card(cid),
	cref_card_id				VARCHAR(64),
		FOREIGN KEY (cref_card_id) REFERENCES gsc_card(cid),
	PRIMARY KEY (cid)
);
---------- GSC Tables ----------
