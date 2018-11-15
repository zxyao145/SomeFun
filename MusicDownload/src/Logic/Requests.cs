using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MusicDownload.Models.EventArgs;
using ErrorEventArgs = MusicDownload.Models.EventArgs.ErrorEventArgs;

namespace MusicDownload.Logic
{
    public class Requests : IRequests
    {
        /// <summary>
        /// 请求启动事件
        /// </summary>
        public event Action<StartEventArgs> OnStart;

        /// <summary>
        /// 请求完成事件
        /// </summary>
        public event Action<CompletedEventArgs> OnCompleted;

        /// <summary>
        /// 请求出错事件
        /// </summary>
        public event Action<ErrorEventArgs> OnError;


        /// <summary>
        /// 定义Cookie容器
        /// </summary>
        public CookieContainer CookiesContainer { get; set; }

        /// <summary>
        /// 代理服务器
        /// </summary>
        public WebProxy Proxy { get; set; }

        /// <summary>
        /// 设置User-Agent，默认伪装成Google Chrome浏览器
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";

        /// <summary>
        /// 定义请求超时时间为5秒
        /// </summary>
        public int Timeout { get; set; } = 5000;

        /// <summary>
        /// 出错重试次数
        /// </summary>
        public int RetryTimes { get; set; } = 5;

        /// <summary>
        /// 开始请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="requestMethod"></param>
        /// <param name="dataDict"></param>
        /// <returns></returns>
        public Task<string> StartAsync(Uri uri, string requestMethod = "GET", Dictionary<string, string> dataDict = null)
        {
            return Task.Run(async () =>
            {
                var threadId = Thread.CurrentThread.ManagedThreadId; //获取当前任务线程ID
                var content = string.Empty;

                var retryTime = 0;

                while (retryTime < RetryTimes)
                {
                    try
                    {
                        OnStart?.Invoke(new StartEventArgs()
                        {
                            ThreadId = Thread.GetDomainID(),
                            Uri = uri
                        });

                        var watch = new Stopwatch();
                        watch.Start();
                        var request = await AssemblyHttpWebRequest(uri);

                        using (var response = (HttpWebResponse)request.GetResponse()) //获取请求响应
                        {
                            foreach (Cookie cookie in response.Cookies)
                                this.CookiesContainer.Add(cookie); //将Cookie加入容器，保存登录状态

                            if (response.ContentEncoding.ToLower().Contains("gzip")) //解压
                            {
                                using (
                                    GZipStream stream = new GZipStream(response.GetResponseStream(),
                                        CompressionMode.Decompress))
                                {
                                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                    {
                                        content = reader.ReadToEnd();
                                    }
                                }
                            }
                            else if (response.ContentEncoding.ToLower().Contains("deflate")) //解压
                            {
                                using (
                                    DeflateStream stream = new DeflateStream(response.GetResponseStream(),
                                        CompressionMode.Decompress))
                                {
                                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                    {
                                        content = reader.ReadToEnd();
                                    }
                                }
                            }
                            else
                            {
                                using (Stream stream = response.GetResponseStream()) //原始
                                {
                                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                    {
                                        content = reader.ReadToEnd();
                                    }
                                }
                            }
                        }

                        request.Abort();
                        watch.Stop();

                        var milliseconds = watch.ElapsedMilliseconds; //获取请求执行时间
                        OnCompleted?.Invoke(new CompletedEventArgs()
                        {
                            Content = content,
                            Milliseconds = milliseconds,
                            ThreadId = threadId,
                            Uri = uri
                        });

                        break;
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke(new ErrorEventArgs()
                        {
                            RetryIndex = retryTime,
                            Exception = ex,
                            Uri = uri,
                            ThreadId = threadId
                        });

                        Thread.Sleep(retryTime * 2000 + 3000);
                        retryTime++;
                    }
                }

                return content;
            });
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public  Task<bool> SaveFileAsync(Uri uri, string fileName)
        {
            return Task.Run(async () =>
            {
                var threadId = Thread.CurrentThread.ManagedThreadId; //获取当前任务线程ID

                var retryTime = 0;

                while (retryTime < RetryTimes)
                {
                    try
                    {
                        OnStart?.Invoke(new StartEventArgs()
                        {
                            ThreadId = Thread.GetDomainID(),
                            Uri = uri
                        });

                        var watch = new Stopwatch();
                        watch.Start();
                        var request = await AssemblyHttpWebRequest(uri);
                        using (var response = (HttpWebResponse)request.GetResponse()) //获取请求响应
                        {
                            foreach (Cookie cookie in response.Cookies)
                                this.CookiesContainer.Add(cookie); //将Cookie加入容器，保存登录状态

                            Stream stream = null;
                            if (response.ContentEncoding.ToLower().Contains("gzip")) //解压
                            {
                                stream = new GZipStream(response.GetResponseStream(),
                                    CompressionMode.Decompress);
                            }
                            else if (response.ContentEncoding.ToLower().Contains("deflate")) //解压
                            {
                                stream = new DeflateStream(response.GetResponseStream(),
                                    CompressionMode.Decompress);
                            }
                            else
                            {
                                //原始
                                stream = response.GetResponseStream();
                            }

                            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                var bufferSize = 4096;

                                byte[] bytes = new byte[bufferSize];
                                int length = stream.Read(bytes, 0, bufferSize);
                                while (length > 0)
                                {
                                    fs.Write(bytes, 0, length);
                                    length = stream.Read(bytes, 0, bufferSize);
                                }

                            }
                            stream.Close();
                        }
                        request.Abort();
                        watch.Stop();

                        var milliseconds = watch.ElapsedMilliseconds; //获取请求执行时间
                        OnCompleted?.Invoke(new CompletedEventArgs()
                        {
                            Content = "",
                            Milliseconds = milliseconds,
                            ThreadId = threadId,
                            Uri = uri
                        });
                        return true;
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke(new ErrorEventArgs()
                        {
                            RetryIndex = retryTime,
                            Exception = ex,
                            Uri = uri,
                            ThreadId = threadId
                        });

                        Thread.Sleep(retryTime * 2000 + 3000);
                        retryTime++;
                        if (retryTime==5)
                        {
                            
                        }
                    }
                }

                return false;
            });
        }

        /// <summary>
        /// 组装HttpWebRequest
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="requestMethod"></param>
        /// <returns></returns>
        private async Task<HttpWebRequest> AssemblyHttpWebRequest(Uri uri, string requestMethod = "GET", Dictionary<string, string> dataDict = null)
        {
            return await Task.Run(async () =>
            {
                var request = (HttpWebRequest) WebRequest.Create(uri);
                request.Accept = "*/*";
                request.ServicePoint.Expect100Continue = false; //加快载入速度
                request.ServicePoint.UseNagleAlgorithm = false; //禁止Nagle算法加快载入速度
                request.AllowWriteStreamBuffering = false; //禁止缓冲加快载入速度
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate"); //定义gzip压缩页面支持
                request.ContentType = "application/x-www-form-urlencoded"; //定义文档类型及编码
                request.AllowAutoRedirect = false; //禁止自动跳转
                request.KeepAlive = true; //启用长连接
                //设置User-Agent
                request.UserAgent = UserAgent;
                //定义请求超时时间
                request.Timeout = Timeout;
                if (Proxy != null) request.Proxy = Proxy; //设置代理服务器IP，伪装请求地址
                request.CookieContainer = this.CookiesContainer; //附加Cookie容器
                request.ServicePoint.ConnectionLimit = int.MaxValue; //定义最大连接数
                request.Method = requestMethod;

                //写入请求的数据
                if (dataDict != null)
                {
                    var data = await GetDataAsync(dataDict);

                    request.ContentLength = data.Length;
                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(data, 0, data.Length);
                    }
                }

                return request;
            });
        }


        /// <summary>
        /// 格式化请求的数据
        /// </summary>
        /// <param name="dataDict"></param>
        /// <returns></returns>
        private Task<byte[]> GetDataAsync(Dictionary<string, string> dataDict)
        {
            return Task.Run(() =>
            {
                StringBuilder builder = new StringBuilder();
                int i = 0;
                foreach (var item in dataDict)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }

                byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
                return data;
            });
        }

    }
}
