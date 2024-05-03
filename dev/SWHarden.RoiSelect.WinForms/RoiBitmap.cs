using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SWHarden.RoiSelect.WinForms;

public class RoiBitmap : IDisposable
{
    private Bitmap OriginalImage { get; }
    private ImageData ImageData { get; }

    public Size OutputImageSize { get; set; }

    public int OriginalWidth => OriginalImage.Width;
    public int OriginalHeight => OriginalImage.Height;
    public Size OriginalSize => new(OriginalWidth, OriginalHeight);

    public float ScaleX => (float)OutputImageSize.Width / OriginalImage.Width;
    public float ScaleY => (float)OutputImageSize.Height / OriginalImage.Height;

    public bool HighlightPixels = true;

    public RoiBitmap(Bitmap bmp)
    {
        ImageData = new(bmp);
        OriginalImage = bmp;
    }

    public RoiBitmap(double[,] values)
    {
        ImageData = new(values);
        OriginalImage = ImageData.GetBitmap();
    }

    public Bitmap GetBitmap(Size size, DraggableRoiCollection roiCollection)
    {
        OutputImageSize = size;

        Bitmap bmp = new(size.Width, size.Height);
        using Graphics gfx = Graphics.FromImage(bmp);
        gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
        gfx.PixelOffsetMode = PixelOffsetMode.Half;

        Rectangle targetRect = new(0, 0, size.Width, size.Height);
        gfx.DrawImage(OriginalImage, targetRect);

        Color highlightColor = Color.FromArgb(30, Color.Yellow);
        using Brush highlightBrush = new SolidBrush(highlightColor);

        foreach (DraggableRoi roi in roiCollection.ROIs)
        {
            if (HighlightPixels && roi.IsSelected)
            {
                RectangleF[] rects = roi.GetPoints(ScaleX, ScaleY)
                    .Select(pt => new RectangleF(pt.X * ScaleX, pt.Y * ScaleY, ScaleX, ScaleY))
                    .ToArray();

                if (rects.Length > 0)
                    gfx.FillRectangles(highlightBrush, rects);
            }

            gfx.DrawRectangle(Pens.Yellow, roi.GetRect());

            if (roi.IsSelected)
            {
                foreach (PointF pt in roi.GetHandlePoints())
                {
                    DrawHandle(gfx, pt);
                }
            }
        }

        return bmp;
    }

    private static void DrawHandle(Graphics gfx, PointF pt)
    {
        const int CornerBlackRadius = 5;
        const int CornerWhiteRadius = 3;

        gfx.FillRectangle(
            Brushes.Black,
            pt.X - CornerBlackRadius,
            pt.Y - CornerBlackRadius,
            CornerBlackRadius * 2,
            CornerBlackRadius * 2);

        gfx.FillRectangle(
            Brushes.White,
            pt.X - CornerWhiteRadius,
            pt.Y - CornerWhiteRadius,
            CornerWhiteRadius * 2,
            CornerWhiteRadius * 2);
    }

    public void Dispose()
    {
        OriginalImage.Dispose();
        GC.SuppressFinalize(this);
    }

    public DataRoi GetDataRoi(RectangleF rect)
    {
        int x1 = (int)(rect.Left / ScaleX);
        int x2 = (int)(rect.Right / ScaleX);
        int width = x2 - x1;
        int y1 = (int)(rect.Top / ScaleY);
        int y2 = (int)(rect.Bottom / ScaleY);
        int height = y2 - y1;
        Rectangle rect2 = new(x1, y1, width, height);

        x1 = Math.Clamp(x1, 0, ImageData.Width - 1);
        x2 = Math.Clamp(x2, 0, ImageData.Width - 1);
        y1 = Math.Clamp(y1, 0, ImageData.Height - 1);
        y2 = Math.Clamp(y2, 0, ImageData.Height - 1);

        double[,] values = ImageData.GetValues(x1, x2, y1, y2);
        return new DataRoi(rect2, values);
    }
}
