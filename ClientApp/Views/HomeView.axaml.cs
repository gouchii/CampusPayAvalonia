using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class HomeView : UserControl
{
    private static readonly Random _random = new();

    public HomeView(HomeViewModel viewModel)
    {
        DataContext = viewModel;

        InitializeComponent();
        // PopulateRectangles(150);
    }


    // private void PopulateCircles(int count)
    // {
    //     var colors = new List<string>
    //     {
    //         "MediumTurquoise",
    //         "SteelBlue",
    //         "SkyBlue",
    //         "PowderBlue",
    //         "CornflowerBlue",
    //         "CadetBlue",
    //         "LightSeaGreen",
    //         "DodgerBlue"
    //     };
    //
    //     for (int i = 0; i < count; i++)
    //     {
    //         var circle = new Ellipse
    //         {
    //             Fill = new SolidColorBrush(Color.Parse(colors[_random.Next(colors.Count)])),
    //             Width = _random.Next(20, 300),
    //             Height = _random.Next(20, 300),
    //             Margin = new Thickness(5)
    //         };
    //
    //         // Make sure it's a circle
    //         circle.Width = circle.Height;
    //
    //         EllipsePanel.Children.Add(circle);
    //     }

    private void PopulateRectangles(int count)
    {
        var colors = new List<string>
        {
            "MediumTurquoise",
            "SteelBlue",
            "SkyBlue",
            "PowderBlue",
            "CornflowerBlue",
            "CadetBlue",
            "LightSeaGreen",
            "DodgerBlue"
        };

        for (int i = 0; i < count; i++)
        {
            var rectangle = new Rectangle
            {
                Fill = new SolidColorBrush(Color.Parse(colors[_random.Next(colors.Count)])),
                Width = 1390,
                Height = 700,
                Margin = new Thickness( 40)
            };

            // RectanglePanel.Children.Add(rectangle);
        }
    }
}