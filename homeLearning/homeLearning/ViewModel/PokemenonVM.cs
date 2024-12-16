using CommunityToolkit.Mvvm.Input;
using homeLearning.Model;
using homeLearning.viewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace homeLearning.viewModel
{
    class PokemenonVM : BaseVM
    {
        public ICommand ViewDetail { get; set; }

        public ICommand SelectMonsterCommand { get; set; }

        public ICommand RequestChangeViewComand { get; set; }

        private ObservableCollection<Monster> _monster;
        public ObservableCollection<Monster> MonsterList { 
            get => _monster;
            set {
                _monster = value;
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
        public PokemenonVM(string MyDatabase, Login MatchingUser)
        {
            _database = MyDatabase;
            _matchingUser = MatchingUser;
            ExerciceMonsterContext _data = new ExerciceMonsterContext(_database);
            MonsterList = new ObservableCollection<Monster>(_data.Monsters.ToList());
            ViewDetail = new RelayCommand<Monster>(HandleRequestChangeViewCommand);
            RequestChangeViewComand = new RelayCommand(ThisConfusing);
            SelectMonsterCommand = new RelayCommand<Monster>(HandleSelectMonsterCommand);
        }

        public void HandleRequestChangeViewCommand(Monster selectedMonster)
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new DetailedPokemenon(_database, _matchingUser, selectedMonster));
        }

        public void ThisConfusing()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new HomePageVM(_database, _matchingUser));
        }

        private void HandleSelectMonsterCommand(Monster selectedMonster)
        {
            
            Player existingMonster; 
            ExerciceMonsterContext _data = new ExerciceMonsterContext(_database);
            try
            {
                _data.Monsters.Attach(selectedMonster);
                existingMonster = _data.Players.Where(x => x.LoginId == _matchingUser.Id).Select(e => e).First();
                if (existingMonster != null)
                {
                    _data.Entry(existingMonster).Property(e => e.Name).CurrentValue = selectedMonster.Name;
                    _data.SaveChanges();
                }
                else { 
                Player player = new Player
                {
                LoginId = _matchingUser.Id,
                Name = selectedMonster.Name,
                };
                    _data.Players.Add(player);
                    _data.SaveChanges();
                }
            }
            catch (Exception ex) {
                Player player = new Player
                {
                    LoginId = _matchingUser.Id,
                    Name = selectedMonster.Name,
                    
                };
                _data.Monsters.Attach(selectedMonster);
                _data.Players.Add(player);
                _data.SaveChanges();
            }

            MessageBox.Show($"You selected: {selectedMonster.Name}");
        }
    }
}
