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
        _context = Gdk.CairoHelper.Create(_widget!.Window);

        // Установим прозрачный фон
        _context.SetSourceRGBA(0.0, 0.0, 0.0, 0.0);
        _context.Paint();
        
        DrawScreenBounds();

        SkyNet.VisionObjects =
        [
            new VisionObject(ObjectType.Ally, new Transform(new Vector2(500, 356), new Vector2(100, 52)))
            {
                Transform = new Transform(new Vector2(760, 456), new Vector2(50, 180))
            }
        ];

        if(Config.DrawCrosshair)
            DrawCrosshair();
        
        foreach (var visionObject in SkyNet.VisionObjects!)
            DrawObject(visionObject);
        
        // Завершить рисование
        _context.Dispose();
    }

    private static void DrawCrosshair()
    {
        _context!.SetSourceColor(Config.CrosshairColor);
        
        DrawCircle(new Vector2(_widget!.Window.Width / 2, _widget!.Window.Height / 2), 7);
    }
    
    private static void DrawScreenBounds()
    {
        // Цвет круга (зеленый)
        _context!.SetSourceRGB(0.0, 1.0, 0.0);
        
        DrawCircle(new Vector2(0, 0), 15);
        DrawCircle(new Vector2(_widget!.Window.Width, 0), 15);
        DrawCircle(new Vector2(_widget.Window.Width, _widget.Window.Height), 15);
        DrawCircle(new Vector2(0, _widget.Window.Height), 15);
    }

    private static void DrawCircle(Vector2 position, double radius)
    {
        _context!.NewPath();
        _context.Arc(position.X, position.Y, radius, 0, 2 * Math.PI);
        _context.FillPreserve();
    }
    
    private static void DrawText(string text, double x, double y)
    {
        // Установить шрифт и размер
        _context!.SetFontSize(32); // Размер шрифта 40
        _context.MoveTo(x, y); // Позиция для текста
    
        // Получить ширину текста для центрирования
        var textExtents = _context.TextExtents(text);
        // Задать точку для отрисовки текста 1/2 ширины текста влево от заданной точки
        _context.MoveTo(x - textExtents.Width / 2, y + textExtents.Height / 2); // Вертикально центрируем текст
    
        // Цвет текста (черный)
        _context.SetSourceRGB(0.0, 0.0, 0.0); // Черный цвет
        _context.ShowText(text); // Отрисовать текст
    }
    
    private static void DrawObject(VisionObject visionObject)
    {
        _context!.SetSourceColor(visionObject.StrokeColor);
        DrawBox(visionObject.Transform, visionObject.StrokeColor);
        DrawText(visionObject.ObjectType.ToString(), visionObject.Transform.Position.X, visionObject.Transform.Position.Y * 0.925f);
    }
    
    private static void DrawBox(Transform transform, Color color)
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