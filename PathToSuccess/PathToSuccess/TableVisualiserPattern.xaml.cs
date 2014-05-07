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
using PathToSuccess.DAL;

namespace PathToSuccess
{
    /// <summary>
    /// Interaction logic for TableVisualiserPattern.xaml
    /// </summary>
    public partial class TableVisualiserPattern : Window
    {
        public TableVisualiserPattern()
        {
            InitializeComponent();

            TableListView.Width = Width - 50;
            TableListView.Height = Height - 50;
           



            var objCollection = SqlRepository.Schedules; //input smthing
           // var obj = objCollection.Cast<Models.Schedule>().FirstOrDefault();
            var objType = typeof(Models.Schedule);
            var gridView = new GridView();

            TableListView.View = gridView;

            foreach (var prop in objType.GetProperties().Where(prop => prop.PropertyType.IsPrimitive 
                                                                       || prop.PropertyType == typeof(string) 
                                                                       || prop.PropertyType == typeof(DateTime)))
            {
                gridView.Columns.Add(new GridViewColumn() { Header = prop.Name, DisplayMemberBinding = new Binding(prop.Name) });
            }
            foreach (var o in objCollection.Cast<Models.Schedule>())
            {
                TableListView.Items.Add(o);
            }
        }
    }
}
