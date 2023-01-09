using BLL.Jobs;
using BookStoreUI;
using Quartz;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await UpdateQuartzJobs(host);
        await host.RunAsync();
    }

    // EF Core uses this method at design time to access the DbContext
    public static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(
                webBuilder => webBuilder.UseStartup<Startup>());
    private static async Task UpdateQuartzJobs(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var environment = services.GetRequiredService<IHostEnvironment>();

            var jobFactory = services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await jobFactory.GetScheduler();

            var job = new JobKey(nameof(SendStatisticJob));
            if (job != null)
            {
                var jD = await scheduler.GetJobDetail(job);
                await scheduler.DeleteJob(job);
                await scheduler.ScheduleJob(jD, TriggerBuilder.Create().WithCronSchedule(environment.IsDevelopment() || environment.IsEnvironment("Local") ? "0 0/2 * 1/1 * ? *": "0 0 12 30 1/1 ? *", x => x.WithMisfireHandlingInstructionIgnoreMisfires()).ForJob(job).Build());
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
