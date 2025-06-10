using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WpfApp1.Data;
using WpfApp1.Services;

namespace WpfApp1
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var services = new ServiceCollection();
                ConfigureServices(services);
                ServiceProvider = services.BuildServiceProvider();

                var testContext = ServiceProvider.GetRequiredService<SyzyfContext>();

                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Startup error: {ex.Message}\n\nStack trace: {ex.StackTrace}");
            }
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<SyzyfContext>(options =>
                options.UseMySQL("server=localhost;port=3306;database=syzyf;user=root;password=;")
            );
            services.AddScoped<AuthService>();
        }
    }
}