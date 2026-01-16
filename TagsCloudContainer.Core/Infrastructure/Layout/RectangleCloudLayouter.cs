using ResultOf;
using SkiaSharp;

namespace TagsCloudContainer.Core.Infrastructure.Layout;
internal class RectangleCloudLayouter(
    SKPoint center,
    ArchimedeanSpiralPointGenerator generator)
{
    private readonly List<SKRect> rectangles = [];
    public Result<SKRect> PutNextRectangle(SKSize size, SKRect bounds, int maxAttempts = 10000)
    {
        if (size.Width <= 0 || size.Height <= 0)
            return Result.Fail<SKRect>("Rectangle size must be greater than zero");

        if (rectangles.Count != 0)
            return FindPlace(size, bounds, maxAttempts).Then(rect =>
            {
                var shifted = ShiftToCenter(rect);
                if (!IsInside(shifted, bounds))
                    return Result.Fail<SKRect>("Tag cloud does not fit the image");
                rectangles.Add(shifted);
                return Result.Ok(shifted);
            });
        var first = CreateRect(center, size);
        if (!IsInside(first, bounds))
            return Result.Fail<SKRect>("Tag cloud does not fit the image");
        rectangles.Add(first);
        return Result.Ok(first);

    }

    private Result<SKRect> FindPlace(SKSize size, SKRect bounds, int
        maxAttempts)
    {
        for (var i = 0; i < maxAttempts; i++)
        {
            var point = generator.GetNextPoint();
            var candidate = CreateRect(point, size);
            if (rectangles.All(r => !r.IntersectsWith(candidate)) &&
                IsInside(candidate, bounds))
                return Result.Ok(candidate);
        }
        return Result.Fail<SKRect>("Tag cloud does not fit the image");
    }


    private SKRect ShiftToCenter(SKRect rect)
    {
        while (true)
        {
            var direction = GetDirectionToCenter(rect);
            if (direction == SKPoint.Empty)
                return rect;

            var shifted = rect.OffsetClone(direction.X, direction.Y);
            if (rectangles.Any(r => r.IntersectsWith(shifted)))
                return rect;

            rect = shifted;
        }
    }
    
    private static bool IsInside(SKRect rect, SKRect bounds)
    {
        return rect.Left >= bounds.Left &&
               rect.Top >= bounds.Top &&
               rect.Right <= bounds.Right &&
               rect.Bottom <= bounds.Bottom;
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
