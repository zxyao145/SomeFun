using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicDownload.Models;

namespace MusicDownload.Business
{
    public class QqDownloaderPool: AbsDownloaderPool
    {

        public QqDownloaderPool(ConcurrentQueue<BasicMusicInfoModel> downloadMusicQueue) : base(downloadMusicQueue)
        {

        }

        public override void StartListen()
        {
            if (_downloaders.Count==0)
            {
                throw new Exception("线程池中的数量为空！");
            }
            Task.Run(() =>
            {
                while (!StopDownload)
                {
                    var haveItem = _downloadMusicQueue.TryDequeue(out var model);
                    if (haveItem)
                    {
                        var downloader = _downloaders.FirstOrDefault(a => a.Status == "stop");
                        while (downloader == null)
                        {
                            Thread.Sleep(300);
                            downloader = _downloaders.FirstOrDefault(a => a.Status == "stop");
                        }

                        downloader.DownloadAsync(model);
                    }

                    Thread.Sleep(500);
                }
            });
        }
    }
}
