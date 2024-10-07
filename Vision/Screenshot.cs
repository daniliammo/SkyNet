using SixLabors.ImageSharp;

namespace SkyNet.Vision;


public static class Screenshot
{
    
    public static Image GetScreenshot()
    {
        #if LINUX
        switch (SkyNet.SessionType)
        {
            case XdgSessionType.Wayland:
                // if (WaylandScreenshot.IsStarted) return WaylandScreenshot.Screenshot;
                WaylandScreenshot.Init();
                return null;

            case XdgSessionType.X11:
                return X11Screenshot.GetScreenshot();
        }
        #endif
        
        #if !LINUX
        return WindowsScreenshot.GetScreenshot();
        #endif
        throw new Exception();
    }
    
}
