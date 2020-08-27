using PDR.PatientBooking.Service.BookingService.Requests;

namespace PDR.PatientBooking.Service.BookingService
{
    public interface IBookingService
    {
        void CancelBooking(CancelBookingRequest cancelBooking);
    }
}