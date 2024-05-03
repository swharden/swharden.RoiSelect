namespace SWHarden.RoiSelect.WinForms;

public class SingleRoiSelect : RoiSelectBase
{
    public override Panel Panel { get; } = new();
    public override PictureBox PictureBox { get; } = new();

    public SingleRoiSelect()
    {
        Panel.Controls.Add(PictureBox);
        Panel.Dock = DockStyle.Fill;
        Controls.Add(Panel);

        SizeChanged += (s, e) => UpdateSize();
        PictureBox.MouseDown += (s, e) => RoiCollection.MouseDown(e.X, e.Y);
        PictureBox.MouseUp += (s, e) => RoiCollection.MouseUp(e.X, e.Y);
        PictureBox.MouseMove += (s, e) =>
        {
            bool movedSomething = RoiCollection.MouseMove(e.X, e.Y);
            Cursor = RoiCollection.GetCursor(e.X, e.Y);
            if (movedSomething)
                UpdateImage();
        };
    }
}
