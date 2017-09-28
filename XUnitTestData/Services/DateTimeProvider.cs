using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Services
{
    [UsedImplicitly]
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
