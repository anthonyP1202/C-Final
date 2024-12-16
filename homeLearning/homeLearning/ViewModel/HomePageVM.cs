using CommunityToolkit.Mvvm.Input;
using homeLearning.Model;
using homeLearning.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace homeLearning.viewModel
{
    class HomePageVM : BaseVM
    {
        public ICommand switchToPokemon { get; set; }
        public ICommand switchToSpell { get; set; }
        public ICommand startGame { get; set; }

        private string _myDatabase;

        private Login _matchingUser;

        public HomePageVM(string MyDatabase, Login MatchingUser)
        {
            switchToPokemon = new RelayCommand(pokemenonSwitch);
            switchToSpell = new RelayCommand(spellSwitch);
            startGame = new RelayCommand(gameStart);
            _matchingUser = MatchingUser;
            _myDatabase = MyDatabase;
        }

        public void pokemenonSwitch()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new PokemenonVM(_myDatabase, _matchingUser));
        }

        public void spellSwitch()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new SpellVM(_myDatabase, _matchingUser));
        }

        public void gameStart()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new PlayVM(_myDatabase, _matchingUser));
        }
    }
}
