using System;

namespace PDR.PatientBooking.Service.Validation
{
    public static class DateTimeExtensions
    {
        public static Boolean IsBetween(this DateTime dt, DateTime startDate, DateTime endDate, Boolean compareTime = false)
        {
            return compareTime ?
                dt >= startDate && dt <= endDate :
                dt.Date >= startDate.Date && dt.Date <= endDate.Date;
        }
    }
}