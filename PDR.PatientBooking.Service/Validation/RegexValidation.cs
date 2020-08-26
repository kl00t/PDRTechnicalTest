using System.Text.RegularExpressions;

namespace PDR.PatientBooking.Service.Validation
{
    public static class RegexValidation
    {
        public static bool IsEmailValid(string email)
        {
            var regex = new Regex(@"^\S+@\S+$");
            var match = regex.Match(email ?? string.Empty);
            return match.Success;
        }
    }
}