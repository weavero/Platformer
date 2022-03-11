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
        DrawingGroup PlayAreaDrawing;
        DrawingGroup HUDDrawing;
        List<int> enemyIndexes;

        public DrawingGroup DrawingGroup { get { return PlayAreaDrawing; } }

        public Renderer(Model model)
        {
            this.model = model;
            enemyIndexes = new List<int>();
            DrawLevel();
            DrawHUD();
        }

        int playerIndex = -1;
        public void Draw(DrawingContext ctx)
        {
            if (playerIndex == -1)
            {
                foreach (GeometryDrawing item in PlayAreaDrawing.Children)
                {
                    if (item.Brush == Config.playerBrush)
                    {
                        playerIndex = PlayAreaDrawing.Children.IndexOf(item);
                    }
                    else if (item.Brush == Config.smallEnemyBrush || item.Brush == Config.bigEnemyBrush)
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
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.coinBrush, null, new RectangleGeometry(new Rect(j * Config.unitWidth, i * Config.unitHeight, 10, 10))));
                            break;

                        case 'F':
                            PlayAreaDrawing.Children.Add(new GeometryDrawing(Config.finishBrush, Config.penBrush, new RectangleGeometry(new Rect(j * Config.unitWidth, i * Config.unitHeight, Config.unitWidth, Config.unitHeight))));
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

        Point coinCounterLocation = new Point(Config.windowWidth - 50, Config.windowHeight - 25);
        private void DrawHUD()
        {
            HUDDrawing = new DrawingGroup();
            GeometryDrawing HUDBackground = new GeometryDrawing(Brushes.Black, null, new RectangleGeometry(new Rect(0, Config.windowHeight - 50, Config.windowWidth, 50)));

            FormattedText cointext = new FormattedText(model.Coin.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            GeometryDrawing coinCounter = new GeometryDrawing(null, new Pen(Brushes.White, 1), cointext.BuildGeometry(coinCounterLocation));

            FormattedText formattedText = new FormattedText(model.player.Area.X.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            GeometryDrawing text = new GeometryDrawing(null, new Pen(Brushes.Black, 1), formattedText.BuildGeometry(new Point(400, 550)));

            FormattedText formattedText2 = new FormattedText(model.player.Area.Y.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            GeometryDrawing text2 = new GeometryDrawing(null, new Pen(Brushes.Black, 1), formattedText2.BuildGeometry(new Point(450, 550)));

            HUDDrawing.Children.Add(HUDBackground);
            HUDDrawing.Children.Add(coinCounter);

            HUDDrawing.Children.Add(text);
            HUDDrawing.Children.Add(text2);
        }

        private void UpdateLevel()
        {
            
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

        private void UpdateHUD()
        {
            FormattedText cointext = new FormattedText(model.Coin.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            HUDDrawing.Children[1] = new GeometryDrawing(null, new Pen(Brushes.White, 1), cointext.BuildGeometry(coinCounterLocation));

            FormattedText formattedText = new FormattedText(model.player.Area.X.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            HUDDrawing.Children[2] = new GeometryDrawing(null, new Pen(Brushes.Black, 1), formattedText.BuildGeometry(new Point(400, 550)));

            FormattedText formattedText2 = new FormattedText(model.player.Area.Y.ToString(), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black);
            HUDDrawing.Children[3] = new GeometryDrawing(null, new Pen(Brushes.Black, 1), formattedText2.BuildGeometry(new Point(450, 550)));
        }
    }
}
