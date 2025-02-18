﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SkiaSharp.Views.Windows;
using ScottPlot.Control;

namespace ScottPlot.WinUI;

public partial class WinUIPlot : UserControl, IPlotControl
{
    private readonly SKXamlCanvas _canvas = CreateRenderTarget();

    public Plot Plot { get; } = new();

    public SkiaSharp.GRContext? GRContext => null;

    public IPlotInteraction Interaction { get; set; }
    public IPlotMenu Menu { get; set; }

    public Window? AppWindow { get; set; } // https://stackoverflow.com/a/74286947

    public float DisplayScale { get; set; }

    public WinUIPlot()
    {
        DisplayScale = DetectDisplayScale();
        Interaction = new Interaction(this);
        Menu = new WinUIPlotMenu(this);

        Background = new SolidColorBrush(Microsoft.UI.Colors.White);

        _canvas.PaintSurface += OnPaintSurface;

        _canvas.PointerWheelChanged += OnPointerWheelChanged;
        _canvas.PointerReleased += OnPointerReleased;
        _canvas.PointerPressed += OnPointerPressed;
        _canvas.PointerMoved += OnPointerMoved;
        _canvas.DoubleTapped += OnDoubleTapped;
        _canvas.KeyDown += OnKeyDown;
        _canvas.KeyUp += OnKeyUp;

        this.Content = _canvas;
    }

    private static SKXamlCanvas CreateRenderTarget()
    {
        return new SKXamlCanvas
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent)
        };
    }

    public void Refresh()
    {
        _canvas.Invalidate();
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu.ShowContextMenu(position);
    }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        Plot.Render(e.Surface.Canvas, (int)e.Surface.Canvas.LocalClipBounds.Width, (int)e.Surface.Canvas.LocalClipBounds.Height);
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        Focus(FocusState.Pointer);

        Interaction.MouseDown(e.Pixel(this), e.ToButton(this));

        (sender as UIElement)?.CapturePointer(e.Pointer);

        base.OnPointerPressed(e);
    }

    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        Interaction.MouseUp(e.Pixel(this), e.ToButton(this));

        (sender as UIElement)?.ReleasePointerCapture(e.Pointer);

        base.OnPointerReleased(e);
    }

    private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        Interaction.OnMouseMove(e.Pixel(this));
        base.OnPointerMoved(e);
    }

    private void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        Interaction.DoubleClick();
        base.OnDoubleTapped(e);
    }

    private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        Interaction.MouseWheelVertical(e.Pixel(this), e.GetCurrentPoint(this).Properties.MouseWheelDelta);
        base.OnPointerWheelChanged(e);
    }

    private void OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        Interaction.KeyDown(e.Key());
        base.OnKeyDown(e);
    }

    private void OnKeyUp(object sender, KeyRoutedEventArgs e)
    {
        Interaction.KeyUp(e.Key());
        base.OnKeyUp(e);
    }

    public Coordinates GetCoordinates(Pixel px, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        return Plot.GetCoordinates(px, xAxis, yAxis);
    }

    public float DetectDisplayScale()
    {
        // TODO: improve support for DPI scale detection
        // https://github.com/ScottPlot/ScottPlot/issues/2760
        return 1.0f;
    }
}
