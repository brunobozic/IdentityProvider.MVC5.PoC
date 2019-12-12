using System;
using System.Collections.Generic;
using System.Linq;
using IdentityProvider.Models.Domain.Account;
using Module.Repository.EF.Repositories;
using MyApplicationUserGetQuery = IdentityProvider.Repository.EF.Queries.UserProfile.MyApplicationUserGetQuery;

namespace IdentityProvider.Repository.EF.Repositories.UserProfile.Extensions
{
    public static class ApplicationUserRepositoryExtensions
    {
        //public static ApplicationUser UserDetailsGetById(this IRepositoryAsync<ApplicationUser> repository,
        //    int userProfileId)
        //{
        //    if (userProfileId == 0) throw new ArgumentNullException(nameof(userProfileId));

        //    var foundAUser =
        //        repository.Queryable().FirstOrDefault(i => i.UserProfile.Equals(userProfileId));

        //    return foundAUser;
        //}


        //public static bool UserProfileChangeProfilePictureForUserName(
        //    this IRepositoryAsync<ApplicationUser> repository
        //    , string userName
        //    , byte[] newPicture
        //    , bool removeCurrentPicture
        //)
        //{
        //    if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
        //    if (!removeCurrentPicture && newPicture.Length == 0) throw new ArgumentNullException(nameof(newPicture));

        //    var foundAUser =
        //        repository.Queryable()
        //            .FirstOrDefault(i => i.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()));

        //    if (foundAUser == null) return false;

        //    if (!removeCurrentPicture && newPicture.Length > 0)
        //        foundAUser.UserImage = newPicture;

        //    if (removeCurrentPicture)
        //        foundAUser.UserImage = null;

        //    repository.Update(foundAUser);

        //    return true;
        //}

        //public static bool UserProfileChangeEmailForUserName(
        //    this IRepositoryAsync<ApplicationUser> repository
        //    , string userName
        //    , string newEmailAdress
        //)
        //{
        //    if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
        //    if (string.IsNullOrEmpty(newEmailAdress)) throw new ArgumentNullException(nameof(newEmailAdress));

        //    var foundAUser =
        //        repository.Queryable()
        //            .FirstOrDefault(i => i.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()));

        //    if (foundAUser == null) return false;
        //    foundAUser.Email = newEmailAdress.ToLower().Trim();
        //    repository.Update(foundAUser);

        //    return true;
        //}

        //public static bool UserProfileChangeMobilePhoneForUserName(
        //    this IRepositoryAsync<ApplicationUser> repository
        //    , string userName
        //    , string newMobilePhoneNumber
        //    , bool justRemoveIt
        //)
        //{
        //    if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));

        //    if (string.IsNullOrEmpty(newMobilePhoneNumber) && !justRemoveIt)
        //        throw new ArgumentNullException(nameof(newMobilePhoneNumber));

        //    var foundAUser =
        //        repository.Queryable()
        //            .FirstOrDefault(i => i.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()));

        //    if (foundAUser == null) return false;

        //    foundAUser.MobilePhone = justRemoveIt ? string.Empty : newMobilePhoneNumber.ToUpper().Trim();

        //    repository.Update(foundAUser);

        //    return true;
        //}

        //public static bool UserProfileChangeHomePhoneForUserName(
        //    this IRepositoryAsync<ApplicationUser> repository
        //    , string userName
        //    , string newHomePhoneNumber
        //    , bool justRemoveIt
        //)
        //{
        //    if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
        //    if (string.IsNullOrEmpty(newHomePhoneNumber) && !justRemoveIt)
        //        throw new ArgumentNullException(nameof(newHomePhoneNumber));
        //    var foundAUser =
        //        repository.Queryable()
        //            .FirstOrDefault(i => i.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()));

        //    if (foundAUser == null) return false;
        //    foundAUser.HomePhone = justRemoveIt ? string.Empty : newHomePhoneNumber.Trim();
        //    repository.Update(foundAUser);

        //    return true;
        //}

        //public static List<ApplicationUser> UserProfileGetAllActiveAsync(
        //    this IRepositoryAsync<ApplicationUser> repository
        //    , out int totalCount
        //    , int page = 0
        //    , int pageSize = 10
        //    , bool active = true
        //)
        //{
        //    var result = repository.Query(new MyApplicationUserGetQuery
        //        {
        //            Active = active
        //        })
        //        .OrderBy(x => x.OrderBy(y => y.CreatedDate)
        //        .ThenBy(z => z.LastLoginDate))
        //        .SelectPage(page, pageSize, out totalCount)
        //        .ToList();

        //    return result;
        //}

        //public static List<ApplicationUser> UserProfileGetAllActiveByEmailAsync(
        //    this IRepositoryAsync<ApplicationUser> repository
        //    , string emailLike
        //    , out int totalCount
        //    , int page = 0
        //    , int pageSize = 10
        //    , bool active = true)
        //{
        //    var result = repository.Query(new MyApplicationUserGetQuery
        //            {
        //                Active = active
        //            }
        //        .WhereEmailLike(emailLike))
        //        .OrderBy(x => x.OrderBy(y => y.CreatedDate)
        //        .ThenBy(z => z.LastLoginDate))
        //        .SelectPage(page, pageSize, out totalCount)
        //        .ToList();

        //    return result;
        //}

        //public static List<ApplicationUser> UserProfileGetAllActiveByUserNameAsync(
        //    this IRepositoryAsync<ApplicationUser> repository
        //    , string userNameLike
        //    , out int totalCount
        //    , int page = 0
        //    , int pageSize = 10
        //    , bool active = true)
        //{
        //    var result = repository.Query(new MyApplicationUserGetQuery
        //            {
        //                Active = active
        //            }
        //        .WhereUserNameLike(userNameLike))
        //        .OrderBy(x => x.OrderBy(y => y.CreatedDate)
        //        .ThenBy(z => z.LastLoginDate))
        //        .SelectPage(page, pageSize, out totalCount)
        //        .ToList();

        //    return result;
        //}

        //public static List<ApplicationUser> UserProfileGetAllActiveByJoinDateAsync(
        //    this IRepositoryAsync<ApplicationUser> repository
        //    , DateTime startDate
        //    , DateTime endDate
        //    , out int totalCount
        //    , int page = 0, int pageSize = 10
        //    , bool active = true)
        //{
        //    var result = repository.Query(new MyApplicationUserGetQuery
        //            {
        //                Active = active
        //            }
        //        .WhereUserHasJoinedBetween(startDate, endDate))
        //        .OrderBy(x => x.OrderBy(y => y.CreatedDate)
        //        .ThenBy(z => z.LastLoginDate))
        //        .SelectPage(page, pageSize, out totalCount)
        //        .ToList();

        //    return result;
        //}

        //public static List<ApplicationUser> UserProfileGetAllActiveByLastLoginAsync(
        //    this IRepositoryAsync<ApplicationUser> repository
        //    , DateTime startDate
        //    , DateTime endDate
        //    , out int totalCount
        //    , int page = 0, int pageSize = 10
        //    , bool active = true)
        //{
        //    var result = repository.Query(new MyApplicationUserGetQuery
        //            {
        //                Active = active
        //            }
        //        .WhereLastLoginDateBetween(startDate, endDate))
        //        .OrderBy(x => x.OrderBy(y => y.CreatedDate)
        //        .ThenBy(z => z.LastLoginDate))
        //        .SelectPage(page, pageSize, out totalCount)
        //        .ToList();

        //    return result;
        //}
    }
}