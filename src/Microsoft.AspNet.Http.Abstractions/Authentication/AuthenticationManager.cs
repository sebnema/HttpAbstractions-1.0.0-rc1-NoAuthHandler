// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Features.Authentication;

namespace Microsoft.AspNet.Http.Authentication
{
    public abstract class AuthenticationManager
    {
        /// <summary>
        /// Constant used to represent the automatic scheme
        /// </summary>
        public const string AutomaticScheme = "Automatic";

        public abstract IEnumerable<AuthenticationDescription> GetAuthenticationSchemes();

        public abstract Task AuthenticateAsync(AuthenticateContext context);

        public virtual async Task<ClaimsPrincipal> AuthenticateAsync(string authenticationScheme)
        {
            if (authenticationScheme == null)
            {
                throw new ArgumentNullException(nameof(authenticationScheme));
            }

            var context = new AuthenticateContext(authenticationScheme);
            await AuthenticateAsync(context);
            return context.Principal;
        }

        public virtual Task ChallengeAsync()
        {
            return ChallengeAsync(properties: null);
        }

        public virtual Task ChallengeAsync(AuthenticationProperties properties)
        {
            return ChallengeAsync(authenticationScheme: AutomaticScheme, properties: properties);
        }

        public virtual Task ChallengeAsync(string authenticationScheme)
        {
            if (string.IsNullOrEmpty(authenticationScheme))
            {
                throw new ArgumentException(nameof(authenticationScheme));
            }

            return ChallengeAsync(authenticationScheme: authenticationScheme, properties: null);
        }

        // Leave it up to authentication handler to do the right thing for the challenge
        public virtual Task ChallengeAsync(string authenticationScheme, AuthenticationProperties properties)
        {
            if (string.IsNullOrEmpty(authenticationScheme))
            {
                throw new ArgumentException(nameof(authenticationScheme));
            }

            return ChallengeAsync(authenticationScheme, properties, ChallengeBehavior.Automatic);
        }

        public virtual Task SignInAsync(string authenticationScheme, ClaimsPrincipal principal)
        {
            if (string.IsNullOrEmpty(authenticationScheme))
            {
                throw new ArgumentException(nameof(authenticationScheme));
            }

            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return SignInAsync(authenticationScheme, principal, properties: null);
        }

        public virtual Task ForbidAsync(string authenticationScheme)
        {
            if (authenticationScheme == null)
            {
                throw new ArgumentNullException(nameof(authenticationScheme));
            }

            return ForbidAsync(authenticationScheme, properties: null);
        }

        // Deny access (typically a 403)
        public virtual Task ForbidAsync(string authenticationScheme, AuthenticationProperties properties)
        {
            if (authenticationScheme == null)
            {
                throw new ArgumentNullException(nameof(authenticationScheme));
            }

            return ChallengeAsync(authenticationScheme, properties, ChallengeBehavior.Forbidden);
        }

        public abstract Task ChallengeAsync(string authenticationScheme, AuthenticationProperties properties, ChallengeBehavior behavior);

        public abstract Task SignInAsync(string authenticationScheme, ClaimsPrincipal principal, AuthenticationProperties properties);

        public virtual Task SignOutAsync(string authenticationScheme)
        {
            if (authenticationScheme == null)
            {
                throw new ArgumentNullException(nameof(authenticationScheme));
            }

            return SignOutAsync(authenticationScheme, properties: null);
        }

        public abstract Task SignOutAsync(string authenticationScheme, AuthenticationProperties properties);
    }
}
