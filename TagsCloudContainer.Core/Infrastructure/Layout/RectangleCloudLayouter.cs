using SkiaSharp;

namespace TagsCloudContainer.Core.Infrastructure.Layout;
internal class RectangleCloudLayouter(
    SKPoint center,
    ArchimedeanSpiralPointGenerator generator)
{
    private readonly List<SKRect> _rectangles = [];
    public SKRect PutNextRectangle(SKSize size)
    {
        if (size.Width <= 0 || size.Height <= 0)
            throw new ArgumentException("Rectangle size must be greater than zero");

        var rect = _rectangles.Count == 0
            ? CreateRect(center, size)
            : ShiftToCenter(FindPlace(size));

        _rectangles.Add(rect);
        return rect;
    }

    private SKRect FindPlace(SKSize size)
    {
        while (true)
        {
            var point = generator.GetNextPoint();
            var candidate = CreateRect(point, size);
            if (_rectangles.All(r => !r.IntersectsWith(candidate)))
                return candidate;
        }
    }

    private SKRect ShiftToCenter(SKRect rect)
    {
        while (true)
        {
            var direction = GetDirectionToCenter(rect);
            if (direction == SKPoint.Empty)
                return rect;

            var shifted = rect.OffsetClone(direction.X, direction.Y);
            if (_rectangles.Any(r => r.IntersectsWith(shifted)))
                return rect;

            rect = shifted;
        }
    }

    private SKPoint GetDirectionToCenter(SKRect rect)
    {
        var center1 = new SKPoint((rect.Left + rect.Right) / 2f, (rect.Top + rect.Bottom) / 2f);
        var dx = center1.X > center.X ? -1 : center1.X < center.X ? 1 : 0;
        var dy = center1.Y > center.Y ? -1 : center1.Y < center.Y ? 1 : 0;
        return new SKPoint(dx, dy);
    }

    private static SKRect CreateRect(SKPoint center, SKSize size)
    {
        var left = center.X - size.Width / 2;
        var top = center.Y - size.Height / 2;
        return new SKRect(left, top, left + size.Width, top + size.Height);
    }
}

internal static class SkRectExtensions
{
    public static SKRect OffsetClone(this SKRect rect, float dx, float dy) =>
        new(rect.Left + dx, rect.Top + dy, rect.Right + dx, rect.Bottom + dy);
}
