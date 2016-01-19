// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNet.Http.Features.Internal
{
    public interface IServiceProvidersFeature
    {
        IServiceProvider ApplicationServices { get; set; }
        IServiceProvider RequestServices { get; set; }
    }
}