// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNet.Http.Extensions
{
    /// <summary>
    /// A helper class for constructing encoded Uris for use in headers and other Uris.
    /// </summary>
    public static class UriHelper
    {
        /// <summary>
        /// Combines the given URI components into a string that is properly encoded for use in HTTP headers.
        /// </summary>
        /// <param name="pathBase"></param>
        /// <param name="path"></param>
        /// <param name="query"></param>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public static string Encode(
            PathString pathBase = new PathString(),
            PathString path = new PathString(),
            QueryString query = new QueryString(),
            FragmentString fragment = new FragmentString())
        {
            string combinePath = (pathBase.HasValue || path.HasValue) ? (pathBase + path).ToString() : "/";
            return combinePath + query + fragment;
        }

        /// <summary>
        /// Combines the given URI components into a string that is properly encoded for use in HTTP headers.
        /// Note that unicode in the HostString will be encoded as punycode.
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="host"></param>
        /// <param name="pathBase"></param>
        /// <param name="path"></param>
        /// <param name="query"></param>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public static string Encode(
            string scheme,
            HostString host,
            PathString pathBase = new PathString(),
            PathString path = new PathString(),
            QueryString query = new QueryString(),
            FragmentString fragment = new FragmentString())
        {
            string combinePath = (pathBase.HasValue || path.HasValue) ? (pathBase + path).ToString() : "/";
            return scheme + "://" + host + combinePath + query + fragment;
        }

        /// <summary>
        /// Generates a string from the given absolute or relative Uri that is appropriately encoded for use in
        /// HTTP headers. Note that a unicode host name will be encoded as punycode.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string Encode(Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                return Encode(
                    scheme: uri.Scheme,
                    host: HostString.FromUriComponent(uri),
                    pathBase: PathString.FromUriComponent(uri),
                    query: QueryString.FromUriComponent(uri),
                    fragment: FragmentString.FromUriComponent(uri));
            }
            else
            {
                return uri.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped);
            }
        }

        /// <summary>
        /// Returns the combined components of the request URL in a fully escaped form suitable for use in HTTP headers
        /// and other HTTP operations.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetEncodedUrl(this HttpRequest request)
        {
            return Encode(request.Scheme, request.Host, request.PathBase, request.Path, request.QueryString);
        }

        /// <summary>
        /// Returns the combined components of the request URL in a fully un-escaped form (except for the QueryString)
        /// suitable only for display. This format should not be used in HTTP headers or other HTTP operations.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetDisplayUrl(this HttpRequest request)
        {
            return request.Scheme + "://" + request.Host.Value + request.PathBase.Value + request.Path.Value + request.QueryString.Value;
        }
    }
}