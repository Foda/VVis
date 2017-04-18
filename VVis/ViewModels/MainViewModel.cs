using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using VVis.Models;

namespace VVis.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public ReactiveList<GameItem> GameList { get; }

        private GameItem _selectedItem;
        public GameItem SelectedItem
        {
            get { return _selectedItem; }
            set { this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        }

        public MainViewModel()
        {
            GameList = new ReactiveList<GameItem>();
            GameList.Add(new GameItem());
            GameList.Add(new GameItem());
            GameList.Add(new GameItem());
        }
    }
}
