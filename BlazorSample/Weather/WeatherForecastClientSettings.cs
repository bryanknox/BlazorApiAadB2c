namespace BlazorSample.Weather
{
    public class WeatherForecastClientSettings
    {
        /// <summary>
        /// The name of the configuration section in the appsettings.json file
        /// that holds the settings.
        /// </summary>
        public const string ConfigurationSectionName = "WeatherForecastClient";

        public string ApiUrl { get; set; }

        public string Scope { get; set; }
    }
}
