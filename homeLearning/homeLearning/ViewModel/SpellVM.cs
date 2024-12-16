using CommunityToolkit.Mvvm.Input;
using homeLearning.Model;
using homeLearning.viewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace homeLearning.viewModel
{
    public class SpellVM : BaseVM
    {
        public ICommand RequestChangeViewComand { get; set; }
     
        public ICommand ViewDetail { get; set; }

        public ICommand ViewLinkedPokemenon { get; set; }

        private ObservableCollection<Spell> _spell;
        public ObservableCollection<Spell> SpellList
        {
            get => _spell;
            set
            {
                _spell = value;
            }
        }

        private Login _matchingUser;

        private string _database;
        public string Database
        {
            get => _database;
            set
            {
                _database = Database;
            }
        }
        public SpellVM(string MyDatabase, Login MatchingUser)
        {
            _database = MyDatabase;
            _matchingUser = MatchingUser;
            ExerciceMonsterContext _data = new ExerciceMonsterContext(_database);
            SpellList = new ObservableCollection<Spell>(_data.Spells.ToList());
            RequestChangeViewComand = new RelayCommand(ThisConfusing);
            ViewDetail = new RelayCommand<Spell>(HandleRequestChangeViewCommand);
            ViewLinkedPokemenon = new RelayCommand<Spell>(LinkedPokemenon);
        }

        public void ThisConfusing()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new HomePageVM(_database, _matchingUser));
        }

        public void HandleRequestChangeViewCommand(Spell selectedSpell)
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new detailedSpell(_database, _matchingUser, selectedSpell));
        }

        public void LinkedPokemenon(Spell selectedSpell)
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new PokemonWithSpellVM(_database, _matchingUser, selectedSpell));
        }
    }
}
