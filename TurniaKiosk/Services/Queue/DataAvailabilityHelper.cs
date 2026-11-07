using System.Collections.Generic;

namespace Services.Queue
{
    public static class DataAvailabilityHelper
    {
        public static DataAvailability Evaluate<T>(ICollection<T>? data)
        {
            return data != null && data.Count > 0
                ? DataAvailability.Available
                : DataAvailability.Empty;
        }
    }
}
