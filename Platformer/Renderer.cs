using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using System.IO;

namespace Platformer
{
    class Renderer
    {
        Model model;
        char[,] map;
        GeometryDrawing background;
        GeometryDrawing player;
        GeometryDrawing enemy;
        GeometryDrawing text;
        GeometryDrawing text2;
        GeometryDrawing ground;
        DrawingGroup dg;

        public GeometryDrawing Ground
        {
            get { return ground; }
        }

        public Renderer(Model model)
        {
            this.model = model;
            background = new GeometryDrawing(Brushes.Red, new Pen(Brushes.Red, 1), new RectangleGeometry(new Rect(0, 0, 1000, 1000)));
            ground = new GeometryDrawing(null, new Pen(Brushes.Black, 1), new LineGeometry(new Point(0, 350), new Point(3000, 350)));

            
        }

        public void Draw(DrawingContext ctx)
        {
            dg = new DrawingGroup();

            FormattedText formattedText = new FormattedText(model.player.Area.X.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            text = new GeometryDrawing(null, new Pen(Brushes.Black, 1), formattedText.BuildGeometry(new Point(400, 50)));

            FormattedText formattedText2 = new FormattedText(model.player.Area.Y.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            text2 = new GeometryDrawing(null, new Pen(Brushes.Black, 1), formattedText2.BuildGeometry(new Point(450, 50)));

            for (int i = 0; i < model.Map.GetLength(0); i++)
            {
                for (int j = 0; j < model.Map.GetLength(1); j++)
                {
                    switch (model.Map[i, j])
                    {
                        case 'g':
                            dg.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Green, 1), new RectangleGeometry(new Rect(j * 40, i * 20 + 300, 40, 20))));
                            break;

                        case 'w':
                            dg.Children.Add(new GeometryDrawing(Brushes.Black, new Pen(Brushes.Black, 1), new RectangleGeometry(new Rect(j * 40, i * 20 + 300, 40, 20))));
                            break;

                        case 'P':
                            dg.Children.Add(new GeometryDrawing(Brushes.Blue, new Pen(Brushes.Blue, 2), new RectangleGeometry(model.player.Area)));
                            break;

                        case 'F':
                            dg.Children.Add(new GeometryDrawing(Brushes.Orange, new Pen(Brushes.Orange, 1), new RectangleGeometry(new Rect(j * 40, i * 20 + 300, 40, 20))));
                            break;

                        case 'E':
                            foreach (Enemy enemy in model.Enemies)
                            {
                                dg.Children.Add(new GeometryDrawing(Brushes.Red, new Pen(Brushes.Red, 1), new RectangleGeometry(enemy.Area)));
                            }
                            
                            break;

                        default:
                            break;
                    }
                }
            }



            /*
            dg.Children.Add(background);
            
            dg.Children.Add(enemy);
            
            dg.Children.Add(ground);
            */

            dg.Children.Add(text);
            dg.Children.Add(text2);
            //dg.Children.Add(player);
            ctx.DrawDrawing(dg);
        }
    }
}
