using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Strainer.Infrastructure
{
    public interface IPickerService
    {
        Task<int?> GetInt(int startValue, int minValue, int maxValue);

        Task<T> Show<T>(IEnumerable<T> pickFrom);

        Task<DateTime?> GetDate(DateTime startDate,DateType dateType);
    }

    public enum DateType
    {
        Default,
        MonthYear,
        Time,
        Time24
    }
}

