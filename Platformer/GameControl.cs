using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.IO;
using System.Windows.Media.Imaging;
using Platformer.Views;
using System.Windows.Controls;

namespace Platformer
{
    class GameControl : Control
    {
        Model model;
        Logic logic;
        Renderer renderer;
        DispatcherTimer timer;
        MainWindow window;

        public GameControl()
        {
            Loaded += Control_Loaded;
            IsVisibleChanged += PControl_IsVisibleChanged;
        }

        private void PControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                //model = new Model();
                //logic = new Logic(model);
                //renderer = new Renderer(model);
            }
        }

        public void Control_Loaded(object sender, RoutedEventArgs e)
        {
            model = new Model();
            logic = new Logic(model);
            renderer = new Renderer(model);
            
            window = (MainWindow)Window.GetWindow(this);
            window.Background = Config.BackgroundImage;
            
            if (window != null)
            {
                if (timer == null)
                {
                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(15);
                    timer.Tick += Timer_Tick;
                }
                if (IsLoaded)
                {
                    window.KeyDown += Win_KeyDown;
                    window.KeyUp += Win_KeyUp;
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            logic.GameTick();
            logic.CollisionCheck(model.player, renderer.DrawingGroup);
            foreach (Enemy enemy in model.Enemies)
            {
                logic.CollisionCheck(enemy, renderer.DrawingGroup);
            }

            if (logic.IsGameOver())
            {
                renderer = new Renderer(model);
            }
            else if (logic.PlayerLives < 1)
            {
                timer.Stop();
                window.ShowGameOver();
            }

            Canvas.SetLeft(this, -model.player.Area.Left + 150);
            Canvas.SetTop(this, -model.player.Area.Top + 150);

            InvalidateVisual();
        }

        private void Win_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsVisible)
            {
                if (e.Key == Key.A || e.Key == Key.Left) { logic.GoLeft = true; }
                else if (e.Key == Key.D || e.Key == Key.Right) { logic.GoRight = true; }
                else if (e.Key == Key.Space || e.Key == Key.Up)
                {
                    if (!logic.IsJumping && !logic.IsFalling)
                    {
                        logic.IsJumping = true;
                    }
                }
                else if (e.Key == Key.Escape) { TimerStartStop(); }
            }
        }

        private void Win_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.Left) { logic.GoLeft = false; }
            else if (e.Key == Key.D || e.Key == Key.Right) { logic.GoRight = false; }
        }

        private void TimerStartStop()
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                model.StopTimer();
                window.ShowPause();
            }
            else
            {
                window.ResumeGame();
                timer.Start();
                model.StartTimer();
            }
        }

        public void TimerStart()
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
                model.StartTimer();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            renderer?.Draw(drawingContext);
        }
    }
}
