using System;
using System.Threading.Tasks;

namespace AntDesign
{

    public class MessageResult
    {
        public MessageResult(Task task)
        {
            Task = task;
        }

        public Task Task { get; private set; }

        public MessageResult Then(Action action)
        {
            var t = Task.ContinueWith((result) =>
            {
                action?.Invoke();
            }, TaskScheduler.Current);
            return new MessageResult(t);
        }

    }
}
