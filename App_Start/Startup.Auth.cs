using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using ClubPortalMS.Models;
using Microsoft.Owin.Security;

namespace ClubPortalMS
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType
            (CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Account/Login"),
                SlidingExpiration = true
            });
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "203002635752-j5g1db4gsujkovumfk6rri9jrppbacpv.apps.googleusercontent.com",
                ClientSecret = "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",
                CallbackPath = new PathString("/GoogleLoginCallback")
            });
        }
    }
}