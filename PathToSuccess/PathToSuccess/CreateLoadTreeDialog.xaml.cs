using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using LinqToDB;

namespace PathToSuccess
{
    /// <summary>
    /// Interaction logic for CreateLoadTreeDiaolg.xaml
    /// </summary>
    public partial class CreateLoadTreeDialog : Window
    {
        public CreateLoadTreeDialog()
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
        }

        private void LeftClick(object sender, EventArgs e)
        {
            MarkAllUiElementsWithTags(); //to delete it later


            var but = sender as Button;
            var move = new TranslateTransform(0, 0);
            var anim = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                To = -but.Margin.Left-but.Width -5,
                AccelerationRatio = 0.5,
            };
            anim.Completed += PanelinoStackerino;
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

            
            
        }

        private void RightClick(object sender, EventArgs e)
        {
            MarkAllUiElementsWithTags(); //to delete it later

            var but = sender as Button;
            var move = new TranslateTransform(0, 0);
            var anim = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                To = but.Margin.Left + Load.Width + 5,
                AccelerationRatio = 0.5,
            };
            anim.Completed += PanelinoStackerino;
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

            
        }

        private void BackClick(object sender, EventArgs e)
        {
            var anim = new DoubleAnimation()
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(.3)),
            };
            Storyboard.SetTarget(anim, BackSecond); 
            Storyboard.SetTargetProperty(anim, new PropertyPath(OpacityProperty));
            var storyboard = new Storyboard();
            storyboard.Children.Add(anim);
            storyboard.Completed += RollBack;
            storyboard.Begin();
            
        }

        private void NextClick(object sender, EventArgs e)
        {
            var panel = (StackPanel)FirstStep.Children.Cast<UIElement>().FirstOrDefault(x => x is StackPanel);
            var anim = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(1)), FillBehavior.Stop);
            panel.BeginAnimation(OpacityProperty, anim);
            
        }

        private void RollBack(object sender, EventArgs e)
        {
            //really sorry costelino de la bidlo
            RemoveAllNonPreviousItems();
            SetUpButtons(true);
            //Back.SetValue(OpacityProperty, 1.0);  //ITS NOT FUCKING WORKING 1.0 or 1 or 100.0 or Integer.1dwfkdlkwlgwddglg!!!!!!!
            var anim = new DoubleAnimation()
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0)),
            };
            Storyboard.SetTarget(anim, BackSecond);
            Storyboard.SetTargetProperty(anim, new PropertyPath(OpacityProperty));
            var storyboard = new Storyboard();
            storyboard.Children.Add(anim);
            storyboard.Begin();
            
            
        }

        private void PanelinoStackerino(object sender, EventArgs e)
        {
            
            var rectangelino = new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.ForestGreen),
                Width = 500,
                Height = 500
            };
            var namePanel = new StackPanel()
            {
                Width = Width/4,
                Height = Height/4,
                Name = "TreeName",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            var nameText = new TextBlock()
            {
                Text = "Choose your destiny",
                Name = "nameText",
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            var inputText = new TextBox();
            namePanel.Children.Add(nameText);
            namePanel.Children.Add(inputText);
            FirstStep.Children.Add(namePanel);

            var next = new Button()
            {
                Name = "Next", 
                Content = "N", 
                Width = 50, 
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            next.Click += NextClick;
            FirstStep.Children.Add(next);

        }

        private void MarkAllUiElementsWithTags()
        {
            foreach (FrameworkElement child in this.FirstStep.Children)
            {
                child.Tag = "previous";
            }
        }

        private void RemoveAllNonPreviousItems()
        {
            for (int index = FirstStep.Children.Count - 1; index >= 0; index--)
            {
                var child = (FrameworkElement)this.FirstStep.Children[index];
                if ((string) child.Tag != "previous")
                    FirstStep.Children.RemoveAt(index);
            }
        }
    }
}
