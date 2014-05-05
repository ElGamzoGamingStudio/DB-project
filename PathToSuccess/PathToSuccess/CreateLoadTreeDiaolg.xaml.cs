using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for CreateLoadTreeDiaolg.xaml
    /// </summary>
    public partial class CreateLoadTreeDiaolg : Window
    {
        public CreateLoadTreeDiaolg()
        {
            InitializeComponent();
            SetUpButtons();
        }

        private void Update(object sender, SizeChangedEventArgs e)
        {
            double widthDifference, heightDifference;
            if (WindowState == WindowState.Maximized)
            {
                widthDifference = SystemParameters.PrimaryScreenWidth - MinWidth;
                heightDifference = SystemParameters.PrimaryScreenHeight - MinHeight - 40;
            }
            else
            {
                widthDifference = Width - MinWidth;
                heightDifference = Height - MinHeight;
            }

        }

        private void SetUpButtons(bool rollBack = false)
        {
            if (!rollBack)
            {
                Create.Width = Width / 2 - Width / 16;
                Create.Height = Height / 4;
                Create.HorizontalAlignment = HorizontalAlignment.Left;
                Create.VerticalAlignment = VerticalAlignment.Center;
                Create.Margin = new Thickness(5, 0, 0, 0);
                Create.RenderTransform = null;

                Load.Width = Width/2 - Width/16;
                Load.Height = Height/4;
                Load.HorizontalAlignment = HorizontalAlignment.Right;
                Load.VerticalAlignment = VerticalAlignment.Center;
                Load.Margin = new Thickness(0, 0, 5, 0);
                Load.RenderTransform = null;
            }
            else
            {
                var moveC = new TranslateTransform(-Create.Margin.Left - Create.Width - 5, 0);
                var animC = new DoubleAnimation()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    To = Create.Margin.Left,
                    AccelerationRatio = 0.5,
                };
                Create.RenderTransform = moveC;
                moveC.BeginAnimation(TranslateTransform.XProperty, animC);

                var moveL = new TranslateTransform(Load.Margin.Left + Load.Width + 5, 0);
                var animL = new DoubleAnimation()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    To = Load.Margin.Left,
                    AccelerationRatio = 0.5,
                };
                Load.RenderTransform = moveL;
                moveL.BeginAnimation(TranslateTransform.XProperty, animL);
            }
            Back.RenderTransform = null;
            Back.Visibility = Visibility.Hidden;
        }

        private void LeftClick(object sender, EventArgs e)
        {
            var but = sender as Button;
            var move = new TranslateTransform(0, 0);
            var anim = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                To = -but.Margin.Left-but.Width -5,
                AccelerationRatio = 0.5,
            };
            but.RenderTransform = move;
            move.BeginAnimation(TranslateTransform.XProperty, anim);

            var moveOther = new TranslateTransform(0, 0);
            var animOther = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.7)),
                To = Load.Margin.Left + Load.Width + 5,
                AccelerationRatio = 0.5,
            };
            Load.RenderTransform = moveOther;
            moveOther.BeginAnimation(TranslateTransform.XProperty, animOther);

            Back.Visibility = Visibility.Visible;
        }

        private void RightClick(object sender, RoutedEventArgs e)
        {
            var but = sender as Button;
            var move = new TranslateTransform(0, 0);
            var anim = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                To = but.Margin.Left + Load.Width + 5,
                AccelerationRatio = 0.5,
            };
            but.RenderTransform = move;
            move.BeginAnimation(TranslateTransform.XProperty, anim);

            var moveOther = new TranslateTransform(0, 0);
            var animOther = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.7)),
                To = -Create.Width - 5,
                AccelerationRatio = 0.5,
            };
            Create.RenderTransform = moveOther;
            moveOther.BeginAnimation(TranslateTransform.XProperty, animOther);

            Back.Visibility = Visibility.Visible;
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            var anim = new DoubleAnimation()
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(.3)),
            };
            Storyboard.SetTarget(anim, Back); 
            Storyboard.SetTargetProperty(anim, new PropertyPath(OpacityProperty));
            var storyboard = new Storyboard();
            storyboard.Children.Add(anim);
            storyboard.Completed += SORRY_FOR_THREAD_GUYS;
            storyboard.Begin();
            
        }

        private void SORRY_FOR_THREAD_GUYS(object sender, EventArgs e)
        {
            SetUpButtons(true);
            //Back.SetValue(OpacityProperty, 1.0);  //ITS NOT FUCKING WORKING 1.0 or 1 or 100.0 or Integer.1dwfkdlkwlgwddglg!!!!!!!
            var anim = new DoubleAnimation()
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0)),
            };
            Storyboard.SetTarget(anim, Back);
            Storyboard.SetTargetProperty(anim, new PropertyPath(OpacityProperty));
            var storyboard = new Storyboard();
            storyboard.Children.Add(anim);
            storyboard.Begin();
            //really sorry
        }
    }
}
