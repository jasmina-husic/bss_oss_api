using Microsoft.EntityFrameworkCore;
namespace ProductCatalog.Domain.Entities;

[Owned]
public class PricePlan
{
    public string Currency { get; set; } = "USD";
    public decimal SetupFee { get; set; }
    public decimal MonthlyFee { get; set; }
    public string BillingCycle { get; set; } = "calendarMonthly";
}
