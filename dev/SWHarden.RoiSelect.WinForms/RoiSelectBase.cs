using System;
using System.Text;

namespace SWHarden.RoiSelect.WinForms;

public class RoiSelectBase : UserControl
{
    public virtual Panel Panel { get; } = new Panel();
    public virtual PictureBox PictureBox { get; } = new PictureBox();

    public readonly DraggableRoiCollection RoiCollection = new();

    private readonly System.Windows.Forms.Timer UpdateTimer = new() { Interval = 20 };

    private bool UpdateNeeded = false;
    public int RenderCount { get; private set; }

    public RoiSelectBase()
    {
        if (!DesignMode)
        {
            UpdateTimer.Tick += (s, e) => UpdateImageIfNeeded();
            UpdateTimer.Start();
        }
    }

    public void UpdateSize()
    {
        // TODO: support non-square images
        int minEdge = Math.Min(Panel.Width, Panel.Height);
        PictureBox.Size = new(minEdge, minEdge);
        PictureBox.Location = new(0, 0);
        RoiCollection.SnapAfterResizing(PictureBox.Size);
        UpdateImage();
    }

    public void UpdateImage()
    {
        UpdateNeeded = true;
    }

    public void UpdateImageIfNeeded()
    {
        if (!UpdateNeeded)
            return;
        UpdateNeeded = false;

        Image? oldImage = PictureBox.Image;
        PictureBox.Image = RoiCollection.GetBitmap(PictureBox.Size);
        oldImage?.Dispose();
    }

    public void SetImage(Bitmap bmp)
    {
        RoiCollection.SetImage(bmp);
        UpdateImage();
    }

    public void SetImage(double[,] values)
    {
        RoiCollection.SetImage(values);
        UpdateImage();
    }

    public void AddCenterRoi()
    {
        if (RoiCollection is null || RoiCollection.RoiBitmap is null)
            return;

        SizeF originalSize = RoiCollection.RoiBitmap.OriginalSize;
        DraggableRoi roi = RoiCollection.GetCenterRoi(PictureBox.Size, originalSize, 20);
        roi.IsSelected = true;
        RoiCollection.ROIs.Add(roi);
        UpdateImage();
    }

    public DataRoi GetDataRoi(int roiIndex)
    {
        if (RoiCollection is null || RoiCollection.RoiBitmap is null)
            throw new InvalidOperationException();

        return RoiCollection.ROIs[roiIndex].GetDataRoi(PictureBox.Size, RoiCollection.RoiBitmap.OriginalSize);
    }

    public void AddDataRoi(DataRoi roi)
    {
        if (RoiCollection is null || RoiCollection.RoiBitmap is null)
            throw new InvalidOperationException();

        float scaleX = (float)PictureBox.Size.Width / RoiCollection.RoiBitmap.OriginalSize.Width;
        float scaleY = (float)PictureBox.Size.Height / RoiCollection.RoiBitmap.OriginalSize.Height;

        DraggableRoi roi2 = new(
            x1: roi.Rect.Left * scaleX,
            x2: roi.Rect.Right * scaleY,
            y1: roi.Rect.Bottom * scaleX,
            y2: roi.Rect.Top * scaleY);

        RoiCollection.ROIs.Add(roi2);
        UpdateImage();
    }

    public void LoadRois(string file)
    {
        if (!File.Exists(file))
            return;

        RoiCollection.ROIs.Clear();
        string[] lines = File.ReadAllLines(file);
        foreach (string line in lines)
        {
            if (!line.StartsWith("ROI"))
                continue;
            string[] parts = line.Split(",");
            int x = int.Parse(parts[1]);
            int y = int.Parse(parts[2]);
            int w = int.Parse(parts[3]);
            int h = int.Parse(parts[4]);
            Rectangle rect = new(x, y, w, h);
            DataRoi roi = new(rect, new double[,] { });
            AddDataRoi(roi);
        }

        RoiCollection.ROIs.First().IsSelected = true;
        UpdateImage();
    }

    public void SaveRois(string folder)
    {
        StringBuilder sb = new();
        sb.AppendLine("Name,X,Y,Width,Height");
        for (int i = 0; i < RoiCollection.ROIs.Count; i++)
        {
            DataRoi roi = GetDataRoi(i);
            sb.AppendLine($"ROI{i = 1},{roi.Rect.X},{roi.Rect.Y},{roi.Rect.Width},{roi.Rect.Height}");
        }
        string saveAs = Path.Join(folder, "rois.csv");
        File.WriteAllText(saveAs, sb.ToString());
    }
}
