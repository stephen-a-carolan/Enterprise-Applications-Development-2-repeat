namespace WeatherService.Models
{
    public class WeatherInfo
    {
        public int Id { get; set; }  // Primary key
        public string City { get; set; }
        public string CurrentCondition { get; set; }
        public int MaxTemp { get; set; }
        public int MinTemp { get; set; }
        public string WindDirection { get; set; }
        public int WindSpeed { get; set; }
        public string Outlook { get; set; }
    }

}
