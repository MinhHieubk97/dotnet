using System.Diagnostics;
using System.Threading.Tasks;
using brechtbaekelandt.cronJob.Services.Interfaces;

namespace brechtbaekelandt.cronJob.Services
{
    public class MyAwesomeService : IMyAwesomeService
    {
        public async Task ExecuteAsync(int i)
        {
            // Do some async operation

            Debug.WriteLine($"Executing ({i})");
        }
    }
}
