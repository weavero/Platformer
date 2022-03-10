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

namespace Platformer
{
    class Control : FrameworkElement
    {
        Model model;
        Logic logic;
        Renderer renderer;
        DispatcherTimer timer;

        public Control()
        {
            Loaded += Control_Loaded;
        }

        void Control_Loaded(object sender, RoutedEventArgs e)
        {
            model = new Model(@"../../../Levels/1.level");
            logic = new Logic(model);
            renderer = new Renderer(model);

            Window window = Window.GetWindow(this);
            window.Background = Config.BackgroundImage;
            ContentElement c = new ContentElement();
            

            if (window != null)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                Uri iconUri = new Uri("../../../img/hitman.jpg", UriKind.RelativeOrAbsolute);
                window.Icon = BitmapFrame.Create(iconUri);
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(15);
                timer.Tick += Timer_Tick;
                timer.Start();
                window.KeyDown += Win_KeyDown;
                window.KeyUp += Win_KeyUp;
            }
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            logic.GameTick();
            logic.CollisionCheck(model.player, renderer.DrawingGroup);
            foreach (Enemy enemy in model.Enemies)
            {
                logic.CollisionCheck(enemy, renderer.DrawingGroup);
            }

            if (logic.GameOver())
            {
                renderer = new Renderer(model);
            }

            InvalidateVisual();
        }

        void Win_KeyDown(object sender, KeyEventArgs e)
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

        private void Win_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.Left) { logic.GoLeft = false; }
            else if (e.Key == Key.D || e.Key == Key.Right) { logic.GoRight = false; }
        }

        private void TimerStartStop()
        {
            if(timer.IsEnabled)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            renderer?.Draw(drawingContext);
        }
    }
}
