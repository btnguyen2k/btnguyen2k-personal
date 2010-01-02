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
	cname						VARCHAR(255),
	PRIMARY KEY (cid)
);
CREATE UNIQUE INDEX gsc_customer_card_id ON gsc_customer(ccard_id);
CREATE INDEX gsc_customer_ref_card_id ON gsc_customer(cref_card_id);

CREATE TABLE gsc_invoice (
	iid							SERIAL,
	icustomer_id				INTEGER				NOT NULL DEFAULT 0,
	icard_id					VARCHAR(64)			NOT NULL,
	itimestamp					INTEGER				NOT NULL DEFAULT 0,
	ivalue						FLOAT8				NOT NULL DEFAULT 0.0,
	PRIMARY KEY (iid)
);
CREATE INDEX gsc_invoice_customer_id ON gsc_invoice(icustomer_id);
CREATE INDEX gsc_invoice_card_id ON gsc_invoice(icard_id);
CREATE INDEX gsc_invoice_timestamp ON gsc_invoice(itimestamp);
---------- GSC Tables ----------
