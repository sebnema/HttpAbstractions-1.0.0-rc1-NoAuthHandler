// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;

namespace Microsoft.AspNet.Http.Features
{
    public interface IHttpRequestLifetimeFeature
    {
        CancellationToken RequestAborted { get; set; }
        void Abort();
    }
}