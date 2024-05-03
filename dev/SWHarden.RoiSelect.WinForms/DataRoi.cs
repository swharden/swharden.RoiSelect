namespace SWHarden.RoiSelect.WinForms;

// TODO: use primitive type for PixelRect
public readonly struct DataRoi(Rectangle rect, double[,] values)
{
    public Rectangle Rect { get; } = rect;
    public double[,] Values { get; } = values;

    public int Width => Rect.Width;
    public int Height => Rect.Height;

    public override string ToString() => $"{Rect} ({Values.Length} values)";

    public double[] Flatten()
    {
        int rows = Values.GetLength(0);
        int cols = Values.GetLength(1);
        double[] flat = new double[rows * cols];
        int index = 0;
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                flat[index++] = Values[i, j];
        return flat;
    }
}
