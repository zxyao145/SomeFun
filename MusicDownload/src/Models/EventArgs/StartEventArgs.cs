using System;

namespace MusicDownload.Models.EventArgs
{
    public class StartEventArgs
    {
        public int ThreadId { get; set; }
        public Uri Uri { get; set; }

    }
}
