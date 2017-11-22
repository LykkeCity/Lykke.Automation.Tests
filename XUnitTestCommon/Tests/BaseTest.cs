using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XUnitTestCommon.Reports;

namespace XUnitTestCommon.Tests
{
    [TestFixture]
    public class BaseTest
    {        
        public IList<string> schemesError;

        public static Dictionary<string, List<Response>> responses;
        private readonly List<Action> _cleanupActions = new List<Action>();
        private readonly List<Action> _oneTimeCleanupActions = new List<Action>();

        protected virtual void Initialize() { }

        #region response info
        public static void ValidateScheme(bool valid, IList<string> errors)
        {
            if (!valid)
            {
                errors.ToList().ForEach(e => Console.WriteLine(e));
                Assert.Fail("Scheme not valid");
            }
        }

        public static void AreEqualByJson(object expected, object actual)
        {

            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.That(expectedJson, Is.EqualTo(actualJson), "Objects are not equals");
        }
        #endregion

        #region before after
        [SetUp]
        public void SetUp()
        {
            AllureReport.GetInstance().CaseStarted(TestContext.CurrentContext.Test.FullName,
                TestContext.CurrentContext.Test.Name, "");
            responses = new Dictionary<string, List<Response>>();
            schemesError = new List<string>();
            TestContext.WriteLine("SetUp");
        }

        [SetUp]
        public void TestInitialize()
        {
            Console.WriteLine("=============================== Test initialize ===============================");
            _cleanupActions.Clear();

            try
            {
                Initialize();
            }
            catch (Exception)
            {
                CallCleanupActions();
                throw;
            }

            Console.WriteLine("=============================== Test method ===============================");
            Console.WriteLine();
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("TearDown");

            AllureReport.GetInstance().CaseFinished(TestContext.CurrentContext.Test.FullName,
                TestContext.CurrentContext.Result.Outcome.Status, null);
        }

        [TearDown]
        public void TestCleanup()
        {
            Console.WriteLine("=============================== Test Cleanup ===============================");
            Console.WriteLine();

            CallCleanupActions();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var path = TestContext.CurrentContext.WorkDirectory.Remove(TestContext.CurrentContext.WorkDirectory.IndexOf("bin")) + "TestReportHelpers/";
            AllureReport.GetInstance().RunStarted(path);
        }

        [OneTimeTearDown]
        public void OneTimeCleanup()
        {
            Console.WriteLine("=============================== Final Cleanup ===============================");
            Console.WriteLine();

            CallCleanupActions(true);
        }

        #endregion

        #region Allure Helpers

        public static void Step(string name, Action action)
        {
            Exception ex = null;
            try
            {
                AllureReport.GetInstance().StepStarted(TestContext.CurrentContext.Test.FullName,
                    name);
                Logger.WriteLine($"Step: {name}");
                action();
            }
            catch (Exception e)
            {
                ex = e;
            }
            finally
            {
                AllureReport.GetInstance().StepFinished(TestContext.CurrentContext.Test.FullName,
             TestContext.CurrentContext.Result.Outcome.Status, ex);
            }
        }
        #endregion

        #region CleanUp Helpers

        private void CallCleanupActions(bool oneTime = false)
        {
            List<Action> cleanupActions;
            if (oneTime)
                cleanupActions = _oneTimeCleanupActions;
            else
                cleanupActions = _cleanupActions;

            cleanupActions.Reverse();
            var exceptions = new List<Exception>();

            foreach (var action in cleanupActions)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    Console.WriteLine("Cleanup action failed: " + ex);
                }
            }

            if (exceptions.Count == 0)
                return;

            throw new AggregateException("Multiple exceptions occurred in Cleanup. See test log for more details", exceptions);
        }

        public void AddCleanupAction(Action cleanupAction)
        {
            _cleanupActions.Add(cleanupAction);
        }

        public void AddOneTimeCleanupAction(Action cleanupAction)
        {
            _oneTimeCleanupActions.Add(cleanupAction);
        }
        #endregion

    }
}
