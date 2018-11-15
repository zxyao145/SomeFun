using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDownload.Models
{

    public class ItemsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int subcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string songmid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vkey { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public int expiration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ItemsItem> items { get; set; }
    }

    public class QqMusicVkeyModel
{
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int cid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
    }
}
