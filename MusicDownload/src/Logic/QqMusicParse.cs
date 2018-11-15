using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jil;
using MusicDownload.Models;

namespace MusicDownload.Logic
{
    public class QqMusicParse:IMusicParse
    {
        public Task<Tuple<int, List<BasicMusicInfoModel>>> ParseAsync(string musicInfo)
        {
            return Task.Run(() =>
            {
                var zuoKuohaoIndex = musicInfo.IndexOf("(", StringComparison.Ordinal);
                musicInfo = musicInfo.Substring(zuoKuohaoIndex + 1, musicInfo.Length - 2 - zuoKuohaoIndex);

                var musicInfoJson = JSON.DeserializeDynamic(musicInfo);

                var dataJson = musicInfoJson.data;
                var songJson = dataJson.song;
                var totalNum = Convert.ToInt32(songJson.totalnum.ToString());
                var songlist = songJson.list;
                var musicInfos = new List<BasicMusicInfoModel>();
                foreach (var singleSong in songlist)
                {
                    var seconds = Convert.ToInt32(singleSong.interval.ToString());
                    var singerName = GetSingerNames(singleSong.singer);
                    string albumWithQuoto = singleSong.album.name.ToString();
                    string songNameWithQuoto = singleSong.name.ToString();
                    var midWithQuoto = singleSong.mid.ToString();


                    var basicMusicInfoModel = new QqMusicInfoModel()
                    {
                        MediaMid = "C400" + midWithQuoto.Substring(1, midWithQuoto.Length - 2),
                        SongId = midWithQuoto.Substring(1, midWithQuoto.Length - 2),
                        SingerName = singerName,
                        Album = albumWithQuoto.Substring(1, albumWithQuoto.Length - 2),
                        SongName = songNameWithQuoto.Substring(1, songNameWithQuoto.Length - 2),
                        Time = GetTime(seconds)
                    };
                    musicInfos.Add(basicMusicInfoModel);
                }

                return new Tuple<int, List<BasicMusicInfoModel>>(totalNum, musicInfos);
            });
        }

        /// <summary>
        /// 对时间进行处理
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private string GetTime(int seconds)
        {
            if (seconds < 60)
            {
                return "00:" + seconds;
            }
            else
            {
                var minite = seconds / 60;
                if (minite < 60)
                {
                    return $"{minite:D2}:{seconds % 60}";
                }
                else
                {
                    var hour = minite / 60;
                    return $"{hour:D2}:{minite % 60}:{seconds % 60}";
                }
            }
        }

        /// <summary>
        /// 歌手名处理
        /// </summary>
        /// <param name="singers"></param>
        /// <returns></returns>
        private string GetSingerNames(dynamic singers)
        {
            var singerNames = new  List<string>();
            foreach (var singer in singers)
            {
                string singName = singer.name.ToString();
                singerNames.Add(singName.Substring(1, singName.Length - 2));
            }

            return string.Join(",", singerNames);
        }
    }
}
