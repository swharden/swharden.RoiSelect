namespace SWHarden.RoiSelect.WinForms;

public partial class MultiRoiSelect : RoiSelectBase
{
    public override Panel Panel => panel1;
    public override PictureBox PictureBox => pictureBox1;

    public MultiRoiSelect()
    {
        InitializeComponent();

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

        btnAdd.Click += (s, e) =>
        {
            AddCenterRoi();
            listView1.Items.Add($"Roi #{RoiCollection.ROIs.Count}");
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Selected = i == listView1.Items.Count - 1;
            }
        };

        btnDelete.Click += (s, e) =>
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            int index = listView1.SelectedItems[0].Index;
            RoiCollection.ROIs.RemoveAt(index);
            listView1.Items.RemoveAt(index);
        };

        listView1.SelectedIndexChanged += (s, e) =>
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                RoiCollection.ROIs[i].IsSelected = listView1.Items[i].Selected;
            }

            UpdateImage();
        };
    }
}
