namespace SchoolLearningSystem.API.Models
{
    // Models/ErrorDetails.cs
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string? Trace { get; set; } // مفيد في بيئة التطوير فقط

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}
