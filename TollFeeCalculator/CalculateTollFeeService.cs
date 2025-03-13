using PublicHoliday;
using static TollFeeCalculator.GothenburgTollFeePolicy;

namespace TollFeeCalculator;

public interface ICalculateTollFeeService
{
    decimal CalculateTotalTollFeeForDate(VehicleType vehicleType, DateOnly date, TimeOnly[] passageTimes);
}

public class CalculateTollFeeService : ICalculateTollFeeService
{
    private static readonly SwedenPublicHoliday HolidayService = new();

    public decimal CalculateTotalTollFeeForDate(VehicleType vehicleType, DateOnly date, TimeOnly[] passageTimes)
    {
        if (passageTimes == null)
            throw new ArgumentNullException(nameof(passageTimes));

        if (passageTimes.Length == 0 || IsTollFreeDate(date) || IsTollFreeVehicleType(vehicleType))
            return NoFee;

        decimal totalFee = 0;
        foreach (var passageTime in passageTimes.GroupBy(time => time.Hour))
        {
            if (passageTime.Count() == 1)
            {
                totalFee += GetTollFeeForPass(passageTime.First());
                continue;
            }

            totalFee += passageTime.Max(GetTollFeeForPass);

            if (totalFee > DailyMaxFee)
                return DailyMaxFee;
        }

        return totalFee;
    }

    private static decimal GetTollFeeForPass(TimeOnly passageTime)
    {
        var rate = TollFeeRates.FirstOrDefault(rate => rate.IsWithinInterval(passageTime));

        if (rate?.TollFee == null)
            throw new MissingTollFeeRateException(passageTime);

        return rate.TollFee.Fee;
    }

    private static bool IsTollFreeDate(DateOnly date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        return HolidayService.IsPublicHoliday(new DateTime(date.Year, date.Month, date.Day));
    }

    private static bool IsTollFreeVehicleType(VehicleType vehicleType) => !TollableVehicleTypes.Contains(vehicleType);
}