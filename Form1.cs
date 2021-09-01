using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 練習
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dynamic fso = Activator.CreateInstance(Type.GetTypeFromProgID("Scripting.FileSystemObject"));

            string path;
            string[] Arr = new string[0];
            int j = 0, para = 16;

            if (System.IO.Directory.Exists(textBox1.Text) == false)
            {
                MessageBox.Show("ファイルがみつかりません。");
                return;
            }

            foreach (var it in System.IO.Directory.GetFiles(textBox1.Text))
            {
                //j +=1;
                path = System.IO.Path.GetFileName(it);
                Array.Resize(ref Arr, Arr.Length + 1);
                Arr[Arr.Length - 1] = path;
                Console.WriteLine(Arr[Arr.Length - 1]);
            }

            int count = Arr.Length, k = count * para;

            var dlg = new OpenFileDialog()
            {
                Multiselect = false,
                Title = "POSファイルの選択",
                CheckFileExists = true,
                RestoreDirectory = true,
                Filter = "posファイル|*.pos"
            };


            string pos;


            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pos = dlg.FileName;
                var rt = new System.IO.StreamReader(pos);
                int linecount = rt.ReadToEnd().Split(new[] { '\n', '\r' }).Length;
                string lineStr;
                string[] SP;

                Console.WriteLine(linecount);

                string[,] PArr = new string[linecount,10];

                for (int m = 1; m < linecount; m++)
                { 
                    lineStr = rt.ReadLine();
                    SP = lineStr.Split(' ');
                    for (int n = 1 ; n < 10 ; n++)
                    {
                        PArr[m, n] = SP[n];
                        Console.WriteLine(PArr[m, n]);                    
                    }
                }
            }
            else
            {
                MessageBox.Show("POSを指定してください");
            }

            for (int l = 1; l < k; k++) ;
            { Parallel.For((l)}

            うんこ
            
        }
    }
}
