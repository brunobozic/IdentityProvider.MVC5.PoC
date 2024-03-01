using Microsoft.AspNetCore.Http;

using System;
using System.Linq;

namespace IdentityProvider.Web.MVC6.Helpers;

public class LoggedInUserFacade : ILoggedInUserFacade
{

    private string _jwtFirstname;
    private string _jwtFullname;
    private string _jwtLastname;
    private string _jwtOIB;
    private long _jwtUserId;

    private string _jwtUsername;


    public LoggedInUserFacade(IHttpContextAccessor contextAccessor)
    {
        ReadJWTData(contextAccessor);
    }





    protected string Username
    {
        get => _jwtUsername;
        private set { }
    }

    protected string Fullname
    {
        get => _jwtFullname;
        private set { }
    }

    protected string Firstname
    {
        get => _jwtFirstname;
        private set { }
    }

    protected string Lastname
    {
        get => _jwtLastname;
        private set { }
    }

    protected string OIB
    {
        get => _jwtOIB;
        private set { }
    }

    protected long UserId
    {
        get => _jwtUserId;
        private set { }
    }

    private void ReadJWTData(IHttpContextAccessor contextAccessor)
    {
        if (contextAccessor.HttpContext.User != null)
        {
            var applicationType = contextAccessor.HttpContext.User.Claims.FirstOrDefault(a =>
                a.Type == JwtClaimNameConstants.APPLICATION_TYPE_CLAIM_NAME);
            var fullname =
                contextAccessor.HttpContext.User.Claims.FirstOrDefault(a =>
                    a.Type == JwtClaimNameConstants.FULL_NAME_CLAIM_NAME);
            var role = contextAccessor.HttpContext.User.Claims.FirstOrDefault(a =>
                a.Type == JwtClaimNameConstants.ROLE_CLAIM_NAME);
            var username =
                contextAccessor.HttpContext.User.Claims.FirstOrDefault(a =>
                    a.Type == JwtClaimNameConstants.USERNAME_CLAIM_NAME);
            var firstname =
                contextAccessor.HttpContext.User.Claims.FirstOrDefault(a =>
                    a.Type == JwtClaimNameConstants.FIRSTNAME_CLAIM_NAME);
            var lastname =
                contextAccessor.HttpContext.User.Claims.FirstOrDefault(a =>
                    a.Type == JwtClaimNameConstants.LASTNAME_CLAIM_NAME);
            var oib = contextAccessor.HttpContext.User.Claims.FirstOrDefault(a =>
                a.Type == JwtClaimNameConstants.OIB_CLAIM_NAME);
            var userId =
                contextAccessor.HttpContext.User.Claims.FirstOrDefault(a =>
                    a.Type == JwtClaimNameConstants.USER_ID_CLAIM_NAME);



            if (userId != null) _jwtUserId = Convert.ToInt64(userId.Value);

            if (username != null) _jwtUsername = username.Value;

            if (fullname != null)
                _jwtFullname = fullname.Value;
            else
                _jwtFullname = "";

            if (firstname != null)
                _jwtFirstname = firstname.Value;
            else
                _jwtFirstname = "";

            if (lastname != null)
                _jwtLastname = lastname.Value;
            else
                _jwtLastname = "";

            if (oib != null)
                _jwtOIB = oib.Value;
            else
                _jwtOIB = "";
        }
    }
}