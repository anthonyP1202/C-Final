using CommunityToolkit.Mvvm.Input;
using homeLearning.View;
using homeLearning.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace homeLearning.viewModel
{
    class GameVM : BaseVM
    {
        public ICommand switchToPokemon { get; set; }
        public ICommand switchToSpell { get; set; }
        public ICommand startGame { get; set; }


        private string _myDatabase;

        public GameVM(string MyDatabase) 
        {
            switchToPokemon = new RelayCommand(pokemenonSwitch);
            switchToPokemon = new RelayCommand(spellSwitch);
            switchToPokemon = new RelayCommand(gameStart);
            _myDatabase = MyDatabase;
        }

        public void pokemenonSwitch()
        {
            //MainWindowVM.OnRequestVMChange?.Invoke(new PokemenonVM(_myDatabase));
        }

        public void spellSwitch()
        {
            //MainWindowVM.OnRequestVMChange?.Invoke(new PokemenonVM(_myDatabase));
        }

        public void gameStart()
        {
            //MainWindowVM.OnRequestVMChange?.Invoke(new PokemenonVM(_myDatabase));
        }
    }
}
