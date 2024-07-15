using MudBlazor;

namespace Nutrifica.Spa.Extensions;

public static class MudBlazorFilterDefinitionExtensions
{
    public static string ToSieveFilters<T>(this IFilterDefinition<T> filterDefinition)
    {
        if (filterDefinition.FieldType.IsString)
            switch (filterDefinition.Operator)
            {
                case FilterOperator.String.Contains:
                    return $"{filterDefinition.Column?.PropertyName}@={filterDefinition.Value}";
                case FilterOperator.String.NotContains:
                    return $"{filterDefinition.Column?.PropertyName}!@={filterDefinition.Value}";
                case FilterOperator.String.StartsWith:
                    return $"{filterDefinition.Column?.PropertyName}_={filterDefinition.Value}";
                case FilterOperator.String.EndsWith:
                    return $"{filterDefinition.Column?.PropertyName}_-={filterDefinition.Value}";
                case FilterOperator.String.Equal:
                    return $"{filterDefinition.Column?.PropertyName}=={filterDefinition.Value}";
                case FilterOperator.String.NotEqual:
                    return $"{filterDefinition.Column?.PropertyName}!={filterDefinition.Value}";
                case FilterOperator.String.Empty:
                    return $"{filterDefinition.Column?.PropertyName}==";
                case FilterOperator.String.NotEmpty:
                    return $"{filterDefinition.Column?.PropertyName}!=";
                default:
                    return string.Empty;
            }

        if (filterDefinition.FieldType.IsBoolean)
            return $"{filterDefinition.Column?.PropertyName}=={filterDefinition.Value}";

        if (filterDefinition.FieldType.IsEnum)
            switch (filterDefinition.Operator)
            {
                case FilterOperator.Enum.Is:
                    return $"{filterDefinition.Column?.PropertyName}=={filterDefinition.Value}";
                case FilterOperator.Enum.IsNot:
                    return $"{filterDefinition.Column?.PropertyName}!={filterDefinition.Value}";
                default:
                    return string.Empty;
            }

        if (filterDefinition.FieldType.IsNumber)
            switch (filterDefinition.Operator)
            {
                case FilterOperator.Number.NotEqual:
                    return $"{filterDefinition.Column?.PropertyName}!={filterDefinition.Value}";
                case FilterOperator.Number.Equal:
                    return $"{filterDefinition.Column?.PropertyName}=={filterDefinition.Value}";
                case FilterOperator.Number.GreaterThan:
                    return $"{filterDefinition.Column?.PropertyName}>{filterDefinition.Value}";
                case FilterOperator.Number.LessThan:
                    return $"{filterDefinition.Column?.PropertyName}<{filterDefinition.Value}";
                case FilterOperator.Number.GreaterThanOrEqual:
                    return $"{filterDefinition.Column?.PropertyName}>={filterDefinition.Value}";
                case FilterOperator.Number.LessThanOrEqual:
                    return $"{filterDefinition.Column?.PropertyName}<={filterDefinition.Value}";
                case FilterOperator.Number.Empty:
                    return $"{filterDefinition.Column?.PropertyName}==";
                case FilterOperator.Number.NotEmpty:
                    return $"{filterDefinition.Column?.PropertyName}!=";
                default:
                    return string.Empty;
            }

        if (filterDefinition.FieldType.IsDateTime)
            switch (filterDefinition.Operator)
            {
                case FilterOperator.DateTime.After:
                    return $"{filterDefinition.Column?.PropertyName}>{filterDefinition.Value}";
                case FilterOperator.DateTime.OnOrAfter:
                    return $"{filterDefinition.Column?.PropertyName}>={filterDefinition.Value}";
                case FilterOperator.DateTime.Before:
                    return $"{filterDefinition.Column?.PropertyName}<{filterDefinition.Value}";
                case FilterOperator.DateTime.OnOrBefore:
                    return $"{filterDefinition.Column?.PropertyName}>={filterDefinition.Value}";
                case FilterOperator.DateTime.Is:
                    return $"{filterDefinition.Column?.PropertyName}=={filterDefinition.Value}";
                case FilterOperator.DateTime.IsNot:
                    return $"{filterDefinition.Column?.PropertyName}!={filterDefinition.Value}";
                case FilterOperator.DateTime.Empty:
                    return $"{filterDefinition.Column?.PropertyName}==";
                case FilterOperator.DateTime.NotEmpty:
                    return $"{filterDefinition.Column?.PropertyName}!=";
                default:
                    return string.Empty;
            }

        return string.Empty;
    }
}