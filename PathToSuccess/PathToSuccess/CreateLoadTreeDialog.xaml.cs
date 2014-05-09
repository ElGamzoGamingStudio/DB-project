﻿using System;
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
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    To = Create.Margin.Left,
                    DecelerationRatio = 0.7,
                };
                Create.RenderTransform = moveC;
                moveC.BeginAnimation(TranslateTransform.XProperty, animC);

                var moveL = new TranslateTransform(Load.Margin.Left + Load.Width + 5, 0);
                var animL = new DoubleAnimation()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    To = Load.Margin.Left,
                    DecelerationRatio = 0.7,
                };
                Load.RenderTransform = moveL;
                moveL.BeginAnimation(TranslateTransform.XProperty, animL);
            }
        }

        public void CreateClick(object sender, EventArgs e)
        {
           
            var but = sender as Button;
            var move = new TranslateTransform(0, 0);
            var anim = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.7)),
                To = -but.Margin.Left-but.Width -5,
                AccelerationRatio = 0.5,
            };
            anim.Completed += MoveToCreate2;
            but.RenderTransform = move;
            move.BeginAnimation(TranslateTransform.XProperty, anim);

            var moveOther = new TranslateTransform(0, 0);
            var animOther = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                To = Load.Margin.Left + Load.Width + 5,
                AccelerationRatio = 0.5,
            };
            Load.RenderTransform = moveOther;
            moveOther.BeginAnimation(TranslateTransform.XProperty, animOther);

            
            
        }

        public void LoadClick(object sender, EventArgs e)
        {
            //MarkAllUiElementsWithTags(); //to delete it later

            var but = sender as Button;
            var move = new TranslateTransform(0, 0);
            var anim = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.7)),
                To = but.Margin.Left + Load.Width + 5,
                AccelerationRatio = 0.5,
            };
            anim.Completed += MoveToLoad;
            but.RenderTransform = move;
            move.BeginAnimation(TranslateTransform.XProperty, anim);

            var moveOther = new TranslateTransform(0, 0);
            var animOther = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                To = -Create.Width - 5,
                AccelerationRatio = 0.5,
            };
            Create.RenderTransform = moveOther;
            moveOther.BeginAnimation(TranslateTransform.XProperty, animOther);

            
        }
        
        private void RollBackFirstTab(object sender, EventArgs e)
        {
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
            
            TreeNamePanel.Width = Width/2;
            TreeNamePanel.Height = Height/2;
        }

        #region MOVE TO TAB
        private void MoveToCreate2(object sender, EventArgs e)
        {
            StepChanger.SelectedItem = SecondTabItem;
        }

        private void MoveToCreate3(object sender, EventArgs e)
        {
            StepChanger.SelectedItem = TreeTabItem;
        }

        private void MoveToLoad(object sender, EventArgs e)
        {
            StepChanger.SelectedItem = LoadTabItem;
        }
        #endregion

        private void BackToChoosing(object sender, RoutedEventArgs e)
        {
            StepChanger.SelectedItem = ChoosingTabItem;
            RollBackFirstTab(sender, e);
        }

        private void NextSecondClick(object sender, RoutedEventArgs e)
        {
            if (!FuckedTreeValidation())
            {
                var anim = new DoubleAnimation()
                {
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(.3)),
                    AutoReverse = true,
                    FillBehavior = FillBehavior.Stop,
                    IsCumulative = false,
                    IsAdditive = false
                };
                Storyboard.SetTarget(anim, TreeNamePanel);
                Storyboard.SetTargetProperty(anim, new PropertyPath(OpacityProperty));
                var storyboard = new Storyboard();
                storyboard.Children.Add(anim);
                storyboard.Begin();
            }
            else
            {
                //TODO
                //MR USHAKOV HERE FIELDS ARE NEEDED TO BE PROCEEDED
                //SIKASIKA
                //NameBox.Text
                //DescriptionBox.Text
                Tree.Text = "GRATZ";

                var move = new TranslateTransform(0, 0);
                var anim = new DoubleAnimation()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    To = -Width,
                    AccelerationRatio = 0.1,
                };
                SecondStep.RenderTransform = move;
                anim.Completed += MoveToCreate3;
                anim.Completed += RollBackSecondTab;
                move.BeginAnimation(TranslateTransform.XProperty, anim);
                ThirdStep.RenderTransform = null;
            }
        }

        private bool FuckedTreeValidation()
        {
            return NameBox.Text.Trim().Count() > 3 && DescriptionBox.Text.Trim().Count() > 6;
        }

        private void RollBackSecondTab(object sender, EventArgs e)
        {
            RollBack(SecondStep);
        }

        private void BackToTreeNameTabFromTreeAdd(object sender, EventArgs e)
        {
            RollForward(ThirdStep, MoveToCreate2);
        }
        





        //use it pls later STIL WIK AVTOBUS
        private void RollBack(UIElement toRoll, EventHandler toDoAfter = null)
        {
            var moveBack = new TranslateTransform(-Width, 0);
            var anim = new DoubleAnimation()
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                DecelerationRatio = 0.3
            };
            toRoll.RenderTransform = moveBack;
            if (toDoAfter != null)
                anim.Completed += toDoAfter;
            moveBack.BeginAnimation(TranslateTransform.XProperty, anim);
        }
        
        private void RollForward(UIElement toRoll, EventHandler toDoAfter = null)
        {
            var moveForward = new TranslateTransform(0, 0);
            var anim = new DoubleAnimation()
            {
                To = Width,
                Duration = new Duration(TimeSpan.FromSeconds(0.7)),
                AccelerationRatio = 0.2
            };
            toRoll.RenderTransform = moveForward;
            if (toDoAfter != null)
                anim.Completed += toDoAfter;
            moveForward.BeginAnimation(TranslateTransform.XProperty, anim);
        }
    }
}
