using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PathToSuccess
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
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
            var anim = new DoubleAnimation() { To = 150, Duration = new Duration(TimeSpan.FromSeconds(0.4)) };
            Fields.BeginAnimation(HeightProperty, anim);
            RegButton.Content = "Back";
            Login.Text = UserName.Text = string.Empty;
            Pass.Clear();
            ConfirmPass.Clear();
            RegButton.Click -= RegisterClick;
            RegButton.Click += BackClick;
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
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
