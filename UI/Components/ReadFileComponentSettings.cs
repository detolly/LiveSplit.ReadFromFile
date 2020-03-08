using LiveSplit.Model;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    public partial class ReadFileComponentSettings : UserControl
    {
        public Color TextColor { get; set; }
        public bool OverrideTextColor { get; set; }
        public Color TimeColor { get; set; }
        public bool OverrideTimeColor { get; set; }

        public Color BackgroundColor { get; set; }
        public Color BackgroundColor2 { get; set; }
        public GradientType BackgroundGradient { get; set; }
        public string GradientString
        {
            get { return BackgroundGradient.ToString(); }
            set { BackgroundGradient = (GradientType)Enum.Parse(typeof(GradientType), value); }
        }

        public string Text1 { get; set; }
        public string Text2 { get; set; }
        private string fileLocation { get; set; }

        public Font Font1 { get; set; }
        public string Font1String { get { return string.Format("{0} {1}", Font1.FontFamily.Name, Font1.Style); } }
        public bool OverrideFont1 { get; set; }
        public Font Font2 { get; set; }
        public string Font2String { get { return string.Format("{0} {1}", Font2.FontFamily.Name, Font2.Style); } }
        public bool OverrideFont2 { get; set; }

        public LayoutMode Mode { get; set; }
        public bool Display2Rows { get; set; }

        public LiveSplitState CurrentState { get; set; }

        public ReadFileComponentSettings()
        {
            InitializeComponent();

            TextColor = Color.FromArgb(255, 255, 255);
            OverrideTextColor = false;
            TimeColor = Color.FromArgb(255, 255, 255);
            OverrideTimeColor = false;
            BackgroundColor = Color.Transparent;
            BackgroundColor2 = Color.Transparent;
            BackgroundGradient = GradientType.Plain;
            Text1 = "Latest Follower";
            Text2 = "tSparkles";
            OverrideFont1 = false;
            OverrideFont2 = false;
            Font1 = new Font("Segoe UI", 13, FontStyle.Regular, GraphicsUnit.Pixel);
            Font2 = new Font("Segoe UI", 13, FontStyle.Regular, GraphicsUnit.Pixel);

            chkOverrideTextColor.DataBindings.Add("Checked", this, "OverrideTextColor", false, DataSourceUpdateMode.OnPropertyChanged);
            btnTextColor.DataBindings.Add("BackColor", this, "TextColor", false, DataSourceUpdateMode.OnPropertyChanged);
            chkOverrideTimeColor.DataBindings.Add("Checked", this, "OverrideTimeColor", false, DataSourceUpdateMode.OnPropertyChanged);
            btnTimeColor.DataBindings.Add("BackColor", this, "TimeColor", false, DataSourceUpdateMode.OnPropertyChanged);
            lblFont.DataBindings.Add("Text", this, "Font1String", false, DataSourceUpdateMode.OnPropertyChanged);
            lblFont2.DataBindings.Add("Text", this, "Font2String", false, DataSourceUpdateMode.OnPropertyChanged);

            cmbGradientType.SelectedIndexChanged += cmbGradientType_SelectedIndexChanged;
            cmbGradientType.DataBindings.Add("SelectedItem", this, "GradientString", false, DataSourceUpdateMode.OnPropertyChanged);
            btnColor1.DataBindings.Add("BackColor", this, "BackgroundColor", false, DataSourceUpdateMode.OnPropertyChanged);
            btnColor2.DataBindings.Add("BackColor", this, "BackgroundColor2", false, DataSourceUpdateMode.OnPropertyChanged);
            txtOne.DataBindings.Add("Text", this, "Text1");
            chkFont.DataBindings.Add("Checked", this, "OverrideFont1", false, DataSourceUpdateMode.OnPropertyChanged);
            chkFont2.DataBindings.Add("Checked", this, "OverrideFont2", false, DataSourceUpdateMode.OnPropertyChanged);
            var t = new System.Threading.Thread(() =>
            {
                while(true)
                {
                    if (fileLocation?.Length > 1)
                    {
                        try
                        {
                            Text2 = System.IO.File.ReadAllText(fileLocation);
                        } catch
                        {
                            Text2 = "ERRPOR: FailedToRead";
                        }
                    }
                    System.Threading.Thread.Sleep(2000);
                }
            });
            t.Start();
        }

        void chkFont2_CheckedChanged(object sender, EventArgs e)
        {
            label7.Enabled = lblFont2.Enabled = btnFont2.Enabled = chkFont2.Checked;
        }

        void chkFont_CheckedChanged(object sender, EventArgs e)
        {
            label5.Enabled = lblFont.Enabled = btnFont.Enabled = chkFont.Checked;
        }

        void chkOverrideTimeColor_CheckedChanged(object sender, EventArgs e)
        {
            label2.Enabled = btnTimeColor.Enabled = chkOverrideTimeColor.Checked;
        }

        void chkOverrideTextColor_CheckedChanged(object sender, EventArgs e)
        {
            label1.Enabled = btnTextColor.Enabled = chkOverrideTextColor.Checked;
        }

        void TextComponentSettings_Load(object sender, EventArgs e)
        {
            chkOverrideTextColor_CheckedChanged(null, null);
            chkOverrideTimeColor_CheckedChanged(null, null);
            chkFont_CheckedChanged(null, null);
            chkFont2_CheckedChanged(null, null);
            if (Mode == LayoutMode.Horizontal)
            {
                chkTwoRows.Enabled = false;
                chkTwoRows.DataBindings.Clear();
                chkTwoRows.Checked = true;
            }
            else
            {
                chkTwoRows.Enabled = true;
                chkTwoRows.DataBindings.Clear();
                chkTwoRows.DataBindings.Add("Checked", this, "Display2Rows", false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        void cmbGradientType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnColor1.Visible = cmbGradientType.SelectedItem.ToString() != "Plain";
            btnColor2.DataBindings.Clear();
            btnColor2.DataBindings.Add("BackColor", this, btnColor1.Visible ? "BackgroundColor2" : "BackgroundColor", false, DataSourceUpdateMode.OnPropertyChanged);
            GradientString = cmbGradientType.SelectedItem.ToString();
        }

        public void SetSettings(XmlNode node)
        {
            var element = (XmlElement)node;
            TextColor = SettingsHelper.ParseColor(element["TextColor"]);
            OverrideTextColor = SettingsHelper.ParseBool(element["OverrideTextColor"]);
            TimeColor = SettingsHelper.ParseColor(element["TimeColor"]);
            OverrideTimeColor = SettingsHelper.ParseBool(element["OverrideTimeColor"]);
            BackgroundColor = SettingsHelper.ParseColor(element["BackgroundColor"]);
            BackgroundColor2 = SettingsHelper.ParseColor(element["BackgroundColor2"]);
            GradientString = SettingsHelper.ParseString(element["BackgroundGradient"]);
            Text1 = SettingsHelper.ParseString(element["Text1"]);
            Text2 = SettingsHelper.ParseString(element["Text2"]);
            fileLocation = SettingsHelper.ParseString(element["fileLocation"]);
            Font1 = SettingsHelper.GetFontFromElement(element["Font1"]);
            Font2 = SettingsHelper.GetFontFromElement(element["Font2"]);
            OverrideFont1 = SettingsHelper.ParseBool(element["OverrideFont1"]);
            OverrideFont2 = SettingsHelper.ParseBool(element["OverrideFont2"]);
            Display2Rows = SettingsHelper.ParseBool(element["Display2Rows"], false);
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            var parent = document.CreateElement("Settings");
            CreateSettingsNode(document, parent);
            return parent;
        }

        public int GetSettingsHashCode()
        {
            return CreateSettingsNode(null, null);
        }

        private int CreateSettingsNode(XmlDocument document, XmlElement parent)
        {
            return  SettingsHelper.CreateSetting(document, parent, "Version", "1.4") ^
                    SettingsHelper.CreateSetting(document, parent, "TextColor", TextColor) ^
                    SettingsHelper.CreateSetting(document, parent, "OverrideTextColor", OverrideTextColor) ^
                    SettingsHelper.CreateSetting(document, parent, "TimeColor", TimeColor) ^
                    SettingsHelper.CreateSetting(document, parent, "OverrideTimeColor", OverrideTimeColor) ^
                    SettingsHelper.CreateSetting(document, parent, "BackgroundColor", BackgroundColor) ^
                    SettingsHelper.CreateSetting(document, parent, "BackgroundColor2", BackgroundColor2) ^
                    SettingsHelper.CreateSetting(document, parent, "BackgroundGradient", BackgroundGradient) ^
                    SettingsHelper.CreateSetting(document, parent, "Text1", Text1) ^
                    SettingsHelper.CreateSetting(document, parent, "Text2", Text2) ^
                    SettingsHelper.CreateSetting(document, parent, "fileLocation", fileLocation) ^
                    SettingsHelper.CreateSetting(document, parent, "Font1", Font1) ^
                    SettingsHelper.CreateSetting(document, parent, "Font2", Font2) ^
                    SettingsHelper.CreateSetting(document, parent, "OverrideFont1", OverrideFont1) ^
                    SettingsHelper.CreateSetting(document, parent, "OverrideFont2", OverrideFont2) ^
                    SettingsHelper.CreateSetting(document, parent, "Display2Rows", Display2Rows);
        }

        private void ColorButtonClick(object sender, EventArgs e)
        {
            SettingsHelper.ColorButtonClick((Button)sender, this);
        }

        private void btnFont1_Click(object sender, EventArgs e)
        {
            var dialog = SettingsHelper.GetFontDialog(Font1, 7, 20);
            dialog.FontChanged += (s, ev) => Font1 = ((CustomFontDialog.FontChangedEventArgs)ev).NewFont;
            dialog.ShowDialog(this);
            lblFont.Text = Font1String;
        }

        private void btnFont2_Click(object sender, EventArgs e)
        {
            var dialog = SettingsHelper.GetFontDialog(Font2, 7, 20);
            dialog.FontChanged += (s, ev) => Font2 = ((CustomFontDialog.FontChangedEventArgs)ev).NewFont;
            dialog.ShowDialog(this);
            lblFont.Text = Font2String;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var a = openFileDialog1.ShowDialog();
            if (a == DialogResult.OK)
            {
                fileLocation = openFileDialog1.FileName;
            } else
            {
                MessageBox.Show("Fail!");
            }

        }
    }
}
