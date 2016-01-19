// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.AspNet.Http.Internal;

namespace Microsoft.AspNet.Http.Features.Internal
{
    public class HttpRequestFeature : IHttpRequestFeature
    {
        public HttpRequestFeature()
        {
            Headers = new HeaderDictionary();
            Body = Stream.Null;
            Protocol = string.Empty;
            Scheme = string.Empty;
            Method = string.Empty;
            PathBase = string.Empty;
            Path = string.Empty;
            QueryString = string.Empty;
        }

        public string Protocol { get; set; }
        public string Scheme { get; set; }
        public string Method { get; set; }
        public string PathBase { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public IHeaderDictionary Headers { get; set; }
        public Stream Body { get; set; }
    }
}