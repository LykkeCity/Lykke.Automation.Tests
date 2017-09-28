using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Config
{
    public static class HttpConfigurationExtentions
    {
        public static IConfigurationBuilder AddHttpJsonConfig(
            this IConfigurationBuilder builder, string sourceUrl, string accessToken, string rootItemName, string testItemName)
        {
            string url = sourceUrl + accessToken;

            return builder.Add(new HttpConfigurationSource(url, rootItemName, testItemName));
        }
    }
}
