using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;

namespace LibraProgramming.Windows.Games.Towers.GameEngine
{
    public sealed class MapBackground : SceneNode
    {
        private readonly Uri spritesheetPath;
        private readonly Color backgroundColor;
        private ICanvasBrush backgroundBrush;
        private CanvasBitmap bitmapBrush;

        public MapBackground(Uri spritesheetPath = null)
        {
            this.spritesheetPath = spritesheetPath;
            backgroundColor = Colors.CadetBlue;
        }

        public override async Task CreateResourcesAsync(ICanvasResourceCreatorWithDpi creator, CanvasCreateResourcesReason reason)
        {
            backgroundBrush = new CanvasSolidColorBrush(creator, backgroundColor);

            if (null != spritesheetPath)
            {
                bitmapBrush = await CanvasBitmap.LoadAsync(creator, spritesheetPath);
//                spritesheetBrush = new CanvasImageBrush(creator, image);
            }

            await base.CreateResourcesAsync(creator, reason);
        }

        public override void Draw(CanvasDrawingSession session)
        {
            var size = new Size(50.0d, 50.0d);
            var sourceRect = new Rect(0.0d, 0.0d, 25.0d, 25.0d);

            using (session.CreateLayer(1.0f))
            {
                using (var batch = session.CreateSpriteBatch(CanvasSpriteSortMode.Bitmap))
                {
                    for (var row = 0; row < 10; row++)
                    {
                        for (var column = 0; column < 10; column++)
                        {
                            var x = size.Width * column;
                            var y = size.Height * row;

                            batch.DrawFromSpriteSheet(bitmapBrush, new Rect(new Point(x, y), size), sourceRect);
                        }
                    }
                }
            }

            base.Draw(session);
        }
    }
}