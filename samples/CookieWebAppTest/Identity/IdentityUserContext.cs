using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace TBMMNet.Web.Core.Identity
{
    public class IdentityUserContext
    {


        ApplicationUser User { get; set; }
        List<IdentityUserRole<string>> UserRoles { get; set; }
        List<IdentityUserClaim<string>> UserClaims { get; set; }

        internal Task<TUser> CreateNewUser<TUser, TKey>(TUser user) where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
        {
            User = user as ApplicationUser;

            //TODO (SA): SAVE new user with Ldap or SQL
            return Task.FromResult(user);
        }

        internal Task<List<string>> GetUserRoles<TUser, TKey>(string userId) where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
        {
            //TODO (SA): User  rollerini yetki veritabanından okuyacak şekilde guncelle
            UserRoles = new List<IdentityUserRole<string>>() {
                new IdentityUserRole<string>() { RoleId = "Visitor", UserId = userId },
                new IdentityUserRole<string>() { RoleId = "RestrictedEditor", UserId = userId }
            };

            var query = from userRole in UserRoles
                        where userRole.UserId.Equals(userId)
                        select userRole.RoleId;
            return Task.FromResult(query.ToList());
        }



        internal Task<List<IdentityUserClaim<TKey>>> GetClaimsAsync<TKey>(TKey id) where TKey : IEquatable<TKey>
        {
            //TODO (SA): User  claims leri yetki veritabanından okuyacak şekilde guncelle
            UserClaims = new List<IdentityUserClaim<string>>() {
                //new IdentityUserClaim<string>() { Id= 1, UserId = User.Id ,ClaimType = ClaimTypes.Role, ClaimValue = "Visitor"},
                //new IdentityUserClaim<string>() { Id = 2, UserId = User.Id ,ClaimType = ClaimTypes.Role, ClaimValue="RestrictedEditor"},
                new IdentityUserClaim<string>() { Id = 1, UserId = User.Id ,ClaimType = "AllowedPageRoute", ClaimValue="Manage/Index"},
                new IdentityUserClaim<string>() { Id = 2, UserId = User.Id ,ClaimType = "AllowedPageRoute", ClaimValue="Manage/ChangePassword"}
            };

            return Task.FromResult(UserClaims as List<IdentityUserClaim<TKey>>);
        }

        internal Task<List<IdentityUserClaim<TKey>>> GetClaimsAsync<TKey>(TKey id, string claimValue, string claimType) where TKey : IEquatable<TKey>
        {
            var userClaims = GetClaimsAsync(id);

            List<IdentityUserClaim<TKey>> query = null;

            if (userClaims.Result != null)
            {
                query = userClaims.Result.Where(uc =>
                        uc.ClaimValue == claimValue
                        && uc.ClaimType == claimType).ToList();
            }
            return Task.FromResult(query);
        }


        internal Task<IdentityUserLogin<TKey>> GetUserLoginAsync<TKey>(TKey userId, string loginProvider, string providerKey) where TKey : IEquatable<TKey>
        {
            var UserLogins = new List<IdentityUserLogin<string>>() {
                new IdentityUserLogin<string>() {
                    LoginProvider = "Forms",
                    ProviderDisplayName = "Forms Authentication",
                    ProviderKey = "11212", UserId= "sdfsdf"},
            };

            //UserLogins.SingleOrDefaultAsync(l => l.UserId.Equals(userId) && l.LoginProvider == loginProvider && l.ProviderKey == providerKey, cancellationToken);

            return Task.FromResult(UserLogins[0] as IdentityUserLogin<TKey>);
        }

        internal Task<IdentityUserLogin<TKey>> GetUserLoginAsync<TKey>(string loginProvider, string providerKey) where TKey : IEquatable<TKey>
        {
            var UserLogins = new List<IdentityUserLogin<string>>() {
                new IdentityUserLogin<string>() {
                    LoginProvider = "Forms",
                    ProviderDisplayName = "Forms Authentication",
                    ProviderKey = "11212", UserId= "sdfsdf"},
            };

            //E.g. UserLogins.FirstOrDefaultAsync(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey, cancellationToken);
            return Task.FromResult(UserLogins[0] as IdentityUserLogin<TKey>);
        }

        internal Task<List<IdentityUserLogin<TKey>>> GetUserLoginsAsync<TKey>(TKey userId) where TKey : IEquatable<TKey>
        {
            var UserLogins = new List<IdentityUserLogin<string>>() {
                new IdentityUserLogin<string>() {
                    LoginProvider = "Forms",
                    ProviderDisplayName = "Forms Authentication",
                    ProviderKey = "11212", UserId= "sdfsdf"},
            };

            //E.g. await UserLogins.Where(l => l.UserId.Equals(userId)).Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToListAsync(cancellationToken

            return Task.FromResult(UserLogins as List<IdentityUserLogin<TKey>>);
        }

        internal Task<TUser> FindByIdAsync<TUser, TKey>(string userId) where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
        {
            User = new ApplicationUser { UserName = "sebnema@outlook.com", Email = "sebnema@outlook.com", PhoneNumber = "123123123" };

            //TODO (SA): Ldap yada SQL den kullanıcı sorgulanır.
            return Task.FromResult(User as TUser);
            //Users.FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken);
        }
        internal Task<TUser> FindByIdAsync<TUser, TKey>(TKey userId) where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
        {
            //TODO (SA): Ldap yada SQL den kullanıcı sorgulanır.
            User = new ApplicationUser { UserName = "sebnema@outlook.com", Email = "sebnema@outlook.com", PhoneNumber = "123123123" };

            return Task.FromResult(User as TUser);

            //Users.FirstOrDefaultAsync(u => u.Id.Equals(userLogin.UserId), cancellationToken);
        }

        internal Task<TUser> FindByNameAsync<TUser, TKey>(string normalizedUserName) where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
        {
            //TODO (SA): Ldap yada SQL den kullanıcı sorgulanır.
            return Task.FromResult(User as TUser);
            //Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        internal Task<TUser> FindByEmailAsync<TUser, TKey>(string normalizedEmail) where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
        {
            //TODO (SA): Ldap yada SQL den kullanıcı sorgulanır.

            return Task.FromResult(User as TUser);
            //Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
        }

        internal Task<IList<TUser>> GetUsersForClaimAsync<TUser>(string value, string type)
        {
            //var query = from userclaims in UserClaims
            //            join user in Users on userclaims.UserId equals user.Id
            //            where userclaims.ClaimValue == claim.Value
            //            && userclaims.ClaimType == claim.Type
            //            select user;
            //return await query.ToListAsync(cancellationToken)

            throw new NotImplementedException();
        }

        internal Task<IList<TUser>> GetUsersInRoleAsync<TUser>(string roleName)
        {

            //var role = await Roles.Where(x => x.Name.Equals(roleName)).FirstOrDefaultAsync(cancellationToken);

            //if (role != null)
            //{
            //    var query = from userrole in UserRoles
            //                join user in Users on userrole.UserId equals user.Id
            //                where userrole.RoleId.Equals(role.Id)
            //                select user;

            //    return await query.ToListAsync(cancellationToken);
            //}
            //return new List<TUser>();

            throw new NotImplementedException();
        }

        internal Task<bool> IsInRoleAsync<TUser, TKey>(TUser user, string roleName) where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
        {
            //if (string.IsNullOrWhiteSpace(roleName))
            //{
            //    throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, nameof(roleName));
            //}
            //var role = await Roles.SingleOrDefaultAsync(r => r.Name.ToUpper() == roleName.ToUpper(), cancellationToken);
            //if (role != null)
            //{
            //    var userId = user.Id;
            //    var roleId = role.Id;
            //    return await UserRoles.AnyAsync(ur => ur.RoleId.Equals(roleId) && ur.UserId.Equals(userId));
            //}
            //return false;

            throw new NotImplementedException();
        }      


        //internal void SaveUser<TUser, TKey>(TUser user) where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
        //{
        //    //TODO (sa): Kullanıcının ldap ve SQL e yetkilerinin kaydedilmesi
        //    User = user as ApplicationUser;
        //    return;
        //}

        //internal IList<IdentityUserRole<TKey>> GetUserRoles<TKey>() where TKey : IEquatable<TKey>
        //{
        //    throw new NotImplementedException();
        //}

        //internal void SaveUserClaims<TKey>(IList<IdentityUserClaim<TKey>> userClaims) where TKey : IEquatable<TKey>
        //{
        //    throw new NotImplementedException();
        //}

        //internal bool AddUserLogin<TKey>(IdentityUserLogin<TKey> userLogin) where TKey : IEquatable<TKey>
        //{
        //    throw new NotImplementedException();
        //}

        //internal IList<IdentityUserLogin<TKey>> GetUserLogins<TKey>() where TKey : IEquatable<TKey>
        //{
        //    throw new NotImplementedException();
        //}

        //internal IList<IdentityUserClaim<TKey>> GetUserClaims<TKey>() where TKey : IEquatable<TKey>
        //{
        //    throw new NotImplementedException();
        //}

        //internal Task<IdentityUserRole<TKey>> RemoveFromUserRoleAsync<TUser, TKey>(TUser user, string roleName) where TKey : IEquatable<TKey>
        //{
        //    //var roleEntity = await Roles.SingleOrDefaultAsync(r => r.Name.ToUpper() == roleName.ToUpper(), cancellationToken);
        //    //if (roleEntity != null)
        //    //{
        //    //    var userRole = await UserRoles.FirstOrDefaultAsync(r => roleEntity.Id.Equals(r.RoleId) && r.UserId.Equals(user.Id), cancellationToken);
        //    //if (userRole != null)
        //    //{
        //    //    UserRoles.Remove(userRole);
        //    //}
        //    //}

        //    throw new NotImplementedException();
        //}

        //internal Task<IdentityUserRole<TKey>> AddToUserRoleAsync<TUser, TKey>(TUser user, string roleName)
        //    where TUser : IdentityUser<TKey>
        //    where TKey : IEquatable<TKey>
        //{
        //    //var roleEntity = await Roles.SingleOrDefaultAsync(r => r.Name.ToUpper() == roleName.ToUpper(), cancellationToken);
        //    //if (roleEntity == null)
        //    //{
        //    //    throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.RoleNotFound, roleName));
        //    //}
        //    //var ur = new IdentityUserRole<TKey> { UserId = user.Id, RoleId = roleEntity.Id };
        //    //UserRoles.Add(ur);

        //    throw new NotImplementedException();
        //}

    }
}
