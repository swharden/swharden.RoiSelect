namespace SWHarden.RoiSelect.WinForms;

partial class MultiRoiSelect
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        btnAdd = new Button();
        btnDelete = new Button();
        tableLayoutPanel1 = new TableLayoutPanel();
        tableLayoutPanel2 = new TableLayoutPanel();
        tableLayoutPanel3 = new TableLayoutPanel();
        listView1 = new ListView();
        panel1 = new Panel();
        pictureBox1 = new PictureBox();
        tableLayoutPanel1.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        tableLayoutPanel3.SuspendLayout();
        panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // btnAdd
        // 
        btnAdd.Dock = DockStyle.Fill;
        btnAdd.Location = new Point(2, 2);
        btnAdd.Margin = new Padding(2);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(97, 28);
        btnAdd.TabIndex = 6;
        btnAdd.Text = "Add";
        btnAdd.UseVisualStyleBackColor = true;
        // 
        // btnDelete
        // 
        btnDelete.Dock = DockStyle.Fill;
        btnDelete.Location = new Point(103, 2);
        btnDelete.Margin = new Padding(2);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(97, 28);
        btnDelete.TabIndex = 5;
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
        tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
        tableLayoutPanel1.Controls.Add(panel1, 0, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Margin = new Padding(2);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Size = new Size(637, 308);
        tableLayoutPanel1.TabIndex = 8;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.ColumnCount = 1;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 1);
        tableLayoutPanel2.Controls.Add(listView1, 0, 0);
        tableLayoutPanel2.Dock = DockStyle.Fill;
        tableLayoutPanel2.Location = new Point(429, 2);
        tableLayoutPanel2.Margin = new Padding(2);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 2;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tableLayoutPanel2.Size = new Size(206, 304);
        tableLayoutPanel2.TabIndex = 0;
        // 
        // tableLayoutPanel3
        // 
        tableLayoutPanel3.ColumnCount = 2;
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.Controls.Add(btnDelete, 1, 0);
        tableLayoutPanel3.Controls.Add(btnAdd, 0, 0);
        tableLayoutPanel3.Location = new Point(2, 270);
        tableLayoutPanel3.Margin = new Padding(2);
        tableLayoutPanel3.Name = "tableLayoutPanel3";
        tableLayoutPanel3.RowCount = 1;
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.Size = new Size(202, 32);
        tableLayoutPanel3.TabIndex = 0;
        // 
        // listView1
        // 
        listView1.AutoArrange = false;
        listView1.Dock = DockStyle.Fill;
        listView1.FullRowSelect = true;
        listView1.HeaderStyle = ColumnHeaderStyle.None;
        listView1.LabelEdit = true;
        listView1.LabelWrap = false;
        listView1.Location = new Point(3, 3);
        listView1.Name = "listView1";
        listView1.Size = new Size(200, 262);
        listView1.TabIndex = 1;
        listView1.UseCompatibleStateImageBehavior = false;
        listView1.View = View.List;
        // 
        // panel1
        // 
        panel1.BackColor = SystemColors.ControlDarkDark;
        panel1.Controls.Add(pictureBox1);
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(2, 2);
        panel1.Margin = new Padding(2);
        panel1.Name = "panel1";
        panel1.Size = new Size(423, 304);
        panel1.TabIndex = 1;
        // 
        // pictureBox1
        // 
        pictureBox1.BackColor = SystemColors.ControlDark;
        pictureBox1.Location = new Point(108, 71);
        pictureBox1.Margin = new Padding(2);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(207, 161);
        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        pictureBox1.TabIndex = 5;
        pictureBox1.TabStop = false;
        // 
        // MultiRoiSelect
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel1);
        Margin = new Padding(2);
        Name = "MultiRoiSelect";
        Size = new Size(637, 308);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel3.ResumeLayout(false);
        panel1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
    }

    #endregion
    private Button btnAdd;
    private Button btnDelete;
    private TableLayoutPanel tableLayoutPanel1;
    private TableLayoutPanel tableLayoutPanel2;
    private TableLayoutPanel tableLayoutPanel3;
    private Panel panel1;
    private PictureBox pictureBox1;
    private ListView listView1;
}
