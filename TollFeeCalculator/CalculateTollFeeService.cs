namespace TollFeeCalculator;

public interface ICalculateTollFeeService
{
    decimal CalculateTotalTollFeeForDate(VehicleType vehicleType, DateOnly date, TimeOnly[] passageTimes);
}

public class CalculateTollFeeService(ITollFeePolicy policy) : ICalculateTollFeeService
{
    private readonly ITollFeePolicy _policy = policy ?? throw new ArgumentNullException(nameof(policy));

    public decimal CalculateTotalTollFeeForDate(VehicleType vehicleType, DateOnly date, TimeOnly[] passageTimes)
    {
        if (passageTimes == null)
            throw new ArgumentNullException(nameof(passageTimes));

        if (passageTimes.Length == 0 || _policy.IsTollFreeDate(date) || _policy.IsTollFreeVehicleType(vehicleType))
            return 0;

        decimal totalFee = 0;
        foreach (var passageTime in passageTimes.GroupBy(time => time.Hour))
        {
            if (passageTime.Count() == 1)
            {
                totalFee += GetTollFeeForPass(passageTime.First());
                continue;
            }

            totalFee += passageTime.Max(GetTollFeeForPass);

            if (totalFee > _policy.DailyMaxFee)
                return _policy.DailyMaxFee;
        }

        return totalFee;
    }

    private decimal GetTollFeeForPass(TimeOnly passageTime)
    {
        var rate = _policy.TollFeeRates.FirstOrDefault(rate => rate.IsWithinTimeInterval(passageTime));

        if (rate?.TollFee == null)
            throw new MissingTollFeeRateException(passageTime);

        return rate.TollFee.Fee;
    }
}