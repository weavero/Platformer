using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media.Imaging;

namespace Platformer
{
    class Renderer
    {
        Model model;
        GeometryDrawing background;
        GeometryDrawing ground;
        DrawingGroup dg;

        public DrawingGroup DrawingGroup { get { return dg; } }

        public GeometryDrawing Ground
        {
            get { return ground; }
        }

        public Renderer(Model model)
        {
            this.model = model;
            background = new GeometryDrawing(Config.backgroundBrush, null, new RectangleGeometry(new Rect(0, 0, 1000, 1000)));
            ground = new GeometryDrawing(Brushes.Black, Config.penBrush, new LineGeometry(new Point(0, 350), new Point(3000, 350)));
        }

        public void Draw(DrawingContext ctx)
        {
            dg = new DrawingGroup();
            //dg.Children.Add(background);
            dg.Children.Add(ground);
            for (int i = 0; i < model.Map.GetLength(0); i++)
            {
                for (int j = 0; j < model.Map.GetLength(1); j++)
                {
                    switch (model.Map[i, j])
                    {
                        case 'g':
                            dg.Children.Add(new GeometryDrawing(Config.groundBrush, Config.penBrush, new RectangleGeometry(new Rect(j * Config.unitHeight, i * Config.unitWidth, Config.unitHeight, Config.unitWidth))));
                            break;

                        case 'w':
                            dg.Children.Add(new GeometryDrawing(Config.wallBrush, Config.penBrush, new RectangleGeometry(new Rect(j * Config.unitHeight, i * Config.unitWidth, Config.unitHeight, Config.unitWidth))));
                            break;

                        case 'F':
                            dg.Children.Add(new GeometryDrawing(Config.finishBrush, Config.penBrush, new RectangleGeometry(new Rect(j * Config.unitHeight, i * Config.unitWidth, Config.unitHeight, Config.unitWidth))));
                            break;

                        case 'P':
                            dg.Children.Add(new GeometryDrawing(Config.playerBrush, null, new RectangleGeometry(model.player.Area)));
                            break;

                        case 'E':
                            foreach (Enemy enemy in model.Enemies)
                            {
                                dg.Children.Add(new GeometryDrawing(Config.enemyBrush, Config.penBrush, new RectangleGeometry(enemy.Area)));
                            }
                            break;
                    }
                }
            }

            GeometryDrawing ground2 = new GeometryDrawing(Brushes.Blue, Config.penBrush, new LineGeometry(new Point(0, 350), new Point(3000, 250)));

            FormattedText formattedText = new FormattedText(model.player.Area.X.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            GeometryDrawing text = new GeometryDrawing(null, new Pen(Brushes.Black, 1), formattedText.BuildGeometry(new Point(400, 550)));

            FormattedText formattedText2 = new FormattedText(model.player.Area.Y.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            GeometryDrawing text2 = new GeometryDrawing(null, new Pen(Brushes.Black, 1), formattedText2.BuildGeometry(new Point(450, 550)));

            dg.Children.Add(text);
            dg.Children.Add(text2);

            ctx.DrawDrawing(dg);

        }

    }
}
