using CommunityToolkit.Mvvm.Input;
using homeLearning.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace homeLearning.viewModel
{
    public class PlayVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }

        public ICommand StartAttack { get; set; }

        private int _score;
        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        private BitmapImage _path;
        public BitmapImage Pathed
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        //other/{0}_your.png

        private int _userMaxHealth;
        public int UserMaxHealth
        {
            get => _userMaxHealth;
            set
            {
                _userMaxHealth = UserMaxHealth;
            }
        }

        private int _ennemyMaxHealth;
        public int EnnemyMaxHealth
        {
            get => _ennemyMaxHealth;
            set
            {
                _ennemyMaxHealth = EnnemyMaxHealth;
            }
        }

        private ExerciceMonsterContext _exerciceMonsterContext;
        public List<Monster> _monsterList;
        public Monster _userMonster = new Monster();
        public Monster UserMonster
        {
            get => _userMonster;
            set => SetProperty(ref _userMonster, value);
        }
        private Monster _ennemyMonster = new Monster();
        public Monster EnnemyMonster
        {
            get => _ennemyMonster;
            set => SetProperty(ref _ennemyMonster, value);
        }

        private Login _user;
        private string _database;
        private Player _player;
        private Spell _spell1;
        
        public Spell Spell1
        {
            get => _spell1;
            set
            {
                _spell1 = Spell1;
            }
        }
        private Spell _spell2;

        public Spell Spell2
        {
            get => _spell2;
            set
            {
                _spell2 = Spell2;
            }
        }
        private Spell _spell3;

        public Spell Spell3
        {
            get => _spell3;
            set
            {
                _spell3 = Spell3;
            }
        }
        private Spell _spell4;

        public Spell Spell4
        {
            get => _spell4;
            set
            {
                _spell4 = Spell4;
            }
        }

        private List<Spell> _ennemySpell;

        public PlayVM(string MyDatabase, Login User, int score, int userMaxHealth, int ennemyMaxHealth, ExerciceMonsterContext exerciceContext, List<Monster> monsterList, Monster userMonster, Monster ennemyMonster, Spell spell1, Spell spell2, Spell spell3, Spell spell4, List<Spell> ennemySpell)
        {
            _score = score;
            _database = MyDatabase;
            _user = User;
            _userMaxHealth = userMaxHealth;
            _ennemyMaxHealth = ennemyMaxHealth;
            _exerciceMonsterContext = exerciceContext;
            _monsterList = monsterList;
            _userMonster = userMonster;
            _ennemyMonster = ennemyMonster;
            _spell1 = spell1;
            _spell2 = spell2;
            _spell3 = spell3;
            _spell4 = spell4;
            _ennemySpell = ennemySpell;
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
            StartAttack = new RelayCommand<int>(action);
        }
        public PlayVM(string MyDatabase, Login User)
        {
            
            _exerciceMonsterContext = new ExerciceMonsterContext(MyDatabase);
            _database = MyDatabase;
            _user = User;
            _monsterList = new List<Monster>(_exerciceMonsterContext.Monsters.Include(e=>e.Spells).ToList());
            try {
                _player = _exerciceMonsterContext.Players.Where(e => e.LoginId == User.Id).First();  
                } 
            catch (Exception ex) { 
            MessageBox.Show(ex.Message);
            }
            
            try
            {
                var placeholder = _exerciceMonsterContext.Monsters.Include(e=>e.Spells).First(e => e.Name == _player.Name);
                _userMonster.Name = placeholder.Name;
                _userMonster.Health = placeholder.Health;
                _userMonster.Spells = placeholder.Spells;
                _userMaxHealth = _userMonster.Health;
                var placeholderspell = new List<Spell>(_userMonster.Spells.ToList());
                _spell1 = placeholderspell[0];
                _spell2 = placeholderspell[1];
                _spell3 = placeholderspell[2];
                _spell4 = placeholderspell[3];
            } catch (Exception ex) {
                var placeholder= _exerciceMonsterContext.Monsters.Include(e => e.Spells).First();
                
                _userMonster.Name = placeholder.Name;
                _userMonster.Health = placeholder.Health;
                _userMonster.Spells = placeholder.Spells;
                _userMaxHealth = _userMonster.Health;
                _path = new BitmapImage(new Uri("other\\nosepass_your.png", UriKind.RelativeOrAbsolute));
                
                var placeholderspell = new List<Spell>(_userMonster.Spells.ToList());
                _spell1 = placeholderspell[0];
                _spell2 = placeholderspell[1];
                _spell3 = placeholderspell[2];
                _spell4 = placeholderspell[3];
            }
            pickNewEnnemy();
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
            StartAttack = new RelayCommand<int>(action);
        }

        public void pickNewEnnemy()
        {
            Random rnd = new Random();
            int num = rnd.Next(0, _monsterList.Count); 
            _ennemyMonster.Name = _monsterList[num].Name;
            _ennemyMonster.Health = (int)(_monsterList[num].Health * ((_score/100f)+1));
            _ennemyMonster.Spells = _monsterList[num].Spells;
            _ennemyMaxHealth = _ennemyMonster.Health;
            _ennemySpell = new List<Spell>(_ennemyMonster.Spells.ToList());
        }

        public void action(int attackDone)
        {
            _ennemyMonster.Health -= attackDone;
            if (_ennemyMonster.Health > 0)
            {
                Random rnd = new Random();
                int num = rnd.Next(1, 4);
                _userMonster.Health -= _ennemySpell[num].Damage;
            }else
            {
                _score += 1;
                MessageBox.Show("the ennemy pokemenon fainted");
                UserMonster.Health = UserMaxHealth;
                pickNewEnnemy();
            }
            if (_userMonster.Health <= 0)
            {
                MessageBox.Show("you lost");
                MainWindowVM.OnRequestVMChange?.Invoke(new HomePageVM(_database, _user));
            }
            else
            {
                MainWindowVM.OnRequestVMChange?.Invoke(new PlayVM(_database, _user, _score, _userMaxHealth, _ennemyMaxHealth, _exerciceMonsterContext, _monsterList, _userMonster, _ennemyMonster, _spell1, _spell2, _spell3, _spell4, _ennemySpell));
            }
        }

        public void HandleRequestChangeViewCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new HomePageVM(_database, _user));
        }
    }
}
