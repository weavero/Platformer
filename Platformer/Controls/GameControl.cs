using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using Platformer.Models;

namespace Platformer.Controls
{
    class GameControl : Control
    {
        Model model;
        Logic logic;
        Renderer renderer;
        DispatcherTimer timer;
        MainWindow window;

        public bool IsInSettings { get; set; }
        public bool IsInGame { get; set; }
        public GameControl()
        {
            Loaded += Control_Loaded;
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
                model.mainWindow.ShowGameOver();
                timer.Stop();
            }

            Canvas.SetLeft(this, -model.player.Area.Left + model.mainWindow.Width / 2);

            InvalidateVisual();
        }

        private void Win_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsInGame)
            {
                //if (model.player.IsJumping || model.player.IsFalling)
                //{
                if (e.Key == Key.A || e.Key == Key.Left) { model.player.GoLeft = true; }
                else if (e.Key == Key.D || e.Key == Key.Right) { model.player.GoRight = true; }
                //}
                else if (e.Key == Key.Space || e.Key == Key.Up)
                {
                    if (!model.player.IsJumping && !model.player.IsFalling)
                    {
                        model.player.IsJumping = true;
                    }
                }
                else if (e.Key == Key.Escape)
                {
                    TimerStartStop();
                    IsInGame = false;
                }
            }
        }

        private void Win_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.Left) { model.player.GoLeft = false; }
            else if (e.Key == Key.D || e.Key == Key.Right) { model.player.GoRight = false; }
        }

        private void TimerStartStop()
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                model.Timer.Stop();
                model.mainWindow.ShowPause();
            }
            else
            {
                model.mainWindow.ResumeGame();
                timer.Start();
                model.Timer.Start();
            }
        }

        public void TimerStart()
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
                model.Timer.Start();
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
            SetEvents();
            timer.Start();
            IsInGame = true;
        }

        private void LevelChange()
        {
            renderer = new Renderer(model);
            //SetEvents();
        }

        private void GameComplete(GameCompleteArgs args)
        {
            timer.Stop();
            model.mainWindow.ShowGameComplete(args);
        }

        private void SetEvents()
        {
            logic.OnGameComplete += (obj, args) => GameComplete(args);
            logic.OnLevelChange += (obj, args) => LevelChange();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            renderer?.Draw(drawingContext);
        }
    }
}
