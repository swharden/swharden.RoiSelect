using System.Text;

namespace SWHarden.RoiSelect.WinForms;

public class DraggableRoiCollection()
{
    // TODO: these should be data ROIs
    public readonly List<DraggableRoi> ROIs = [];
    public RoiBitmap? RoiBitmap { get; set; } = null;

    private DraggableRoi? RoiUnderMouse = null;
    public bool IsRoiUnderMouse => RoiUnderMouse is not null;
    public bool IsDraggingHandle => RoiUnderMouse is not null && RoiUnderMouse.IsDraggingHandle;
    public bool SnapToPixels { get; set; } = true;


    public EventHandler<DataRoi> SelectedRoiChanged = delegate { };

    public DraggableRoi GetCenterRoi(SizeF controlSize, SizeF originalImageSize, int radius)
    {
        float centerX = controlSize.Width / 2;
        float centerY = controlSize.Height / 2;
        DraggableRoi roi = new(centerX - radius, centerX + radius, centerY - radius, centerY + radius);

        float scaleX = controlSize.Width / originalImageSize.Width;
        float scaleY = controlSize.Height / originalImageSize.Height;
        roi.Snap(scaleX, scaleY);
        return roi;
    }

    public DraggableRoi GetRandomRoi()
    {
        if (RoiBitmap is null)
            throw new InvalidOperationException();

        float x1 = Random.Shared.Next(0, RoiBitmap.OriginalWidth - 1) * RoiBitmap.ScaleX;
        float x2 = Random.Shared.Next(0, RoiBitmap.OriginalWidth - 1) * RoiBitmap.ScaleX;
        float y1 = Random.Shared.Next(0, RoiBitmap.OriginalHeight - 1) * RoiBitmap.ScaleY;
        float y2 = Random.Shared.Next(0, RoiBitmap.OriginalHeight - 1) * RoiBitmap.ScaleY;
        DraggableRoi roi = new(x1, x2, y1, y2);
        return roi;
    }

    public void SetImage(Bitmap bmp)
    {
        RoiBitmap? oldBmp = RoiBitmap;
        RoiBitmap = new(bmp);
        oldBmp?.Dispose();
    }

    public void SetImage(double[,] values)
    {
        RoiBitmap? oldBmp = RoiBitmap;
        RoiBitmap = new(values);
        oldBmp?.Dispose();
    }

    public Bitmap? GetBitmap(Size size)
    {
        if (RoiBitmap is null)
            return null!;

        return RoiBitmap.GetBitmap(size, this);
    }

    public Cursor GetCursor(float x, float y)
    {
        foreach (DraggableRoi roi in ROIs)
        {
            if (!roi.IsSelected) // only interact with selected ROIs
                continue;

            Cursor? roiCursor = roi.GetCursor(x, y);

            if (roiCursor is not null)
                return roiCursor;
        }

        return Cursors.Arrow;
    }

    public void SnapAfterResizing(Size newSize)
    {
        if (RoiBitmap is null)
            return;

        RoiBitmap.OutputImageSize = newSize;
        foreach (var roi in ROIs)
        {
            roi.Snap(RoiBitmap.ScaleX, RoiBitmap!.ScaleY);
        }
    }

    public bool MouseMove(float x, float y)
    {
        if (RoiBitmap is null)
            return false;

        foreach (DraggableRoi roi in ROIs)
        {
            if (!roi.IsSelected) // only interact with selected ROIs
                continue;

            string before = roi.ToString();
            roi.MouseMove(x, y, RoiBitmap.OutputImageSize);

            if (roi.IsDraggingHandle)
            {
                roi.Snap(RoiBitmap.ScaleX, RoiBitmap.ScaleY);
            }

            if (roi.IsHandleUnderMouse)
            {
                RoiUnderMouse = roi;
                string after = roi.ToString();
                bool roiChanged = before != after;

                if (roiChanged)
                {
                    RectangleF dataRect = roi.GetRect();
                    DataRoi roi2 = RoiBitmap.GetDataRoi(dataRect);
                    SelectedRoiChanged.Invoke(this, roi2);
                }

                return roiChanged;
            }
        }

        RoiUnderMouse = null;
        return false;
    }

    public void MouseDown(float x, float y)
    {
        RoiUnderMouse?.MouseDown(x, y);
    }

    public void MouseUp(float x, float y)
    {
        RoiUnderMouse?.MouseUp(x, y);
    }
}
