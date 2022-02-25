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
            

            if (window != null)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                Uri iconUri = new Uri("../../../hitman.jpg", UriKind.RelativeOrAbsolute);
                window.Icon = BitmapFrame.Create(iconUri);
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(15);
                timer.Tick += Timer_Tick;
                timer.Start();

                window.KeyDown += Win_KeyDown;
                window.KeyUp += Win_KeyUp;
            }

            logic.RefreshScreen += (obj, args) => InvalidateVisual();
            InvalidateVisual();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if (model.player.Area.IntersectsWith(renderer.Ground.Bounds))
            {
                model.player.SetXY(model.player.Area.X, renderer.Ground.Bounds.Y - model.player.Area.Height - 1); //-1 hogy megint tudjon ugrani

            }
            //logic.MoveAI();
            InvalidateVisual();
            logic.GameTick();
            
            logic.Move();
            //logic.CollisionCheck(model.player, renderer.DrawingGroup);
        }

        void Win_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.Left) { logic.GoLeft = true; }
            else if (e.Key == Key.D || e.Key == Key.Right) { logic.GoRight = true; }
            else if (e.Key == Key.Space || e.Key == Key.Up)
            {
                if (!logic.IsJumping)
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
