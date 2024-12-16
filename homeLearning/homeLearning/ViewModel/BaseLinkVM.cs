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
using System.Windows.Controls;
using System.Windows.Input;

namespace homeLearning.viewModel
{
    public class BaseLinkVM:BaseVM
    {

        private string _database { get; set; }
        public string Mydatabase { get => _database;
            set {
                _database = value;
                OnPropertyChanged();
            }
        }
        public ICommand GenerateDB { get; set; }

        public BaseLinkVM()
        {
            GenerateDB = new RelayCommand(HandleRequestChangeViewCommand);
        }

        public void HandleRequestChangeViewCommand()
        {
            ChargeDB();
            MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM(Mydatabase));
        }

        public void ChargeDB()
        {

            MessageBox.Show(_database);

            ExerciceMonsterContext _dataContext = new ExerciceMonsterContext(_database);
         
            var charged = new ObservableCollection<Monster>(_dataContext.Monsters.ToList());

            if (charged.Any() == false)
            {

                Spell tackle = new Spell
                {
                    Name = "tackle",
                    Damage = 20,
                    Description = "charge at your opponent"
                };
                _dataContext.Spells.Add(tackle);

                Spell bite = new Spell
                {
                    Name = "bite",
                    Damage = 30,
                    Description = "bite your opponent"
                };
                _dataContext.Spells.Add(bite);

                Spell rock_throw = new Spell
                {
                    Name = "rock throw",
                    Damage = 30,
                    Description = "throw a rock at your opponent"
                };
                _dataContext.Spells.Add(rock_throw);

                Spell ice_fang = new Spell
                {
                    Name = "ice fang",
                    Damage = 35,
                    Description = "bite at your opponent but your teeth are cold"
                };
                _dataContext.Spells.Add(ice_fang);

                Spell spark = new Spell
                {
                    Name = "spark",
                    Damage = 30,
                    Description = "throw electricity at your opponent"
                };
                _dataContext.Spells.Add(spark);

                Spell slap = new Spell
                {
                    Name = "slap",
                    Damage = 20,
                    Description = "slap your opponent"
                };
                _dataContext.Spells.Add(slap);

                Monster nosepass = new Monster
                {
                    Health = 30,
                    Name = "nosepass",
                    Spells = new List<Spell> { tackle, rock_throw, spark, slap}
                };
                _dataContext.Monsters.Add(nosepass);

                Monster swinub = new Monster
                {
                    Health = 50,
                    Name = "swinub",
                    Spells = new List<Spell> { tackle, bite, ice_fang, slap }
                };
                _dataContext.Monsters.Add(swinub);

                Monster porygon = new Monster
                {
                    Health = 65,
                    Name = "porygon",
                    Spells = new List<Spell> { tackle, spark, slap, rock_throw }
                };
                _dataContext.Monsters.Add(porygon);
                _dataContext.SaveChanges();
                MessageBox.Show("dbcharged");

                
            }
        }
    }
}
