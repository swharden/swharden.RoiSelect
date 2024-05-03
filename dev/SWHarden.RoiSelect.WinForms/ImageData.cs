using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SWHarden.RoiSelect.WinForms;

public class ImageData
{
    public double[,] Values { get; }
    public int Width { get; }
    public int Height { get; }

    public ImageData(double[,] values)
    {
        Values = values;
        Width = values.GetLength(1);
        Height = values.GetLength(0);
    }

    public ImageData(Bitmap bmp)
    {
        Values = ImageToArray(bmp);
        Width = Values.GetLength(1);
        Height = Values.GetLength(0);
    }

    public double[,] GetValues(int x1, int x2, int y1, int y2)
    {
        int width = x2 - x1;
        int height = y2 - y1;

        double[,] values = new double[width, height];

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                values[y, x] = Values[y1 + y, x1 + x];
            }
        }

        return values;
    }

    private double[,] GetAutoscaledValues(double max = 255)
    {
        double[,] values = new double[Height, Width];

        double valueMax = double.MinValue;
        for (int i = 0; i < Height; i++)
            for (int j = 0; j < Width; j++)
                valueMax = Math.Max(valueMax, Values[i, j]);

        for (int i = 0; i < Height; i++)
            for (int j = 0; j < Width; j++)
                values[i, j] = Values[i, j] * max / valueMax;

        return values;
    }

    public Bitmap GetBitmap(bool autoscale = true)
    {
        double[,] values = autoscale ? GetAutoscaledValues() : Values;

        byte[,] byteValues = new byte[Height, Width];
        for (int i = 0; i < Height; i++)
            for (int j = 0; j < Width; j++)
                byteValues[i, j] = (byte)values[i, j];

        return ArrayToImage(byteValues);
    }

    static Bitmap ArrayToImage(byte[,] pixelArray)
    {
        int width = pixelArray.GetLength(1);
        int height = pixelArray.GetLength(0);
        int stride = (width % 4 == 0) ? width : width + 4 - width % 4;
        int bytesPerPixel = 3;

        byte[] bytes = new byte[stride * height * bytesPerPixel];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int offset = (y * stride + x) * bytesPerPixel;
                bytes[offset + 0] = pixelArray[y, x]; // blue
                bytes[offset + 1] = pixelArray[y, x]; // green
                bytes[offset + 2] = pixelArray[y, x]; // red
            }
        }

        PixelFormat formatOutput = PixelFormat.Format24bppRgb;
        Rectangle rect = new(0, 0, width, height);
        Bitmap bmp = new(stride, height, formatOutput);
        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, formatOutput);
        Marshal.Copy(bytes, 0, bmpData.Scan0, bytes.Length);
        bmp.UnlockBits(bmpData);

        Bitmap bmp2 = new(width, height, PixelFormat.Format32bppPArgb);
        Graphics gfx2 = Graphics.FromImage(bmp2);
        gfx2.DrawImage(bmp, 0, 0);

        return bmp2;
    }

    double[,] ImageToArray(Bitmap bmp)
    {
        int bytesPerPixel = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
        Rectangle rect = new(0, 0, bmp.Width, bmp.Height);
        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
        int byteCount = Math.Abs(bmpData.Stride) * bmp.Height;
        byte[] bytes = new byte[byteCount];
        Marshal.Copy(bmpData.Scan0, bytes, 0, byteCount);
        bmp.UnlockBits(bmpData);

        double[,] pixelValues = new double[bmp.Height, bmp.Width];
        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                int offset = (y * bmpData.Stride) + x * bytesPerPixel;
                byte r = bytes[offset + 2];
                byte g = bytes[offset + 1];
                byte b = bytes[offset + 0];
                pixelValues[y, x] = (r + g + b) / 3;
            }
        }

        return pixelValues;
    }
}
