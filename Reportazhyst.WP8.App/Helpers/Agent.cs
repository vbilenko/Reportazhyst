using System;
using Microsoft.Phone.Scheduler;
using Reportazhyst.WP8.Common;

namespace Reportazhyst.WP8.Helpers
{
    public static class Agent
    {
        public static void Start()
        {
            Stop();
            try
            {
                var uahExchangeAgent = new PeriodicTask(Constants.TaskName)
                {
                    Description = Constants.TaskDescription
                };
                ScheduledActionService.Add(uahExchangeAgent);
#if DEBUG
                ScheduledActionService.LaunchForTest(Constants.TaskName, new TimeSpan(0, 0, 1));
#endif
            }
            catch (Exception ex)
            {
                Alert.Show("Background agent failed", ex.Message);
            }
        }

        public static void Stop()
        {
            if (ScheduledActionService.Find(Constants.TaskName) != null)
                ScheduledActionService.Remove(Constants.TaskName);
        }
    }
}
