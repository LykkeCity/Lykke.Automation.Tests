namespace AFTests.HftTests
{
    using NUnit.Framework;
    using System.Net;

    class IsAliveTests
    {
        public class GetIsAlive : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetIsAliveTest()
            {
                var response = hft.IsAlive.GetIsAlive().Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.ResponseObject.Name, Is.EqualTo("Lykke.Service.HFT"));
                Assert.That(response.ResponseObject.Version, Is.Not.Null.Or.Empty, "Version is null or empty");
                Assert.That(response.ResponseObject.Env, Is.Not.Null.Or.Empty, "Environment is null or empty");
            }
        }
    }
}
