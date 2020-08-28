﻿using System;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.BookingService.Requests;
using PDR.PatientBooking.Service.BookingService.Validation;
using PDR.PatientBooking.Service.ClinicServices.Requests;

namespace PDR.PatientBooking.Service.Tests.BookingService.Validation
{
    [TestFixture]
    public class AddBookingRequestValidatorTests
    {
        private IFixture _fixture;

        private PatientBookingContext _context;

        private AddBookingRequestValidator _addBookingRequestValidator;

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
            _addBookingRequestValidator = new AddBookingRequestValidator(_context
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

            //act
            var res = _addBookingRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeTrue();
        }

        [Test]
        public void ValidateRequest_BookingIsInPast_ReturnsFailedValidationResult()
        {
            //arrange
            var request = _fixture.Create<AddBookingRequest>();
            request.DoctorId = 1;
            request.PatientId = 1;
            request.StartTime = new DateTime().AddDays(1);
            request.EndTime = new DateTime().AddDays(1);

            //act
            var res = _addBookingRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeFalse();
            res.Errors.Should().Contain("Booking cannot be in the past");
        }

        private AddBookingRequest GetValidRequest()
        {
            var request = _fixture.Create<AddBookingRequest>();
            request.DoctorId = 1;
            request.PatientId = 1;
            request.StartTime = DateTime.UtcNow.AddDays(1);
            request.EndTime = DateTime.UtcNow.AddDays(1);
            return request;
        }
    }
}