using Cairo;
using Gtk;
using Window = Gtk.Window;

namespace SkyNet;

public static class WindowController
{

    private static Context? _context;
    private static Widget? _widget;
    
    
    public static void Main()
    {
        Application.Init();

        // Создаем окно с заголовком
        var window = new Window($"SkyNet, Window backend: Gtk# v3, AI Backend: YOLOV10, Cython, Screenshot Backend: Gnome portal, C#, {SkyNet.SessionType}");
        window.SetDefaultSize(800, 600);  // Уменьшите размер окна для тестирования
        window.SetPosition(WindowPosition.Center);
        window.DeleteEvent += delegate { Application.Quit(); };

        // Настройка параметров окна
        window.AppPaintable = true;
        window.ScreenChanged += ScreenChanged;
        window.Drawn += ExposeDraw;

        // Позволяем заголовок (управление окна)
        window.Decorated = true;
        window.ButtonPressEvent += Clicked;

        // Добавление KeyPressEvent
        window.KeyPressEvent += KeyPressed;

        // Настройка контейнера
        var fixedContainer = new Fixed();
        window.Add(fixedContainer);
        
        var button = new Button("button1");
        button.SetSizeRequest(52, 52);
        fixedContainer.Put(button, 52, 52);
        
        window.ShowAll();
        Application.Run();
    }
    
    private static void ScreenChanged(object o, ScreenChangedArgs args)
    {
        _widget = o as Widget;
        var screen = _widget!.Window.Screen;

        // Проверяем, поддерживает ли экран альфа-канал
        Console.WriteLine("Your screen does not support alpha channels!");

        _widget.Visual = screen.SystemVisual;
    }

    private static void ExposeDraw(object sender, DrawnArgs args)
    {
        _widget = sender as Widget;
        _context = Gdk.CairoHelper.Create(_widget.GdkWindow);

        // Установим прозрачный фон
        _context.SetSourceRGBA(0.0, 0.0, 0.0, 0.0);
        _context.Paint();
        
        DrawCircles();
        
        // foreach (var visionObject in SkyNet.VisionObjects!)
        //     DrawObject(visionObject);
        
        // Завершить рисование
        _context.Dispose();
    }

    private static void DrawCircles()
    {
        // Цвет круга (зеленый)
        _context!.SetSourceRGB(0.0, 1.0, 0.0);
        
        _context.NewPath();
        _context.Arc(0, 0, 15, 0, 2 * Math.PI);
        _context.FillPreserve();
        
        _context.NewPath();
        _context.Arc(_widget!.Window.Width, _widget.Window.Height, 15, 0, 2 * Math.PI);
        _context.FillPreserve();
        
        _context.NewPath();
        _context.Arc(_widget.Window.Width, 0, 15, 0, 2 * Math.PI);
        _context.FillPreserve();
        
        _context.NewPath();
        _context.Arc(0, _widget.Window.Height, 15, 0, 2 * Math.PI);
        _context.FillPreserve();
    }
    
    private static void DrawObject(VisionObject visionObject)
    {
        _context!.SetSourceRGB(visionObject.StrokeColor.R, visionObject.StrokeColor.G, visionObject.StrokeColor.B);
        DrawBox(visionObject.Transform, visionObject.StrokeColor);
    }
    
    private static void DrawBox(Transform transform, System.Drawing.Color color)
    {
        _context!.SetSourceRGB(color.R, color.G, color.B);
        _context.Rectangle(transform.Position.X, transform.Position.Y, transform.Scale.X, transform.Scale.Y); // Рисуем квадрат
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