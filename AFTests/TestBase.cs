using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AFTests
{
    [TestFixture]
    public abstract class TestBase
    {
        private readonly List<Action> _cleanupActions = new List<Action>();

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

        protected virtual void Initialize() { }

        [TearDown]
        public void TestCleanup()
        {
            Console.WriteLine("=============================== Test Cleanup ===============================");
            Console.WriteLine();

            CallCleanupActions();
        }

        private void CallCleanupActions()
        {
            _cleanupActions.Reverse();
            var exceptions = new List<Exception>();

            foreach (var action in _cleanupActions)
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
    }
}