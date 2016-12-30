using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pot_sem2
{
    public class GameStateVisualiser : Control
    {
        public delegate void TileSelected(int x, int y);

        public event TileSelected OnTileSelected;

        private Random random = new Random();
        private GameState gameState = null;

        static GameStateVisualiser()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GameStateVisualiser), new FrameworkPropertyMetadata(typeof(GameStateVisualiser)));
        }

        public void SetState(GameState gameState)
        {
            this.gameState = gameState;
            base.InvalidateVisual();
            base.InvalidateMeasure();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double size = Math.Min(constraint.Width, constraint.Height);
            return new Size(size, size);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (ActualWidth > 0 && ActualHeight > 0 && DesiredSize.Width > 0 && gameState != null)
            {
                double xOffset = (ActualWidth - DesiredSize.Width) / 2;
                double yOffset = (ActualHeight - DesiredSize.Width) / 2;

                int xIndex = (int)(8 * (e.GetPosition(this).X - xOffset) / DesiredSize.Width);
                int yIndex = (int)(8 * (e.GetPosition(this).Y - yOffset) / DesiredSize.Width);

                if (xIndex >= 0 && xIndex < 8 && yIndex >= 0 && yIndex < 8)
                {
                    if (OnTileSelected != null)
                    {
                        OnTileSelected(xIndex, yIndex);
                    }
                }
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (ActualWidth > 0 && ActualHeight > 0 && DesiredSize.Width > 0)
            {
                double xOffset = (ActualWidth - DesiredSize.Width) / 2;
                double yOffset = (ActualHeight - DesiredSize.Width) / 2;

                drawingContext.PushTransform(new TranslateTransform(xOffset, yOffset));
                DrawGame(drawingContext, DesiredSize.Width);
                drawingContext.Pop();
            }
        }

        private void DrawGame(DrawingContext drawingContext, double renderSize)
        {
            Pen pen = new Pen(Brushes.Black, 2);
            drawingContext.DrawRectangle(null, pen, new Rect(new Point(0, 0), new Point(renderSize, renderSize)));
            
            if (gameState != null)
            {
                Boolean white = false;
                for (int i=0; i<8; i++)
                {
                    for (int j=0; j<8; j++)
                    {
                        drawingContext.PushTransform(new TranslateTransform(i * (renderSize / 8), j * (renderSize / 8)));
                        DrawField(drawingContext, gameState[i,j], renderSize / 8, white);
                        drawingContext.Pop();
                        white = !white;
                    }
                    white = !white;
                }
            }
        }

        private void DrawField(DrawingContext drawingContext, Field field, double renderSize, Boolean isWhite)
        {
            drawingContext.DrawRectangle(isWhite ? Brushes.LightGray : Brushes.Gray, null, new Rect(new Point(0, 0), new Point(renderSize, renderSize)));
            
            if (field.Player != Player.NONE && field.Figure != Figure.NONE)
            {
                Brush brush = field.Player == Player.WHITE ? Brushes.White : Brushes.Black;
                Brush negativeBrush = field.Player == Player.WHITE ? Brushes.Black : Brushes.White;

                if (field.Figure == Figure.KING)
                {
                    drawingContext.DrawEllipse(brush, null, new Point(renderSize / 2, renderSize / 2), renderSize * 0.4, renderSize * 0.4);
                    drawingContext.DrawEllipse(negativeBrush, null, new Point(renderSize / 2, renderSize / 2), renderSize * 0.2, renderSize * 0.2);
                }
                else
                {
                    drawingContext.DrawEllipse(brush, null, new Point(renderSize / 2, renderSize / 2), renderSize * 0.4, renderSize * 0.4);
                }
            }

            if (field.Selected)
            {
                drawingContext.DrawRectangle(null, new Pen(Brushes.Red, 3), new Rect(new Point(0, 0), new Point(renderSize, renderSize)));
            }
        }
    }
}
