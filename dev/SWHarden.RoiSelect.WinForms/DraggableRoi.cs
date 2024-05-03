using System.Data;

namespace SWHarden.RoiSelect.WinForms;

public class DraggableRoi(float x1, float x2, float y1, float y2)
{
    private class DraggableEdge
    {
        public float Position { get; set; }
        public void Snap(float scale) => Position = (int)(Position / scale) * scale;
        public void MoveBy(float scale) => Position += scale;
    }

    private class DraggableVerticalEdge : DraggableEdge { }

    private class DraggableHorizontalEdge : DraggableEdge { }

    private class DraggableBody
    {
        public RectangleF StartDragRect { get; set; }
        public PointF StartDragPoint { get; set; }
    }

    private class DraggableHandle()
    {
        public DraggableVerticalEdge? VerticalEdge { get; set; }
        public DraggableHorizontalEdge? HorizontalEdge { get; set; }
        public DraggableBody? Body { get; set; }

    }

    private readonly DraggableVerticalEdge X1 = new() { Position = x1 };
    private readonly DraggableVerticalEdge X2 = new() { Position = x2 };
    private readonly DraggableHorizontalEdge Y1 = new() { Position = y1 };
    private readonly DraggableHorizontalEdge Y2 = new() { Position = y2 };

    public override string ToString() => $"X=[{XMin},{XMax}], y=[{YMin},{YMax}]";

    public bool IsSelected { get; set; } = false;

    private float XMin => Math.Min(X1.Position, X2.Position);
    private float XMax => Math.Max(X1.Position, X2.Position);
    private float YMin => Math.Min(Y1.Position, Y2.Position);
    private float YMax => Math.Max(Y1.Position, Y2.Position);
    private float Width => XMax - XMin;
    private float Height => YMax - YMin;
    private float XCenter => (X1.Position + X2.Position) / 2;
    private float YCenter => (Y1.Position + Y2.Position) / 2;

    private DraggableHandle? HandleBeingDragged { get; set; } = null;
    public bool IsDraggingHandle => HandleBeingDragged is not null;

    private DraggableHandle? HandleUnderMouse { get; set; } = null;
    public bool IsHandleUnderMouse => HandleUnderMouse is not null;

    public RectangleF GetRect() => new(XMin, YMin, Width, Height);

    public DataRoi GetDataRoi(SizeF controlSize, SizeF originalImageSize)
    {
        float scaleX = controlSize.Width / originalImageSize.Width;
        float scaleY = controlSize.Height / originalImageSize.Height;
        Rectangle rect = new((int)(XMin / scaleX), (int)(YMin / scaleY), (int)(Width / scaleX), (int)(Height / scaleY));
        double[,] values = new double[0, 0];
        return new DataRoi(rect, values);
    }

    public Cursor? GetCursor(float x, float y)
    {
        if (HandleUnderMouse is not null)
        {
            if (HandleUnderMouse.VerticalEdge is not null && HandleUnderMouse.HorizontalEdge is null)
            {
                return Cursors.SizeWE;
            }
            else if (HandleUnderMouse.VerticalEdge is null && HandleUnderMouse.HorizontalEdge is not null)
            {
                return Cursors.SizeNS;
            }
            else if (HandleUnderMouse.VerticalEdge is not null && HandleUnderMouse.HorizontalEdge is not null)
            {
                return Cursors.Hand;
            }
        }

        if (GetRect().Contains(x, y))
        {
            return Cursors.SizeAll;
        }

        return null;
    }

    public void MouseDown(float x, float y)
    {
        HandleBeingDragged = GetHandle(new(x, y));
    }

    public void MouseMove(float x, float y, SizeF maxSize)
    {
        if (HandleBeingDragged is not null)
        {
            x = Math.Max(0, x);
            x = Math.Min(x, maxSize.Width);
            y = Math.Max(0, y);
            y = Math.Min(y, maxSize.Height);

            if (HandleBeingDragged.VerticalEdge is not null)
                HandleBeingDragged.VerticalEdge.Position = x;

            if (HandleBeingDragged.HorizontalEdge is not null)
                HandleBeingDragged.HorizontalEdge.Position = y;


            if (HandleBeingDragged.Body is not null)
            {
                SizeF originalSize = new(Width, Height);

                float dX = x - HandleBeingDragged.Body.StartDragPoint.X;
                float dY = y - HandleBeingDragged.Body.StartDragPoint.Y;

                X1.Position = HandleBeingDragged.Body.StartDragRect.Left + dX;
                X2.Position = HandleBeingDragged.Body.StartDragRect.Right + dX;
                Y1.Position = HandleBeingDragged.Body.StartDragRect.Top + dY;
                Y2.Position = HandleBeingDragged.Body.StartDragRect.Bottom + dY;

                MoveInBounds(originalSize, maxSize);
            }

            return;
        }

        HandleUnderMouse = GetHandle(new(x, y));
    }

    public void Snap(float scaleX, float scaleY)
    {
        X1.Snap(scaleX);
        X2.Snap(scaleX);
        Y1.Snap(scaleY);
        Y2.Snap(scaleY);

        if (X1.Position == X2.Position)
        {
            HandleBeingDragged?.VerticalEdge?.MoveBy(scaleX);
        }

        if (Y1.Position == Y2.Position)
        {
            HandleBeingDragged?.HorizontalEdge?.MoveBy(scaleY);
        }
    }

    private void MoveInBounds(SizeF originalSize, SizeF maxSize)
    {
        if (XMin < 0)
        {
            X1.Position = 0;
            X2.Position = X1.Position + originalSize.Width;
        }

        if (XMax >= maxSize.Width)
        {
            X2.Position = maxSize.Width;
            X1.Position = X2.Position - originalSize.Width + 1;
        }

        if (YMin < 0)
        {
            Y1.Position = 0;
            Y2.Position = Y1.Position + originalSize.Height;
        }

        if (YMax >= maxSize.Height)
        {
            Y2.Position = maxSize.Height;
            Y1.Position = Y2.Position - originalSize.Height + 1;
        }
    }

    public void MouseUp(float x, float y)
    {
        HandleBeingDragged = null;
    }

    public PointF[] GetHandlePoints()
    {
        return [
            new(XMin, YMin), new(XCenter, YMin), new(XMax, YMin),
            new(XMin, YCenter), /*new(XCenter, YCenter),*/ new(XMax, YCenter),
            new(XMin, YMax), new(XCenter, YMax), new(XMax, YMax),
        ];
    }

    public Point[] GetPoints(float scaleX, float scaleY)
    {
        List<Point> points = [];

        int actualX1 = (int)(XMin / scaleX);
        int actualX2 = (int)(XMax / scaleX);
        int actualY1 = (int)(YMin / scaleY);
        int actualY2 = (int)(YMax / scaleY);

        for (int x = actualX1; x < actualX2; x++)
        {
            for (int y = actualY1; y < actualY2; y++)
            {
                points.Add(new Point(x, y));
            }
        }

        return [.. points];
    }

    private DraggableHandle? GetHandle(PointF pt, float pad = 5)
    {
        float[] xs = [XMin, XCenter, XMax];
        float[] ys = [YMin, YCenter, YMax];

        for (int x = 0; x < 3; x++)
        {
            bool isVertical = x != 1;
            for (int y = 0; y < 3; y++)
            {
                bool isHorizontal = y != 1;
                PointF handlePoint = new(xs[x], ys[y]);
                PointF rectLocation = new(handlePoint.X - pad, handlePoint.Y - pad);
                SizeF rectSize = new(pad * 2, pad * 2);
                RectangleF handleRect = new(rectLocation, rectSize);

                if (!handleRect.Contains(pt))
                    continue;

                DraggableHandle draggableHandle = new();

                if (isVertical)
                {
                    double d1 = Math.Abs(X1.Position - pt.X);
                    double d2 = Math.Abs(X2.Position - pt.X);
                    draggableHandle.VerticalEdge = (d1 <= d2) ? X1 : X2;
                }

                if (isHorizontal)
                {
                    double d1 = Math.Abs(Y1.Position - pt.Y);
                    double d2 = Math.Abs(Y2.Position - pt.Y);
                    draggableHandle.HorizontalEdge = (d1 <= d2) ? Y1 : Y2;
                }

                return draggableHandle;
            }
        }

        if (GetRect().Contains(pt))
        {
            return new DraggableHandle()
            {
                Body = new DraggableBody()
                {
                    StartDragRect = GetRect(),
                    StartDragPoint = pt,
                }
            };
        }

        return null;
    }
}
