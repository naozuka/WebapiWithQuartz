using Quartz;

namespace WebapiWithQuartz
{    
    [DisallowConcurrentExecution]
    public class HaveSomeCoffeJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss")}] Starting job - Here have some coffee");

            Random random = new Random();	// A seed that is reset every loop.
			int randomSeconds = random.Next(10)*1000;           
            Thread.Sleep(randomSeconds);

            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss")}] Ending job - Thank you");
            Console.WriteLine("----------------------------------------");
            
            return Task.CompletedTask;
        }
    }
}