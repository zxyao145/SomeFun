using System;

namespace MusicDownload.Models.EventArgs
{
    public class ErrorEventArgs
    {
        public int RetryIndex { get; set; }
        public int ThreadId { get; set; }
        public Uri Uri { get; set; }
        public Exception Exception { get; set; }
    }
}
