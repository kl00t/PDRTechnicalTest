using System;
using FluentAssertions;
using NUnit.Framework;
using PDR.PatientBooking.Service.Validation;

namespace PDR.PatientBooking.Service.Tests.Extensions
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void IsBetween_Date_Returns_True()
        {
            var today = DateTime.Now;
            var start = DateTime.Now.AddHours(-1);
            var end = DateTime.Now.AddHours(1);

            var isBetween = today.IsBetween(start, end, true);

            isBetween.Should().BeTrue();
        }

        [Test]
        public void IsBetween_Date_Returns_False()
        {
            var today = DateTime.Now;
            var start = DateTime.Now.AddHours(1);
            var end = DateTime.Now.AddHours(2);

            var isBetween = today.IsBetween(start, end, true);

            isBetween.Should().BeFalse();
        }
    }
}
