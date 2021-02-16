using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorSample.Weather
{
    // See: https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-5.0#typed-httpclient
    public class WeatherForecastClient
    {
        private readonly HttpClient _httpClient;

        public WeatherForecastClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<WeatherForecast[]> GetForecastAsync()
        {
            var forecasts = new WeatherForecast[0];

            try
            {
                // Call the API using a path that is relative to the BaseAddress configured in
                // the HttpClient used to call the weather API.
                // The BaseAddress is configured as the full URL, so we use an empty relative path here.
                forecasts = await _httpClient.GetFromJsonAsync<WeatherForecast[]>(string.Empty);
            }
            catch (AccessTokenNotAvailableException exception)
            {
                // Navigates to Microsoft.AspNetCore.Components.WebAssembly.Authentication.AccessTokenResult.RedirectUrl
                // to allow refreshing the access token.
                exception.Redirect();
            }

            return forecasts;
        }
    }
}
