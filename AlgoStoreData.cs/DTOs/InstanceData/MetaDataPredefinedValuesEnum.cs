namespace AlgoStoreData.DTOs.InstanceData
{
    public enum CandleTimeInterval
    {
        Unspecified = 0,
        Second = 1,
        Minute = 60,
        FiveMinutes = 300,
        FifteenMinutes = 900,
        ThirtyMinutes = 1800,
        Hour = 3600,
        FourHours = 14400,
        SixHours = 21600,
        TwelveHours = 42300,
        Day = 86400,
        Week = 604800,
        Month = 2419200
    }

    public enum CandleOperationMode
    {
        OPEN = 0,
        CLOSE = 1,
        LOW = 2,
        HIGH = 3
    }
}
