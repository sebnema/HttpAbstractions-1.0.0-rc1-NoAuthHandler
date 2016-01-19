using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TBMMNet.Web.Core.Identity
{
    public class IdentityRoleContext
    {
        /// <summary>
        /// A navigation property for the roles the store contains.
        /// </summary>
        internal IList<TRole> GetRoles<TRole>() where TRole : IdentityRole<string>, new()
        {
            var roles = new List<IdentityRole<string>>()
            {
                new IdentityRole<string>() { Id = "1" , Name = "SystemAdministrator", NormalizedName = "SystemAdministrator", ConcurrencyStamp= "1"},
                new IdentityRole<string>() { Id = "2", Name = "Administrator", NormalizedName = "Administrator", ConcurrencyStamp= "2"},
                new IdentityRole<string>() { Id = "3", Name = "Editor", NormalizedName = "Editor", ConcurrencyStamp= "3"},
                new IdentityRole<string>() { Id = "4", Name = "RestrictedEditor", NormalizedName = "RestrictedEditor", ConcurrencyStamp= "4"},
                new IdentityRole<string>() { Id = "5", Name = "Visitor", NormalizedName = "Visitor", ConcurrencyStamp= "5"},
                new IdentityRole<string>() { Id = "6", Name = "RestrictedVisitor", NormalizedName = "RestrictedVisitor", ConcurrencyStamp= "6"}
            };

            return roles as IList<TRole>;
        }

        internal Task Add<TRole>(TRole role)
        {
            throw new NotImplementedException();
        }

        internal Task<TRole> Save<TRole>(TRole role)
        {
            throw new NotImplementedException();
        }

        internal Task Remove<TRole>(TRole role)
        {
            throw new NotImplementedException();
        }

        internal Task<TRole> FindByIdAsync<TRole, TKey>(TKey roleId) where TRole : IdentityRole<TKey> where TKey : IEquatable<TKey>
        {
            //throw new NotImplementedException();
            return Task.FromResult(GetRoles<IdentityRole<string>>().FirstOrDefault(u => u.Id.Equals(roleId)) as TRole);
            //Roles.FirstOrDefaultAsync(u => u.Id.Equals(roleId), cancellationToken)
        }

        internal Task<TRole> FindByNameAsync<TRole, TKey>(string normalizedName) where TRole : IdentityRole<TKey> where TKey : IEquatable<TKey>
        {
            return Task.FromResult(GetRoles<IdentityRole<string>>().FirstOrDefault(r => r.NormalizedName == normalizedName) as TRole);
        }

        internal Task<IList<IdentityRoleClaim<TKey>>> GetClaimsAsync<TKey>(TKey id) where TKey : IEquatable<TKey>
        {
            //RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToListAsync(cancellationToken);
            throw new NotImplementedException();
        }

        internal Task<bool> AddIdentityRoleClaimAysnc<TKey>(TKey id, string type, string value) where TKey : IEquatable<TKey>
        {
            //RoleClaims.Add(new IdentityRoleClaim<TKey> { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
            throw new NotImplementedException();
        }

        internal Task<bool> AddIdentityRoleClaimAysnc<TKey>(IdentityRoleClaim<TKey> identityRoleClaim) where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        internal Task RemoveClaims<TKey>(TKey id, string value, string type) where TKey : IEquatable<TKey>
        {
            //var claims = RoleClaims.Where(rc => rc.RoleId.Equals(role.Id) && rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type).ToListAsync(cancellationToken);
            //foreach (var c in claims)
            //{
            //    RoleClaims.Remove(c);
            //}

            throw new NotImplementedException();
        }
    }
}
