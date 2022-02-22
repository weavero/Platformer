﻿using System;
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
        GeometryDrawing background;
        GeometryDrawing ground;
        DrawingGroup dg;

        public GeometryDrawing Ground
        {
            get { return ground; }
        }

        public Renderer(Model model)
        {
            this.model = model;
            background = new GeometryDrawing(Brushes.Cyan, Config.penBrush, new RectangleGeometry(new Rect(0, 0, 1000, 1000)));
            ground = new GeometryDrawing(null, Config.penBrush, new LineGeometry(new Point(0, 350), new Point(3000, 350)));
        }

        public void Draw(DrawingContext ctx)
        {
            dg = new DrawingGroup();
            dg.Children.Add(background);
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

                        case 'P':
                            dg.Children.Add(new GeometryDrawing(Config.playerBrush, Config.penBrush, new RectangleGeometry(model.player.Area)));
                            break;

                        case 'F':
                            dg.Children.Add(new GeometryDrawing(Brushes.Orange, Config.penBrush, new RectangleGeometry(new Rect(j * Config.unitHeight, i * Config.unitWidth, Config.unitHeight, Config.unitWidth))));
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




            
            
            ctx.DrawDrawing(dg);
        }
    }
}
