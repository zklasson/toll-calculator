namespace TollFeeCalculator;

public interface ITollFeePolicy
{
    string Currency { get; }
    decimal DailyMaxFee { get; }
    TollFeeRate[] TollFeeRates { get; }
    bool IsTollFreeDate(DateOnly date);
    bool IsTollFreeVehicleType(VehicleType vehicleType);
}