using System;
using System.Threading.Tasks;
using Jil;
using MusicDownload.Logic;
using MusicDownload.Models;

namespace MusicDownload.Business
{
    public class QqDownloader:IDownloader
    {
        private IRequests _requests;
        

        private const string _fcgUrl =
            "https://c.y.qq.com/base/fcgi-bin/fcg_music_express_mobile3.fcg?g_tk=5381&jsonpCallback=MusicJsonCallback9239412173137234&loginUin=0&hostUin=0&format=json&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0&cid=205361747&callback=MusicJsonCallback9239412173137234&uin=0&songmid={0}&filename={1}.m4a&guid=8208467632";

        private const string _downloadUrl =
            "http://dl.stream.qqmusic.qq.com/{0}.m4a?vkey={1}&guid=8208467632&uin=0&fromtag=66";

        public string Status { get; set; } = "stop";


        public event Action<string> OnBeforeDownload;
        public event Action<string> OnAfterDownload;
        public event Action<Exception,string> OnDownloadError;

       

        public QqDownloader(IRequests requests)
        {
            _requests = requests;
            OnBeforeDownload += (e) => { this.Status = "running"; };
            OnAfterDownload += (e) => { this.Status = "stop"; };
            OnDownloadError += (e,songInfo) => { this.Status = "stop"; };
        }
        /// <summary>
        /// 根据解析出来的模型进行下载
        /// </summary>
        /// <param name="basicModel"></param>
        /// <returns></returns>
        public async Task<bool> DownloadAsync(BasicMusicInfoModel basicModel)
        {
            return await Task.Run(async () =>
            {
                var obj = (QqMusicInfoModel) basicModel;
                try
                {
                    OnBeforeDownload?.Invoke($"{basicModel.SongName}_{basicModel.SingerName}");
                    var vkeyInfo = await GetVkeyInfo(obj.SongId, obj.MediaMid);
                    var url = new Uri(string.Format(_downloadUrl, obj.MediaMid, vkeyInfo.data.items[0].vkey));
                    
                    var saveName = $"{obj.SongName}_{obj.SingerName}";                    
                    await _requests.SaveFileAsync(url, $"{saveName}.m4a");

                    OnAfterDownload?.Invoke($"{basicModel.SongName}_{basicModel.SingerName}");
                    return true;
                }
                catch (Exception e)
                {
                    OnDownloadError?.Invoke(e, $"{basicModel.SongName}_{basicModel.SingerName}");
                    return false;
                }
            });
        }


        /// <summary>
        /// 获取下载链接中的vkey
        /// </summary>
        /// <param name="songMid"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private async Task<QqMusicVkeyModel> GetVkeyInfo(string songMid, string fileName)
        {
            return await Task.Run(async () =>
            {
                var url = new Uri(string.Format(_fcgUrl, songMid, fileName));
                var vkeyInfo = await _requests.StartAsync(url);
                var leftQuotoIndex = vkeyInfo.IndexOf("(", StringComparison.Ordinal);
                vkeyInfo = vkeyInfo.Substring(leftQuotoIndex + 1, vkeyInfo.Length - leftQuotoIndex - 2);
                var models = JSON.Deserialize<QqMusicVkeyModel>(vkeyInfo);
                return models;
            });
        }
    }
}
