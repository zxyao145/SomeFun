using System;
using System.Threading.Tasks;

namespace MusicDownload.Business
{
    public interface IMusicSearcher
    {
        string Status { get; set; }

        event Action OnBeforeSearch;
        event Action OnAfterSearch;
        event Action<Exception> OnSearchError;

        /// <summary>
        /// 根据关键字和页数，默认页数为1进行搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<string> SearchAsync(string keyword, int page = 1);
    }
}
