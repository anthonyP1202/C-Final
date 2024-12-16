using CommunityToolkit.Mvvm.Input;
using homeLearning.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace homeLearning.viewModel
{
    class DetailedPokemenon : BaseVM
    {
        public ICommand shitGoBack { get; set; }
        public ICommand SelectPokemenon { get; set; }

        private string _myDatabase;
        private Login _matchingUser;
        private Monster _monster;
        private string _spell1;
        private string _spell2;
        private string _spell3;
        private string _spell4;

        public Monster theMonster
        {
            get => _monster;
            set
            {
                _monster = value;
            }
        }

        public string Spell1
        {
            get => _spell1;
            set
            {
                _spell1 = value;
            }
        }

        public string Spell2
        {
            get => _spell2;
            set
            {
                _spell2 = value;
            }
        }

        public string Spell3
        {
            get => _spell3;
            set
            {
                _spell3 = value;
            }
        }

        public string Spell4
        {
            get => _spell4;
            set
            {
                _spell4 = value;
            }
        }

        public DetailedPokemenon(string MyDatabase, Login MatchingUser, Monster SlectedMonster) {
            shitGoBack = new RelayCommand(GoBack);
            SelectPokemenon = new RelayCommand(SelectPok);
            _myDatabase = MyDatabase;
            _matchingUser = MatchingUser;
            //_monster = SlectedMonster;
            var data = new ExerciceMonsterContext(MyDatabase);
            _monster = data.Monsters.Include(m => m.Spells).First(e => e.Id == SlectedMonster.Id);
            List<Spell> SpellList = (List<Spell>)_monster.Spells;
            Spell1 = SpellList[0].Name;
            Spell2 = SpellList[1].Name;
            Spell3 = SpellList[2].Name;
            Spell4 = SpellList[3].Name;
        }

        public void GoBack()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new PokemenonVM(_myDatabase, _matchingUser));
        }

        public void SelectPok()
        {
            Player existingMonster;
            ExerciceMonsterContext _data = new ExerciceMonsterContext(_myDatabase);
            try
            {
                _data.Monsters.Attach(_monster);
                existingMonster = _data.Players.Where(x => x.LoginId == _matchingUser.Id).Select(e => e).First();
                if (existingMonster != null)
                {
                    _data.Entry(existingMonster).Property(e => e.Name).CurrentValue = _monster.Name;
                    _data.SaveChanges();
                }
                else
                {
                    Player player = new Player
                    {
                        LoginId = _matchingUser.Id,
                        Name = _monster.Name,
                    };
                    _data.Players.Add(player);
                    _data.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Player player = new Player
                {
                    LoginId = _matchingUser.Id,
                    Name = _monster.Name,

                };
                _data.Monsters.Attach(_monster);
                _data.Players.Add(player);
                _data.SaveChanges();
            }

            MessageBox.Show($"You selected: {_monster.Name}");
        }
    }
}
