-- PandaPortal: Sample data for TXBB
-- DBMS: PostgreSQL - version: N/A
-- Prerequisite: schema-txbb.sql

-- User Account: moderator (loginname="moderator"/password="password")
INSERT INTO daf_user (ulogin_name, upassword, uemail, uregister_timestamp, ulast_update_timestamp)
VALUES ('moderator', '5f4dcc3b5aa765d61d8327deb882cf99', 'moderator@localhost', floor(date_part('epoch',now())), floor(date_part('epoch',now())));
-- Role: User Moderator <-> Group Moderator
INSERT INTO daf_role (gid, uid) VALUES((SELECT gid FROM daf_group WHERE gname='TXBB Moderator'), (SELECT uid FROM daf_user WHERE ulogin_name='moderator'));

-- User Account: member (loginname="member"/password="password")
INSERT INTO daf_user (ulogin_name, upassword, uemail, uregister_timestamp, ulast_update_timestamp)
VALUES ('member', '5f4dcc3b5aa765d61d8327deb882cf99', 'member@localhost', floor(date_part('epoch',now())), floor(date_part('epoch',now())));
-- Role: User Member <-> Group Member
INSERT INTO daf_role (gid, uid) VALUES((SELECT gid FROM daf_group WHERE gname='Member'), (SELECT uid FROM daf_user WHERE ulogin_name='member'));

