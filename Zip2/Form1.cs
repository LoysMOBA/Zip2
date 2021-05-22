using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zip2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog bd = new FolderBrowserDialog();
            if (bd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = bd.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sourceFolder = (textBox1.Text).Replace("'\'", "'\\'");
            DirectoryInfo directorySelected = new DirectoryInfo(sourceFolder);
            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                    {
                        using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                               CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }
                        FileInfo info = new FileInfo(sourceFolder + Path.DirectorySeparatorChar + fileToCompress.Name + ".gz");
                        MessageBox.Show($"Compressed {fileToCompress.Name} from {fileToCompress.Length.ToString()} to {info.Length.ToString()} bytes.");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sourceFolder = (textBox1.Text).Replace("'\'", "'\\'");
            DirectoryInfo directorySelected = new DirectoryInfo(sourceFolder);
            foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.gz"))
            {
                Decompress(fileToDecompress);
            }
        }
        public static void Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        MessageBox.Show($"Decompressed: {fileToDecompress.Name}");
                    }
                }
            }
        }
    }
}
