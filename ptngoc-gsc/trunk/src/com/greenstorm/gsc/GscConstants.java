package com.greenstorm.gsc;

import org.ddth.panda.portal.PandaPortalConstants;

public class GscConstants extends PandaPortalConstants {
    public final static String MODULE_KEY_NAME = "GSC";

    public final static String MODULE_NAME = "gsc";

    public final static String ACTION_INDEX = "index";

    public final static double TXBB_POINTS_PER_TOPIC = 10.0;

    public final static double TXBB_POINTS_PER_POST = 3.0;

    public final static double TXBB_POINTS_PER_VIEW = 0.01;

    public final static String TITLE_SEPARATOR = " | ";

    public final static int NUM_RECENT_PUBLISHED_TOPICS_PER_BOX = 5;

    public final static int NUM_RECENT_PUBLISHED_TOPICS_ALL_BOXES = 12;

    public final static int NUM_RECENT_COMMENTED_TOPICS = 8;

    public final static String ACTION_HOME = "index";

    public final static String ACTION_VIEW_BOX = "box";

    public final static String ACTION_VIEW_TOPIC = "topic";

    public final static String ACTION_VIEW_COMMENT = "comment";

    public final static String ACTION_VIEW_TOPIC_FROM_BOX = "boxTopic";

    public final static String ACTION_VIEW_COMMENT_FROM_BOX = "boxComment";

    public final static String ACTION_PC_INDEX = "pcIndex";

    public final static String ACTION_PC_VIEW_DRAFT_TOPICS =
            "pcViewDraftTopics";

    public final static String ACTION_PC_VIEW_PUBLISHED_TOPICS =
            "pcViewPublishedTopics";

    public final static String ACTION_PC_CREATE_TOPIC = "pcCreateTopic";

    public final static String ACTION_PC_DELETE_TOPIC = "pcDeleteTopic";

    public final static String ACTION_PC_EDIT_TOPIC = "pcEditTopic";

    public final static String ACTION_PC_LOCK_TOPIC = "pcLockTopic";

    public final static String ACTION_PC_UNLOCK_TOPIC = "pcUnlockTopic";

    public final static String ACTION_PC_PUBLISH_TOPIC = "pcPublishTopic";

    public final static String ACTION_PC_UNPUBLISH_TOPIC = "pcUnpublishTopic";

    public final static String ACTION_ADMIN_HOME = "admin";

    public final static String ACTION_ADMIN_LIST_BOXES = "adminListBoxes";

    public final static String ACTION_ADMIN_CREATE_BOX = "adminCreateBox";

    public final static String ACTION_ADMIN_DELETE_BOX = "adminDeleteBox";

    public final static String ACTION_ADMIN_MOVEUP_BOX = "adminMoveUpBox";

    public final static String ACTION_ADMIN_MOVEDOWN_BOX = "adminMoveDownBox";

    public final static String ACTION_ADMIN_EDIT_BOX = "adminEditBox";

    public final static String PROPERTY_SEO_UNICODE_CLASS = "txbb.seoUnicode";

    public final static String PROPERTY_TXCODE_PARSER = "txbb.txCodeParser";

    public final static String SERVLET_CTX_ATTR_TXBB_MANAGER =
            "TXBB_TXBB_MANAGER";
}
