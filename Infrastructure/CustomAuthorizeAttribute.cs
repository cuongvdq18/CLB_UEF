using ClubPortalMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CustomAuthorizationFilter.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var userId = Convert.ToString(httpContext.Session["UserId"]);
            if (!string.IsNullOrEmpty(userId))
                using (var context = new ApplicationDbContext())
                {
                  
                    var userRole = (from u in context.DBUserRoles
                                    join d in context.DBUser on u.UserID equals d.ID
                                    join r in context.DBRoles on u.RoleID equals r.ID
                                    where u.UserID.ToString() == userId
                                    select new
                                    {
                                        r.Name
                                    });
                    foreach (var role in allowedroles)
                    {
                        foreach (var userrole in userRole) { 
                        if (role == userrole.Name) return true;
                        }
                    }
                }


            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Error" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}