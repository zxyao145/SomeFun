using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDownload.Models
{
    public class BasicMusicInfoModel
    {
        /// <summary>
        /// 歌曲名
        /// </summary>
        public string SongName { get; set; }

        /// <summary>
        /// 歌曲id
        /// </summary>
        public string SongId { get; set; }

        /// <summary>
        /// 歌手名
        /// </summary>
        public string SingerName { get; set; }
        public string Album { get; set; }

        public string Time { get; set; }
    }
}
