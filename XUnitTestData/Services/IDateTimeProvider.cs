using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Services
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
