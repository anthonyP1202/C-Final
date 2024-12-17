using CommunityToolkit.Mvvm.Input;
using homeLearning.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace homeLearning.viewModel
{
    class MainViewVM : BaseVM
    {
        private string _password = "Password";

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _username = "Username";

        public string User
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public ICommand LoginAttempt { get; set; }
        public ICommand CreateAccount{ get; set; }

        private string _myDatabase;
        public MainViewVM(string MyDatabase) 
        {
            LoginAttempt = new RelayCommand(HandleLoginAttempt);
            CreateAccount = new RelayCommand(CreateAcc);
            _myDatabase = MyDatabase;
        }

        public void HandleLoginAttempt()
        {
            if ((string.IsNullOrWhiteSpace(User) || string.IsNullOrWhiteSpace(Password)))
            {
                MessageBox.Show("password or Username invalide");
            }
            else
            {
                string Passwordhash = HashString(Password);
                ExerciceMonsterContext _dataContext = new ExerciceMonsterContext(_myDatabase);
                Login matchingUser;
                matchingUser = _dataContext.Logins.Where(x => x.Username == _username && x.PasswordHash == Passwordhash).Select(e => e).FirstOrDefault();
                if (matchingUser == null)
                {
                    MessageBox.Show("user not found");
                } else if (matchingUser.Id != 0)
                {
                    MainWindowVM.OnRequestVMChange?.Invoke(new HomePageVM(_myDatabase, matchingUser));
                }
            }
        }

        public void CreateAcc()
        {
            string Passwordhash = HashString(Password);
            ExerciceMonsterContext _dataContext = new ExerciceMonsterContext(_myDatabase);
            Login matchingUser;

            try
            {
                matchingUser = _dataContext.Logins.Where(x => x.Username == _username).Select(e => e).First();
                MessageBox.Show("username already picked");
            }
            catch
            {
                MessageBox.Show("creating");
                Login newLogin = new Login
                {
                    Username = User,
                    PasswordHash = Passwordhash
                };
                _dataContext.Logins.Add(newLogin);
                _dataContext.SaveChanges();
            }
        }

        public static string HashString(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
