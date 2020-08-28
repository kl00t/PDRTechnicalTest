using System;
using System.Collections.Generic;
using System.Linq;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.BookingService.Requests;
using PDR.PatientBooking.Service.BookingService.Validation;

namespace PDR.PatientBooking.Service.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly PatientBookingContext _context;
        private readonly ICancelBookingRequestValidator _cancelBookingRequestValidator;
        private readonly IAddBookingRequestValidator _addBookingRequestValidator;

        public BookingService(PatientBookingContext context, ICancelBookingRequestValidator cancelBookingRequestValidator, IAddBookingRequestValidator addBookingRequestValidator)
        {
            _context = context;
            _cancelBookingRequestValidator = cancelBookingRequestValidator;
            _addBookingRequestValidator = addBookingRequestValidator;
        }

        public void CancelBooking(CancelBookingRequest cancelBookingRequest)
        {
            var validationResult = _cancelBookingRequestValidator.ValidateRequest(cancelBookingRequest);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            var order = _context.Order.First(x => x.Id.Equals(cancelBookingRequest.Id));
            _context.Order.Remove(order);
            _context.SaveChanges();
        }

        public void AddBooking(AddBookingRequest addBookingRequest)
        {
            var validationResult = _addBookingRequestValidator.ValidateRequest(addBookingRequest);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            var bookingId = new Guid();
            var bookingStartTime = addBookingRequest.StartTime;
            var bookingEndTime = addBookingRequest.EndTime;
            var bookingPatientId = addBookingRequest.PatientId;
            var bookingPatient = _context.Patient.FirstOrDefault(x => x.Id == addBookingRequest.PatientId);
            var bookingDoctorId = addBookingRequest.DoctorId;
            var bookingDoctor = _context.Doctor.FirstOrDefault(x => x.Id == addBookingRequest.DoctorId);
            var bookingSurgeryType = _context.Patient.FirstOrDefault(x => x.Id == bookingPatientId).Clinic.SurgeryType;

            var myBooking = new Order
            {
                Id = bookingId,
                StartTime = bookingStartTime,
                EndTime = bookingEndTime,
                PatientId = bookingPatientId,
                DoctorId = bookingDoctorId,
                Patient = bookingPatient,
                Doctor = bookingDoctor,
                SurgeryType = (int)bookingSurgeryType
            };

            _context.Order.AddRange(new List<Order> { myBooking });
            _context.SaveChanges();
        }
    }
}