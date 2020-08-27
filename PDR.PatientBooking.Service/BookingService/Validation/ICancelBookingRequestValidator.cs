using PDR.PatientBooking.Service.BookingService.Requests;
using PDR.PatientBooking.Service.Validation;

namespace PDR.PatientBooking.Service.BookingService.Validation
{
    public interface ICancelBookingRequestValidator
    {
        PdrValidationResult ValidateRequest(CancelBookingRequest request);
    }
}