using System;
using System.Threading;
using System.Threading.Tasks;

namespace MGS2_CheatTrainer_V2.Models
{
    public class PeriodicTask
    { 
        public static async Task Run(Action action, TimeSpan period, CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(period, cancellationToken);

                if(!cancellationToken.IsCancellationRequested)
                {
                    action();
                }
            }
        }

        public static Task Run(Action action, TimeSpan period)
        {
            return Run(action, period, CancellationToken.None);
        }
    }
}
