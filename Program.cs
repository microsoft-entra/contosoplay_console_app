using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

// Get app settings
var builder = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.json", true, true);

var config = builder.Build();
var TenantId = config["appSettings:TenantId"];
var AppId = config["appSettings:AppId"];
var ClientSecret = config["appSettings:ClientSecret"];

// Get an access token for MS Graph API
var scopes = new[] { "https://graph.microsoft.com/.default" };
var clientSecretCredential = new ClientSecretCredential(TenantId, AppId, ClientSecret);
var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

// Get groups
var groups = await graphClient.Groups
                    .Request()
                    .Select(e => new
                    {
                        e.DisplayName,
                        e.Id
                    })
                    .GetAsync();

foreach (var group in groups.CurrentPage)
{
    Console.WriteLine(group.DisplayName);
}