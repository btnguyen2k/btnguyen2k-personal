package com.greenstorm.gsc.util;

import java.util.ArrayList;
import java.util.Collection;

import org.ddth.ehconfig.AppConfig;
import org.ddth.panda.core.ApplicationRepository;
import org.ddth.panda.portal.PandaPortalApplication;
import org.ddth.panda.portal.PandaPortalConfigConstants;
import org.ddth.panda.portal.module.ModuleDescriptor;
import org.ddth.panda.utils.DateTimeUtils;
import org.ddth.panda.utils.StringUtils;

import com.greenstorm.gsc.TxbbConstants;
import com.greenstorm.gsc.TxbbPermissionConstants;
import com.greenstorm.gsc.bo.Box;
import com.greenstorm.gsc.bo.TxbbManager;

/**
 * Helper class for TXBB
 * 
 * @author Thanh Ba Nguyen <btnguyen2k@gmail.com>
 * @copyright See README.TXT file for more details
 * @version 3.0
 */
public class TxbbHelper {

    private static SeoUnicode seoUnicode;

    private static TXCodeParser txbbCodeParser;

    private static PandaPortalApplication getApp() {
        return (PandaPortalApplication)ApplicationRepository.getCurrentApp();
    }

    /**
     * Gets all viewable boxes.
     * 
     * @return Collection<Box>
     * @throws Exception
     */
    public static Collection<Box> getViewableBoxes() throws Exception {
        PandaPortalApplication app = getApp();
        TxbbManager fm = app.getBundleManager().getService(TxbbManager.class);

        Box[] boxTree = fm.getBoxTree();
        Collection<Box> viewableBoxes = new ArrayList<Box>();
        for ( Box box : boxTree ) {
            checkViewableBox(app, viewableBoxes, box);
        }
        return viewableBoxes;
    }

    private static void checkViewableBox(PandaPortalApplication app,
            Collection<Box> viewableBoxes, Box box) throws Exception {
        if ( app.hasPermission(TxbbPermissionConstants.PERMISSION_VIEW_BOX, box) ) {
            viewableBoxes.add(box);
            for ( Box child : box.getChildren() ) {
                checkViewableBox(app, viewableBoxes, child);
            }
        }
    }

    /**
     * Gets the TxbbCodeParser instance.
     * 
     * @return TxbbCodeParser
     * @throws Exception
     */
    public static TXCodeParser getTxbbCodeParser() throws Exception {
        if ( txbbCodeParser == null ) {
            PandaPortalApplication app = getApp();
            ModuleDescriptor moduleTxbb =
                    app.getModule(TxbbConstants.MODULE_NAME);
            String clazz =
                    moduleTxbb.getModuleProperty(TxbbConstants.PROPERTY_TXCODE_PARSER);
            if ( StringUtils.isEmpty(clazz) ) {
                txbbCodeParser = new XhtmlTXCodeParser();
            } else {
                Class<?> cl = Class.forName(clazz);
                txbbCodeParser = (TXCodeParser)cl.newInstance();
            }
        }
        return txbbCodeParser;
    }

    /**
     * Gets the SeoUnicode instance.
     * 
     * @return SeoUnicode
     * @throws Exception
     */
    public static SeoUnicode getSeoUnicode() throws Exception {
        if ( seoUnicode == null ) {
            PandaPortalApplication app = getApp();
            ModuleDescriptor moduleTxbb =
                    app.getModule(TxbbConstants.MODULE_NAME);
            String clazz =
                    moduleTxbb.getModuleProperty(TxbbConstants.PROPERTY_SEO_UNICODE_CLASS);
            if ( StringUtils.isEmpty(clazz) ) {
                seoUnicode = new DefaultSeoUnicode();
            } else {
                Class<?> cl = Class.forName(clazz);
                seoUnicode = (SeoUnicode)cl.newInstance();
            }
        }
        return seoUnicode;
    }

    private final static String DEFAULT_LONG_DATETIME_FORMAT =
            "yyyy-MM-dd kk:mm:ss";

    /**
     * Converts UNIX timestamp to String.
     * 
     * @param timestamp int UNIX timestamp (resolution to second)
     * @return String
     * @throws Exception
     */
    public static String timestamp2String(int timestamp) throws Exception {
        PandaPortalApplication app = getApp();
        AppConfig config =
                app.getAppConfig(PandaPortalConfigConstants.CONFIG_LONG_DATETIME_FORMAT);
        String format = config != null
                ? config.getConfigValue() : DEFAULT_LONG_DATETIME_FORMAT;
        return DateTimeUtils.timestampToStr((long)timestamp * 1000, format);
    }
}
