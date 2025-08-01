using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CountryFlagAPI.IntegrationTests.Configuration
{

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _mockServerUrl;

        public CustomWebApplicationFactory(string mockServerUrl)
        {
            _mockServerUrl = mockServerUrl;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.Sources.Clear();
                var dict = new Dictionary<string, string>
                {
                    { "CountryApi:BaseUrl", _mockServerUrl }
                };
                config.AddInMemoryCollection(dict);
            });
        }
    }

}
