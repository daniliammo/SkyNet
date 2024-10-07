using System.Diagnostics.Contracts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SkyNet.Vision;


public static class ResizeImage
{
    [Pure]
    public static Image ResizeWithConfig(this Image image)
    {
        // KnownResamplers.NearestNeighbor наверное самый быстрый метод
        image.Mutate(x => x.Resize((int)Math.Floor(image.Width / Config.ScaleFactor), (int)Math.Floor(image.Height / Config.ScaleFactor), KnownResamplers.NearestNeighbor));
        return image;
    }
}
