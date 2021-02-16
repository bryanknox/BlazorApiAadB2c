using System.Threading.Tasks;
using BlazorSample.Weather;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddWeatherForecast(builder.Configuration);

            // Get WeatherForecastClientSettings from configuration
            // for use below.
            var weatherApiSettings = new WeatherForecastClientSettings();
            builder.Configuration
               .GetSection(WeatherForecastClientSettings.ConfigurationSectionName)
               .Bind(weatherApiSettings);

            // Configure MSAL stuff.

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
                options.ProviderOptions.DefaultAccessTokenScopes.Add(weatherApiSettings.Scope);

                // Use redirect instead of popup for login UI.
                // options.ProviderOptions.LoginMode = "redirect";
            });

            await builder.Build().RunAsync();
        }
    }
}
