using System;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.BookingService.Requests;
using PDR.PatientBooking.Service.Validation;

namespace PDR.PatientBooking.Service.BookingService.Validation
{
    public class AddBookingRequestValidator : IAddBookingRequestValidator
    {
        private readonly PatientBookingContext _context;

        public AddBookingRequestValidator(PatientBookingContext context)
        {
            _context = context;
        }

        public PdrValidationResult ValidateRequest(AddBookingRequest request)
        {
            var result = new PdrValidationResult(true);

            if (IsBookingInPast(request, ref result))
                return result;

            return result;
        }

        private bool IsBookingInPast(AddBookingRequest request, ref PdrValidationResult result)
        {
            if (request.StartTime < DateTime.UtcNow || request.EndTime < DateTime.UtcNow)
            {
                result.PassedValidation = false;
                result.Errors.Add("Booking cannot be in the past");
                return true;
            }

            return false;
        }
    }
}