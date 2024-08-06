using System.Configuration;
using System.Data;
using System.Windows;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sprava_Vyrobku_a_Dilu.Database;
using Sprava_Vyrobku_a_Dilu.Models;
using Sprava_Vyrobku_a_Dilu.Services;

namespace Sprava_Vyrobku_a_Dilu
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
                    services.AddSingleton<IDbService,DbService>();
                    services.AddSingleton<ObservableDataModel>();
                    services.AddAutoMapper(typeof(MappingProfile));
                    services.AddSingleton<PridatDilWindow>();
                    services.AddSingleton<PridatVyrobekWindow>();
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

                var dbService = AppHost!.Services.GetRequiredService<IDbService>();
                var mapper = AppHost!.Services.GetRequiredService<IMapper>();
                var observableData = AppHost!.Services.GetRequiredService<ObservableDataModel>();

                observableData.IsLoading = true;

                var vyrobky = await dbService.GetAllVyrobkyAsync();
                var dily = await dbService.GetAllDilyAsync();
                
                foreach (var d in dily)
                {
                    observableData.Dily.Add(d);
                }

                foreach (var vyrobekModel in vyrobky)
                {
                    observableData.Vyrobky.Add(vyrobekModel);
                    var viewableModel = mapper.Map<VyrobekViewableModel>(vyrobekModel);
                    viewableModel.CountOfDily = observableData.Dily.Count(d => d.VyrobekId == vyrobekModel.VyrobekId);
                    observableData.ViewableVyrobky.Add(viewableModel);
                }

                var startupForm = AppHost!.Services.GetRequiredService<MainWindow>();
                startupForm.Show();

                observableData.IsLoading = false;

                base.OnStartup(e);
                await AppHost.WaitForShutdownAsync();
            }
            catch(Exception ex) 
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
                MessageBox.Show("Exception occured on Exit" + ex.Message + ex.StackTrace, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }

}
