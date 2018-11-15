using System.Collections.Concurrent;
using System.Collections.Generic;
using MusicDownload.Logic;
using MusicDownload.Models;

namespace MusicDownload.Business
{
    public abstract class AbsDownloaderPool
    {

        public bool StopDownload { get; private set; } = false;
        public bool Stop { get; private set; } = false;


        protected readonly List<IDownloader> _downloaders;

        protected ConcurrentQueue<BasicMusicInfoModel> _downloadMusicQueue;

        public void StopPool()
        {
            Stop = true;
            StopDownload = true;
        }
        

        protected AbsDownloaderPool(ConcurrentQueue<BasicMusicInfoModel> downloadMusicQueue)
        {
            _downloadMusicQueue = downloadMusicQueue;
            _downloaders = new List<IDownloader>();
        }

        public void AddDownloader(IDownloader downloader)
        {
            _downloaders.Add(downloader);
        }

        public abstract void StartListen();
    }
}
