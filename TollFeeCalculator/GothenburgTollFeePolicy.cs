namespace TollFeeCalculator;

public static class GothenburgTollFeePolicy
{
    public const string Currency = "SEK";
    public const decimal DailyMaxFee = 60;
    public const decimal LowFee = 8;
    public const decimal MediumFee = 13;
    public const decimal HighFee = 18;
    public const decimal NoFee = 0;

    public static readonly TollFeeRate[] TollFeeRates =
    [
        new(new TimeOnly(6, 0), new TimeOnly(6, 29), new TollFee(LowFee, Currency)), // 06:00 - 06:29
        new(new TimeOnly(6, 30), new TimeOnly(6, 59), new TollFee(MediumFee, Currency)), // 06:30 - 06:59
        new(new TimeOnly(7, 0), new TimeOnly(7, 59), new TollFee(HighFee, Currency)), // 07:00 - 07:59
        new(new TimeOnly(8, 0), new TimeOnly(8, 29), new TollFee(MediumFee, Currency)), // 08:00 - 08:29
        new(new TimeOnly(8, 30), new TimeOnly(14, 59), new TollFee(LowFee, Currency)), // 08:30 - 14:59
        new(new TimeOnly(15, 0), new TimeOnly(15, 29), new TollFee(MediumFee, Currency)), // 15:00 - 15:29
        new(new TimeOnly(15, 30), new TimeOnly(16, 59), new TollFee(HighFee, Currency)), // 15:30 - 16:59
        new(new TimeOnly(17, 0), new TimeOnly(17, 59), new TollFee(MediumFee, Currency)), // 17:00 - 17:59
        new(new TimeOnly(18, 0), new TimeOnly(18, 29), new TollFee(LowFee, Currency)), // 18:00 - 18:29
        new(new TimeOnly(18, 30), new TimeOnly(5, 59), new TollFee(NoFee, Currency)), // 18:30 - 05:59
    ];

    public static readonly VehicleType[] TollableVehicleTypes =
    [
        VehicleType.Car,
    ];
}