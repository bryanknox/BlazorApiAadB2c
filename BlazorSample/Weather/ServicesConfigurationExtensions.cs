using System;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSample.Weather
{
    public static class ServicesConfigurationExtensions
    {
        public static void AddWeatherForecast(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // See https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-5.0#typed-httpclient

            // Get WeatherForecastClientSettings configuration from the named configuration section
            // of appsetttings.json.
            IConfigurationSection weatherApiConfigurationSection = configuration.GetSection(
                WeatherForecastClientSettings.ConfigurationSectionName);

            // Configure dependency injection of WeatherForecastClientSettings
            // so that they will be injected into services
            // as IOptions<WeatherForecastClientSettings>.
            services.AddOptions<WeatherForecastClientSettings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    weatherApiConfigurationSection.Bind(settings);
                });

            // Get WeatherForecastClientSettings for use below.
            var weatherForecastClientSettings = new WeatherForecastClientSettings();
            weatherApiConfigurationSection.Bind(weatherForecastClientSettings);

            // Configure typed HttpClient for use by WeatherForecastClient.
            // This adds WeatherForecastClient to the dependency injection,
            // with the correctly configured HttpClient injected into it.
            // See: https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-5.0#configure-the-httpclient-handler
            services.AddHttpClient<WeatherForecastClient>(
                client => client.BaseAddress = new Uri(weatherForecastClientSettings.ApiUrl))
            .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: new[] { weatherForecastClientSettings.ApiUrl },
                scopes: new[] { weatherForecastClientSettings.Scope }));
        }
    }
}
