using BlueApiData.DTOs;
using BlueApiData.Fixtures;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using System.Threading.Tasks;

namespace AFTests.BlueApi
{
    [Category("FullRegression")]
    [Category("BlueApiService")]
    public partial class BlueApiTests
    {
        [Test]
        [Category("Smoke")]
        [Category("Twitter")]
        [Category("TwitterPost")]
        public async Task GetTweets()
        {
            string url = ApiPaths.TWITTER_BASE_PATH;
            int pageSize = Helpers.Random.Next(1, 101); // from 1 to 100 records on a page
            var body = new TwitterSearchDTO()
            {
                SearchQuery = _fixture.TwitterSearchQuery,
                MaxResult = 100,
                UntilDate = _fixture.TwitterSearchUntilDate, // hardcoded
                AccountEmail = _fixture.AccountEmail,
                PageNumber = 1,
                PageSize = pageSize
            };
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), RestSharp.Method.POST);

            // checks if the response is 200 OK
            Assert.True(response.Status == System.Net.HttpStatusCode.OK);

            List<TweetDTO> tweets = JsonUtils.DeserializeJson<List<TweetDTO>>(response.ResponseJson);

            // checks if the right amout of tweets is returned on a single page
            Assert.True(tweets.Count == pageSize);

            bool containsSearchQuery = true;
            tweets.ForEach(tweet =>
            {
                if (!tweet.Title.ToLower().Contains(_fixture.TwitterSearchQuery))
                    containsSearchQuery = false;
            });

            //checks if all tweets contain the search query in their title
            Assert.True(containsSearchQuery);
        }
    }
}
