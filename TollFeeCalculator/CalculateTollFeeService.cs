﻿using TollFeeCalculator.Exceptions;
using TollFeeCalculator.Models;
using TollFeeCalculator.Policies;

namespace TollFeeCalculator;

public interface ICalculateTollFeeService
{
    TollFeeCalculationResult CalculateTotalTollFee(VehicleType vehicleType, DateOnly date, TimeOnly[] passageTimes);
}

public class CalculateTollFeeService(ITollFeePolicy policy) : ICalculateTollFeeService
{
    private readonly ITollFeePolicy _policy = policy ?? throw new ArgumentNullException(nameof(policy));

    public TollFeeCalculationResult CalculateTotalTollFee(VehicleType vehicleType, DateOnly date, TimeOnly[] passageTimes)
    {
        if (passageTimes == null)
            throw new ArgumentNullException(nameof(passageTimes));

        var currency = _policy.Currency;
        var dailyMaxFee = _policy.DailyMaxFee;

        if (passageTimes.Length == 0 || _policy.IsTollFreeDate(date) || _policy.IsTollFreeVehicleType(vehicleType))
            return new TollFeeCalculationResult(date, 0, currency);

        decimal totalFee = 0;
        foreach (var passageTime in passageTimes.GroupBy(time => time.Hour))
        {
            if (passageTime.Count() == 1)
            {
                totalFee += GetTollFeeForPass(passageTime.First());
                continue;
            }

            totalFee += passageTime.Max(GetTollFeeForPass);
        }

        return totalFee > dailyMaxFee ? new TollFeeCalculationResult(date, dailyMaxFee, currency) : new TollFeeCalculationResult(date, totalFee, currency);
    }

    private decimal GetTollFeeForPass(TimeOnly passageTime)
    {
        var rate = _policy.TollFeeRates.FirstOrDefault(rate => rate.IsWithinTimeInterval(passageTime));

        if (rate == null)
            throw new MissingTollFeeRateException(passageTime);

        return rate.Fee;
    }
}