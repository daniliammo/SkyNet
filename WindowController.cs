using Cairo;
using GLib;
using Gtk;
using SkyNet.Vision;
using Window = Gtk.Window;
using SkyNet.Vision.Objects;
using Application = Gtk.Application;
using Color = Cairo.Color;
using Rectangle = SixLabors.ImageSharp.Rectangle;

namespace SkyNet;


public static class WindowController
{
    
    private static Context _context;
    private static Widget _widget;
    
    
    private static void Main()
    {
        Application.Init();
        
        SkyNet.Init();
        
        // Создаем окно с заголовком
        var window = new Window($"SkyNet, Window backend: Gtk# v3 and Cairo, AI Backend: YOLOV10, Cython, Screenshot Backend: Gnome portal, C#, {SkyNet.SessionType}");
        window.SetDefaultSize(800, 600);
        
        // window.Icon = new Pixbuf(new FileStream("windowIcon.ico", FileMode.Open));
        window.DeleteEvent += delegate { Application.Quit(); };
        window.DeleteEvent += delegate { WaylandScreenshot.Stop(); };
        
        // Настройка параметров окна
        window.AppPaintable = true;
        
        // window.ScreenChanged += ScreenChanged;
        window.Drawn += ExposeDraw;
        
        window.ButtonPressEvent += Clicked;

        // Добавление KeyPressEvent
        window.KeyPressEvent += KeyPressed;
        
        window.ShowAll();
        Application.Run();
    }
    
    private static void ScreenChanged(object o, ScreenChangedArgs args)
    {
        _widget = o as Widget;
        var screen = _widget!.Window.Screen;

        // _widget.Visual = screen.SystemVisual;
    }

    private static void ExposeDraw(object sender, DrawnArgs args)
    {
        if (_widget != null)
        {
            DrawAllObjects();
            return;
        }
        _widget = sender as Widget;
        _context = Gdk.CairoHelper.Create(_widget!.Window);

        // Установим прозрачный фон
        _context.SetSourceColor(new Color(0, 0, 0, 0));
        _context.Paint();
        
        DrawScreenBounds();
        
        // Завершить рисование
        _context.Dispose();
        // Console.WriteLine("called");
    }

    public static void DrawAllObjects()
    {
        _context = Gdk.CairoHelper.Create(_widget!.Window);

        // Установим прозрачный фон
        _context.SetSourceColor(new Color(0, 0, 0, 0));
        _context.Paint();
        
        DrawScreenBounds();
        DrawCircle(new Vector2(0, 0), 15);
        
        foreach (var visionObject in SkyNet.VisionObjects!)
            DrawObject(visionObject);
        
        _context!.Dispose();
    }
    
    private static void DrawCrosshair(Vector2 position)
    {
        _context!.SetSourceColor(Config.CrosshairColor);
        
        DrawCircle(position, 7);
    }
    
    private static void DrawScreenBounds()
    {
        // Цвет круга (зеленый)
        _context!.SetSourceColor(new Color(0, 1, 0));
        
        DrawCircle(new Vector2(0, 0), 20);
        DrawCircle(new Vector2(_widget!.Window.Width, 0), 20);
        DrawCircle(new Vector2(_widget.Window.Width, _widget.Window.Height), 20);
        DrawCircle(new Vector2(0, _widget.Window.Height), 20);
    }

    private static void DrawCircle(Vector2 position, double radius)
    {
        _context!.NewPath();
        _context.Arc(position.X, position.Y, radius, 0, 2 * Math.PI);
        _context.FillPreserve();
    }
    
    private static void DrawText(string text, Vector2 position, Color color)
    {
        _context!.NewPath();
        // Установить шрифт и размер
        _context!.SetFontSize(Config.FontSize); // Размер шрифта 
        _context.MoveTo(position.X, position.Y); // Позиция для текста
    
        // Получить ширину текста для центрирования
        var textExtents = _context.TextExtents(text);
        _context.MoveTo(position.X - textExtents.Width + 50, position.Y + textExtents.Height - 50); // Вертикально центрируем текст
    
        // Цвет текста (черный)
        _context.SetSourceColor(color.Invert());
        _context.ShowText(text); // Отрисовать текст
    }

    private static Color Invert(this Color color) => new(255 - color.R, 255 - color.G, 255 - color.B);
    
    private static void DrawObject(VisionObject visionObject)
    {
        if(visionObject.ObjectType == ObjectType.GreenLaser || visionObject.ObjectType == ObjectType.RedLaser && Config.DrawCrosshair)
        {
            DrawCrosshair(new Vector2(visionObject.Bounds.Width, visionObject.Bounds.Height));
            return;
        }
        
        _context!.SetSourceColor(visionObject.StrokeColor);
        DrawBox(visionObject.Bounds, visionObject.StrokeColor);
        DrawText($"{visionObject.ObjectType.ToString()} {visionObject.Percents}", new Vector2(visionObject.Bounds.X, visionObject.Bounds.Y), visionObject.StrokeColor);
    }
    
    private static void DrawBox(Rectangle bounds, Color color)
    {
        _context!.NewPath();
        _context.SetSourceColor(color);
        _context.Rectangle(bounds.Location.X, bounds.Location.Y, bounds.Width, bounds.Height); // Рисуем квадрат
        _context.Stroke(); // Обводим квадрат
    }
    
    private static void Clicked(object sender, ButtonPressEventArgs args)
    {
        var window = sender as Window;
        window!.Decorated = !window.Decorated; // Переключение оконных рамок
    }

    private static void KeyPressed(object sender, KeyPressEventArgs args)
    {
        // Проверяем, была ли нажата клавиша F3
        if (args.Event.Key == Gdk.Key.F3)
        {
            var window = sender as Window;
            window!.Iconify(); // Свести окно
            args.RetVal = true; // Сообщаем, что событие обработано
        }
    }
    
}
