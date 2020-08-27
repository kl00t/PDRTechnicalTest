using System.Linq;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.BookingService.Requests;
using PDR.PatientBooking.Service.Validation;

namespace PDR.PatientBooking.Service.BookingService.Validation
{
    public class CancelBookingRequestValidator : ICancelBookingRequestValidator
    {
        private readonly PatientBookingContext _context;

        public CancelBookingRequestValidator(PatientBookingContext context)
        {
            _context = context;
        }

        public PdrValidationResult ValidateRequest(CancelBookingRequest request)
        {
            var result = new PdrValidationResult(true);

            if (BookingFoundInDb(request, ref result))
                return result;

            return result;
        }

        private bool BookingFoundInDb(CancelBookingRequest request, ref PdrValidationResult result)
        {
            if (!_context.Order.Any(x => x.Id.Equals(request.Id)))
            {
                result.PassedValidation = false;
                result.Errors.Add("A booking was not found with that booking id");
                return true;
            }

            return false;
        }
    }
}