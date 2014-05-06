using System;
using System.Windows;
using System.Windows.Media.Animation;
using Npgsql;
using NpgsqlTypes;
using PathToSuccess.Models;

namespace PathToSuccess
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public bool? RightPass;
        public Models.User RegUser;
        public LoginWindow()
        {
            InitializeComponent();
            RightPass = null;
        }

        private void ShowFields(object sender, EventArgs e)
        {
            Fields.IsEnabled = true;
            var anim = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(1)));
            Fields.BeginAnimation(OpacityProperty,anim);
            LogButton.IsEnabled = RegButton.IsEnabled = true;
            LogButton.BeginAnimation(OpacityProperty,anim);
            RegButton.BeginAnimation(OpacityProperty,anim);
            Birth.DisplayDateEnd = DateTime.Today;
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            S1.IsEnabled = S2.IsEnabled = S3.IsEnabled = true;
            var anim = new DoubleAnimation { To = 150, Duration = new Duration(TimeSpan.FromSeconds(0.4)) };
            Fields.BeginAnimation(HeightProperty, anim);
            RegButton.Content = "Back";
            Login.Text = UserName.Text = string.Empty;
            Pass.Clear();
            ConfirmPass.Clear();
            RegButton.Click -= RegisterClick;
            RegButton.Click += BackClick;
            LogButton.Click -= LoginClick;
            LogButton.Click += OkClick;
            LogButton.Content = "OK";
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            
            S1.IsEnabled = S2.IsEnabled = S3.IsEnabled = false;
            var anim = new DoubleAnimation(){ To = 60,Duration = new Duration(TimeSpan.FromSeconds(0.4))};
            Fields.BeginAnimation(HeightProperty,anim);
            Login.Text = UserName.Text = string.Empty;
            Pass.Clear();
            ConfirmPass.Clear();
            RegButton.Content = "Register";
            RegButton.Click -= BackClick;
            RegButton.Click += RegisterClick;
            LogButton.Click -= OkClick;
            LogButton.Click += LoginClick;
            LogButton.Content = "Login";
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (Login.Text.Length < 3 || Pass.Password.Length < 6 || ConfirmPass.Password.Length < 6 || UserName.Text.Length < 3 || Birth.SelectedDate == null)
            {
                MessageBox.Show("Not all fields have enough chars", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Pass.Password != ConfirmPass.Password)
            {
                MessageBox.Show("Passwords are not equals", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RegUser = new User(Login.Text, UserName.Text, (DateTime) Birth.SelectedDate, Pass.Password, DateTime.Today);
            BL.Authentication.Register(RegUser);
            RightPass = true;
            DialogResult = true;
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            if (Login.Text.Length < 3 || Pass.Password.Length < 6)
            {
                MessageBox.Show("Login must have 3 or more chars and password - 6 or more chars","Warning",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
            if (BL.Authentication.Authenticate(Login.Text, Pass.Password))
            {
                RightPass = true;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Wrong login or password", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
