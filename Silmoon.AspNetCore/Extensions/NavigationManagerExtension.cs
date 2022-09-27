using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

namespace Silmoon.AspNetCore.Extensions
{
    public static class NavigationManagerExtension
    {
        public static Dictionary<string, StringValues> GetQueryStringValues(this NavigationManager navigationManager)
        {
            return QueryHelpers.ParseQuery(new Uri(navigationManager.Uri).Query);
        }
    }
}
