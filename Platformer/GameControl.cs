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
                
            }
        }

        public void Control_Loaded(object sender, RoutedEventArgs e)
        {
            
            window = (MainWindow)Window.GetWindow(this);

            if (window != null)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(15);
                timer.Tick += Timer_Tick;
                window.KeyDown += Win_KeyDown;
                window.KeyUp += Win_KeyUp;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            logic.GameTick();
            logic.CollisionCheck(model.player, renderer.DrawingGroup);
            EnemyCollisionCheck();


            if (logic.IsGameOver())
            {
                window.ShowGameOver();
                timer.Stop();
            }
            else
            {

            }

            logic.OnGameComplete += (obj, args) => window.ShowGameComplete();

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

        private void EnemyCollisionCheck()
        {
            foreach (Enemy enemy in model.Enemies)
            {
                logic.CollisionCheck(enemy, renderer.DrawingGroup);
            }
        }

        public void NewGame()
        {
            model = new Model();
            logic = new Logic(model);
            renderer = new Renderer(model);
            model.mainWindow = window;
            timer.Start();

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            renderer?.Draw(drawingContext);
        }
    }
}
