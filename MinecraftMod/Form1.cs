using System;
using System.Diagnostics;
using System.Windows.Forms;
using Windows.UI.Xaml.Media.Animation;

namespace MinecraftMod
{
    public partial class Form1 : Form
    {
        string package = "Microsoft.MinecraftUWP_8wekyb3d8bbwe";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Minecraft.MultiInstance;
            textBox1.Text = Minecraft.Backcolor;
            CaptionTitleTextbox.Text = Minecraft.CaptionTitle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mcDirPath = Application.StartupPath + "\\MinecraftData";
            
            checkRepeat:
            Process[] procs = Process.GetProcessesByName("Minecraft.Windows");
            if (procs.Length > 0)
            {
                foreach (var proc in procs)
                {
                    try
                    {
                        proc.Kill();
                    }
                    catch { }
                }

                goto checkRepeat;
            }

            Appx.BackupMC(package); // backup mc data

            Appx.APPX_Unregister(package);
            Appx.APPX_Register(mcDirPath); // reregister mc

            Appx.RestoreMC(package); // restore mc data
        }

        private void button2_Click(object sender, EventArgs e) => Appx.APPX_Unregister(package);

        private void checkBox1_CheckedChanged(object sender, EventArgs e) => Minecraft.MultiInstance = checkBox1.Checked;

        private void textBox1_TextChanged(object sender, EventArgs e) => Minecraft.Backcolor = textBox1.Text;

        private void OnCaptionChanged(object sender, EventArgs e) => Minecraft.CaptionTitle = CaptionTitleTextbox.Text;
    }
}