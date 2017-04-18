using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace VVis.Models
{
    public class GameItem : ReactiveObject
    {
        public GameInfo Info { get; private set; }
        public string BoxImageURL { get { return Info.Image.SmallURL; } }

        public GameItem()
        {
            Info = new GameInfo()
            {
                Title = "Blah",
                ReleaseDate = "1990",
                Image = new ImageURL()
                {
                    SmallURL = "https://upload.wikimedia.org/wikipedia/commons/e/e1/FullMoon2010.jpg"
                }
            };
        }
    }
}
