package org.ddth.eis.bo.daf;

import org.ddth.daf.DataProvider;
import org.springframework.security.authentication.AuthenticationProvider;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;

public class DafAuthenticationProvider implements AuthenticationProvider {

    private DataProvider dafDataProvider;

    public DataProvider getDafDataProvider() {
        return dafDataProvider;
    }

    public void setDafDataProvider(DataProvider dafDataProvider) {
        this.dafDataProvider = dafDataProvider;
    }

    /**
     * {@inheritDoc}
     */
    public Authentication authenticate(Authentication authentication)
            throws AuthenticationException {

        // TODO Auto-generated method stub
        return null;
    }

    /**
     * {@inheritDoc}
     */
    public boolean supports(Class<? extends Object> authentication) {
        return UsernamePasswordAuthenticationToken.class.isAssignableFrom(authentication);
    }
}
