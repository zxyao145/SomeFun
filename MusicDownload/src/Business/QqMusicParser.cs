using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicDownload.Logic;
using MusicDownload.Models;

namespace MusicDownload.Business
{
    public class QqMusicParser : IMusicParser
    {
        public string Status { get; set; } = "stop";
        public event Action OnBeforeParse;
        public event Action OnAfterParse;
        public event Action<Exception> OnParseError;

        private QqMusicParse _qqMusicParse;
        public QqMusicParser()
        {
            OnBeforeParse += () => { this.Status = "running"; };
            OnAfterParse += () => { this.Status = "stop"; };
            OnParseError += (e) => { this.Status = "stop"; };
            _qqMusicParse =new QqMusicParse();
        }

        /// <summary>
        /// 对qq音乐服务器返回内容进行解析
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public async Task<Tuple<int, List<BasicMusicInfoModel>>> ParseAsync(string searchInfo)
        {
            return await Task.Run(async () =>
             {
                 try
                 {
                     OnBeforeParse?.Invoke();
                     var results = await _qqMusicParse.ParseAsync(searchInfo);
                     OnAfterParse?.Invoke();
                     return results;
                 }
                 catch (Exception exception)
                 {
                     OnParseError?.Invoke(exception);
                     return null;
                 }
             });
        }
    }
}
