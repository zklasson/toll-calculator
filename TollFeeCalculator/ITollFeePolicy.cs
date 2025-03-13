namespace TollFeeCalculator;

public interface ITollFeePolicy
{
    decimal DailyMaxFee { get; }
    TollFeeRate[] TollFeeRates { get; }
    bool IsTollFreeDate(DateOnly date);
    bool IsTollFreeVehicleType(VehicleType vehicleType);
}