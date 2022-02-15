using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.IO;

namespace Platformer
{
    class Control : FrameworkElement
    {
        Model model;
        Logic logic;
        Renderer renderer;
        DispatcherTimer timer;

        bool jump = false;
        double max = -5;
        double h;

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
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(15);
                timer.Tick += Timer_Tick;
                timer.Start();

                win.KeyDown += Win_KeyDown;
            }

            logic.RefreshScreen += (obj, args) => InvalidateVisual();
            InvalidateVisual();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            logic.MoveAI();
            InvalidateVisual();

            if (jump)
            {
                if (!model.player.Area.IntersectsWith(renderer.Ground.Bounds))
                {
                    h += 0.1;
                    model.player.SetY(h);
                }
                else
                {
                    model.player.SetXY(model.player.Area.X, renderer.Ground.Bounds.Y - model.player.Area.Height - 1); //-1 az intersect miatt
                    jump = false;
                }
            }
        }

        void Win_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    //if(Key.LeftShift)
                    logic.Move(Logic.Direction.Left);
                    break;

                case Key.D:
                    logic.Move(Logic.Direction.Right);
                    break;

                case Key.Space:
                    if (!jump)
                    {
                        jump = true;
                        h = max;
                    }
                    break;

                case Key.Escape:
                    TimerStartStop();
                    break;
            }
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
