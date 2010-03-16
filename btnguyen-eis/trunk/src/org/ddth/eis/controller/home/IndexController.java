package org.ddth.eis.controller.home;

import java.util.Map;

import org.ddth.eis.EisConstants;
import org.ddth.eis.controller.BaseController;
import org.ddth.eis.controller.IRequireAuthenticationController;
import org.ddth.panda.UrlCreator;

public class IndexController extends BaseController implements IRequireAuthenticationController {

    private final static String VIEW_NAME = EisConstants.MODULE_HOME + "."
            + EisConstants.ACTION_HOME_INDEX;

    private final static String MODEL_URL_UPDATE_PROFILE = "urlProfile";

    private final static String MODEL_URL_UPDATE_SKILL_INVENTORY = "urlSkillInventory";

    /**
     * {@inheritDoc}
     */
    @Override
    protected String getViewName() {
        return VIEW_NAME;
    }

    /**
     * {@inheritDoc}
     */
    @Override
    protected void modelPageContent(Map<String, Object> modelPage) {
        UrlCreator urlCreator = getUrlCreator();
        String urlUpdateProfile = urlCreator.createUri(EisConstants.MODULE_STAFF,
                                                       EisConstants.ACTION_STAFF_PROFILE);
        modelPage.put(MODEL_URL_UPDATE_PROFILE, urlUpdateProfile);
        String urlUpdateSkillInventory = urlCreator
                .createUri(EisConstants.MODULE_STAFF, EisConstants.ACTION_STAFF_SKILL_INVENTORY);
        modelPage.put(MODEL_URL_UPDATE_SKILL_INVENTORY, urlUpdateSkillInventory);
    }
}
