using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace VSCleaner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            radioButton2.Checked = true;
        }

        static void DirSearch(string fDir)
        {
            bool findFile = false;
            try
            {
                foreach (string d in Directory.GetDirectories(fDir))
                {
                    string[] dStrings = d.Split('\\');
                    string folderName = dStrings[dStrings.Length - 1];
                    if (folderName == "obj" || folderName == "bin" || folderName == "packages" || folderName == "Debug")
                    {
                        DeleteFolder(d);
                        findFile = true;
                    }
                    if (findFile)
                    {
                        Directory.CreateDirectory(d);
                    }
                    if (!findFile)
                    {
                        DirSearch(d);
                    }
                    findFile = false;
                }
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
        }

        static void FindSdfFile(string fDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(fDir))
                {
                    foreach (string f in Directory.GetFiles(d, "*.sdf"))
                    {
                        File.Delete(f);
                    }
                    FindSdfFile(d);
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.Message);
            }
        }

        static void DeleteFolder(string fDir)
        {
            try
            {
                foreach (string f in Directory.GetFileSystemEntries(fDir))
                {
                    if (File.Exists(f))
                    {
                        File.Delete(f);
                    }
                    else
                    {
                        DeleteFolder(f);
                    }
                }
                Directory.Delete(fDir);
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (textBox1.Text != "")
                {
                    DirSearch(textBox1.Text);
                    FindSdfFile(textBox1.Text);
                }
                else
                {
                    MessageBox.Show("请选择文件夹");
                }

                try
                {
                    string zipFilePath = textBox1.Text;
                    zipFilePath += string.Format("({0:yyyy-MM-dd H-mm}).zip", DateTime.Now);
                    ZipFile.CreateFromDirectory(textBox1.Text, zipFilePath);
                    MessageBox.Show("打包完成，文件为" + zipFilePath);
                }
                catch(Exception except)
                {
                    MessageBox.Show(except.Message);
                }
            }
            else
            {
                if (textBox1.Text != "")
                {
                    DirSearch(textBox1.Text);
                    FindSdfFile(textBox1.Text);
                    MessageBox.Show("清除完成");
                }
                else
                {
                    MessageBox.Show("请选择文件夹");
                }
            }
        }
    }
}
