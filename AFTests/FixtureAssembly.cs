using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

[SetUpFixture]
public class FixtureAssembly
{
    [OneTimeTearDown]
    public void AfterTests()
    {
        var context = TestContext.CurrentContext;
        File.WriteAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "passed.txt"), context.Result.PassCount.ToString());
        File.WriteAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "failed.txt"), context.Result.FailCount.ToString());
        File.WriteAllText(Path.Combine(TestContext.CurrentContext.WorkDirectory, "skipped.txt"), context.Result.SkipCount.ToString());
    }
}
