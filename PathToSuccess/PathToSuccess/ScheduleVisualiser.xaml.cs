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
            ScheduleListView.Height = Height / 3;
            Edit.Width = Add.Width = Remove.Width = Width - ScheduleListView.Width - 20;
            Edit.Height = Add.Height = Remove.Height = Remove.Width / 2;
            MondayPanel.MaxWidth =
                TuesdayPanel.MaxWidth =
                    WednesdayPanel.MaxWidth =
                        ThursdayPanel.MaxWidth =
                            FridayPanel.MaxWidth =
                                SaturdayPanel.MaxWidth =
                                    SundayPanel.MaxWidth =
                                        MondayPanel.Width =
                                            TuesdayPanel.Width =
                                                WednesdayPanel.Width =
                                                    ThursdayPanel.Width =
                                                        FridayPanel.Width = SaturdayPanel.Width = SundayPanel.Width = ScheduleListView.Width / 7;



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

            ScheduleListView.SelectionChanged += SelectionChanged;

            ScheduleListView.SelectedItem = ScheduleListView.Items[0];
            var item = (ScheduleListView.SelectedItem as Models.Schedule);
            MondayFrom.Text = item.MondayInterval != null ? item.MondayInterval.BeginTime.ToString() : "";
            MondayTo.Text = item.MondayInterval != null ? item.MondayInterval.EndTime.ToString() : "";
            TuesdayFrom.Text = item.TuesdayInterval != null ? item.TuesdayInterval.BeginTime.ToString() : "";
            TuesdayTo.Text = item.TuesdayInterval != null ? item.TuesdayInterval.EndTime.ToString() : "";
            WednesdayFrom.Text = item.WednesdayInterval != null ? item.WednesdayInterval.BeginTime.ToString() : "";
            WednesdayTo.Text = item.WednesdayInterval != null ? item.WednesdayInterval.EndTime.ToString() : "";
            ThursdayFrom.Text = item.ThursdayInterval != null ? item.ThursdayInterval.BeginTime.ToString() : "";
            ThursdayTo.Text = item.ThursdayInterval != null ? item.ThursdayInterval.EndTime.ToString() : "";
            FridayFrom.Text = item.FridayInterval != null ? item.FridayInterval.BeginTime.ToString() : "";
            FridayTo.Text = item.FridayInterval != null ? item.FridayInterval.EndTime.ToString() : "";
            SaturdayFrom.Text = item.SaturdayInterval != null ? item.SaturdayInterval.BeginTime.ToString() : "";
            SaturdayTo.Text = item.SaturdayInterval != null ? item.SaturdayInterval.EndTime.ToString() : "";
            SundayFrom.Text = item.SundayInterval != null ? item.SundayInterval.BeginTime.ToString() : "";
            SundayTo.Text = item.SundayInterval != null ? item.SundayInterval.EndTime.ToString() : "";
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ScheduleListView.SelectedItem as Models.Schedule);
            MondayFrom.Text = item.MondayInterval != null ? item.MondayInterval.BeginTime.ToString() : "";
            MondayTo.Text = item.MondayInterval != null ? item.MondayInterval.EndTime.ToString() : "";
            TuesdayFrom.Text = item.TuesdayInterval != null ? item.TuesdayInterval.BeginTime.ToString() : "";
            TuesdayTo.Text = item.TuesdayInterval != null ? item.TuesdayInterval.EndTime.ToString() : "";
            WednesdayFrom.Text = item.WednesdayInterval != null ? item.WednesdayInterval.BeginTime.ToString() : "";
            WednesdayTo.Text = item.WednesdayInterval != null ? item.WednesdayInterval.EndTime.ToString() : "";
            ThursdayFrom.Text = item.ThursdayInterval != null ? item.ThursdayInterval.BeginTime.ToString() : "";
            ThursdayTo.Text = item.ThursdayInterval != null ? item.ThursdayInterval.EndTime.ToString() : "";
            FridayFrom.Text = item.FridayInterval != null ? item.FridayInterval.BeginTime.ToString() : "";
            FridayTo.Text = item.FridayInterval != null ? item.FridayInterval.EndTime.ToString() : "";
            SaturdayFrom.Text = item.SaturdayInterval!= null ? item.SaturdayInterval.BeginTime.ToString() : "";
            SaturdayTo.Text = item.SaturdayInterval != null ? item.SaturdayInterval.EndTime.ToString() : "";
            SundayFrom.Text = item.SundayInterval != null ? item.SundayInterval.BeginTime.ToString() : "";
            SundayTo.Text = item.SundayInterval!= null ? item.SundayInterval.EndTime.ToString() : "";
        }
    }
}
