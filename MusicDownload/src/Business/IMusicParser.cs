using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicDownload.Models;

namespace MusicDownload.Business
{
    interface IMusicParser
    {
        string Status { get; set; }

        event Action OnBeforeParse;
        event Action OnAfterParse;
        event Action<Exception> OnParseError;

        /// <summary>
        /// 对服务器返回的结果进行解析
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        Task<Tuple<int, List<BasicMusicInfoModel>>> ParseAsync(string searchInfo);
    }
}
