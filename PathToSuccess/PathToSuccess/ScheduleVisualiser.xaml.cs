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
using LinqToDB;
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
            ScheduleListView.Height = Height /2;
            Edit.Width = Add.Width = Remove.Width = Width - ScheduleListView.Width - 20;
            Edit.Height = Add.Height = Remove.Height = Height / 3 - 15;
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
            foreach (var o in objCollection.Cast<Models.Schedule>().ToList())
            {
                ScheduleListView.Items.Add(o);
            }


            ScheduleListView.SelectionChanged += SelectionChanged;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ScheduleListView.SelectedItem as Models.Schedule);
            if (item == null) return;
            MondayFrom.Text = item.MondayInterval != null ? item.MondayInterval.BeginTime.Hour.ToString() : "NULL";
            MondayTo.Text = item.MondayInterval != null ? item.MondayInterval.EndTime.Hour.ToString() : "NULL";
            TuesdayFrom.Text = item.TuesdayInterval != null ? item.TuesdayInterval.BeginTime.Hour.ToString() : "NULL";
            TuesdayTo.Text = item.TuesdayInterval != null ? item.TuesdayInterval.EndTime.Hour.ToString() : "NULL";
            WednesdayFrom.Text = item.WednesdayInterval != null ? item.WednesdayInterval.BeginTime.Hour.ToString() : "NULL";
            WednesdayTo.Text = item.WednesdayInterval != null ? item.WednesdayInterval.EndTime.Hour.ToString() : "NULL";
            ThursdayFrom.Text = item.ThursdayInterval != null ? item.ThursdayInterval.BeginTime.Hour.ToString() : "NULL";
            ThursdayTo.Text = item.ThursdayInterval != null ? item.ThursdayInterval.EndTime.Hour.ToString() : "NULL";
            FridayFrom.Text = item.FridayInterval != null ? item.FridayInterval.BeginTime.Hour.ToString() : "NULL";
            FridayTo.Text = item.FridayInterval != null ? item.FridayInterval.EndTime.Hour.ToString() : "NULL";
            SaturdayFrom.Text = item.SaturdayInterval != null ? item.SaturdayInterval.BeginTime.Hour.ToString() : "NULL";
            SaturdayTo.Text = item.SaturdayInterval != null ? item.SaturdayInterval.EndTime.Hour.ToString() : "NULL";
            SundayFrom.Text = item.SundayInterval != null ? item.SundayInterval.BeginTime.Hour.ToString() : "NULL";
            SundayTo.Text = item.SundayInterval != null ? item.SundayInterval.EndTime.Hour.ToString() : "NULL";
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            if (ScheduleListView.SelectedItem == null) return;
            SqlRepository.Schedules.Remove(ScheduleListView.SelectedItem as Models.Schedule);
            SqlRepository.Save();
            ScheduleListView.Items.Clear();
            foreach (var o in SqlRepository.Schedules.Cast<Models.Schedule>().ToList())
            {
                ScheduleListView.Items.Add(o);
            }
            Interval.RemoveTrash();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            var intervals = SqlRepository.Intervals;
            var schedules = SqlRepository.Schedules;

            int numberMondayFrom;
            int numberMondayTo;
            Interval monday;
            if (IsOk(MondayFrom.Text, out numberMondayFrom) && IsOk(MondayTo.Text, out numberMondayTo))
            {
                monday = Interval.CreateInterval(new DateTime(300, 5, 3, numberMondayFrom, 0, 0),
                   new DateTime(300, 5, 3, numberMondayTo, 0, 0));
                intervals.Add(monday);
            }
            else
            {
                monday = (Interval)intervals.Find(-1);
            }


            int numberTuesdayFrom;
            int numberTuesdayTo;
            Interval tuesday;
            if (IsOk(TuesdayFrom.Text, out numberTuesdayFrom) && IsOk(TuesdayTo.Text, out numberTuesdayTo))
            {
                tuesday = Interval.CreateInterval(new DateTime(300, 5, 3, numberTuesdayFrom, 0, 0),
                    new DateTime(300, 2, 3, numberTuesdayTo, 0, 0));
                intervals.Add(tuesday);
            }
            else
            {
                tuesday = (Interval)intervals.Find(-1);
            }

            int numberWednesdayFrom;
            int numberWednesdayTo;
            Interval wednesday;
            if (IsOk(WednesdayFrom.Text, out numberWednesdayFrom) && IsOk(WednesdayTo.Text, out numberWednesdayTo))
            {
                wednesday = Interval.CreateInterval(new DateTime(300, 5, 3, numberWednesdayFrom, 0, 0),
                    new DateTime(300, 5, 3, numberWednesdayTo, 0, 0));
                intervals.Add(wednesday);
            }
            else
            {
                wednesday = (Interval)intervals.Find(-1);
            }

            int numberThursdayFrom;
            int numberThursdayTo;
            Interval thursday;
            if (IsOk(ThursdayFrom.Text, out numberThursdayFrom) && IsOk(ThursdayTo.Text, out numberThursdayTo))
            {
                thursday = Interval.CreateInterval(new DateTime(300, 5, 3, numberThursdayFrom, 0, 0),
                    new DateTime(300, 5, 3, numberThursdayTo, 0, 0));
                intervals.Add(thursday);
            }
            else
            {
                thursday = (Interval)intervals.Find(-1);
            }

            int numberFridayFrom;
            int numberFridayTo;
            Interval friday;
            if (IsOk(FridayFrom.Text, out numberFridayFrom) && IsOk(FridayTo.Text, out numberFridayTo))
            {
                friday = Interval.CreateInterval(new DateTime(300, 5, 3, numberThursdayFrom, 0, 0),
                    new DateTime(300, 5, 3, numberFridayTo, 0, 0));
                intervals.Add(friday);
            }
            else
            {
                friday = (Interval)intervals.Find(-1);
            }

            int numberSaturdayFrom;
            int numberSaturdayTo;
            Interval saturday;
            if (IsOk(SaturdayFrom.Text, out numberSaturdayFrom) && IsOk(SaturdayTo.Text, out numberSaturdayTo))
            {
                saturday = Interval.CreateInterval(new DateTime(300, 5, 3, numberSaturdayFrom, 0, 0),
                    new DateTime(300, 5, 3, numberSaturdayTo, 0, 0));
                intervals.Add(saturday);
            }
            else
            {
                saturday = (Interval)intervals.Find(-1);
            }

            int numberSundayFrom;
            int numberSundayTo;
            Interval sunday;
            if (IsOk(FridayFrom.Text, out numberSundayFrom) && IsOk(FridayTo.Text, out numberSundayTo))
            {
                sunday = Interval.CreateInterval(new DateTime(300, 5, 3, numberSundayFrom, 0, 0),
                    new DateTime(300, 5, 3, numberSundayTo, 0, 0));
                intervals.Add(sunday);
            }
            else
            {
                sunday = (Interval)intervals.Find(-1);
            }

            if (UserApproovedCheckBox.IsChecked != null && (bool)UserApproovedCheckBox.IsChecked)
            {
                monday.EndTime = Interval.PIOS;
                tuesday.EndTime = Interval.PIOS;
                wednesday.EndTime = Interval.PIOS;
                thursday.EndTime = Interval.PIOS;
                friday.EndTime = Interval.PIOS;
                saturday.EndTime = Interval.PIOS;
                sunday.EndTime = Interval.PIOS;
            }

<<<<<<< HEAD
            var creation = Models.Schedule.CreateSchedule(monday, tuesday, wednesday, thursday, friday, saturday, sunday, ScheduleNameBox.Text);
=======
            var creation = Models.Schedule.CreateSchedule(monday, tuesday, wednesday, thursday, friday, saturday, sunday, ScheduleNameBox.Text.Trim());
>>>>>>> fe5a8672320fce067ab6d776e974bbe69c224b87
            schedules.Add(creation);
            SqlRepository.Save();
            Interval.RemoveTrash();
            UpdateTableView();
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            var item = (ScheduleListView.SelectedItem as Models.Schedule);
            if (item == null) return;
            var intervals = SqlRepository.Intervals;
            var schedules = SqlRepository.Schedules;

            int numberMondayFrom;
            int numberMondayTo;
            Interval monday;
            if (IsOk(MondayFrom.Text, out numberMondayFrom) && IsOk(MondayTo.Text, out numberMondayTo))
            {
                monday = Interval.CreateInterval(new DateTime(300, 5, 3, numberMondayFrom, 0, 0),
                   new DateTime(300, 5, 3, numberMondayTo, 0, 0));
                intervals.Add(monday);
            }
            else
            {
                monday = (Interval)intervals.Find(-1);
            }


            int numberTuesdayFrom;
            int numberTuesdayTo;
            Interval tuesday;
            if (IsOk(TuesdayFrom.Text, out numberTuesdayFrom) && IsOk(TuesdayTo.Text, out numberTuesdayTo))
            {
                tuesday = Interval.CreateInterval(new DateTime(300, 5, 3, numberTuesdayFrom, 0, 0),
                    new DateTime(300, 2, 3, numberTuesdayTo, 0, 0));
                intervals.Add(tuesday);
            }
            else
            {
                tuesday = (Interval)intervals.Find(-1);
            }

            int numberWednesdayFrom;
            int numberWednesdayTo;
            Interval wednesday;
            if (IsOk(WednesdayFrom.Text, out numberWednesdayFrom) && IsOk(WednesdayTo.Text, out numberWednesdayTo))
            {
                wednesday = Interval.CreateInterval(new DateTime(300, 5, 3, numberWednesdayFrom, 0, 0),
                    new DateTime(300, 5, 3, numberWednesdayTo, 0, 0));
                intervals.Add(wednesday);
            }
            else
            {
                wednesday = (Interval)intervals.Find(-1);
            }

            int numberThursdayFrom;
            int numberThursdayTo;
            Interval thursday;
            if (IsOk(ThursdayFrom.Text, out numberThursdayFrom) && IsOk(ThursdayTo.Text, out numberThursdayTo))
            {
                thursday = Interval.CreateInterval(new DateTime(300, 5, 3, numberThursdayFrom, 0, 0),
                    new DateTime(300, 5, 3, numberThursdayTo, 0, 0));
                intervals.Add(thursday);
            }
            else
            {
                thursday = (Interval)intervals.Find(-1);
            }

            int numberFridayFrom;
            int numberFridayTo;
            Interval friday;
            if (IsOk(FridayFrom.Text, out numberFridayFrom) && IsOk(FridayTo.Text, out numberFridayTo))
            {
                friday = Interval.CreateInterval(new DateTime(300, 5, 3, numberThursdayFrom, 0, 0),
                    new DateTime(300, 5, 3, numberFridayTo, 0, 0));
                intervals.Add(friday);
            }
            else
            {
                friday = (Interval)intervals.Find(-1);
            }

            int numberSaturdayFrom;
            int numberSaturdayTo;
            Interval saturday;
            if (IsOk(SaturdayFrom.Text, out numberSaturdayFrom) && IsOk(SaturdayTo.Text, out numberSaturdayTo))
            {
                saturday = Interval.CreateInterval(new DateTime(300, 5, 3, numberSaturdayFrom, 0, 0),
                    new DateTime(300, 5, 3, numberSaturdayTo, 0, 0));
                intervals.Add(saturday);
            }
            else
            {
                saturday = (Interval)intervals.Find(-1);
            }

            int numberSundayFrom;
            int numberSundayTo;
            Interval sunday;
            if (IsOk(FridayFrom.Text, out numberSundayFrom) && IsOk(FridayTo.Text, out numberSundayTo))
            {
                sunday = Interval.CreateInterval(new DateTime(300, 5, 3, numberSundayFrom, 0, 0),
                    new DateTime(300, 5, 3, numberSundayTo, 0, 0));
                intervals.Add(sunday);
            }
            else
            {
                sunday = (Interval)intervals.Find(-1);
            }

            if (UserApproovedCheckBox.IsChecked != null && (bool) UserApproovedCheckBox.IsChecked)
            {
                monday.EndTime = Interval.PIOS;
                tuesday.EndTime = Interval.PIOS;
                wednesday.EndTime = Interval.PIOS;
                thursday.EndTime = Interval.PIOS;
                friday.EndTime = Interval.PIOS;
                saturday.EndTime = Interval.PIOS;
                sunday.EndTime = Interval.PIOS;
            }
            item.MondayInterval = monday;
            item.MondayIntervalId = monday.Id;
            item.TuesdayInterval = tuesday;
            item.TuesdayIntervalId = tuesday.Id;
            item.WednesdayInterval = wednesday;
            item.WednesdayIntervalId = wednesday.Id;
            item.ThursdayInterval = thursday;
            item.ThursdayIntervalId = thursday.Id;
            item.FridayInterval = friday;
            item.FridayIntervalId = friday.Id;
            item.SaturdayInterval = saturday;
            item.SaturdayIntervalId = saturday.Id;
            item.SundayInterval = sunday;
            item.SundayIntervalId = sunday.Id;

            SqlRepository.Save();
            Interval.RemoveTrash();
            UpdateTableView();
        }

        private bool IsOk(string intervalToCheck, out int number)
        {
            return Int32.TryParse(intervalToCheck.Trim(), out number) && number >= 0 && number <= 24;
        }

        private void UpdateTableView()
        {
            ScheduleListView.Items.Clear();
            foreach (var o in SqlRepository.Schedules.Cast<Models.Schedule>().ToList())
            {
                ScheduleListView.Items.Add(o);
            }
        }
    }
}
