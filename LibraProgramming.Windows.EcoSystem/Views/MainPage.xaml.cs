using LibraProgramming.Windows.EcoSystem.GameEngine;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace LibraProgramming.Windows.EcoSystem.Views
{
    public sealed partial class MainPage
    {
        private IEcoSystemController controller;
        
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCanvasAnimatedControlCreateResources(ICanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            var obstacles = GetObstacles();
            controller = new EcoSystemController(sender as CanvasAnimatedControl, new MapSize(60, 40), obstacles);
            args.TrackAsyncAction(controller.InitializeAsync(args.Reason).AsAsyncAction());
        }

        private void OnCanvasAnimatedControlDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Antialiasing = CanvasAntialiasing.Aliased;
            controller.Scene.Draw(args.DrawingSession);
        }

        private void OnCanvasAnimatedControlUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            controller.Update(args.Timing.ElapsedTime);
        }

        private void OnCanvasAnimatedControlUnloaded(object sender, RoutedEventArgs e)
        {
            controller.Shutdown();
        }

        private static IEnumerable<Coordinates> GetObstacles()
        {
            var walls = new List<Coordinates>();

            for (var x = 0; x < 60; x++)
            {
                walls.Add(new Coordinates(x, 0));
                walls.Add(new Coordinates(x, 39));
            }

            for (var y = 1; y < 39; y++)
            {
                walls.Add(new Coordinates(0, y));
                walls.Add(new Coordinates(59, y));
            }

            for(var index = 0; index < 5; index++)
            {
                walls.Add(new Coordinates(5 + index, 3));
                walls.Add(new Coordinates(22, 20 + index));
                walls.Add(new Coordinates(38, 7 + index));
                walls.Add(new Coordinates(15 + index, 36));
                walls.Add(new Coordinates(54, 28 + index));
            }

            for (var index = 0; index < 2; index++)
            {
                walls.Add(new Coordinates(47 + index, 32));
            }

            walls.Add(new Coordinates(25, 33));
            walls.Add(new Coordinates(9, 10));

            return walls;
        }
    }
}
