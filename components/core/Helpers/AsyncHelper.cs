using System;
using System.Threading.Tasks;

namespace AntDesign
{
    public static class AsyncHelper
    {
        /// <summary>
        /// Will probe a check predicate every given milliseconds until predicate is true or until
        /// runs out of number of probings.
        /// </summary>
        /// <param name="check">A predicate that will be run every time after waitTimeInMilisecondsPerProbing will pass.</param>
        /// <param name="probings">Maximum number of probings. After this number is reached, the method finishes.</param>
        /// <param name="waitTimeInMilisecondsPerProbing">How long to wait between each probing.</param>
        /// <returns>Task</returns>
        public static async Task<bool> WaitFor(Func<bool> check, int probings = 100, int waitTimeInMilisecondsPerProbing = 10)
        {
            if (!check())
            {
                for (int i = 0; i < probings; i++)
                {
                    await Task.Delay(waitTimeInMilisecondsPerProbing);
                    if (check())
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

    }
}
