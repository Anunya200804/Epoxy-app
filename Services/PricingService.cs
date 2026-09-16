using System;

namespace EpoxyFloorManager.Services;

public class PricingCalculationResult
{
    public bool IsDailyRate { get; set; }
    public decimal BasePrice { get; set; }
    public decimal ThicknessSurcharge { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal BaseRateUsed { get; set; } // Daily rate or Per sqm rate
}

public class PricingService
{
    // Constant rates for easy configuration in the future
    public const decimal DAILY_RATE_EPOXY_COATING = 6000m;
    public const decimal DAILY_RATE_EPOXY_LEVELING = 6000m;
    public const decimal DAILY_RATE_PU = 8000m;

    public const decimal SQM_RATE_WEEKDAY_EPOXY_COATING = 80m;
    public const decimal SQM_RATE_WEEKDAY_EPOXY_LEVELING = 80m;
    public const decimal SQM_RATE_WEEKDAY_PU = 100m;

    public const decimal SQM_RATE_HOLIDAY_EPOXY_COATING = 160m;
    public const decimal SQM_RATE_HOLIDAY_EPOXY_LEVELING = 160m;
    public const decimal SQM_RATE_HOLIDAY_PU = 200m;

    public const decimal PU_THICKNESS_SURCHARGE_RATE_PER_2MM = 20m;

    public PricingCalculationResult CalculatePrice(
        string floorType, 
        decimal areaSqm, 
        int workingDays, 
        bool isHolidaySurcharge, 
        decimal additionalThicknessMm)
    {
        // Validation & normalization
        if (areaSqm < 0) areaSqm = 0;
        if (workingDays < 0) workingDays = 0;
        if (additionalThicknessMm < 0) additionalThicknessMm = 0;

        var result = new PricingCalculationResult();

        // 1. Determine base price
        if (areaSqm < 300)
        {
            result.IsDailyRate = true;
            decimal dailyRate = floorType switch
            {
                "Epoxy Coating" => DAILY_RATE_EPOXY_COATING,
                "Epoxy Self-Leveling" => DAILY_RATE_EPOXY_LEVELING,
                "PU" => DAILY_RATE_PU,
                _ => 0m
            };
            result.BaseRateUsed = dailyRate;
            result.BasePrice = dailyRate * workingDays;
        }
        else
        {
            result.IsDailyRate = false;
            decimal sqmRate = 0m;
            if (isHolidaySurcharge)
            {
                sqmRate = floorType switch
                {
                    "Epoxy Coating" => SQM_RATE_HOLIDAY_EPOXY_COATING,
                    "Epoxy Self-Leveling" => SQM_RATE_HOLIDAY_EPOXY_LEVELING,
                    "PU" => SQM_RATE_HOLIDAY_PU,
                    _ => 0m
                };
            }
            else
            {
                sqmRate = floorType switch
                {
                    "Epoxy Coating" => SQM_RATE_WEEKDAY_EPOXY_COATING,
                    "Epoxy Self-Leveling" => SQM_RATE_WEEKDAY_EPOXY_LEVELING,
                    "PU" => SQM_RATE_WEEKDAY_PU,
                    _ => 0m
                };
            }
            result.BaseRateUsed = sqmRate;
            result.BasePrice = sqmRate * areaSqm;
        }

        // 2. Thickness surcharge (only for PU)
        if (floorType == "PU" && additionalThicknessMm > 0)
        {
            // CEIL(thickness / 2) * 20 * area
            decimal intervals = Math.Ceiling(additionalThicknessMm / 2m);
            result.ThicknessSurcharge = intervals * PU_THICKNESS_SURCHARGE_RATE_PER_2MM * areaSqm;
        }
        else
        {
            result.ThicknessSurcharge = 0m;
        }

        // 3. Totals
        result.TotalPrice = result.BasePrice + result.ThicknessSurcharge;
        if (areaSqm > 0)
        {
            result.PricePerUnit = result.TotalPrice / areaSqm;
        }
        else
        {
            result.PricePerUnit = 0m;
        }

        return result;
    }
}
