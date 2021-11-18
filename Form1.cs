using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Hanoi_wolczuk
{
    public partial class Form1 : Form
    {
        static int movecount = 0;
        static public void Solve2Discs(Stack<int> source, Stack<int> temp, Stack<int> dest)
        {
            temp.Push(source.Pop());
            movecount++;
            PrintStacks();
            dest.Push(source.Pop());
            movecount++;
            PrintStacks();
            dest.Push(temp.Pop());
            movecount++;
            PrintStacks();
        }

        static public bool SolveTOH(int nDiscs, Stack<int> source, Stack<int> temp, Stack<int> dest)
        {
            if (nDiscs <= 4)
            {
                if ((nDiscs % 2) == 0)
                {
                    Solve2Discs(source, temp, dest);
                    nDiscs = nDiscs - 1;
                    if (nDiscs == 1)
                        return true;

                    temp.Push(source.Pop());
                    movecount++;
                    PrintStacks();
                    Solve2Discs(dest, source, temp);
                    dest.Push(source.Pop());
                    movecount++;
                    PrintStacks();
                    SolveTOH(nDiscs, temp, source, dest);
                }
                else
                {
                    if (nDiscs == 1)
                        return false;
                    Solve2Discs(source, dest, temp);
                    nDiscs = nDiscs - 1;
                    dest.Push(source.Pop());
                    movecount++;
                    PrintStacks();
                    Solve2Discs(temp, source, dest);
                }
                return true;
            }
            else if (nDiscs >= 5)
            {
                SolveTOH(nDiscs - 2, source, temp, dest);
                temp.Push(source.Pop());
                movecount++;
                PrintStacks();
                SolveTOH(nDiscs - 2, dest, source, temp);
                dest.Push(source.Pop());
                movecount++;
                PrintStacks();
                SolveTOH(nDiscs - 1, temp, source, dest);
            }
            return true;
        }

        static public Stack<int> A = new Stack<int>();
        static public Stack<int> B = new Stack<int>();
        static public Stack<int> C = new Stack<int>();

        public class MoveInfo
        {
            public string src;
            public string dest;
            public int number;


            public MoveInfo(string s, string d, int n)
            {
                src = s;
                dest = d;
                number = n;
            }
        }
        static public List<MoveInfo> algorithm_steps = new List<MoveInfo>();
        static int currentStep = 0;
        static int countA = 0;
        static int countB = 0;
        static int countC = 0;

        static public void PrintStacks()
        {
            if (countA != A.Count ||
                countB != B.Count ||
                countC != C.Count)
            {
                int diffA = A.Count - countA;
                int diffB = B.Count - countB;
                int diffC = C.Count - countC;
                if (diffA == 1)
                {
                    if (diffB == -1)
                        algorithm_steps.Add(new MoveInfo("B", "A", A.Peek()));
                    else
                        algorithm_steps.Add(new MoveInfo("C", "A", A.Peek()));
                }
                else if (diffB == 1)
                {
                    if (diffA == -1)
                        algorithm_steps.Add(new MoveInfo("A", "B", B.Peek()));
                    else
                        algorithm_steps.Add(new MoveInfo("C", "B", B.Peek()));
                }
                else //if (diffC == 1)
                {
                    if (diffA == -1)
                        algorithm_steps.Add(new MoveInfo("A", "C", C.Peek()));
                    else
                        algorithm_steps.Add(new MoveInfo("B", "C", C.Peek()));
                }
                countA = A.Count;
                countB = B.Count;
                countC = C.Count;
                Console.WriteLine();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int maxdisc = Convert.ToInt32(comboBox1.Text);
            movecount = 0;
            algorithm_steps.Clear();
            for (int i = maxdisc; i >= 1; i--)
                A.Push(i);
            countA = A.Count;
            countB = B.Count;
            countC = C.Count;
            PrintStacks();
            SolveTOH(maxdisc, A, B, C);

            DiscInfo d1 = new DiscInfo(15, 15, Color.Black, 1);
            DiscInfo d2 = new DiscInfo(25, 25, Color.Black, 2);
            DiscInfo d3 = new DiscInfo(35, 35, Color.Black, 3);
            DiscInfo d4 = new DiscInfo(45, 45, Color.Black, 4);
            DiscInfo d5 = new DiscInfo(55, 55, Color.Black, 5);
            DiscInfo d6 = new DiscInfo(65, 65, Color.Black, 6);
            DiscInfo d7 = new DiscInfo(75, 75, Color.Black, 7);
            DiscInfo d8 = new DiscInfo(85, 85, Color.Black, 8);
            DiscInfo d9 = new DiscInfo(95, 195, Color.Black, 9);

            DiscInfo[] arrDiscs = { d1, d2, d3, d4, d5, d6, d7, d8, d9 };

            towerA.Clear();
            towerB.Clear();
            towerC.Clear();
            for (int i = maxdisc - 1; i >= 0; i--)
            {
                towerA.Push(arrDiscs[i]);
            }
            currentStep = 0;
            textBox1.Text = currentStep.ToString() + " z " + algorithm_steps.Count.ToString();
            Invalidate();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "6";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentStep == algorithm_steps.Count)
                return; // koniec krokó

            MoveInfo info = algorithm_steps[currentStep++];

            textBox1.Text = currentStep.ToString() + " z " + algorithm_steps.Count.ToString();

            if (info.src == "A")
            {
                if (info.dest == "B")
                    towerB.Push(towerA.Pop());
                else
                    towerC.Push(towerA.Pop());
            }
            else if (info.src == "B")
            {
                if (info.dest == "C")
                    towerC.Push(towerB.Pop());
                else
                    towerA.Push(towerB.Pop());
            }
            else if (info.src == "C")
            {
                if (info.dest == "A")
                    towerA.Push(towerC.Pop());
                else
                    towerB.Push(towerC.Pop());
            }

            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics myGraphics = e.Graphics;
            //myGraphics.Clear(Color.White);

            int maxdisc = Convert.ToInt32(comboBox1.Text);
            DrawTower(towerA, maxdisc, 120, 100, e.Graphics);
            DrawTower(towerB, maxdisc, 320, 100, e.Graphics);
            DrawTower(towerC, maxdisc, 520, 100, e.Graphics);

            myGraphics.Dispose();
        }


        public class DiscInfo
        {
            public int width;
            public int height;
            public Color color;
            public int number;
            public Brush brush;

            public DiscInfo(int w, int h, Color c, int n)
            {
                width = w;
                height = h;
                color = c;
                number = n;
                brush = new SolidBrush(color);
            }
        }
        public Stack<DiscInfo> towerA = new Stack<DiscInfo>();
        public Stack<DiscInfo> towerB = new Stack<DiscInfo>();
        public Stack<DiscInfo> towerC = new Stack<DiscInfo>();

        public void DrawTower(Stack<DiscInfo> tower, int maxDiscs, int xbeg, int ybeg, Graphics graphics)
        {
            Stack<DiscInfo>.Enumerator et = tower.GetEnumerator();
            List<DiscInfo> drList = new List<DiscInfo>();
            int yoffset = maxDiscs - tower.Count; // 6 = maks kr¹¿ków

            while (true)
            {
                if (et.MoveNext() == false)
                    break;

                drList.Add(et.Current);
            }
            for (int i = drList.Count - 1; i >= 0; i--)
            {
                Rectangle r = new Rectangle(xbeg - drList[i].number * 10, ybeg + (i + yoffset) * 10, drList[i].width, drList[i].height);
                Pen p = new Pen(Color.White);
                r.Y += 5;
                graphics.FillEllipse(drList[i].brush, r);
                graphics.DrawEllipse(p, r);
                r.Y -= 5;
                graphics.FillEllipse(drList[i].brush, r);
                graphics.DrawEllipse(p, r);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}