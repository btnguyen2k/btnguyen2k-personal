package com.greenstorm.gsc.model;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.ddth.panda.core.ApplicationRepository;
import org.ddth.panda.core.daf.DafUser;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.module.ModuleDescriptor;
import org.ddth.panda.web.UrlCreator;
import org.ddth.txbb.model.BoxModel;
import org.ddth.txbb.model.TopicContentModel;
import org.ddth.txbb.model.TopicModel;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.TxbbPermissionConstants;
import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.Topic;
import com.greenstorm.gsc.bo.TopicContent;
import com.greenstorm.gsc.bo.TxbbManager;
import com.greenstorm.gsc.util.SeoUnicode;
import com.greenstorm.gsc.util.TXCodeParser;
import com.greenstorm.gsc.util.TxbbHelper;

/**
 * A data model bean that encapsulates a Topic object and delegates selected
 * method calls to this object.
 * 
 * @author Thanh Ba Nguyen
 */
public class TopicModel {
    private Topic topic;

    /**
     * Gets an instance of TopicModel.
     * 
     * @param topic Topic
     * @return TopicModel
     */
    public static TopicModel getInstance(Topic topic) {
        PandaPortalApplication app =
                (PandaPortalApplication)ApplicationRepository.getCurrentApp();
        String topicId = String.valueOf(topic.getId());
        TopicModel result = app.getAttribute(topicId, TopicModel.class);
        if ( result == null ) {
            result = new TopicModel(topic);
        } else {
            result.topic = topic;
            result.invalidateCache();
        }
        return result;
    }

    protected TopicModel(Topic topic) {
        this.topic = topic;
    }

    protected void invalidateCache() {
        // TODO
    }

    public int getId() {
        return topic.getId();
    }

    public String getType() {
        return topic.getType();
    }

    public String getTitle() {
        return topic.getTitle();
    }

    public String getTitleExcerpt(int maxLength) {
        String title = topic.getTitle();
        if ( maxLength < title.length() ) {
            title = title.substring(0, maxLength);
        }
        return title.trim() + "...";
    }

    public String getTitleForUrl() {
        try {
            String title = topic.getTitle();
            SeoUnicode seoUnicode = TxbbHelper.getSeoUnicode();
            return seoUnicode != null
                    ? seoUnicode.textToUrl(getTitle()) : title;
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    public boolean isLocked() {
        return topic.isLocked();
    }

    private String plainContent;

    public String getContentExcerpt(int maxLength) {
        try {
            if ( plainContent == null ) {
                Collection<TopicContent> contents = topic.getTopicContents();
                if ( contents != null ) {
                    TXCodeParser txcodeParser = TxbbHelper.getTxbbCodeParser();
                    String content = contents.iterator().next().getContent();
                    plainContent = txcodeParser.parsePlain(content).trim();
                } else {
                    plainContent = "";
                }
            }
            return plainContent.substring(0, plainContent.length() > maxLength
                    ? maxLength : plainContent.length()).replaceAll(
                    "/[^\\.,]+/$", "");
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    private Collection<TopicContentModel> cacheContents;

    public Collection<TopicContentModel> getContents() {
        if ( cacheContents == null ) {
            cacheContents = new ArrayList<TopicContentModel>();
            Collection<TopicContent> contents = topic.getTopicContents();
            if ( contents != null ) {
                for ( TopicContent topicContent : contents ) {
                    cacheContents.add(new TopicContentModel(topicContent));
                }
            }
        }
        return cacheContents;
    }

    private List<String> cacheContentsForDisplay;

    public List<String> getContentsForDisplay() {
        try {
            if ( cacheContentsForDisplay == null ) {
                TXCodeParser txcodeParser = TxbbHelper.getTxbbCodeParser();
                cacheContentsForDisplay = new ArrayList<String>();
                Collection<TopicContent> contents = topic.getTopicContents();
                if ( contents != null ) {
                    for ( TopicContent topicContent : contents ) {
                        cacheContentsForDisplay.add(txcodeParser.parse(topicContent.getContent()));
                    }
                }
            }
            return cacheContentsForDisplay;
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    public String getCreationTime() {
        try {
            return TxbbHelper.timestamp2String(topic.getCreationTimestamp());
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    public String getLastupdateTime() {
        try {
            return TxbbHelper.timestamp2String(topic.getLastupdateTimestamp());
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }
    
    public int getNumComments() {
        return topic.getNumPosts();
    }
    
    public int getNumViews() {
        return topic.getNumViews();
    }

    private String urlEdit;

    public String getUrlEdit() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            DafUser currentUser = app.getCurrentUser();
            if ( currentUser != null
                    && ((Number)currentUser.getId()).intValue() == topic.getMemberId()
                            .intValue() ) {
                if ( urlEdit == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    Map<String, String> params = new HashMap<String, String>();
                    params.put("id", String.valueOf(getId()));
                    urlEdit =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_PC_EDIT_TOPIC, null,
                                    params);
                }
                return urlEdit;
            } else {
                return null;
            }
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    private String urlDelete;

    public String getUrlDelete() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            DafUser currentUser = app.getCurrentUser();
            if ( currentUser != null
                    && ((Number)currentUser.getId()).intValue() == topic.getMemberId()
                            .intValue() ) {
                if ( urlDelete == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    Map<String, String> params = new HashMap<String, String>();
                    params.put("id", String.valueOf(getId()));
                    urlDelete =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_PC_DELETE_TOPIC, null,
                                    params);
                }
                return urlDelete;
            } else {
                return null;
            }
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    private String urlLock;

    public String getUrlLock() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            DafUser currentUser = app.getCurrentUser();
            if ( currentUser != null
                    && ((Number)currentUser.getId()).intValue() == topic.getMemberId()
                            .intValue() ) {
                if ( urlLock == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    Map<String, String> params = new HashMap<String, String>();
                    params.put("id", String.valueOf(getId()));
                    urlLock =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_PC_LOCK_TOPIC, null,
                                    params);
                }
                return urlLock;
            } else {
                return null;
            }
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    private String urlUnlock;

    public String getUrlUnlock() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            DafUser currentUser = app.getCurrentUser();
            if ( currentUser != null
                    && ((Number)currentUser.getId()).intValue() == topic.getMemberId()
                            .intValue() ) {
                if ( urlUnlock == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    Map<String, String> params = new HashMap<String, String>();
                    params.put("id", String.valueOf(getId()));
                    urlUnlock =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_PC_UNLOCK_TOPIC, null,
                                    params);
                }
                return urlUnlock;
            } else {
                return null;
            }
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    private String urlPublish;

    public String getUrlPublish() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            DafUser currentUser = app.getCurrentUser();
            if ( currentUser != null
                    && ((Number)currentUser.getId()).intValue() == topic.getMemberId()
                            .intValue() ) {
                if ( urlPublish == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    Map<String, String> params = new HashMap<String, String>();
                    params.put("id", String.valueOf(getId()));
                    urlPublish =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_PC_PUBLISH_TOPIC,
                                    null, params);
                }
                return urlPublish;
            } else {
                return null;
            }
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    private String urlUnpublish;

    public String getUrlUnpublish() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            DafUser currentUser = app.getCurrentUser();
            if ( currentUser != null
                    && ((Number)currentUser.getId()).intValue() == topic.getMemberId()
                            .intValue() ) {
                if ( urlUnpublish == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    Map<String, String> params = new HashMap<String, String>();
                    params.put("id", String.valueOf(getId()));
                    urlUnpublish =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_PC_UNPUBLISH_TOPIC,
                                    null, params);
                }
                return urlUnpublish;
            } else {
                return null;
            }
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    public String getUrlViewViaBox(BoxModel box) {
        return getUrlViewViaBox(box.getId());
    }

    public String getUrlViewViaBox(Box box) {
        return getUrlViewViaBox(box.getId());
    }

    public String getUrlViewViaBox(int boxId) {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            TxbbManager txbbMan =
                    app.getBundleManager().getService(TxbbManager.class);
            Box box = txbbMan.getBox(boxId);
            if ( box == null ) {
                return null;
            }
            if ( app.hasPermission(TxbbPermissionConstants.PERMISSION_VIEW_BOX,
                    box) ) {
                ModuleDescriptor moduleTxbb =
                        app.getModule(TxbbConstants.MODULE_NAME);
                UrlCreator urlCreator = app.getUrlCreator();
                List<String> vParams = new ArrayList<String>();
                vParams.add(String.valueOf(getId()));
                vParams.add(getTitleForUrl());
                Map<String, String> urlParams = new HashMap<String, String>();
                urlParams.put("box", String.valueOf(box.getId()));
                return urlCreator.createUri(moduleTxbb.getUrlMapping(),
                        TxbbConstants.ACTION_VIEW_TOPIC_FROM_BOX,
                        vParams.toArray(new String[0]), urlParams);

            }
            return null;
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    private String urlView;

    public String getUrlView() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            TxbbManager txbbMan =
                    app.getBundleManager().getService(TxbbManager.class);
            // check for view permission
            Box[] publishedBoxes = txbbMan.getPublishedBoxes(topic);
            // if there is no published box, then topic is viewable via member's
            // blog
            boolean canView = topic.getIsPublished();
            if ( publishedBoxes != null && publishedBoxes.length > 0 ) {
                // assume there is no viewable box
                canView = false;
                for ( Box box : publishedBoxes ) {
                    if ( app.hasPermission(
                            TxbbPermissionConstants.PERMISSION_VIEW_BOX, box) ) {
                        // as long as there is a viewable box, this topic is
                        // viewable.
                        canView = true;
                        break;
                    }
                }
            }
            if ( canView ) {
                if ( urlView == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    List<String> vParams = new ArrayList<String>();
                    vParams.add(String.valueOf(getId()));
                    vParams.add(getTitleForUrl());
                    urlView =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_VIEW_TOPIC,
                                    vParams.toArray(new String[0]), null);
                }
                return urlView;
            } else {
                return null;
            }
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    public String getUrlViewCommentsViaBox(BoxModel box) {
        return getUrlViewCommentsViaBox(box.getId());
    }

    public String getUrlViewCommentsViaBox(Box box) {
        return getUrlViewCommentsViaBox(box.getId());
    }

    public String getUrlViewCommentsViaBox(int boxId) {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            TxbbManager txbbMan =
                    app.getBundleManager().getService(TxbbManager.class);
            Box box = txbbMan.getBox(boxId);
            if ( box == null ) {
                return null;
            }
            if ( app.hasPermission(TxbbPermissionConstants.PERMISSION_VIEW_BOX,
                    box) ) {
                ModuleDescriptor moduleTxbb =
                        app.getModule(TxbbConstants.MODULE_NAME);
                UrlCreator urlCreator = app.getUrlCreator();
                List<String> vParams = new ArrayList<String>();
                vParams.add(String.valueOf(getId()));
                vParams.add(getTitleForUrl());
                Map<String, String> urlParams = new HashMap<String, String>();
                urlParams.put("box", String.valueOf(box.getId()));
                return urlCreator.createUri(moduleTxbb.getUrlMapping(),
                        TxbbConstants.ACTION_VIEW_COMMENT_FROM_BOX,
                        vParams.toArray(new String[0]), urlParams);

            }
            return null;
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    private String urlViewComments;

    public String getUrlViewComments() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            TxbbManager txbbMan =
                    app.getBundleManager().getService(TxbbManager.class);
            // check for view permission
            Box[] publishedBoxes = txbbMan.getPublishedBoxes(topic);
            // if there is no published box, then topic is viewable via member's
            // blog
            boolean canView = topic.getIsPublished();
            if ( publishedBoxes != null && publishedBoxes.length > 0 ) {
                // assume there is no viewable box
                canView = false;
                for ( Box box : publishedBoxes ) {
                    if ( app.hasPermission(
                            TxbbPermissionConstants.PERMISSION_VIEW_BOX, box) ) {
                        // as long as there is a viewable box, this topic is
                        // viewable.
                        canView = true;
                        break;
                    }
                }
            }
            if ( canView ) {
                if ( urlViewComments == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    List<String> vParams = new ArrayList<String>();
                    vParams.add(String.valueOf(getId()));
                    vParams.add(getTitleForUrl());
                    urlViewComments =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_VIEW_COMMENT,
                                    vParams.toArray(new String[0]), null);
                }
                return urlViewComments;
            } else {
                return null;
            }
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    /*
     * private String urlView; public String getUrlView() {
     * PandaPortalApplication app =
     * (PandaPortalApplication)ApplicationRepository.getCurrentApp(); if (
     * app.hasPermission(TxbbPermissionConstants.PERMISSION_VIEW_BOX, topic) ) {
     * // DafUser currentUser = app.getCurrentUser(); // DafGroup guestGroup =
     * app.getGroupGuest(); // if ( box.canView(currentUser, guestGroup) ) { if
     * ( urlView == null ) { SeoUnicode seoUnicode = TxbbHelper.getSeoUnicode();
     * ModuleDescriptor moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
     * UrlCreator urlCreator = app.getUrlCreator(); List<String> vpParams = new
     * ArrayList<String>(); vpParams.add(String.valueOf(getId()));
     * vpParams.add(seoUnicode.textToUrl(getTitle())); urlView =
     * urlCreator.createUri(moduleTxbb.getUrlMapping(),
     * TxbbConstants.ACTION_VIEW_BOX, vpParams.toArray(new String[0]), null); }
     * return urlView; } else { return null; } } private String urlEdit; public
     * String getUrlEdit() { PandaPortalApplication app =
     * (PandaPortalApplication)ApplicationRepository.getCurrentApp(); if (
     * app.hasPermission(TxbbPermissionConstants.PERMISSION_MANAGE_BOXES) ) { if
     * ( urlEdit == null ) { ModuleDescriptor moduleTxbb; moduleTxbb =
     * app.getModule(TxbbConstants.MODULE_NAME); UrlCreator urlCreator =
     * app.getUrlCreator(); Map<String, String> params = new HashMap<String,
     * String>(); params.put("id", String.valueOf(getId())); urlEdit =
     * urlCreator.createUri(moduleTxbb.getUrlMapping(),
     * TxbbConstants.ACTION_ADMIN_EDIT_BOX, null, params); } return urlEdit; }
     * else { return null; } } private String urlDelete; public String
     * getUrlDelete() { PandaPortalApplication app =
     * (PandaPortalApplication)ApplicationRepository.getCurrentApp(); if (
     * app.hasPermission(TxbbPermissionConstants.PERMISSION_MANAGE_BOXES) ) { if
     * ( urlDelete == null ) { ModuleDescriptor moduleTxbb; moduleTxbb =
     * app.getModule(TxbbConstants.MODULE_NAME); UrlCreator urlCreator =
     * app.getUrlCreator(); Map<String, String> params = new HashMap<String,
     * String>(); params.put("id", String.valueOf(getId())); urlDelete =
     * urlCreator.createUri(moduleTxbb.getUrlMapping(),
     * TxbbConstants.ACTION_ADMIN_DELETE_BOX, null, params); } return urlDelete;
     * } else { return null; } } private String urlMoveUp; public String
     * getUrlMoveUp() { PandaPortalApplication app =
     * (PandaPortalApplication)ApplicationRepository.getCurrentApp(); if (
     * app.hasPermission(TxbbPermissionConstants.PERMISSION_MANAGE_BOXES) ) { if
     * ( urlMoveUp == null ) { ModuleDescriptor moduleTxbb; moduleTxbb =
     * app.getModule(TxbbConstants.MODULE_NAME); UrlCreator urlCreator =
     * app.getUrlCreator(); Map<String, String> params = new HashMap<String,
     * String>(); params.put("id", String.valueOf(getId())); urlMoveUp =
     * urlCreator.createUri(moduleTxbb.getUrlMapping(),
     * TxbbConstants.ACTION_ADMIN_MOVEUP_BOX, null, params); } return urlMoveUp;
     * } else { return null; } } private String urlMoveDown; public String
     * getUrlMoveDown() { PandaPortalApplication app =
     * (PandaPortalApplication)ApplicationRepository.getCurrentApp(); if (
     * app.hasPermission(TxbbPermissionConstants.PERMISSION_MANAGE_BOXES) ) { if
     * ( urlMoveDown == null ) { ModuleDescriptor moduleTxbb; moduleTxbb =
     * app.getModule(TxbbConstants.MODULE_NAME); UrlCreator urlCreator =
     * app.getUrlCreator(); Map<String, String> params = new HashMap<String,
     * String>(); params.put("id", String.valueOf(getId())); urlMoveDown =
     * urlCreator.createUri(moduleTxbb.getUrlMapping(),
     * TxbbConstants.ACTION_ADMIN_MOVEDOWN_BOX, null, params); } return
     * urlMoveDown; } else { return null; } }
     */
}
