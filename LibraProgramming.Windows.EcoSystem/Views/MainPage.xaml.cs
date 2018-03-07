using LibraProgramming.Windows.EcoSystem.GameEngine;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
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
            controller = new EcoSystemController(sender as CanvasAnimatedControl);
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
    }
}
