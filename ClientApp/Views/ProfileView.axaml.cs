using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class ProfileView : UserControl
{
    private Random _random = new();

    public ProfileView(ProfileViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
        PopulateRectangles(50);
    }

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

            RectanglePanel.Children.Add(rectangle);
        }
    }
}