using Quartz;

namespace WebapiWithQuartz
{
    public class NoQuartzScheduleException : Exception
    {
        public NoQuartzScheduleException(string message) : base(message)
        {            
        }
    }

    public static class QuartzExtensions
    {
        public static void AddQuartzServices<T>(this IServiceCollection services, IConfiguration configuration) where T : IJob
        {
            services.AddQuartz(q =>  
            {
                // Use a Scoped container to create jobs. I'll touch on this later
                q.UseMicrosoftDependencyInjectionJobFactory();

                // Register the job, loading the schedule from configuration
                q.AddJobAndTrigger<T>(configuration);
            });

            // Add the Quartz.NET hosted service
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);            
        }

        private static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration config) where T : IJob
        {
            // Use the name of the IJob as the appsettings.json key
            string jobName = typeof(T).Name;

            // Try and load the schedule from configuration
            var configKey = $"Quartz:{jobName}";
            var cronSchedule = config[configKey];

            // Some minor validation
            if (string.IsNullOrEmpty(cronSchedule))
            {
                throw new NoQuartzScheduleException($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");
            }

            // register the job as before
            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule)); // use the schedule from configuration
        }
    }
}