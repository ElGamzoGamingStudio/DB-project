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
using PathToSuccess.BL;
using PathToSuccess.DAL;
using PathToSuccess.Models;

namespace PathToSuccess
{
    /// <summary>
    /// Interaction logic for ScheduleVisualiser.xaml
    /// </summary>
    public partial class ScheduleVisualiser : Window
    {
        public ScheduleVisualiser()
        {
            InitializeComponent();

            ScheduleListView.Width = Width - 150;
            ScheduleListView.Height = Height - 50;
            Edit.Width = Add.Width = Remove.Width = Width - ScheduleListView.Width - 20;
            Edit.Height = Add.Height = Remove.Height = Remove.Width / 2;



            var objCollection = SqlRepository.Schedules; //input smthing
            var obj = objCollection.Cast<Models.Schedule>().FirstOrDefault();
            var objType = typeof(Models.Schedule);
            var gridView = new GridView();

            ScheduleListView.View = gridView;

            foreach (var prop in objType.GetProperties().Where(prop => prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string) || prop.PropertyType == typeof(DateTime)))
            {
                gridView.Columns.Add(new GridViewColumn() { Header = prop.Name, DisplayMemberBinding = new Binding(prop.Name) });
            }
            foreach (var o in objCollection.Cast<Models.Schedule>())
            {
                ScheduleListView.Items.Add(o);
            }

        }
    }
}
