using CommunityToolkit.Mvvm.Input;
using homeLearning.Model;
using homeLearning.viewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace homeLearning.viewModel
{
    class detailedSpell : BaseVM
    {
        public ICommand shitGoBack { get; set; }

        private string _myDatabase;
        private Login _matchingUser;
        private Spell _spell;

        public Spell theSpell
        {
            get => _spell;
            set
            {
                _spell = value;
            }
        }

        public detailedSpell(string MyDatabase, Login MatchingUser, Spell SelectedSpell)
        {
            shitGoBack = new RelayCommand(GoBack);
            //SelectPokemenon = new RelayCommand(SelectPok);
            _myDatabase = MyDatabase;
            _matchingUser = MatchingUser;
            _spell = SelectedSpell;
        }

        public void GoBack()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new SpellVM(_myDatabase, _matchingUser));
        }
    }
}
