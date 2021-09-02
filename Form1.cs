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
            string[,] PArr = { { "aa", "bb" } ,  };
            string lineStr;
            string[] SP;
            int linecount;


            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pos = dlg.FileName;
                var rt = new System.IO.StreamReader(pos);
                linecount = rt.ReadToEnd().Split(new[] { '\n', '\r' }).Length;
                PArr = new string[linecount, 10];

                Console.WriteLine(linecount);                

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

            string fname;
 

            for (int l = 1; l < k; k++) 
            {
                Parallel.For((l - 1) * para + 1, para * l + 1, i =>
                {
                    Console.WriteLine(i);
                    fname = Arr[i];

                    Process(PArr ,fname) ;
                
                });
                
            }

            
        }


        private void Process(string[,] Parr, string fname) 
        {
            var IF = new System.IO.StreamReader(textBox1.Text + "/" + fname);
            var OF = new System.IO.StreamWriter(textBox2.Text + "/" + fname);
            int Sys = int.Parse(textBox3.Text);
            string lineStr;
            string[] SP;
            int n = 1;
            double hight;
            double CarHight = 2.036;
            double x1;
            double x2;
            double y1;
            double y2;


            lineStr = IF.ReadLine();
            SP = lineStr.Split(' ');

            while (IF.EndOfStream)
            {
                if (double.Parse(Parr[n, 0]) > double.Parse(SP[4]))
                {
                    lineStr = IF.ReadLine();
                    SP = lineStr.Split(' ');
                    hight = double.Parse(SP[2]) - double.Parse(Parr[n, 3]) + CarHight;

                    Function z = new Function();
                    y1 = z.ReprojectPoints(Parr[n - 1, 1], Parr[n - 1, 2]);

                }
            }

        }



    }
}
