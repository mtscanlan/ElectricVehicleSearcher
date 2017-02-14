using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Clients.EdmundsApi.Contracts
{
    public enum FuelTypeEnum
    {
        [Display(Name = "gas")]
        Gas = 0,
        [Display(Name = "diesel")]
        Diesel,
        [Display(Name = "electric")]
        Electric,
        [Display(Name = "hybrid")]
        Hybrid,
        [Display(Name = "flex-fuel-ffv")]
        FlexFuelFFV,
        [Display(Name = "natural-gas-cng")]
        NaturalGasCNG
    }

    internal static class FuelTypeEnumDisplayExtension
    {
        public static string GetDisplayName(this FuelTypeEnum fuelTypeEnum) => 
            fuelTypeEnum
                .GetType()
                .GetMember(fuelTypeEnum.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .Name;
    }
}
