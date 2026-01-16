using SkiaSharp;

namespace TagsCloudContainer.Core.Infrastructure.Layout;
internal class ArchimedeanSpiralPointGenerator(
    SKPoint center,
    double angleStep = 0.1,
    double radiusStep = 0.5)
{
    private double _angle;

    public SKPoint GetNextPoint()
    {
        var radius = radiusStep * _angle;
        var x = center.X + (float)Math.Round(radius * Math.Cos(_angle));
        var y = center.Y + (float)Math.Round(radius * Math.Sin(_angle));
        _angle += angleStep;
        return new SKPoint(x, y);
    }
}