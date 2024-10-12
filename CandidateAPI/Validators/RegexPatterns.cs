namespace CandidateAPI.Validators
{
    public class RegexPatterns
    {
        public const string GitHubProfileUrl = @"^https:\/\/(www\.)?github\.com\/[a-zA-Z0-9](?:[a-zA-Z0-9\-]{0,38}[a-zA-Z0-9])?$";
        public const string LinkedInProfileUrl = @"^https:\/\/(www\.)?linkedin\.com\/in\/[a-zA-Z0-9\-]+$";
        public const string PhoneNumber = @"^\+?[1-9]\d{1,14}$";
    }
}
