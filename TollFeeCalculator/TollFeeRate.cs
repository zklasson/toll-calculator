namespace TollFeeCalculator;

public record TollFeeRate(TimeOnly StartTime, TimeOnly EndTime, TollFee TollFee)
{
    public bool IsWithinTimeInterval(TimeOnly passageTime)
    {
        if (StartTime <= EndTime)
            return passageTime >= StartTime && passageTime <= EndTime;

        return passageTime >= StartTime || passageTime <= EndTime;
    }
}