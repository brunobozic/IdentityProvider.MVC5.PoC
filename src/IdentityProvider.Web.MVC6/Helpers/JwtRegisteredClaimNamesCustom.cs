namespace IdentityProvider.Web.MVC6.Helpers;

public struct JwtRegisteredClaimNamesCustom
{
    public const string APPLICATION_TYPE = "APPLICATION_TYPE";
    public const string FULL_NAME = "FULL_NAME";
    public const string USERNAME = "USERNAME";
    public const string USER_ID = "USER_ID";
    public const string EMAIL = "EMAIL";
    public const string ORGANIZATION_UNITS = "ORGANIZATION_UNITS";
    public const string ROLES = "ROLES";
}

public class JwtClaimNameConstants
{
    public const string ID_CLAIM_NAME = "Id";
    public const string ROLE_ID_CLAIM_NAME = "RoleId";
    public const string APPLICATION_ACTIONS_CLAIM_NAME = "ApplicationActions";
    public const string Organization_UNIT = "OrganizationUnit";
    public const string GUEST_CLAIM_NAME = "GuestRoleName";

    public const string SUPERUSER_CLAIM_NAME = "SuperUserRoleName";
    public static string APPLICATION_TYPE_CLAIM_NAME = "ApplicationType";
    public static string FULL_NAME_CLAIM_NAME = "FullName";
    public static string ROLE_CLAIM_NAME = "Role";
    public static string USERNAME_CLAIM_NAME = "Username";
    public static string FIRSTNAME_CLAIM_NAME = "FirstName";
    public static string LASTNAME_CLAIM_NAME = "LastName";
    public static string OIB_CLAIM_NAME = "OIB";
    public static string USER_ID_CLAIM_NAME = "UserId";
}