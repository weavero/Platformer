﻿using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using Platformer.Models;

namespace Platformer.Controls
{
    class Renderer
    {
        Model model;
        DrawingGroup PlayAreaDrawing;
        DrawingGroup HUDDrawing;
        List<int> enemyIndexes;

        public DrawingGroup DrawingGroup { get { return PlayAreaDrawing; } }

        public Renderer(Model model)
        {
            this.model = model;
            DrawLevel();
            DrawHUD();
        }

        int playerIndex = -1;
        bool levelUpdated = true;
        public void Draw(DrawingContext ctx)
        {
            UpdateLevel();
            if (levelUpdated)
            {
                levelUpdated = false;
                enemyIndexes = new List<int>();
                foreach (GeometryDrawing item in PlayAreaDrawing.Children)
                {
                    if (Config.PlayerBrushes.Contains(item.Brush) || item.Brush == null)
                    {
                        playerIndex = PlayAreaDrawing.Children.IndexOf(item);
                    }
                    else if (Config.EnemyBrushes.Contains(item.Brush))
                    {
                        enemyIndexes.Add(PlayAreaDrawing.Children.IndexOf(item));
                    }
                }
                UpdateActors();
            }
            else
            {
                UpdateActors();
            }
            
            UpdateHUD();

            ctx.DrawDrawing(PlayAreaDrawing);
            ctx.DrawDrawing(HUDDrawing);
        }

        private void DrawLevel()
        {
            // kisebb kép unitnak középpontba helyezése:
            // j * Config.unitWidth + (Config.unitWidth - adott elem szélessége) / 2
            PlayAreaDrawing = new DrawingGroup();

            int index = 0;
            for (int i = 0; i < model.Map.GetLength(0); i++)
            {
                for (int j = 0; j < model.Map.GetLength(1); j++)
                {
                    switch (model.Map[i, j])
                    {
                        case 'g':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.groundBrush, null, new RectangleGeometry(new Rect(j * Config.UnitWidth, i * Config.UnitHeight, Config.UnitWidth, Config.UnitHeight))));
                            break;

                        case 'w':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.wallBrush, null, new RectangleGeometry(new Rect(j * Config.UnitWidth, i * Config.UnitHeight, Config.UnitWidth, Config.UnitHeight))));
                            break;

                        case 's':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.spikeBrush, null, new RectangleGeometry(new Rect(j * Config.UnitWidth, i * Config.UnitHeight, Config.UnitWidth, Config.UnitHeight))));
                            break;

                        case 'F':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.finishBrush, null, new RectangleGeometry(new Rect(j * Config.UnitWidth + Config.UnitWidth / 2, i * Config.UnitHeight - Config.UnitHeight, Config.UnitWidth, Config.UnitHeight * 2))));
                            break;

                        case 'c':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.CoinBrush, null, new RectangleGeometry(new Rect(j * Config.UnitWidth, i * Config.UnitHeight, Config.UnitWidth, Config.UnitHeight))));
                            break;

                        case 'l':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.LifePickup, null, new RectangleGeometry(new Rect(j * Config.UnitWidth, i * Config.UnitHeight, Config.UnitWidth, Config.UnitHeight))));
                            break;

                        case 'P':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.PlayerBrush, null, new RectangleGeometry(model.player.Area)));
                            break;

                        case 'e':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.SmallEnemyBrush, null, new RectangleGeometry(model.Enemies[index].Area)));
                            index++;
                            break;

                        case 'E':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.BigEnemyBrush, null, new RectangleGeometry(model.Enemies[index].Area)));
                            index++;
                            break;

                        case 'f':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.FlyingEnemy1, null, new RectangleGeometry(model.Enemies[index].Area)));
                            index++;
                            break;
                    }
                }
            }
        }

        private void UpdateLevel()
        {
            if (model.PickupableIndex != -1)
            {
                PlayAreaDrawing.Children.RemoveAt(model.PickupableIndex);
                model.SetPickupIndex(-1);
                levelUpdated = true;
            }

            int i = 0;
            while (i < PlayAreaDrawing.Children.Count && PlayAreaDrawing.Children[i].Bounds.Y < 2000)
            {
                i++;
            }

            if (i < PlayAreaDrawing.Children.Count)
            {
                PlayAreaDrawing.Children.RemoveAt(i);
            }
        }

        private void UpdateActors()
        {
            PlayAreaDrawing.Children[playerIndex] = new GeometryDrawing(Config.PlayerBrush, null, new RectangleGeometry(model.player.Area));
            
            int i = 0;
            foreach (int enemyIndex in enemyIndexes)
            {
                if (model.Enemies[i] is SmallEnemy)
                {
                    PlayAreaDrawing.Children[enemyIndex] = new GeometryDrawing(Config.SmallEnemyBrush, null, new RectangleGeometry(model.Enemies[i].Area));
                }
                else if (model.Enemies[i] is BigEnemy)
                {
                    PlayAreaDrawing.Children[enemyIndex] = new GeometryDrawing(Config.BigEnemyBrush, null, new RectangleGeometry(model.Enemies[i].Area));
                }
                else if (model.Enemies[i] is FlyingEnemy)
                {
                    PlayAreaDrawing.Children[enemyIndex] = new GeometryDrawing(Config.FlyingEnemy1, null, new RectangleGeometry(model.Enemies[i].Area));
                }
                i++;
            }
        }

        GeometryDrawing HUDBackground;
        GeometryDrawing lives;
        GeometryDrawing timeElapsed;
        GeometryDrawing points;
        GeometryDrawing coinCounter;
        private void DrawHUD()
        {
            HUDDrawing = new DrawingGroup();
            HUDBackground = new GeometryDrawing(Brushes.Black, null, new RectangleGeometry(new Rect(0, 0, 0, 50)));

            GeometryDrawing lifePic = new GeometryDrawing(Config.LifePickup, null, new RectangleGeometry(new Rect(0, 0, 0, 0)));

            FormattedText formattedText3 = new FormattedText(model.player.HitsToKill.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.White);
            lives = new GeometryDrawing(null, new Pen(Brushes.White, 1), formattedText3.BuildGeometry(new Point(50, 0)));

            FormattedText formattedText4 = new FormattedText(model.Timer.Elapsed.TotalSeconds.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.White);
            timeElapsed = new GeometryDrawing(null, new Pen(Brushes.White, 1), formattedText4.BuildGeometry(new Point(150, 0)));

            FormattedText pointText = new FormattedText(model.Points.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.White);
            points = new GeometryDrawing(null, new Pen(Brushes.White, 1), pointText.BuildGeometry(new Point(150, 0)));

            GeometryDrawing coinPic = new GeometryDrawing(Config.CoinBrush, null, new RectangleGeometry(new Rect(0, 0, 0, 0)));

            FormattedText cointext = new FormattedText(model.Coin.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.White);
            coinCounter = new GeometryDrawing(null, new Pen(Brushes.White, 1), cointext.BuildGeometry(new Point(0, 0)));

            
            HUDDrawing.Children.Add(HUDBackground);
            HUDDrawing.Children.Add(lifePic);
            HUDDrawing.Children.Add(lives);
            HUDDrawing.Children.Add(timeElapsed);
            HUDDrawing.Children.Add(points);
            HUDDrawing.Children.Add(coinPic);
            HUDDrawing.Children.Add(coinCounter);
            HUDDrawing.Children.Add(coinCounter);
            HUDDrawing.Children.Add(coinCounter);
        }

        double HUDHeight = 100;
        private void UpdateHUD()
        {
            HUDBackground.Geometry = new RectangleGeometry(new Rect(model.player.Area.Left - model.mainWindow.Width, model.mainWindow.Height - HUDHeight, model.mainWindow.Width * 2, HUDHeight));
            HUDDrawing.Children[0] = HUDBackground;

            HUDDrawing.Children[1] = new GeometryDrawing(Config.LifePickup, null, new RectangleGeometry(new Rect(model.player.Area.Left - 300, model.mainWindow.Height - HUDHeight / 2 - Config.UnitHeight / 4, Config.UnitWidth, Config.UnitHeight)));

            FormattedText playerLives = new FormattedText(model.Retries.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Config.typeface, 24, Brushes.Black);
            lives.Geometry = playerLives.BuildGeometry(new Point(HUDDrawing.Children[1].Bounds.Right + 10, HUDDrawing.Children[1].Bounds.Top));
            HUDDrawing.Children[2] = lives;

            FormattedText elapsedTime = new FormattedText("Idö " + Convert.ToInt32(model.Timer.Elapsed.TotalSeconds).ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Config.typeface, 24, Brushes.White);
            timeElapsed.Geometry = elapsedTime.BuildGeometry(new Point(model.player.Area.Left - 100, model.mainWindow.Height - HUDHeight / 2 - Config.UnitHeight / 4));
            HUDDrawing.Children[3] = timeElapsed;

            FormattedText pointText = new FormattedText("Pontok: " + model.Points.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Config.typeface, 24, Brushes.White);
            points.Geometry = pointText.BuildGeometry(new Point(model.player.Area.Left + 50, model.mainWindow.Height - HUDHeight / 2 - Config.UnitHeight / 4));
            HUDDrawing.Children[4] = points;

            HUDDrawing.Children[5] = new GeometryDrawing(Config.CoinBrush, null, new RectangleGeometry(new Rect(model.player.Area.Left + 250, model.mainWindow.Height - HUDHeight / 2 - Config.UnitHeight / 4, Config.UnitWidth, Config.UnitHeight)));
            
            FormattedText coinText = new FormattedText(model.Coin.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Config.typeface, 24, Brushes.Black);
            coinCounter.Geometry = coinText.BuildGeometry(new Point(HUDDrawing.Children[5].Bounds.Right + 10, HUDDrawing.Children[5].Bounds.Top));
            HUDDrawing.Children[6] = coinCounter;


            FormattedText X = new FormattedText(model.player.Area.Left.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Config.typeface, 24, Brushes.Black);
            HUDDrawing.Children[7] = new GeometryDrawing(null, Config.penBrush, X.BuildGeometry(new Point(model.player.Area.Left, 100)));

            FormattedText Y = new FormattedText(model.player.Area.Top.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Config.typeface, 24, Brushes.Black);
            HUDDrawing.Children[8] = new GeometryDrawing(null, Config.penBrush, Y.BuildGeometry(new Point(model.player.Area.Left + 100, 100)));
        }
    }
}
