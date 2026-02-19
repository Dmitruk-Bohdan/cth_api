namespace CTHelper.Presentation.Settings
{
    public static class PresentationSettingsSetup
    {
        public static IServiceCollection AddPresentationSettings(this IServiceCollection services)
        {
            //services.AddSingleton<ILibrarySettings, LibrarySettingsLocalFile>();
            return services;
        }
    }
}
