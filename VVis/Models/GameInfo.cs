using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using Newtonsoft.Json;

namespace VVis.Models
{
    public class GameInfo
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Title { get; set; }

        [JsonProperty("original_release_date")]
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Brief summary of the game 
        /// </summary>
        [JsonProperty("deck")]
        public string Deck { get; set; }

        /// <summary>
        /// Primary art of the game (box?)
        /// </summary>
        [JsonProperty("image")]
        public ImageURL Image { get; set; }

        /// <summary>
        /// Screenshots of the game. Will use for background images
        /// </summary>
        [JsonProperty("images")]
        public List<ImageURL> Images { get; set; }
    }
}
