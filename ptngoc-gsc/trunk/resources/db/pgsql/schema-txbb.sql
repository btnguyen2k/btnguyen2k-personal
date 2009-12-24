-- PandaPortal: Schema for TXBB tables
-- DBMS: PostgreSQL - version: N/A
-- Prerequisite: schema-daf.sql

-- Populate TXBB application configurations

-- //Populate TXBB application configurations

-- Populate TXBB permissions
INSERT INTO daf_permission (pmodule, paction, pdescription, ppassport_class_name)
VALUES ('TXBB', 'VIEW_BOX', 'Has permission to view topics in a box.', 'org.ddth.daf.impl.BooleanPassport');

INSERT INTO daf_permission (pmodule, paction, pdescription, ppassport_class_name)
VALUES ('TXBB', 'PUBLISH_TOPIC', 'Has permission to publish topics.', 'org.ddth.daf.impl.BooleanPassport');

INSERT INTO daf_permission (pmodule, paction, pdescription, ppassport_class_name)
VALUES ('TXBB', 'ACCESS_ADMINCP', 'Has permission to access TXBB Administration Control Panel.', 'org.ddth.daf.impl.BooleanPassport');

INSERT INTO daf_permission (pmodule, paction, pdescription, ppassport_class_name)
VALUES ('TXBB', 'MANAGE_BOXES', 'Has permission to manage boxes.', 'org.ddth.daf.impl.BooleanPassport');
-- //Populate TXBB permissions

-- Populate TXBB user groups
INSERT INTO daf_group (gis_god, gname, gdescription, gprefix, gsuffix)
VALUES (0, 'TXBB Moderator', 'TXBB Moderator.', '', '');
-- //Populate TXBB user groups

-- Populate TXBB group rules
	-- User Group: TXBB Moderator can view box
INSERT INTO daf_group_rule (gid, pmodule, paction, is_global)
VALUES ((SELECT gid FROM daf_group WHERE gname='TXBB Moderator'), 'TXBB', 'VIEW_BOX', 'N');

	-- User Group: TXBB Moderator can publish topic
INSERT INTO daf_group_rule (gid, pmodule, paction, is_global)
VALUES ((SELECT gid FROM daf_group WHERE gname='TXBB Moderator'), 'TXBB', 'PUBLISH_TOPIC', 'N');

	-- User Group: Member can view box
INSERT INTO daf_group_rule (gid, pmodule, paction, is_global)
VALUES ((SELECT gid FROM daf_group WHERE gname='Member'), 'TXBB', 'VIEW_BOX', 'N');

	-- User Group: Member can publish topic
INSERT INTO daf_group_rule (gid, pmodule, paction, is_global)
VALUES ((SELECT gid FROM daf_group WHERE gname='Member'), 'TXBB', 'PUBLISH_TOPIC', 'N');

	-- User Group: Guest can view box
INSERT INTO daf_group_rule (gid, pmodule, paction, is_global)
VALUES ((SELECT gid FROM daf_group WHERE gname='Guest'), 'TXBB', 'VIEW_BOX', 'N');
-- //Populate TXBB group rules

---------- TXBB Tables ----------
SELECT panda_drop_table_if_exists('txbb_group_moderate_box');
SELECT panda_drop_table_if_exists('txbb_member_moderate_box');
SELECT panda_drop_table_if_exists('txbb_publishing');
SELECT panda_drop_table_if_exists('txbb_can_view_topic');
SELECT panda_drop_table_if_exists('txbb_can_publish_topic');
SELECT panda_drop_table_if_exists('txbb_box');
SELECT panda_drop_table_if_exists('txbb_post');
SELECT panda_drop_table_if_exists('txbb_topic_content');
SELECT panda_drop_table_if_exists('txbb_topic_attachment');
SELECT panda_drop_table_if_exists('txbb_topic');
SELECT panda_drop_table_if_exists('txbb_member');

CREATE TABLE txbb_member (
	mid							INTEGER				NOT NULL,
		FOREIGN KEY (mid) REFERENCES daf_user(uid) ON DELETE CASCADE,
	mnum_topics					INTEGER				NOT NULL DEFAULT 0,
	mnum_posts					INTEGER				NOT NULL DEFAULT 0,
	mtxbb_points				FLOAT8				NOT NULL DEFAULT 0.0,
	mlastvisit_timestamp		INTEGER				NOT NULL DEFAULT 0,
	mview_signature				CHAR(1)				NOT NULL DEFAULT 'Y',
	mview_avatar				CHAR(1)				NOT NULL DEFAULT 'Y',
	msignature					TEXT,
	PRIMARY KEY (mid)
);
CREATE INDEX txbb_member_num_topics ON txbb_member(mnum_topics);
CREATE INDEX txbb_member_num_posts ON txbb_member(mnum_posts);
CREATE INDEX txbb_member_lastvisit_timestamp ON txbb_member(mlastvisit_timestamp);

CREATE TABLE txbb_topic (
	tid							SERIAL,
	tmember_id					INTEGER				NOT NULL,
	ttitle						VARCHAR(255),
	ttype						VARCHAR(32)			NOT NULL,
	tcreation_timestamp			INTEGER				NOT NULL DEFAULT 0,
	tlastpost_timestamp			INTEGER				NOT NULL DEFAULT 0,
	tlastupdate_timestamp		INTEGER				NOT NULL DEFAULT 0,
	tnum_views					INTEGER				NOT NULL DEFAULT 0,
	tnum_posts					INTEGER				NOT NULL DEFAULT 0,
	tis_published				CHAR(1)				NOT NULL DEFAULT 'N',
	tis_locked					CHAR(1)				NOT NULL DEFAULT 'N',
	PRIMARY KEY (tid)
);
CREATE INDEX txbb_topic_member_id ON txbb_topic(tmember_id);
CREATE INDEX txbb_topic_type ON txbb_topic(ttype);
CREATE INDEX txbb_topic_creation_timestamp ON txbb_topic(tcreation_timestamp);
CREATE INDEX txbb_topic_lastpost_timestamp ON txbb_topic(tlastpost_timestamp);
CREATE INDEX txbb_topic_lastupdate_timestamp ON txbb_topic(tlastupdate_timestamp);

CREATE TABLE txbb_topic_attachment (
	taid						SERIAL,
	ttopic_id					INTEGER				NOT NULL,
		FOREIGN KEY (ttopic_id) REFERENCES txbb_topic(tid) ON DELETE CASCADE,
	tfile_name					VARCHAR(64)			NOT NULL DEFAULT '',
	tfile_size					INTEGER				NOT NULL DEFAULT 0,
	tfile_type					VARCHAR(64)			NOT NULL DEFAULT '',
	tfile_content				BYTEA,
	PRIMARY KEY (taid)
);
CREATE INDEX txbb_topic_attachment_topic_id ON txbb_topic_attachment(ttopic_id);

CREATE TABLE txbb_topic_content (
	tcid						SERIAL,
	ttopic_id					INTEGER				NOT NULL,
		FOREIGN KEY (ttopic_id) REFERENCES txbb_topic(tid) ON DELETE CASCADE,
	torder						INTEGER				NOT NULL DEFAULT 0,
	tcontent					TEXT,
	PRIMARY KEY (tcid)
);
CREATE INDEX txbb_topic_content_topic_id ON txbb_topic_content(ttopic_id);

CREATE TABLE txbb_post (
	pid							SERIAL,
	ptopic_id					INTEGER				NOT NULL,
		FOREIGN KEY (ptopic_id) REFERENCES txbb_topic(tid) ON DELETE CASCADE,
	pmember_id					INTEGER				NOT NULL,
	pcreation_timestamp			INTEGER				NOT NULL DEFAULT 0,
	ptitle						VARCHAR(255)		NOT NULL DEFAULT '',
	pcontent					TEXT,
	PRIMARY KEY (pid)
);
CREATE INDEX txbb_post_topic_id ON txbb_post(ptopic_id);
CREATE INDEX txbb_post_member_id ON txbb_post(pmember_id);
CREATE INDEX txbb_post_creation_timestamp ON txbb_post(pcreation_timestamp);

CREATE TABLE txbb_box (
	bid							SERIAL,
	bis_deleted					CHAR(1)				NOT NULL DEFAULT 'N',
	bparent_id					INTEGER,
	bposition					INTEGER				NOT NULL DEFAULT 0,
	bicon_new					VARCHAR(64),
	bicon_nonew					VARCHAR(64),
	blast_topic_id				INTEGER,
	btype						SMALLINT			NOT NULL DEFAULT 0,
	btitle						VARCHAR(255)		NOT NULL DEFAULT '',
	bouter_description			TEXT,
	binner_description			TEXT,
	PRIMARY KEY (bid)
);
CREATE INDEX txbb_box_parent_id ON txbb_box (bparent_id);
CREATE INDEX txbb_box_position ON txbb_box (bposition);

CREATE TABLE txbb_can_view_topic (
	bid							INTEGER,
		FOREIGN KEY (bid) REFERENCES txbb_box(bid) ON DELETE CASCADE,
	gid							INTEGER,
		FOREIGN KEY (gid) REFERENCES daf_group(gid) ON DELETE CASCADE,
	PRIMARY KEY (bid, gid)
);
CREATE INDEX txbb_can_view_topic_gid ON txbb_can_view_topic(gid);

CREATE TABLE txbb_can_publish_topic (
	bid							INTEGER,
		FOREIGN KEY (bid) REFERENCES txbb_box(bid) ON DELETE CASCADE,
	gid							INTEGER,
		FOREIGN KEY (gid) REFERENCES daf_group(gid) ON DELETE CASCADE,
	PRIMARY KEY (bid, gid)
);
CREATE INDEX txbb_can_publish_topic_gid ON txbb_can_publish_topic(gid);

CREATE TABLE txbb_publishing (
	bid							INTEGER				NOT NULL,
		FOREIGN KEY (bid) REFERENCES txbb_box(bid) ON DELETE CASCADE,
	tid							INTEGER				NOT NULL,
		FOREIGN KEY (tid) REFERENCES txbb_topic(tid) ON DELETE CASCADE,
	ttimestamp					INTEGER				NOT NULL DEFAULT 0,
	PRIMARY KEY (bid, tid)
);
CREATE INDEX txbb_publishing_tid ON txbb_publishing(tid);
CREATE INDEX txbb_publishing_ttimestamp ON txbb_publishing(ttimestamp);

CREATE TABLE txbb_member_moderate_box (
	bid							INTEGER				NOT NULL,
		FOREIGN KEY (bid) REFERENCES txbb_box(bid) ON DELETE CASCADE,
	mid							INTEGER				NOT NULL,
		FOREIGN KEY (mid) REFERENCES txbb_member(mid) ON DELETE CASCADE,
	PRIMARY KEY (bid, mid)
);
CREATE INDEX txbb_member_moderate_box_mid ON txbb_member_moderate_box(mid);

CREATE TABLE txbb_group_moderate_box (
	bid							INTEGER				NOT NULL,
		FOREIGN KEY (bid) REFERENCES txbb_box(bid) ON DELETE CASCADE,
	gid							INTEGER				NOT NULL,
		FOREIGN KEY (gid) REFERENCES daf_group(gid) ON DELETE CASCADE,
	PRIMARY KEY (bid, gid)
);
CREATE INDEX txbb_group_moderate_box_gid ON txbb_group_moderate_box(gid);
---------- TXBB Tables ----------
