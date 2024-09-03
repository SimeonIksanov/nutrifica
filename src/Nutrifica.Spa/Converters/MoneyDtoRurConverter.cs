using System.Globalization;

using MudBlazor;

using Nutrifica.Api.Contracts.Orders;

namespace Nutrifica.Spa.Converters;

public class MoneyDtoRurConverter : Converter<MoneyDto>
{
    private readonly CurrencyDto _currency = new CurrencyDto { Code = "RUR" };
    public MoneyDtoRurConverter()
    {
        SetFunc = money => money?.Amount.ToString(CultureInfo.InvariantCulture)?? "0";
        GetFunc = value =>
        {
            if (decimal.TryParse(value, out decimal amount))
            {
                return new MoneyDto { Amount = amount, Currency = _currency };
            }
            return new MoneyDto { Amount = 0, Currency = _currency };
        };
    }
}