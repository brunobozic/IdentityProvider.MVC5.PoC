namespace IdentityProvider.Repository.EFCore.Queries.UserProfile
{
    //public class MyApplicationUserGetQuery : QueryObject<ApplicationUser>
    //{
    //    public bool Active { get; set; }
    //    public DateTime? JoinedFrom { get; set; }
    //    public DateTime? JoinedTo { get; set; }
    //    public DateTime? LastLoginDateFrom { get; set; }
    //    public DateTime? LastLoginDateTo { get; set; }
    //    public string UserNameLike { get; set; }
    //    public string EmailLike { get; set; }

    //    public int UserIdIs { get; set; }

    //    public override Expression<Func<ApplicationUser, bool>> Query()
    //    {
    //        return x =>
    //            x.UserName != null;
    //    }

    //    public MyApplicationUserGetQuery WhereUserProfileIdEquals(int userIdIs)
    //    {
    //        And(
    //            x =>
    //                x.UserProfile.Equals(userIdIs));

    //        return this;
    //    }

    //    public MyApplicationUserGetQuery WhereUserHasJoinedBetween(DateTime? startDate, DateTime? endDate)
    //    {
    //        And(
    //            x =>
    //                x.CreatedDate.GetValueOrDefault(DateTime.Now.AddDays(1)) >= startDate &&
    //                x.CreatedDate <= endDate.GetValueOrDefault(DateTime.Now));

    //        return this;
    //    }

    //    public MyApplicationUserGetQuery WhereLastLoginDateBetween(DateTime? startDate, DateTime? endDate)
    //    {
    //        And(
    //            x =>
    //                x.LastLoginDate.GetValueOrDefault(DateTime.Now.AddDays(1)) >= startDate &&
    //                x.CreatedDate <= endDate.GetValueOrDefault(DateTime.Now));

    //        return this;
    //    }

    //    public MyApplicationUserGetQuery WhereUserNameLike(string userNameNonCaseSensitive)
    //    {
    //        And(x => x.UserName.Trim().ToLower().Contains(userNameNonCaseSensitive.Trim().ToLower()));

    //        return this;
    //    }

    //    public MyApplicationUserGetQuery WhereEmailLike(string emailNonCaseSensitive)
    //    {
    //        And(x => x.UserName.Trim().ToLower().Contains(emailNonCaseSensitive.Trim().ToLower()));

    //        return this;
    //    }
    //}
}