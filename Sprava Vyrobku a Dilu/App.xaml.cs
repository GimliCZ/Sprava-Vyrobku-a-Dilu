using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpravaVyrobkuaDilu.Database;
using SpravaVyrobkuaDilu.Models;
using SpravaVyrobkuaDilu.Services;

namespace SpravaVyrobkuaDilu
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            var configuration = new ConfigurationBuilder()
                //IOptions etc.
                .AddJsonFile("appsettings.json")
                .Build();

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {

                    services.AddDbContextFactory<AppDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
                    services.AddSingleton<IDbService, DbService>();
                    services.AddSingleton<ObservableDataProvider>();
                    services.AddAutoMapper(typeof(MappingProfile));
                    services.AddTransient<PridatDilWindow>();
                    services.AddTransient<PridatVyrobekWindow>();
                    services.AddTransient<UpravitVyrobekWindow>();
                    services.AddTransient<UpravitDilWindow>();
                    services.AddSingleton<MainWindow>();
                    //telemetry etc. 
                })
                .Build();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                await AppHost!.StartAsync();

                var observableData = AppHost!.Services.GetRequiredService<ObservableDataProvider>();
                await observableData.Refresh();

                var startupForm = AppHost!.Services.GetRequiredService<MainWindow>();
                startupForm.Show();

                base.OnStartup(e);
                await AppHost.WaitForShutdownAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured on Startup" + ex.Message + ex.StackTrace, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                await AppHost!.StopAsync();
                base.OnExit(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured on Exit" + ex.Message + ex.StackTrace, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }

}
