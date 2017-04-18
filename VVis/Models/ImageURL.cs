using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using Newtonsoft.Json;

namespace VVis.Models
{
    public class ImageURL
    {
        /// <summary>
        /// Probably box art 
        /// 455x640
        /// </summary>
        [JsonProperty("small_url")]
        public string SmallURL { get; set; }

        /// <summary>
        /// Large box art?
        /// 683x960
        /// </summary>
        [JsonProperty("medium_url")]
        public string MediumURL { get; set; }

        /// <summary>
        /// Really large art. Will use this for backgrounds
        /// </summary>
        [JsonProperty("super_url")]
        public string SuperURL { get; set; }

        /// <summary>
        /// Probably box art
        /// 113x160?
        /// </summary>
        [JsonProperty("thumb_url")]
        public string ThumbURL { get; set; }

        /// <summary>
        /// Screen? Kinda looks like a banner
        /// 480x270
        /// </summary>
        [JsonProperty("screen_url")]
        public string ScreenURL { get; set; }
    }
}
