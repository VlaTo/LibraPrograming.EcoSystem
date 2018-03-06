using LibraProgramming.Windows.EcoSystem.GameEngine;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;

namespace LibraProgramming.Windows.EcoSystem.Views
{
    public sealed partial class MainPage
    {
        private static readonly Vector2 GridSize = new Vector2(20.0f);
        private IEcoSystemController controller;
        private IScene scene;
        
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCanvasAnimatedControlDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Antialiasing = CanvasAntialiasing.Aliased;
            DrawGrid(sender, args.DrawingSession, sender.Size.ToVector2());
            scene.Draw(args.DrawingSession);
        }

        private void DrawGrid(ICanvasAnimatedControl control, CanvasDrawingSession session, Vector2 size)
        {
            var brush = new CanvasSolidColorBrush(control, Colors.Gray);
            
            for(var y = 0.0f; y <= size.Y; y += GridSize.Y)
            {
                session.DrawLine(new Vector2(0.0f, y), new Vector2(size.X, y), brush, 1.0f);
            }

            for (var x = 0.0f; x <= size.X; x += GridSize.X)
            {
                session.DrawLine(new Vector2(x, 0.0f), new Vector2(x, size.Y), brush, 1.0f);
            }
        }

        private void OnCanvasAnimatedControlCreateResources(ICanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            controller = new EcoSystemController(sender as CanvasAnimatedControl);
            scene = new Scene(controller, sender.Size.ToVector2());
            args.TrackAsyncAction(controller.InitializeAsync(args.Reason).AsAsyncAction());
        }

        private void OnCanvasAnimatedControlUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            controller.Update(args.Timing.ElapsedTime);
        }

        private void OnCanvasAnimatedControlUnloaded(object sender, RoutedEventArgs e)
        {
            controller.Shutdown();
        }
    }
}
