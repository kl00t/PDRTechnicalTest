using System;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.BookingService.Requests;
using PDR.PatientBooking.Service.BookingService.Validation;
using PDR.PatientBooking.Service.ClinicServices.Requests;
using PDR.PatientBooking.Service.ClinicServices.Validation;

namespace PDR.PatientBooking.Service.Tests.BookingService.Validation
{
    [TestFixture]
    public class CancelBookingRequestValidatorTests
    {
        private IFixture _fixture;

        private PatientBookingContext _context;

        private CancelBookingRequestValidator _cancelBookingRequestValidator;

        [SetUp]
        public void SetUp()
        {
            // Boilerplate
            _fixture = new Fixture();

            //Prevent fixture from generating from entity circular references 
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            // Mock setup
            _context = new PatientBookingContext(new DbContextOptionsBuilder<PatientBookingContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            // Mock default
            SetupMockDefaults();

            // Sut instantiation
            _cancelBookingRequestValidator = new CancelBookingRequestValidator(_context
            );
        }

        private void SetupMockDefaults()
        {

        }

        [Test]
        public void ValidateRequest_AllChecksPass_ReturnsPassedValidationResult()
        {
            //arrange
            var request = GetValidRequest();

            var existingBooking = _fixture
                .Build<Order>()
                .With(x => x.Id, BookingId)
                .Create();

            _context.Add(existingBooking);
            _context.SaveChanges();

            //act
            var res = _cancelBookingRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeTrue();
        }

        [Test]
        public void ValidateRequest_BookingDoesNotExist_ReturnsFailedValidationResult()
        {
            //arrange
            var request = GetValidRequest();

            var existingBooking = _fixture
                .Build<Order>()
                .With(x => x.Id, Guid.NewGuid())
                .Create();

            _context.Add(existingBooking);
            _context.SaveChanges();

            //act
            var res = _cancelBookingRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeFalse();
            res.Errors.Should().Contain("A booking was not found with that booking id");
        }

        private readonly Guid BookingId = new Guid("E039171E-B710-4F86-B919-9F76BA26410A");

        private CancelBookingRequest GetValidRequest()
        {
            var request = _fixture.Create<CancelBookingRequest>();
            request.Id = BookingId;
            return request;
        }
    }
}
