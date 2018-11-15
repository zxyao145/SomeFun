using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicDownload.Models;

namespace MusicDownload.Logic
{
    public interface IMusicParse
    {
        Task<Tuple<int, List<BasicMusicInfoModel>>> ParseAsync(string musicInfo);
    }
}
