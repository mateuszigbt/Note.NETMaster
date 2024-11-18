namespace Noteapp.Util
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSeedAdminUser(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var seedAdminUser = serviceProvider.GetRequiredService<SeedAdminUser>();
                seedAdminUser.InitializeAsync().Wait(); //Najpierw aktualizacja do bazy danych
            }
            return app;
        }
    }
}
