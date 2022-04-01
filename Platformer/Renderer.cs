using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media.Imaging;
using Platformer.Views;
using System.Windows.Controls;

namespace Platformer
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
                    if (item.Brush is ImageBrush)
                    {
                        if (item.Bounds.Height == Config.playerHeight)
                        {
                            playerIndex = PlayAreaDrawing.Children.IndexOf(item);
                        }
                        else if (item.Brush == Config.smallEnemyBrush || item.Brush == Config.bigEnemyBrush)
                        {
                            enemyIndexes.Add(PlayAreaDrawing.Children.IndexOf(item));
                        }
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
            // kisebb kép unitnek középpontba helyezése:
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
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.groundBrush, null, new RectangleGeometry(new Rect(j * Config.unitWidth, i * Config.unitHeight, Config.unitWidth, Config.unitHeight))));
                            break;

                        case 'w':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.wallBrush, null, new RectangleGeometry(new Rect(j * Config.unitWidth, i * Config.unitHeight, Config.unitWidth, Config.unitHeight))));
                            break;

                        case 'c':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.coinBrush, null, new RectangleGeometry(new Rect(j * Config.unitWidth + (Config.unitWidth - Config.coinSize) / 2, i * Config.unitHeight, Config.coinSize, Config.coinSize))));
                            break;

                        case 'F':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.finishBrush, null, new RectangleGeometry(new Rect(j * Config.unitWidth + (Config.unitWidth - 30) / 2, i * Config.unitHeight - 50, 50, 100))));
                            break;

                        case 'l':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.lifePickup, null, new RectangleGeometry(new Rect(j * Config.unitWidth, i * Config.unitHeight, 32, 32))));
                            break;

                        case 'P':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.playerBrush, null, new RectangleGeometry(model.player.Area)));
                            break;

                        case 'e':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.smallEnemyBrush, null, new RectangleGeometry(model.Enemies[index].Area)));
                            index++;
                            break;

                        case 'E':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.bigEnemyBrush, null, new RectangleGeometry(model.Enemies[index].Area)));
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
            PlayAreaDrawing.Children[playerIndex] = new GeometryDrawing(Config.playerBrush, null, new RectangleGeometry(model.player.Area));
            int i = 0;
            foreach (int enemyIndex in enemyIndexes)
            {
                if (model.Enemies[i] is SmallEnemy)
                {
                    PlayAreaDrawing.Children[enemyIndex] = new GeometryDrawing(Config.smallEnemyBrush, null, new RectangleGeometry(model.Enemies[i].Area));
                }
                else
                {
                    PlayAreaDrawing.Children[enemyIndex] = new GeometryDrawing(Config.bigEnemyBrush, null, new RectangleGeometry(model.Enemies[i].Area));
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
            HUDBackground = new GeometryDrawing(Brushes.Black, null, new RectangleGeometry(new Rect(0, Config.windowHeight - 50, Config.windowWidth, 50)));

            //FormattedText formattedText = new FormattedText(model.player.Area.X.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            //GeometryDrawing text = new GeometryDrawing(null, new Pen(Brushes.Black, 1), formattedText.BuildGeometry(new Point(400, 550)));

            //FormattedText formattedText2 = new FormattedText(model.player.Area.Y.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            //GeometryDrawing text2 = new GeometryDrawing(null, new Pen(Brushes.Black, 1), formattedText2.BuildGeometry(new Point(450, 550)));

            FormattedText formattedText3 = new FormattedText(model.player.Lives.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            lives = new GeometryDrawing(null, new Pen(Brushes.White, 1), formattedText3.BuildGeometry(new Point(50, Config.windowHeight - 25)));

            FormattedText formattedText4 = new FormattedText(model.Timer.Elapsed.TotalSeconds.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            timeElapsed = new GeometryDrawing(null, new Pen(Brushes.White, 1), formattedText4.BuildGeometry(new Point(150, Config.windowHeight - 25)));

            FormattedText pointText = new FormattedText(model.Points.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            points = new GeometryDrawing(null, new Pen(Brushes.White, 1), pointText.BuildGeometry(new Point(150, Config.windowHeight - 25)));

            GeometryDrawing coinPic = new GeometryDrawing(Config.coinBrush, null, new RectangleGeometry(new Rect(0, 0, 0, 0)));

            FormattedText cointext = new FormattedText(model.Coin.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            coinCounter = new GeometryDrawing(null, new Pen(Brushes.White, 1), cointext.BuildGeometry(new Point(0, 0)));

            HUDDrawing.Children.Add(HUDBackground);

            //HUDDrawing.Children.Add(text);
            //HUDDrawing.Children.Add(text2);

            HUDDrawing.Children.Add(lives);

            HUDDrawing.Children.Add(timeElapsed);
            HUDDrawing.Children.Add(points);
            HUDDrawing.Children.Add(coinPic);
            HUDDrawing.Children.Add(coinCounter);
        }

        private void UpdateHUD()
        {
            //HUD background
            HUDBackground.Geometry = new RectangleGeometry(new Rect(model.player.Area.Left - 150, model.player.Area.Bottom + 450, model.mainWindow.Width, 100));
            HUDDrawing.Children[0] = HUDBackground;

            //FormattedText playerX = new FormattedText(model.player.Area.X.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            //HUDDrawing.Children[2] = new GeometryDrawing(null, new Pen(Brushes.Black, 1), playerX.BuildGeometry(new Point(400, 550)));

            //FormattedText playerY = new FormattedText(model.player.Area.Y.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            //HUDDrawing.Children[3] = new GeometryDrawing(null, new Pen(Brushes.Black, 1), playerY.BuildGeometry(new Point(450, 550)));

            FormattedText playerLives = new FormattedText("Életek " + model.Retries.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            lives.Geometry = playerLives.BuildGeometry(new Point(model.player.Area.Left - 100, model.player.Area.Top + 540));
            HUDDrawing.Children[1] = lives;

            FormattedText elapsedTime = new FormattedText("Idő " + Convert.ToInt32(model.Timer.Elapsed.TotalSeconds).ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            timeElapsed.Geometry = elapsedTime.BuildGeometry(new Point(model.player.Area.Left, model.player.Area.Top + 540));
            HUDDrawing.Children[2] = timeElapsed;

            FormattedText pointText = new FormattedText("Pontok: " + model.Points.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            points.Geometry = pointText.BuildGeometry(new Point(model.player.Area.Left + 50, model.player.Area.Top + 540));
            HUDDrawing.Children[3] = points;

            HUDDrawing.Children[4] = new GeometryDrawing(Config.coinBrush, null, new RectangleGeometry(new Rect(model.player.Area.Left + 180, model.player.Area.Top + 545, 10, 10)));
            
            FormattedText coinText = new FormattedText(model.Coin.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            coinCounter.Geometry = coinText.BuildGeometry(new Point(model.player.Area.Left + 200, model.player.Area.Top + 540));
            HUDDrawing.Children[5] = coinCounter;
        }
    }
}
