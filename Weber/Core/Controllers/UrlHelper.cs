using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Controllers
{
    /// <summary>
    /// Contains methods for cleaning and normalizing the query string
    /// </summary>
    public class UrlHelper
    {
        public static string ClearAndNormalize(string url)
        {
            url = url.Replace("//", "/");
            url = url.Replace("\\", "/");
            return url;
        }

        public static string RemoveQueryParams(string url)
        {
            if (url.Contains("?"))
            {
                var d = url.IndexOf('?');
                var clearUri = url.Substring(0, d);
                return clearUri;
            }

            return url;
        }
    }
}
