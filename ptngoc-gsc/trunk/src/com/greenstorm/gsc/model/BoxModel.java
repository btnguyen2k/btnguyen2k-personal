package com.greenstorm.gsc.model;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.ddth.panda.core.ApplicationRepository;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.module.ModuleDescriptor;
import org.ddth.panda.web.UrlCreator;
import org.ddth.txbb.model.BoxModel;
import org.ddth.txbb.model.TopicModel;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.TxbbPermissionConstants;
import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.Topic;
import com.greenstorm.gsc.bo.TxbbManager;
import com.greenstorm.gsc.util.SeoUnicode;
import com.greenstorm.gsc.util.TxbbHelper;

/**
 * A data model bean that encapsulates a Box object and delegates selected
 * method calls to this object.
 * 
 * @author Thanh Ba Nguyen
 */
public class BoxModel {
    private Box box;

    /**
     * Gets an instance of BoxModel.
     * 
     * @param box Box
     * @return BoxModel
     */
    public static BoxModel getInstance(Box box) {
        PandaPortalApplication app =
                (PandaPortalApplication)ApplicationRepository.getCurrentApp();
        String boxId = String.valueOf(box.getId());
        BoxModel result = app.getAttribute(boxId, BoxModel.class);
        if ( result == null ) {
            result = new BoxModel(box);
        } else {
            result.box = box;
            result.invalidateCache();
        }
        return result;
    }

    protected BoxModel(Box box) {
        this.box = box;
    }

    protected void invalidateCache() {
        // TODO
    }

    public boolean canView() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            return app.hasPermission(
                    TxbbPermissionConstants.PERMISSION_VIEW_BOX, box);
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    public boolean canPublish() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            return app.hasPermission(
                    TxbbPermissionConstants.PERMISSION_PUBLISH_TOPIC, box);
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    public TopicModel getLastTopic() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            TxbbManager txbbMan =
                    app.getBundleManager().getService(TxbbManager.class);
            Collection<Box> viewableBoxes = new ArrayList<Box>();
            viewableBoxes.add(box);
            for ( Box child : box.getChildren() ) {
                if ( app.hasPermission(
                        TxbbPermissionConstants.PERMISSION_VIEW_BOX, child) )
                    viewableBoxes.add(child);
            }
            Topic[] lastTopic =
                    txbbMan.getRecentPublishedTopics(1, viewableBoxes);
            return lastTopic != null && lastTopic.length > 0
                    ? TopicModel.getInstance(lastTopic[0]) : null;
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    private TopicModel[] recentPublishedTopics;

    public TopicModel[] getRecentPublishedTopics() {
        try {
            if ( recentPublishedTopics == null ) {
                PandaPortalApplication app =
                        (PandaPortalApplication)ApplicationRepository.getCurrentApp();
                TxbbManager txbbMan =
                        app.getBundleManager().getService(TxbbManager.class);

                Collection<Box> viewableBoxes = new ArrayList<Box>();
                viewableBoxes.add(box);
                for ( Box child : box.getChildren() ) {
                    if ( app.hasPermission(
                            TxbbPermissionConstants.PERMISSION_VIEW_BOX, child) ) {
                        viewableBoxes.add(child);
                    }
                }
                // recent published topics
                Topic[] topics =
                        txbbMan.getRecentPublishedTopics(
                                TxbbConstants.NUM_RECENT_PUBLISHED_TOPICS_PER_BOX,
                                viewableBoxes);
                Collection<TopicModel> result = new ArrayList<TopicModel>();
                for ( Topic topic : topics ) {
                    result.add(TopicModel.getInstance(topic));
                }
                recentPublishedTopics = result.toArray(new TopicModel[0]);
            }

            return recentPublishedTopics;
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    public int getId() {
        return box.getId();
    }

    public String getTitle() {
        return box.getTitle();
    }

    public String getTitleForUrl() {
        try {
            String title = box.getTitle();
            SeoUnicode seoUnicode = TxbbHelper.getSeoUnicode();
            return seoUnicode != null
                    ? seoUnicode.textToUrl(getTitle()) : title;
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    public String getInnerDescription() {
        return box.getInnerDescription();
    }

    public String getOuterDescription() {
        return box.getOuterDescription();
    }

    public String getIconNewPost() {
        return box.getIconNewPost();
    }

    public String getIconNoNewPost() {
        return box.getIconNoNewPost();
    }

    private Collection<BoxModel> cacheChildren;

    public Collection<BoxModel> getChildren() {
        if ( cacheChildren == null ) {
            cacheChildren = new ArrayList<BoxModel>();
            for ( Box child : box.getChildren() ) {
                cacheChildren.add(new BoxModel(child));
            }
        }
        return cacheChildren;
    }

    private String urlView;

    public String getUrlView() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            if ( canView() ) {
                if ( urlView == null ) {
                    ModuleDescriptor moduleTxbb =
                            app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    List<String> vpParams = new ArrayList<String>();
                    vpParams.add(String.valueOf(getId()));
                    vpParams.add(getTitleForUrl());
                    urlView =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_VIEW_BOX,
                                    vpParams.toArray(new String[0]), null);
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

    private String urlEdit;

    public String getUrlEdit() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            if ( app.hasPermission(TxbbPermissionConstants.PERMISSION_MANAGE_BOXES) ) {
                if ( urlEdit == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    Map<String, String> params = new HashMap<String, String>();
                    params.put("id", String.valueOf(getId()));
                    urlEdit =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_ADMIN_EDIT_BOX, null,
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
            if ( app.hasPermission(TxbbPermissionConstants.PERMISSION_MANAGE_BOXES) ) {
                if ( urlDelete == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    Map<String, String> params = new HashMap<String, String>();
                    params.put("id", String.valueOf(getId()));
                    urlDelete =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_ADMIN_DELETE_BOX,
                                    null, params);
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

    private String urlMoveUp;

    public String getUrlMoveUp() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            if ( app.hasPermission(TxbbPermissionConstants.PERMISSION_MANAGE_BOXES) ) {
                if ( urlMoveUp == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    Map<String, String> params = new HashMap<String, String>();
                    params.put("id", String.valueOf(getId()));
                    urlMoveUp =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_ADMIN_MOVEUP_BOX,
                                    null, params);
                }
                return urlMoveUp;
            } else {
                return null;
            }
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }

    private String urlMoveDown;

    public String getUrlMoveDown() {
        try {
            PandaPortalApplication app =
                    (PandaPortalApplication)ApplicationRepository.getCurrentApp();
            if ( app.hasPermission(TxbbPermissionConstants.PERMISSION_MANAGE_BOXES) ) {
                if ( urlMoveDown == null ) {
                    ModuleDescriptor moduleTxbb;
                    moduleTxbb = app.getModule(TxbbConstants.MODULE_NAME);
                    UrlCreator urlCreator = app.getUrlCreator();
                    Map<String, String> params = new HashMap<String, String>();
                    params.put("id", String.valueOf(getId()));
                    urlMoveDown =
                            urlCreator.createUri(moduleTxbb.getUrlMapping(),
                                    TxbbConstants.ACTION_ADMIN_MOVEDOWN_BOX,
                                    null, params);
                }
                return urlMoveDown;
            } else {
                return null;
            }
        } catch ( Exception e ) {
            throw e instanceof RuntimeException
                    ? (RuntimeException)e : new RuntimeException(e);
        }
    }
}
