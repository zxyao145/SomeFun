using System;
using System.Threading.Tasks;
using MusicDownload.Logic;

namespace MusicDownload.Business
{
    public class QqMusicMusicSearcher:IMusicSearcher
    {
        private IRequests _requests;

        public string Status { get; set; } = "stop";

        public event Action OnBeforeSearch;
        public event Action OnAfterSearch;
        public event Action<Exception> OnSearchError;

        private const string SearchUrl =
            "https://c.y.qq.com/soso/fcgi-bin/client_search_cp?ct=24&qqmusic_ver=1298&new_json=1&remoteplace=txt.yqq.top&searchid=34725291680541638&t=0&aggr=1&cr=1&catZhida=1&lossless=0&flag_qc=0&p={0}&n=20&w={1}&g_tk=5381&jsonpCallback=MusicJsonCallback703296236531272&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0";

        public QqMusicMusicSearcher(IRequests requests)
        {
            _requests = requests;
            OnBeforeSearch += () => { this.Status = "running"; };
            OnAfterSearch += () => { this.Status = "stop"; };
            OnSearchError += (e) => { this.Status = "stop"; };
        }

        /// <summary>
        /// 根据关键字进行搜索
        /// </summary>
        /// <param name="keyword">音乐名或者歌手</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public async Task<string> SearchAsync(string keyword, int page = 1)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    OnBeforeSearch?.Invoke();

                    var songAboutInfo = await _requests.StartAsync(new Uri(string.Format(SearchUrl, page, keyword)));

                    OnAfterSearch?.Invoke();
                    return songAboutInfo;
                }
                catch (Exception e)
                {
                    OnSearchError?.Invoke(e);
                    return "";
                }
            });
        }
    }
}
