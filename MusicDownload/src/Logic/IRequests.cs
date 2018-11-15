using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MusicDownload.Models.EventArgs;
using ErrorEventArgs = MusicDownload.Models.EventArgs.ErrorEventArgs;

namespace MusicDownload.Logic
{
    public interface IRequests
    {

        /// <summary>
        /// 请求启动事件
        /// </summary>
        event Action<StartEventArgs> OnStart;

        /// <summary>
        /// 请求完成事件
        /// </summary>
        event Action<CompletedEventArgs> OnCompleted;

        /// <summary>
        /// 请求出错事件
        /// </summary>
        event Action<ErrorEventArgs> OnError;


        /// <summary>
        /// 代理服务器
        /// </summary>
        WebProxy Proxy { get; set; }

        /// <summary>
        /// 设置User-Agent，默认伪装成Google Chrome浏览器
        /// </summary>
        string UserAgent { get; set; }

        /// <summary>
        /// 定义请求超时时间为5秒
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// 开始进行web请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="requestMethod"></param>
        /// <param name="dataDict"></param>
        /// <returns></returns>
        Task<string> StartAsync(Uri uri, string requestMethod = "GET", Dictionary<string, string> dataDict = null);

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<bool> SaveFileAsync(Uri uri, string fileName);
    }
}
