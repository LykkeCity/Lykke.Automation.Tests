using BlueApiData.DTOs;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using System.Threading.Tasks;
using BlueApiData;
using XUnitTestCommon.Reports;

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
            Logger.WriteLine("This endpoint returns cached tweets more ofthen than not, due to Twitter api limitations. Cached tweets don't always match the search query, or are old enough to have the 140 character limitation. By default the test doesn't check the tweet 'fulltext' for the search query, but if you want to enable that, toggle AutomatedFunctionalTests.BlueApi.TwitterAggessiveCheck to True in settings service");

            this.PrepareTwitterData();
            string url = ApiPaths.TWITTER_BASE_PATH;
            int pageSize = Helpers.Random.Next(1, 101); // from 1 to 100 records on a page
            var body = new TwitterSearchDTO()
            {                
                SearchQuery = Constants.TWITTER_SEARCH_QUERY,
                MaxResult = 100,
                AccountEmail = this.AccountEmail,
                PageNumber = 1,
                PageSize = pageSize
            };
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), RestSharp.Method.POST);

            // checks if the response is 200 OK
            Assert.True(response.Status == System.Net.HttpStatusCode.OK);

            List<TweetDTO> tweets = JsonUtils.DeserializeJson<List<TweetDTO>>(response.ResponseJson);

            // checks if the right amout of tweets is returned on a single page
            Assert.True(tweets.Count <= pageSize);

            if (TwitterAggressiveCheck)
            {
                tweets.ForEach(tweet =>
                {
                    //check if the tweet matches search query
                    Assert.True(tweet.full_text.ToLower().Contains(Constants.TWITTER_SEARCH_QUERY));
                });
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Twitter")]
        [Category("TwitterPost")]
        public async Task GetTweetsExtendedSearch()
        {
            Logger.WriteLine("This endpoint returns cached tweets more ofthen than not, due to Twitter api limitations. Cached tweets don't always match the search query, or are old enough to have the 140 character limitation. By default the test doesn't check the tweet 'fulltext' for the search query, but if you want to enable that, toggle AutomatedFunctionalTests.BlueApi.TwitterAggessiveCheck to True in settings service");

            this.PrepareTwitterData();
            string url = ApiPaths.TWITTER_BASE_PATH;
            int pageSize = Helpers.Random.Next(1, 101); // from 1 to 100 records on a page
            var body = new TwitterSearchDTO()
            {
                IsExtendedSearch = true, 
                SearchQuery = Constants.TWITTER_SEARCH_QUERY,
                MaxResult = 100,
                UntilDate = DateTime.Now.AddMinutes(-1), // API updates every 1 min, so new tweets should not be younger than (now - 1 min)
                AccountEmail = this.AccountEmail,
                PageNumber = 1,
                PageSize = pageSize
            };
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), RestSharp.Method.POST);

            // checks if the response is 200 OK
            Assert.True(response.Status == System.Net.HttpStatusCode.OK);

            List<TweetDTO> tweets = JsonUtils.DeserializeJson<List<TweetDTO>>(response.ResponseJson);

            // checks if the right amout of tweets is returned on a single page
            Assert.True(tweets.Count <= pageSize);

            tweets.ForEach(tweet =>
            {
                if (TwitterAggressiveCheck)
                {
                    //checks if all tweets contain the search query in their title ( may fail for older tweets due to chars limitations )
                    Assert.True(tweet.full_text.ToLower().Contains(Constants.TWITTER_SEARCH_QUERY));
                }

                //check if the tweets are published before the until date
                Assert.True(tweet.created_at <= body.UntilDate);
            });

        }
    }
}
