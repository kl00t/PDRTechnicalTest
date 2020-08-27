using System;
using System.Linq;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.BookingService.Requests;
using PDR.PatientBooking.Service.BookingService.Validation;

namespace PDR.PatientBooking.Service.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly PatientBookingContext _context;
        private readonly ICancelBookingRequestValidator _validator;

        public BookingService(PatientBookingContext context, ICancelBookingRequestValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public void CancelBooking(CancelBookingRequest cancelBookingRequest)
        {
            var validationResult = _validator.ValidateRequest(cancelBookingRequest);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            var order = _context.Order.First(x => x.Id.Equals(cancelBookingRequest.Id));
            _context.Order.Remove(order);
            _context.SaveChanges();
        }
    }
}