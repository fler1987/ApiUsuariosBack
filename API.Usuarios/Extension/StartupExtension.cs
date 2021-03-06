using Hangfire;
using Hangfire.SqlServer;
using Hangfire.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace API.Usuarios.Extension
{
    public static class StartupExtension
    {
        public static void ConfigureHangFire(this IServiceCollection services, IConfiguration configuration)
        {
          

            services.AddHangfire(config => config .UseStorage(
    new MySqlStorage(
        configuration.GetConnectionString("HangFire"),
        //"HangFire",
        new MySqlStorageOptions
        {
            TransactionIsolationLevel = (System.Transactions.IsolationLevel?)IsolationLevel.ReadCommitted,
            QueuePollInterval = TimeSpan.FromSeconds(15),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true,
            DashboardJobListLimit = 50000,
            TransactionTimeout = TimeSpan.FromMinutes(1),
            TablesPrefix = "HangFire"
        })));

            services.AddHangfireServer();
        }

    }
}
