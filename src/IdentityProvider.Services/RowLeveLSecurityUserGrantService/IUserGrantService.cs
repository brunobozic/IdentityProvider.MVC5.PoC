namespace IdentityProvider.Services.RowLeveLSecurityUserGrantService
{
    public interface IUserGrantService
    {
        GrantedPriviligesResponse OrgUnitGrantedPriviligesByUser(string userId);
        GrantedPriviligesResponse OrgUnitGrantedPriviligesByEmployee( int employeeId );
    }
}