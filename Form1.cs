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


        [STAThread]
        public static void Main() 
        {
            Application.Run(new Form1());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //dynamic fso = Activator.CreateInstance(Type.GetTypeFromProgID("Scripting.FileSystemObject"));

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

            int count = Arr.Length, k = count / para;

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
                linecount = rt.ReadToEnd().Count(c => c == '\n') + 1;
                PArr = new string[linecount, 10];

                Console.WriteLine(linecount);
                rt = new System.IO.StreamReader(pos);
                for (int m = 1; m < linecount; m++)
                { 
                    lineStr = rt.ReadLine();
                    SP = lineStr.Split(' ');
                    for (int n = 0 ; n < 10 ; n++)
                    {
                        PArr[m, n] = SP[n];
                        //Console.WriteLine(PArr[m, n]);                    
                    }
                }
            }
            else
            {
                MessageBox.Show("POSを指定してください");
            }

            string fname;
 

            for (int l = 1; l < k; l++) 
            {
                Parallel.For((l - 1) * para , para * l + 1, i =>
                {
                    Console.WriteLine(i);
                    fname = Arr[i];

                    Process(PArr ,fname) ;                
                });                
            }



            Parallel.For((k -1) * para + 1, count, i =>
            {
                Console.WriteLine(i);
                fname = Arr[i];

                Process(PArr, fname);
            });

            MessageBox.Show("End");
        }


        private void Process(string[,] Parr, string fname) 
        {
            var IF = new System.IO.StreamReader(textBox1.Text + "/" + fname);
            var OF = new System.IO.StreamWriter(textBox2.Text + "/" + fname);
            int Sys = int.Parse(textBox3.Text);
            string lineStr;
            string[] SP;
            int n = 1;
            double hight, Dist,Xr,HightDif;
            double CarHight = 2.036;
            double x,y,x1, x2, y1, y2, a, b, c, angle;
            //string flag;


            lineStr = IF.ReadLine();
            SP = lineStr.Split(',');

            while (IF.EndOfStream==false)
            {
                if (double.Parse(Parr[n, 0]) > double.Parse(SP[4]))
                {
                    lineStr = IF.ReadLine();
                    SP = lineStr.Split(',');
                    hight = double.Parse(SP[2]) - double.Parse(Parr[n, 3]) + CarHight;

                    Function z = new Function();
                    y1 = z.ReprojectPoints(Parr[n - 1, 1], Parr[n - 1, 2], Sys)[0];
                    y2 = z.ReprojectPoints(Parr[n, 1], Parr[n, 2], Sys)[0];
                    x1 = z.ReprojectPoints(Parr[n - 1, 1], Parr[n - 1, 2], Sys)[1];
                    x2 = z.ReprojectPoints(Parr[n, 1], Parr[n, 2], Sys)[1];

                    //Console.WriteLine("y1:" + y1 + "  x1:" + x1);

                    //２点を通る１次方程式の求め方
                    //点(x1,y1)と点(x2,y2)を通る直線の１次方程式は
                    //(y1 - y2) * x - (x1 - x2) * y + (x1 - x2) * y1 - (y1 - y2) * x1 = 0

                    a = y1 - y2;
                    b = (x1 - x2) * (-1);
                    c = (x1 - x2) * y1 - (y1 - y2) * x1;


                    //a* x + b * y + c = 0
                    //b* y = -c - a * x
                    //y = -(c + a * x) / b

                    //a* x = -b * y - c
                    //x = -(b * y + c) / a

                    if (checkBox1.Checked == true)
                    {
                        x = z.ReprojectPoints(Parr[n, 1], Parr[n, 2], Sys)[0] - double.Parse(SP[0]);
                        y = z.ReprojectPoints(Parr[n, 1], Parr[n, 2], Sys)[1] - double.Parse(SP[1]);

                        Dist = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                        Xr = Math.Abs(a * double.Parse(SP[1]) + b * double.Parse(SP[0]) + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                        angle = double.Parse(Parr[n, 4]);

                        //アフィン変換

                        if (double.Parse(Parr[n, 6]) <= 90 || double.Parse(Parr[n, 6]) >= 270)
                        {
                            if (double.Parse(SP[1]) >= -(b * double.Parse(SP[0]) + c) / a)
                            {
                                HightDif = z.AffineY(Xr, hight, angle);
                                //flag
                            }
                            else
                            {
                                HightDif = z.AffineY(-Xr, hight, angle);
                                //flag
                            }
                        }
                        else 
                        {
                            if (double.Parse(SP[1]) >= -(b * double.Parse(SP[0]) + c) / a)
                            {
                                HightDif = z.AffineY(-Xr, hight, angle);
                                //flag
                            }
                            else
                            {
                                HightDif = z.AffineY(Xr, hight, angle);
                                //flag
                            }
                        }

                        if (double.IsNaN(HightDif)) 
                        {
                            Console.WriteLine(fname + "でNaNを検出しました。");
                        }
                        else 
                        {
                            OF.WriteLine(SP[0] + "," + SP[1] + "," + HightDif + "," + SP[3] + "," + SP[4]);
                            //Console.WriteLine(SP[0] + "," + SP[1] + "," + HightDif + "," + SP[3] + "," + SP[4]);
                        }

                     }

                }
                else
                {
                    n += 1;
                }
            }
        }


    }
}
