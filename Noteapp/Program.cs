using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Represents the entry point for the application.
/// </summary>
public class Program
{
    /// <summary>
    /// Main method to run the application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Creates the host builder for the application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>The configured host builder.</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>(); // Specify the Startup class to be used for configuring the application
            });
}