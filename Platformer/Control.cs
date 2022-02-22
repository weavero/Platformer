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

        bool jump;
        double maxJump = -5;
        double jumpHeight;

        public Control()
        {
            Loaded += Control_Loaded;
        }

        void Control_Loaded(object sender, RoutedEventArgs e)
        {
            model = new Model(@"../../../Levels/1.level");
            logic = new Logic(model);
            renderer = new Renderer(model);

            Window win = Window.GetWindow(this);
            

            if (win != null)
            {
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                Uri iconUri = new Uri("../../../hitman.jpg", UriKind.RelativeOrAbsolute);
                win.Icon = BitmapFrame.Create(iconUri);
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(15);
                timer.Tick += Timer_Tick;
                timer.Start();

                win.KeyDown += Win_KeyDown;
                win.KeyUp += Win_KeyUp;
            }

            logic.RefreshScreen += (obj, args) => InvalidateVisual();
            InvalidateVisual();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            //logic.MoveAI();
            InvalidateVisual();

            
            logic.Move();
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
                    jumpHeight = maxJump;
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
