using System;
using System.Threading.Tasks;
using MusicDownload.Models;

namespace MusicDownload.Business
{
    public interface IDownloader
    {
        string Status { get; set; }

        event Action<string> OnBeforeDownload;
        event Action<string> OnAfterDownload;
        event Action<Exception, string> OnDownloadError;

        /// <summary>
        /// 根据解析出来的模型进行下载
        /// </summary>
        /// <param name="basicModel"></param>
        /// <returns></returns>
        Task<bool> DownloadAsync(BasicMusicInfoModel basicModel);
    }
}
