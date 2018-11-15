using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDownload.Models.EventArgs
{
    public class CompletedEventArgs
    {
        /// <summary>
        /// 请求的URL地址
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// 任务线程ID
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// 页面源代码
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 爬虫请求执行事件
        /// </summary>
        public long Milliseconds { get; set; }
    }
}
