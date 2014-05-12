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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PathToSuccess
{
    /// <summary>
    /// Interaction logic for QueryDialog.xaml
    /// </summary>
    public partial class QueryDialog : Window
    {
        public QueryDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string result = PathToSuccess.BL.RawSqlPusher.PushQuery(QueryTextBox.Text);
            QueryTextBox.Text = result;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            QueryTextBox.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
