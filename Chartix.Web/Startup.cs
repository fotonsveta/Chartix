using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Db;
using Chartix.Infrastructure.Repositories;
using Chartix.Infrastructure.Telegram.Bot;
using Chartix.Infrastructure.Telegram.ButtonCommands;
using Chartix.Infrastructure.Telegram.Commands;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.MessageVisitors;
using Chartix.Infrastructure.Telegram.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Chartix.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITBotClientProvider, TBotClientProvider>();
            services.AddSingleton<IBotClient, BotClient>();
            services.AddSingleton<ILocalizer, Localizer>();

            services.AddScoped<IProcessMessageVisitor, ProcessMessageVisitor>();
            services.AddScoped<IBotUpdateService, BotUpdateService>();
            services.AddScoped<IValueHandlerService, ValueHandlerService>();

            ConfigureCommands(services);
            ConfigureButtonCommands(services);

            services.AddSingleton(Configuration.GetSection("BotConfig").Get<BotConfig>());

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("AppContext")));

            ConfigureRepositories(services);

            services.AddControllers()
                    .AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<ISourceRepository, SourceRepository>();
            services.AddScoped<IUpdateRepository, UpdateRepository>();
            services.AddScoped<IMetricRepository, MetricRepository>();
            services.AddScoped<IValueRepository, ValueRepository>();
        }

        private void ConfigureCommands(IServiceCollection services)
        {
            services.AddScoped<ICommand, AboutCommand>();
            services.AddScoped<ICommand, HelpCommand>();
            services.AddScoped<ICommand, MenuCommand>();
            services.AddScoped<ICommand, StartCommand>();
            services.AddScoped<ICommand, UnknownCommand>();
            services.AddScoped<ICommandStrategyFactory<ICommand>, CommandStrategyFactory>();
        }

        private void ConfigureButtonCommands(IServiceCollection services)
        {
            services.AddScoped<IButtonCommand, AddMetricButtonCommand>();
            services.AddScoped<IButtonCommand, ChooseDelMetricButtonCommand>();
            services.AddScoped<IButtonCommand, ChooseDelValueButtonCommand>();
            services.AddScoped<IButtonCommand, ChooseMainMetricButtonCommand>();
            services.AddScoped<IButtonCommand, DelMetricButtonCommand>();
            services.AddScoped<IButtonCommand, DelValueButtonCommand>();
            services.AddScoped<IButtonCommand, FileMenuButtonCommand>();
            services.AddScoped<IButtonCommand, FromJsonButtonCommand>();
            services.AddScoped<IButtonCommand, PlotButtonCommand>();
            services.AddScoped<IButtonCommand, MetricMenuButtonCommand>();
            services.AddScoped<IButtonCommand, SetMainMetricButtonCommand>();
            services.AddScoped<IButtonCommand, ShowMetricButtonCommand>();
            services.AddScoped<IButtonCommand, ToJsonButtonCommand>();
            services.AddScoped<IButtonCommand, TopMenuButtonCommand>();
            services.AddScoped<ICommandStrategyFactory<IButtonCommand>, ButtonCommandStrategyFactory>();
        }
    }
}
