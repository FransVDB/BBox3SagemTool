using System;

namespace BBox3Tool.exception
{
    public class ThreadCancelledException: Exception
    {
        public ThreadCancelledException()
        {
        }

        public ThreadCancelledException(string message)
            : base(message)
        {
        }

        public ThreadCancelledException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
