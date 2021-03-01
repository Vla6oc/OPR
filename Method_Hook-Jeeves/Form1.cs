using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using info.lundin.math;

namespace Method_Hook_Jeeves
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            comboBox1.Text = "Max";
            comboBox2.Text = "Метод Сеток";
            comboBox3.Text = ">=";
            comboBox4.Text = "<=";
        }
        double x1test, x2test; // функция для проверки поля
        public double f(double x1, double x2)
        {
                x1test = x1;
                x2test = x2;
                ExpressionParser parser = new ExpressionParser();
                parser.Values.Add("x1", x1test);
                parser.Values.Add("x2", x2test);
                return parser.Parse(textBox11.Text);
        }
        double xo1, yo1;
        public bool ogr1(double x1, double x2)
        {
            xo1 = 0;
            yo1 = 0;
            xo1 = x1;
            yo1 = x2;
            ExpressionParser parser = new ExpressionParser();
            parser.Values.Add("x1", xo1);
            parser.Values.Add("x2", yo1);
            string s1 = textBox12.Text;
            string s2 = textBox13.Text;
            if (comboBox3.Text == ">=" && parser.Parse(s1) >= parser.Parse(s2) || comboBox3.Text == "<=" && parser.Parse(s1) <= parser.Parse(s2) ||
                comboBox3.Text == ">" && parser.Parse(s1) > parser.Parse(s2) || comboBox3.Text == "<" && parser.Parse(s1) < parser.Parse(s2) ||
                comboBox3.Text == "==" && parser.Parse(s1) == parser.Parse(s2)) return true;
            else return false;
        }
        public bool ogr2(double x1, double x2)
        {
            xo1 = 0;
            yo1 = 0;
            xo1 = x1;
            yo1 = x2;
            string a1 = textBox2.Text;
            string a2 = textBox5.Text;
            ExpressionParser parser = new ExpressionParser();
            parser.Values.Add("x1", xo1);
            parser.Values.Add("x2", yo1);
            string s1 = textBox15.Text;
            string s2 = textBox14.Text;
            if (comboBox4.Text == ">=" && parser.Parse(s1) >= parser.Parse(s2) || comboBox4.Text == "<=" && parser.Parse(s1) <= parser.Parse(s2) ||
                comboBox4.Text == ">" && parser.Parse(s1) > parser.Parse(s2) || comboBox4.Text == "<" && parser.Parse(s1) < parser.Parse(s2) ||
                comboBox4.Text == "==" && parser.Parse(s1) == parser.Parse(s2) && x1 <= parser.Parse(s1) && x2 <= parser.Parse(s2)) return true;
            else return false;
        }
        public bool ogr3(double x1)
        {
            if (x1 >= 0) return true;
            else return false;
        }
        public bool ogr4(double x2)
        {
            if (x2 >= 0) return true;
            else return false;
        }
        public double mac1 = 0, mac2 = 0;
        public double mic1 = 0, mic2 = 0;
        public void MethodPereboraMin(double x1A, double x1B, double x1H, double x2A, double x2B, double x2H)
        {
            Stopwatch time = new Stopwatch();
            time.Start();
            int n1 = (int)((Math.Abs(x1A) + Math.Abs(x1B)) / x1H);
            int n2 = (int)((Math.Abs(x2A) + Math.Abs(x2B)) / x2H);
            dataGridView1.RowCount = n1 + 1;
            dataGridView1.ColumnCount = n2 + 1;
            int im = 0, jm = 0;
            double x1 = x1A, x2 = x2A, x1m = 0, x2m = 0, mValue = double.MaxValue; 
            for (int i = 0; i <= n1; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = x1.ToString();
                for (int j = 0; j <= n2; j++)
                {
                    dataGridView1.Columns[j].HeaderText = x2.ToString();
                    dataGridView1.Rows[i].Cells[j].Value = f(x1, x2);
                    if (radioButton1.Checked == true)
                    {
                        if (ogr1(x1, x2) == false || ogr2(x1, x2) == false || ogr3(x1) == false || ogr4(x2) == false)
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;

                        }
                        else
                        {
                            if (ogr1(x1, x2) == true && ogr2(x1, x2) == true && ogr3(x1) == true && ogr4(x2) == true && mValue > f(x1, x2))
                            {
                                mValue = f(x1, x2);
                                im = i; jm = j; x1m = x1; x2m = x2;
                            }
                        }

                    }
                    if (radioButton2.Checked == true)
                    {
                        if (mValue > f(x1, x2))
                        {
                            mValue = f(x1, x2);
                            im = i; jm = j; x1m = x1; x2m = x2;
                        }
                    }
                    x2 += x2H;
                }
                x2 = x2A;
                x1 += x1H;
            }
            time.Stop();
            dataGridView1.Rows[im].Cells[jm].Style.BackColor = Color.GreenYellow;
            label8.Text = "f(" + x1m + "; " + x2m + ") = " + f(x1m, x2m).ToString("0.###") + "\n Время выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            chart1.ChartAreas[0].AxisX.Title = "X1";
            chart1.ChartAreas[0].AxisY.Title = "X2";
            chart1.ChartAreas[0].AxisX.Interval = x1H;
            chart1.ChartAreas[0].AxisY.Interval = x2H;
            chart1.ChartAreas[0].AxisX.Maximum = x1B;
            chart1.ChartAreas[0].AxisX.Minimum = x1A;
            chart1.ChartAreas[0].AxisY.Maximum = x2B;
            chart1.ChartAreas[0].AxisY.Minimum = x2A;
            if (radioButton1.Checked == true)
            {
                Series series1 = chart1.Series.Add("Решение F(x1, x2) = " + f(x1m, x2m).ToString("0.###"));
                series1.ChartType = SeriesChartType.Bubble;
                chart1.Series[0].Points.AddXY(x1m - 0.0001, x2m - 0.0001);
                Series series2 = chart1.Series.Add("Допустимые решение");
                series2.ChartType = SeriesChartType.Point;
                for (double x = x1A; x <= x1B; x += x1H)
                {
                    for (double y = x2A; y <= x2B; y += x2H)
                    {
                        f(x, y).ToString("0.###");
                        if (ogr1(x, y) == false || ogr2(x, y) == false || ogr3(x) == false || ogr4(x) == false)
                        { }
                        else { chart1.Series[1].Points.AddXY(x, y); }
                    }
                }
            }
            if (radioButton2.Checked == true)
            {
                Series series1 = chart1.Series.Add("Решение F(x1, x2) = " + f(x1m, x2m).ToString("0.###"));
                series1.ChartType = SeriesChartType.Bubble;
                mic1 = x1m; mic2 = x2m;
                chart1.Series[0].Points.AddXY(x1m + 0.0001, x2m);
                Series series2 = chart1.Series.Add("Уровень 1 решений");
                series2.ChartType = SeriesChartType.Point;
                for (double i = -x1H; i <= x1H; i += 0.001)
                {
                    chart1.Series[1].Points.AddXY(i + x1m, Math.Sqrt((Math.Pow(x1H, 2) - Math.Pow(i, 2))) + x2m);
                    chart1.Series[1].Points.AddXY(i + x1m, -Math.Sqrt((Math.Pow(x1H, 2) - Math.Pow(i, 2))) + x2m);
                }
                Series series3 = chart1.Series.Add("Уровень 2 решений");
                series3.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 2 * x1H; i <= x1H + 2 * x1H; i += 0.001)
                {
                    chart1.Series[2].Points.AddXY(i + x1m, Math.Sqrt((Math.Pow(2 * x1H, 2) - Math.Pow(i, 2))) + x2m);
                    chart1.Series[2].Points.AddXY(i + x1m, -Math.Sqrt((Math.Pow(2 * x1H, 2) - Math.Pow(i, 2))) + x2m);
                }
                Series series4 = chart1.Series.Add("Уровень 3 решений");
                series4.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 3 * x1H; i <= x1H + 3 * x1H; i += 0.001)
                {
                    chart1.Series[3].Points.AddXY(i + x1m, Math.Sqrt((Math.Pow(3 * x1H, 2) - Math.Pow(i, 2))) + x2m);
                    chart1.Series[3].Points.AddXY(i + x1m, -Math.Sqrt((Math.Pow(3 * x1H, 2) - Math.Pow(i, 2))) + x2m);
                }
                Series series5 = chart1.Series.Add("Уровень 4 решений");
                series5.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 4 * x1H; i <= x1H + 4 * x1H; i += 0.001)
                {
                    chart1.Series[4].Points.AddXY(i + x1m, Math.Sqrt((Math.Pow(4 * x1H, 2) - Math.Pow(i, 2))) + x2m);
                    chart1.Series[4].Points.AddXY(i + x1m, -Math.Sqrt((Math.Pow(4 * x1H, 2) - Math.Pow(i, 2))) + x2m);
                }
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                textBox12.Enabled = false;
                textBox13.Enabled = false;
                textBox14.Enabled = false;
                textBox15.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
            }
            else
            {
                textBox12.Enabled = true;
                textBox13.Enabled = true;
                textBox14.Enabled = true;
                textBox15.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                textBox12.Enabled = false;
                textBox13.Enabled = false;
                textBox14.Enabled = false;
                textBox15.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
            }
            else
            {
                textBox12.Enabled = true;
                textBox13.Enabled = true;
                textBox14.Enabled = true;
                textBox15.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
            }
        }
        public void MethodPereboraMax(double x1A, double x1B, double x1H, double x2A, double x2B, double x2H)
        {
            Stopwatch time = new Stopwatch();
            time.Start();
            int n1 = (int)((Math.Abs(x1A) + Math.Abs(x1B)) / x1H);
            int n2 = (int)((Math.Abs(x2A) + Math.Abs(x2B)) / x2H);
            dataGridView1.RowCount = n1 + 1;
            dataGridView1.ColumnCount = n2 + 1;
            int im = 0, jm = 0;
            double x1 = x1A, x2 = x2A, x1m = 0, x2m = 0, mValue = double.MinValue;
            for (int i = 0; i <= n1; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = x1.ToString();
                for (int j = 0; j <= n2; j++)
                {
                    dataGridView1.Columns[j].HeaderText = x2.ToString();
                    dataGridView1.Rows[i].Cells[j].Value = f(x1, x2);
                    if (radioButton1.Checked == true)
                    {
                        if (ogr1(x1, x2) == false || ogr2(x1, x2) == false || ogr3(x1) == false || ogr4(x2) == false)
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            if (mValue < f(x1, x2))
                            {
                                mValue = f(x1, x2);
                                im = i; jm = j; x1m = x1; x2m = x2;
                            }
                        }
                    }
                    if (radioButton2.Checked == true)
                    {
                        if (mValue < f(x1, x2))
                        {
                            mValue = f(x1, x2);
                            im = i; jm = j; x1m = x1; x2m = x2;
                        }
                    }
                    x2 += x2H;
                }
                x2 = x2A;
                x1 += x1H;
            }
            time.Stop();
            dataGridView1.Rows[im].Cells[jm].Style.BackColor = Color.GreenYellow;
            label8.Text = "f(" + x1m + "; " + x2m + ") = " + f(x1m, x2m).ToString("0.###") + "\n Время выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            chart1.ChartAreas[0].AxisX.Title = "X1";
            chart1.ChartAreas[0].AxisY.Title = "X2";
            chart1.ChartAreas[0].AxisX.Interval = x1H;
            chart1.ChartAreas[0].AxisY.Interval = x2H;
            chart1.ChartAreas[0].AxisX.Maximum = x1B;
            chart1.ChartAreas[0].AxisX.Minimum = x1A;
            chart1.ChartAreas[0].AxisY.Maximum = x2B;
            chart1.ChartAreas[0].AxisY.Minimum = x2A;
            if (radioButton1.Checked == true)
            {
                Series series1 = chart1.Series.Add("Решение F(x1, x2) = " + f(x1m, x2m).ToString("0.###"));
                series1.ChartType = SeriesChartType.Bubble;
                chart1.Series[0].Points.AddXY(x1m - 0.0001, x2m - 0.0001);
                Series series2 = chart1.Series.Add("Допустимые решение");
                series2.ChartType = SeriesChartType.Point;
                for (double x = x1A; x <= x1B; x += x1H)
                {
                    for (double y = x2A; y <= x2B; y += x2H)
                    {
                        f(x, y).ToString("0.###");
                        if (ogr1(x, y) == false || ogr2(x, y) == false || ogr3(x) == false || ogr4(x) == false)
                        { }
                        else { chart1.Series[1].Points.AddXY(x, y); }
                    }
                }
            }
            if (radioButton2.Checked == true)
            {
                mac1 = x1m; mac2 = x2m;
                Series series1 = chart1.Series.Add("Решение F(x1, x2) = " + f(x1m, x2m).ToString("0.###"));
                series1.ChartType = SeriesChartType.Bubble;
                chart1.Series[0].Points.AddXY(x1m + 0.0001, x2m);
                 Series series2 = chart1.Series.Add("Уровень 1 решений");
                 series2.ChartType = SeriesChartType.Point;
                for (double i = -x1H; i <= x1H; i += 0.001)
                {
                    chart1.Series[1].Points.AddXY(i + x1m, Math.Sqrt((Math.Pow(x1H, 2) - Math.Pow(i, 2))) + x2m);
                    chart1.Series[1].Points.AddXY(i + x1m, -Math.Sqrt((Math.Pow(x1H, 2) - Math.Pow(i, 2))) + x2m);
                }
                Series series3 = chart1.Series.Add("Уровень 2 решений");
                 series3.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 2* x1H; i <= x1H + 2* x1H; i += 0.001)
                {
                    chart1.Series[2].Points.AddXY(i + x1m, Math.Sqrt((Math.Pow(2* x1H, 2) - Math.Pow(i, 2))) + x2m);
                    chart1.Series[2].Points.AddXY(i + x1m, -Math.Sqrt((Math.Pow(2* x1H, 2) - Math.Pow(i, 2))) + x2m);
                }
                Series series4 = chart1.Series.Add("Уровень 3 решений");
                 series4.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 3* x1H; i <= x1H + 3*x1H; i += 0.001)
                {
                    chart1.Series[3].Points.AddXY(i + x1m, Math.Sqrt((Math.Pow(3* x1H, 2) - Math.Pow(i, 2))) + x2m);
                    chart1.Series[3].Points.AddXY(i + x1m, -Math.Sqrt((Math.Pow(3* x1H, 2) - Math.Pow(i, 2))) + x2m);
                }
                Series series5 = chart1.Series.Add("Уровень 4 решений");
                series5.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 4* x1H; i <= x1H + 4* x1H; i += 0.001)
                {
                    chart1.Series[4].Points.AddXY(i + x1m, Math.Sqrt((Math.Pow(4* x1H, 2) - Math.Pow(i, 2))) + x2m);
                    chart1.Series[4].Points.AddXY(i + x1m, -Math.Sqrt((Math.Pow(4* x1H, 2) - Math.Pow(i, 2))) + x2m);
                }
            }
        }
        public void MonteCarloMax(double x1A, double x1B, double x2A, double x2B, int N, double x1H, double x2H)
        {
            Stopwatch time = new Stopwatch();
            time.Start();
            double x1min = x1A;
            double x1max = x1B;
            double x2min = x2A;
            double x2max = x2B;
            dataGridView2.RowCount = N;
            dataGridView2.ColumnCount = 3;
            dataGridView2.Columns[0].HeaderText = "x1";
            dataGridView2.Columns[1].HeaderText = "x2";
            dataGridView2.Columns[2].HeaderText = "f(x1,x2)";
            double m = double.MinValue;
            double mx1 = 0.0, mx2 = 0.0, x2, x1;
            Random r = new Random();
            int indexM = 0;
            for (int i = 0; i < N; i++)
            {
                x1 = r.NextDouble() * (Math.Abs(x1max) + Math.Abs(x1min)) + x1min;
                x2 = r.NextDouble() * (Math.Abs(x2max) + Math.Abs(x2min)) + x2min;
                if (radioButton1.Checked == true)
                {
                    if ((ogr1(x1, x2) != false && ogr2(x1, x2) != false && ogr3(x1) != false && ogr4(x1) != false) && f(x1, x2) > m)
                    {
                        m = f(x1, x2);
                        mx1 = x1;
                        mx2 = x2;
                        indexM = i;
                    }
                }
                if (radioButton2.Checked == true)
                {
                    if (f(x1, x2) > m)
                    {
                        m = f(x1, x2);
                        mx1 = x1;
                        mx2 = x2;
                        indexM = i;
                    }
                }
                dataGridView2[0, i].Value = Math.Round(x1, 4).ToString();
                dataGridView2[1, i].Value = Math.Round(x2, 4).ToString();
                dataGridView2[2, i].Value = Math.Round(f(x1, x2), 4).ToString();
            }
            time.Stop();
            dataGridView2[0, indexM].Style.BackColor = Color.GreenYellow;
            dataGridView2[1, indexM].Style.BackColor = Color.GreenYellow;
            dataGridView2[2, indexM].Style.BackColor = Color.GreenYellow;
            label8.Text = "x1 = " + Math.Round(mx1, 4).ToString() + "; " + " x2 = " + Math.Round(mx2, 4).ToString() + "; " + "\nf = " + Math.Round(m, 3) + ";".ToString() + "\nВремя выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            chart2.ChartAreas[0].AxisX.Title = "X1";
            chart2.ChartAreas[0].AxisY.Title = "X2";
            chart2.ChartAreas[0].AxisX.Interval = x1H;
            chart2.ChartAreas[0].AxisY.Interval = x2H;
            chart2.ChartAreas[0].AxisX.Maximum = x1B;
            chart2.ChartAreas[0].AxisX.Minimum = x1A;
            chart2.ChartAreas[0].AxisY.Maximum = x2B;
            chart2.ChartAreas[0].AxisY.Minimum = x2A;
            if (radioButton1.Checked == true)
            {
                Series series1 = chart2.Series.Add("Решение F(x1, x2) = " + m.ToString("0.###"));
                series1.ChartType = SeriesChartType.Bubble;
                chart2.Series[0].Points.AddXY(mx1 + 0.0001, mx2);
                Series series2 = chart2.Series.Add("Допустимые решение");
                series2.ChartType = SeriesChartType.Point;
                for (double x = x1A; x <= x1B; x += x1H)
                {
                    for (double y = x2A; y <= x2B; y += x2H)
                    {
                        f(x, y).ToString("0.###");
                        if (ogr1(x, y) == false || ogr2(x, y) == false || ogr3(x) == false || ogr4(x) == false)
                        { }
                        else { chart2.Series[1].Points.AddXY(x, y); }
                    }
                }
            }
            if (radioButton2.Checked == true)
            {
                Series series1 = chart2.Series.Add("Решение F(x1, x2) = " + m.ToString("0.###"));
                series1.ChartType = SeriesChartType.Bubble;
                chart2.Series[0].Points.AddXY(mx1 + 0.0001, mx2);
                Series series2 = chart2.Series.Add("Уровень 1 решений");
                series2.ChartType = SeriesChartType.Point;
                for (double i = -x1H; i <= x1H; i += 0.001)
                {
                    chart2.Series[1].Points.AddXY(i + mac1, Math.Sqrt((Math.Pow(x1H, 2) - Math.Pow(i, 2))) + mac2);
                    chart2.Series[1].Points.AddXY(i + mac1, -Math.Sqrt((Math.Pow(x1H, 2) - Math.Pow(i, 2))) + mac2);
                }
                Series series3 = chart2.Series.Add("Уровень 2 решений");
                series3.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 2 * x1H; i <= x1H + 2 * x1H; i += 0.001)
                {
                    chart2.Series[2].Points.AddXY(i + mac1, Math.Sqrt((Math.Pow(2 * x1H, 2) - Math.Pow(i, 2))) + mac2);
                    chart2.Series[2].Points.AddXY(i + mac1, -Math.Sqrt((Math.Pow(2 * x1H, 2) - Math.Pow(i, 2))) + mac2);
                }
                Series series4 = chart2.Series.Add("Уровень 3 решений");
                series4.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 3 * x1H; i <= x1H + 3 * x1H; i += 0.001)
                {
                    chart2.Series[3].Points.AddXY(i + mac1, Math.Sqrt((Math.Pow(3 * x1H, 2) - Math.Pow(i, 2))) + mac2);
                    chart2.Series[3].Points.AddXY(i + mac1, -Math.Sqrt((Math.Pow(3 * x1H, 2) - Math.Pow(i, 2))) + mac2);
                }
                Series series5 = chart2.Series.Add("Уровень 4 решений");
                series5.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 4 * x1H; i <= x1H + 4 * x1H; i += 0.001)
                {
                    chart2.Series[4].Points.AddXY(i + mac1, Math.Sqrt((Math.Pow(4 * x1H, 2) - Math.Pow(i, 2))) + mac2);
                    chart2.Series[4].Points.AddXY(i + mac1, -Math.Sqrt((Math.Pow(4 * x1H, 2) - Math.Pow(i, 2))) + mac2);
                }
            }
        }
        public void MonteCarloMin(double x1A, double x1B, double x2A, double x2B, int N, double x1H, double x2H)
        {
            Stopwatch time = new Stopwatch();
            time.Start();
            double x1min = x1A;
            double x1max = x1B;
            double x2min = x2A;
            double x2max = x2B;
            dataGridView2.RowCount = N;
            dataGridView2.ColumnCount = 3;
            dataGridView2.Columns[0].HeaderText = "x1";
            dataGridView2.Columns[1].HeaderText = "x2";
            dataGridView2.Columns[2].HeaderText = "f(x1,x2)";
            double m = double.MaxValue;
            double mx1 = 0.0, mx2 = 0.0, x2, x1;
            Random r = new Random();
            int indexM = 0;
            for (int i = 0; i < N; i++)
            {
                x1 = r.NextDouble() * (Math.Abs(x1max) + Math.Abs(x1min)) + x1min;
                x2 = r.NextDouble() * (Math.Abs(x2max) + Math.Abs(x2min)) + x2min;
                if (radioButton1.Checked == true)
                {
                    if ((ogr1(x1, x2) != false && ogr2(x1, x2) != false && ogr3(x1) != false && ogr4(x1) != false) && f(x1, x2) < m)
                    {
                        m = f(x1, x2);
                        mx1 = x1;
                        mx2 = x2;
                        indexM = i;
                    }
                }
                if (radioButton2.Checked == true)
                {
                    if (f(x1, x2) < m)
                    {
                        m = f(x1, x2);
                        mx1 = x1;
                        mx2 = x2;
                        indexM = i;
                    }
                }
                dataGridView2[0, i].Value = Math.Round(x1, 4).ToString();
                dataGridView2[1, i].Value = Math.Round(x2, 4).ToString();
                dataGridView2[2, i].Value = Math.Round(f(x1, x2), 4).ToString();
            }
            time.Stop();
            dataGridView2[0, indexM].Style.BackColor = Color.GreenYellow;
            dataGridView2[1, indexM].Style.BackColor = Color.GreenYellow;
            dataGridView2[2, indexM].Style.BackColor = Color.GreenYellow;
            label8.Text = "x1 = " + Math.Round(mx1, 4).ToString() + "; " + " x2 = " + Math.Round(mx2, 4).ToString() + "; " + "\nf = " + Math.Round(m, 3) + ";".ToString() + "\n Время выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            chart2.ChartAreas[0].AxisX.Title = "X1";
            chart2.ChartAreas[0].AxisY.Title = "X2";
            chart2.ChartAreas[0].AxisX.Interval = x1H;
            chart2.ChartAreas[0].AxisY.Interval = x2H;
            chart2.ChartAreas[0].AxisX.Maximum = x1B;
            chart2.ChartAreas[0].AxisX.Minimum = x1A;
            chart2.ChartAreas[0].AxisY.Maximum = x2B;
            chart2.ChartAreas[0].AxisY.Minimum = x2A;
            if (radioButton1.Checked == true)
            {
                Series series1 = chart2.Series.Add("Решение F(x1, x2)  = " + m.ToString("0.###"));
                series1.ChartType = SeriesChartType.Bubble;
                chart2.Series[0].Points.AddXY(mx1 + 0.0001, mx2);
                Series series2 = chart2.Series.Add("Допустимые решение");
                series2.ChartType = SeriesChartType.Point;
                for (double x = x1A; x <= x1B; x += x1H)
                {
                    for (double y = x2A; y <= x2B; y += x2H)
                    {
                        f(x, y).ToString("0.###");
                        if (ogr1(x, y) == false || ogr2(x, y) == false || ogr3(x) == false || ogr4(x) == false)
                        { }
                        else { chart2.Series[1].Points.AddXY(x, y); }
                    }
                }
            }
            if (radioButton2.Checked == true)
            {
                Series series1 = chart2.Series.Add("Решение F(x1, x2) = " + m.ToString("0.###"));
                series1.ChartType = SeriesChartType.Bubble;
                chart2.Series[0].Points.AddXY(mx1 + 0.0001, mx2);
                Series series2 = chart2.Series.Add("Уровень 1 решений");
                series2.ChartType = SeriesChartType.Point;
                for (double i = -x1H; i <= x1H; i += 0.001)
                {
                    chart2.Series[1].Points.AddXY(i + mic1, Math.Sqrt((Math.Pow(x1H, 2) - Math.Pow(i, 2))) + mic2);
                    chart2.Series[1].Points.AddXY(i + mic1, -Math.Sqrt((Math.Pow(x1H, 2) - Math.Pow(i, 2))) + mic2);
                }
                Series series3 = chart2.Series.Add("Уровень 2 решений");
                series3.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 2 * x1H; i <= x1H + 2 * x1H; i += 0.001)
                {
                    chart2.Series[2].Points.AddXY(i + mic1, Math.Sqrt((Math.Pow(2 * x1H, 2) - Math.Pow(i, 2))) + mic2);
                    chart2.Series[2].Points.AddXY(i + mic1, -Math.Sqrt((Math.Pow(2 * x1H, 2) - Math.Pow(i, 2))) + mic2);
                }
                Series series4 = chart2.Series.Add("Уровень 3 решений");
                series4.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 3 * x1H; i <= x1H + 3 * x1H; i += 0.001)
                {
                    chart2.Series[3].Points.AddXY(i + mic1, Math.Sqrt((Math.Pow(3 * x1H, 2) - Math.Pow(i, 2))) + mic2);
                    chart2.Series[3].Points.AddXY(i + mic1, -Math.Sqrt((Math.Pow(3 * x1H, 2) - Math.Pow(i, 2))) + mic2);
                }
                Series series5 = chart2.Series.Add("Уровень 4 решений");
                series5.ChartType = SeriesChartType.Point;
                for (double i = -x1H - 4 * x1H; i <= x1H + 4 * x1H; i += 0.001)
                {
                    chart2.Series[4].Points.AddXY(i + mic1, Math.Sqrt((Math.Pow(4 * x1H, 2) - Math.Pow(i, 2))) + mic2);
                    chart2.Series[4].Points.AddXY(i + mic1, -Math.Sqrt((Math.Pow(4 * x1H, 2) - Math.Pow(i, 2))) + mic2);
                }
            }
        }
        public bool CheckOgr(double x1, double x2)
        {
            if (ogr1(x1, x2) == true && ogr2(x1, x2) == true && ogr3(x1) == true && ogr4(x2) == true) return true;
            else return false;
        }
        public void MethodHukaMax(double x1A, double x1B, double x2A, double x2B, double x1H, double x2H, double E, double step)
        {
           if (radioButton1.Checked == true)
                {
                double x1, x2;
                Random rand = new Random();
                do      // генерация 1ой ранд точки
                {
                    x1 = rand.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                while (CheckOgr(x1, x2) == false);
                chart3.ChartAreas[0].AxisX.Title = "X1";
                chart3.ChartAreas[0].AxisY.Title = "X2";
                chart3.ChartAreas[0].AxisX.Interval = x1H;
                chart3.ChartAreas[0].AxisY.Interval = x2H;
                chart3.ChartAreas[0].AxisX.Maximum = x1B;
                chart3.ChartAreas[0].AxisX.Minimum = x1A;
                chart3.ChartAreas[0].AxisY.Maximum = x2B;
                chart3.ChartAreas[0].AxisY.Minimum = x2A;
                chart3.Series.Add("Начало x1 = " + x1 + "; x2 = " + x2 + ";").ChartType = SeriesChartType.Bubble;
                chart3.Series.Last().Points.AddXY(x1, x2);
                chart3.Series.Add("Путь").ChartType = SeriesChartType.Line;
                chart3.Series[1].Color = Color.LawnGreen;
                chart3.Series[1].BorderWidth = 5;
                chart3.Series.Last().Points.AddXY(x1, x2);
                double fBazis = f(x1, x2);
                Stopwatch time = new Stopwatch();
                time.Start();
                double a1 = Convert.ToDouble(textBox2.Text);
                double a2 = Convert.ToDouble(textBox5.Text);
                while (step >= E && x1 <= a1 && x2 <= a2)       // Расчет остальных точке
                {
                    if (fBazis < f(x1 + step, x2) && CheckOgr(x1 + step, x2) == true)
                    {

                        fBazis = f(x1 + step, x2);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step;
                        continue;
                    }

                    if (fBazis < f(x1 - step, x2) && CheckOgr(x1 - step, x2) == true)
                    {
                        fBazis = f(x1 - step, x2);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step;
                        continue;
                    }
                    if (fBazis < f(x1, x2 - step) && CheckOgr(x1, x2 - step) == true)
                    {
                        fBazis = f(x1, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x2 -= step;
                        continue;
                    }
                    if (fBazis < f(x1, x2 + step) && CheckOgr(x1, x2 + step) == true)
                    {
                        fBazis = f(x1, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x2 += step;
                        continue;
                    }
                    if (fBazis < f(x1 - step, x2 - step) && CheckOgr(x1 - step, x2 - step) == true)
                    {
                        fBazis = f(x1 - step, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 -= step;
                        continue;
                    }
                    if (fBazis < f(x1 + step, x2 - step) && CheckOgr(x1 + step, x2 - step) == true)
                    {
                        fBazis = f(x1 + step, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 -= step;
                        continue;
                    }
                    if (fBazis < f(x1 - step, x2 + step) && CheckOgr(x1 - step, x2 + step) == true)
                    {
                        fBazis = f(x1 - step, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 += step;
                        continue;
                    }
                    if (fBazis < f(x1 + step, x2 + step) && CheckOgr(x1 + step, x2 + step) == true)
                    {
                        fBazis = f(x1 + step, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 += step;
                    }
                    else
                    {
                        step /= 2;
                    }
                }
                time.Stop();
                chart3.Series.Add("Решение f(x1, x2) = " + fBazis.ToString("0.###")).ChartType = SeriesChartType.Bubble;
                chart3.Series.Last().Points.AddXY(x1, x2);
                label8.Text = "Ответ: x1 = " + x1.ToString("0.###") + "; x2 = " + x2.ToString("0.###") + ";\nF(x1,x2) = " + fBazis.ToString("0.###") + "; " +
               "\nВремя выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            }
            if (radioButton2.Checked == true)
            {
                double x1, x2;
                Random rand = new Random();
                do      // генерация 1ой ранд точки
                {
                    x1 = rand.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                while (CheckOgr(x1, x2) == false);
                chart3.ChartAreas[0].AxisX.Title = "X1";
                chart3.ChartAreas[0].AxisY.Title = "X2";
                chart3.ChartAreas[0].AxisX.Interval = x1H;
                chart3.ChartAreas[0].AxisY.Interval = x2H;
                chart3.ChartAreas[0].AxisX.Maximum = x1B;
                chart3.ChartAreas[0].AxisX.Minimum = x1A;
                chart3.ChartAreas[0].AxisY.Maximum = x2B;
                chart3.ChartAreas[0].AxisY.Minimum = x2A;
                chart3.Series.Add("Начало x1 = " + x1 + "; x2 = " + x2 + ";").ChartType = SeriesChartType.Bubble;
                chart3.Series.Last().Points.AddXY(x1, x2);
                chart3.Series.Add("Путь").ChartType = SeriesChartType.Line;
                chart3.Series[1].Color = Color.LawnGreen;
                chart3.Series[1].BorderWidth = 5;
                chart3.Series.Last().Points.AddXY(x1, x2);
                double fBazis = f(x1, x2);
                Random rand1 = new Random();
                do      // генерация 1ой ранд точки
                {
                    x1 = rand1.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand1.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                while (x1 > x1B && x2 > x2B);
                double a1 = Convert.ToDouble(textBox2.Text);
                double a2 = Convert.ToDouble(textBox5.Text);
                Stopwatch time = new Stopwatch();
                time.Start();
                while (step >= E && x1 <= a1 && x2 <= a2)       // Расчет остальных точке
                {
                    if (fBazis < f(x1 + step, x2) && x1 + step <= x1B && x2 <= x2B)
                    {

                        fBazis = f(x1 + step, x2);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step;
                        continue;
                    }

                    if (fBazis < f(x1 - step, x2) && x1 - step <= x1B && x2 <= x2B)
                    {
                        fBazis = f(x1 - step, x2);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step;
                        continue;
                    }
                    if (fBazis < f(x1, x2 - step) && x1 <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x2 -= step;
                        continue;
                    }
                    if (fBazis < f(x1, x2 + step) && x1 <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x2 += step;
                        continue;
                    }
                    if (fBazis < f(x1 - step, x2 - step) && x1 - step <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1 - step, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 -= step;
                        continue;
                    }
                    if (fBazis < f(x1 + step, x2 - step) && x1 + step <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1 + step, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 -= step;
                        continue;
                    }
                    if (fBazis < f(x1 - step, x2 + step) && x1 - step <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1 - step, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 += step;
                        continue;
                    }
                    if (fBazis < f(x1 + step, x2 + step) && x1 + step <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1 + step, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 += step;
                    }
                    else
                    {
                        step /= 2;
                    }
                }
                time.Stop();
                chart3.Series.Add("Решение f(x1, x2) = " + fBazis.ToString("0.###")).ChartType = SeriesChartType.Bubble;
                chart3.Series.Last().Points.AddXY(x1, x2);
                label8.Text = "Ответ: x1 = " + x1.ToString("0.###") + "; x2 = " + x2.ToString("0.###") + ";\nF(x1,x2) = " + fBazis.ToString("0.###") + "; " +
               "\nВремя выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            }
        }
        public void MethodHukaMin(double x1A, double x1B, double x2A, double x2B, double x1H, double x2H, double E, double step)
        {
            if (radioButton1.Checked == true)
            {
                double x1, x2;
                Random rand = new Random();
                do      // генерация 1ой ранд точки
                {
                    x1 = rand.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                while (CheckOgr(x1, x2) == false);
                chart3.ChartAreas[0].AxisX.Title = "X1";
                chart3.ChartAreas[0].AxisY.Title = "X2";
                chart3.ChartAreas[0].AxisX.Interval = x1H;
                chart3.ChartAreas[0].AxisY.Interval = x2H;
                chart3.ChartAreas[0].AxisX.Maximum = x1B;
                chart3.ChartAreas[0].AxisX.Minimum = x1A;
                chart3.ChartAreas[0].AxisY.Maximum = x2B;
                chart3.ChartAreas[0].AxisY.Minimum = x2A;
                chart3.Series.Add("Начало x1 = " + x1 + "; x2 = " + x2 + ";").ChartType = SeriesChartType.Bubble;
                chart3.Series.Last().Points.AddXY(x1, x2);
                chart3.Series.Add("Путь").ChartType = SeriesChartType.Line;
                chart3.Series[1].Color = Color.LawnGreen;
                chart3.Series[1].BorderWidth = 5;
                chart3.Series.Last().Points.AddXY(x1, x2);
                double fBazis = f(x1, x2);
                double a1 = Convert.ToDouble(textBox2.Text);
                double a2 = Convert.ToDouble(textBox5.Text);
                Stopwatch time = new Stopwatch();
                time.Start();
                while (step >= E && x1 <= a1 && x2 <= a2)       // Расчет остальных точке
                { 
                    if (fBazis > f(x1 + step, x2) && CheckOgr(x1 + step, x2) == true)
                    {

                        fBazis = f(x1 + step, x2);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step;
                        continue;
                    }

                    if (fBazis > f(x1 - step, x2) && CheckOgr(x1 - step, x2) == true)
                    {
                        fBazis = f(x1 - step, x2);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step;
                        continue;
                    }
                    if (fBazis > f(x1, x2 - step) && CheckOgr(x1, x2 - step) == true)
                    {
                        fBazis = f(x1, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x2 -= step;
                        continue;
                    }
                    if (fBazis > f(x1, x2 + step) && CheckOgr(x1, x2 + step) == true)
                    {
                        fBazis = f(x1, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x2 += step;
                        continue;
                    }
                    if (fBazis > f(x1 - step, x2 - step) && CheckOgr(x1 - step, x2 - step) == true)
                    {
                        fBazis = f(x1 - step, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 -= step;
                        continue;
                    }
                    if (fBazis > f(x1 + step, x2 - step) && CheckOgr(x1 + step, x2 - step) == true)
                    {
                        fBazis = f(x1 + step, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 -= step;
                        continue;
                    }
                    if (fBazis > f(x1 - step, x2 + step) && CheckOgr(x1 - step, x2 + step) == true)
                    {
                        fBazis = f(x1 - step, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 += step;
                        continue;
                    }
                    if (fBazis > f(x1 + step, x2 + step) && CheckOgr(x1 + step, x2 + step) == true)
                    {
                        fBazis = f(x1 + step, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 += step;
                    }
                    else
                    {
                        step /= 2;
                    }
                }
                time.Stop();
                chart3.Series.Add("Решение f(x1, x2) = " + fBazis.ToString("0.###")).ChartType = SeriesChartType.Bubble;
                chart3.Series.Last().Points.AddXY(x1+0.001, x2+0.001);
                label8.Text = "Ответ: x1 = " + x1.ToString("0.###") + "; x2 = " + x2.ToString("0.###") + ";\nF(x1,x2) = " + fBazis.ToString("0.###") + "; " +
               "\nВремя выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            }
            if (radioButton2.Checked == true)
            {
                double x1, x2;
                Random rand = new Random();
                do      // генерация 1ой ранд точки
                {
                    x1 = rand.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                while (CheckOgr(x1, x2) == false);
                chart3.ChartAreas[0].AxisX.Title = "X1";
                chart3.ChartAreas[0].AxisY.Title = "X2";
                chart3.ChartAreas[0].AxisX.Interval = x1H;
                chart3.ChartAreas[0].AxisY.Interval = x2H;
                chart3.ChartAreas[0].AxisX.Maximum = x1B;
                chart3.ChartAreas[0].AxisX.Minimum = x1A;
                chart3.ChartAreas[0].AxisY.Maximum = x2B;
                chart3.ChartAreas[0].AxisY.Minimum = x2A;
                chart3.Series.Add("Начало x1 = " + x1 + "; x2 = " + x2 + ";").ChartType = SeriesChartType.Bubble;
                chart3.Series.Last().Points.AddXY(x1, x2);
                chart3.Series.Add("Путь").ChartType = SeriesChartType.Line;
                chart3.Series[1].Color = Color.LawnGreen;
                chart3.Series[1].BorderWidth = 5;
                chart3.Series.Last().Points.AddXY(x1, x2);
                double fBazis = f(x1, x2);
                Random rand1 = new Random();
                do      // генерация 1ой ранд точки
                {
                    x1 = rand1.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand1.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                while (x1 > x1B && x2 > x2B);
                double a1 = Convert.ToDouble(textBox2.Text);
                double a2 = Convert.ToDouble(textBox5.Text);
                Stopwatch time = new Stopwatch();
                time.Start();
                while (step >= E && x1 <= a1 && x2 <= a2)       // Расчет остальных точке
                {
                    if (fBazis > f(x1 + step, x2) && x1 + step <= x1B && x2 <= x2B)
                    {

                        fBazis = f(x1 + step, x2);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step;
                        continue;
                    }

                    if (fBazis > f(x1 - step, x2) && x1 - step <= x1B && x2 <= x2B)
                    {
                        fBazis = f(x1 - step, x2);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step;
                        continue;
                    }
                    if (fBazis > f(x1, x2 - step) && x1 <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x2 -= step;
                        continue;
                    }
                    if (fBazis > f(x1, x2 + step) && x1 <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x2 += step;
                        continue;
                    }
                    if (fBazis > f(x1 - step, x2 - step) && x1 - step <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1 - step, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 -= step;
                        continue;
                    }
                    if (fBazis > f(x1 + step, x2 - step) && x1 + step <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1 + step, x2 - step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 -= step;
                        continue;
                    }
                    if (fBazis > f(x1 - step, x2 + step) && x1 - step <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1 - step, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 += step;
                        continue;
                    }
                    if (fBazis > f(x1 + step, x2 + step) && x1 + step <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1 + step, x2 + step);
                        dataGridView4.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart3.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 += step;
                    }
                    else
                    {
                        step /= 2;
                    }
                }
                time.Stop();
                chart3.Series.Add("Решение f(x1, x2) = " + fBazis.ToString("0.###")).ChartType = SeriesChartType.Bubble;
                chart3.Series.Last().Points.AddXY(x1, x2);
                label8.Text = "Ответ: x1 = " + x1.ToString("0.###") + "; x2 = " + x2.ToString("0.###") + ";\nF(x1,x2) = " + fBazis.ToString("0.###") + "; " +
               "\nВремя выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            }
        }
        public double Penalty(double x1, double x2)
        {
            xo1 = 0;
            yo1 = 0;
            xo1 = x1;
            yo1 = x2;
            ExpressionParser parser = new ExpressionParser();
            parser.Values.Add("x1", xo1);
            parser.Values.Add("x2", yo1);
            string s1 = textBox12.Text;
            string s2 = textBox13.Text;
            string s3 = textBox15.Text;
            string s4 = textBox14.Text;
            double r = 4, pen = 0;
            if (x1 < 0 && (parser.Parse(s1) - parser.Parse(s2)) < 0 && (parser.Parse(s3) - parser.Parse(s4)) < 0 && x2 < 0) 
                pen = 1 / ((parser.Parse(s1) - parser.Parse(s2)) + x1 + x2 + (parser.Parse(s3) - parser.Parse(s4)));
            return r * pen;
        }
        public double Penalty1(double x1, double x2)
        {
            xo1 = 0;
            yo1 = 0;
            xo1 = x1;
            yo1 = x2;
            ExpressionParser parser = new ExpressionParser();
            parser.Values.Add("x1", xo1);
            parser.Values.Add("x2", yo1);
            string s1 = textBox12.Text;
            string s2 = textBox13.Text;
            string s3 = textBox15.Text;
            string s4 = textBox14.Text;
            double r = 4, pen = 0;
            if (x1 >= 0 && (parser.Parse(s1) - parser.Parse(s2)) >= 0 && (parser.Parse(s3) - parser.Parse(s4)) >= 0 && x2 >= 0)
                pen = 1 / ((parser.Parse(s1) - parser.Parse(s2)) + x1 + x2 + (parser.Parse(s3) - parser.Parse(s4)));
            return r * pen;
        }
        public void MethodOfPenaltyFuncMax(double x1A, double x1B, double x2A, double x2B, double x1H, double x2H, double E, double step)
        {
            if (radioButton1.Checked == true)
            {
                double x1, x2;
                Random rand = new Random();
                do      // генерация 1ой ранд точки
                {
                    x1 = rand.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                while (CheckOgr(x1, x2) == false);
                chart4.ChartAreas[0].AxisX.Title = "X1";
                chart4.ChartAreas[0].AxisY.Title = "X2";
                chart4.ChartAreas[0].AxisX.Interval = x1H;
                chart4.ChartAreas[0].AxisY.Interval = x2H;
                chart4.ChartAreas[0].AxisX.Maximum = x1B;
                chart4.ChartAreas[0].AxisX.Minimum = x1A;
                chart4.ChartAreas[0].AxisY.Maximum = x2B;
                chart4.ChartAreas[0].AxisY.Minimum = x2A;
                chart4.Series.Add("Начало x1 = " + x1 + "; x2 = " + x2 + ";").ChartType = SeriesChartType.Bubble;
                chart4.Series.Last().Points.AddXY(x1, x2);
                chart4.Series.Add("Путь").ChartType = SeriesChartType.Line;
                chart4.Series[1].Color = Color.LawnGreen;
                chart4.Series[1].BorderWidth = 5;
                chart4.Series.Last().Points.AddXY(x1, x2);
                double fBazis = f(x1, x2) + Penalty(x1, x2);
                double a1 = Convert.ToDouble(textBox2.Text);
                double a2 = Convert.ToDouble(textBox5.Text);
                Stopwatch time = new Stopwatch();
                time.Start();
                while (step >= E && x1 <= a1 && x2 <= a2)       // Расчет остальных точке
                {
                    if (fBazis < (f(x1 + step, x2) + Penalty(x1 + step, x2)) && CheckOgr(x1 + step, x2) == true)
                    {

                        fBazis = f(x1 + step, x2);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step;
                        continue;
                    }

                    if (fBazis < (f(x1 - step, x2) + Penalty(x1 - step, x2)) && CheckOgr(x1 - step, x2) == true)
                    {
                        fBazis = f(x1 - step, x2);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step;
                        continue;
                    }
                    if (fBazis < (f(x1, x2 - step) + Penalty(x1, x2 - step)) && CheckOgr(x1, x2 - step) == true)
                    {
                        fBazis = f(x1, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x2 -= step;
                        continue;
                    }
                    if (fBazis < (f(x1, x2 + step) + Penalty(x1, x2 + step)) && CheckOgr(x1, x2 + step) == true)
                    {
                        fBazis = f(x1, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x2 += step;
                        continue;
                    }
                    if (fBazis < (f(x1 - step, x2 - step) + Penalty(x1 - step, x2 - step)) && CheckOgr(x1 - step, x2 - step) == true)
                    {
                        fBazis = f(x1 - step, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 -= step;
                        continue;
                    }
                    if (fBazis < (f(x1 + step, x2 - step) + Penalty(x1 + step, x2 - step)) && CheckOgr(x1 + step, x2 - step) == true)
                    {
                        fBazis = f(x1 + step, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 -= step;
                        continue;
                    }
                    if (fBazis < (f(x1 - step, x2 + step) + Penalty(x1 - step, x2 + step)) && CheckOgr(x1 - step, x2 + step) == true)
                    {
                        fBazis = f(x1 - step, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 += step;
                        continue;
                    }
                    if (fBazis < (f(x1 + step, x2 + step) + Penalty(x1 + step, x2 + step)) && CheckOgr(x1 + step, x2 + step) == true)
                    {
                        fBazis = f(x1 + step, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 += step;
                    }
                    else
                    {
                        step /= 2;
                    }
                }
                time.Stop();
                chart4.Series.Add("Решение f(x1, x2) = " + fBazis.ToString("0.###")).ChartType = SeriesChartType.Bubble;
                chart4.Series.Last().Points.AddXY(x1, x2);
                label8.Text = "Ответ: x1 = " + x1.ToString("0.###") + "; x2 = " + x2.ToString("0.###") + ";\nF(x1,x2) = " + fBazis.ToString("0.###") + "; " +
               "\nВремя выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            }
            if (radioButton2.Checked == true)
            {
                double x1, x2;
                Random rand = new Random();
                do      // генерация 1ой ранд точки
                {
                    x1 = rand.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                while (CheckOgr(x1, x2) == false);
                chart4.ChartAreas[0].AxisX.Title = "X1";
                chart4.ChartAreas[0].AxisY.Title = "X2";
                chart4.ChartAreas[0].AxisX.Interval = x1H;
                chart4.ChartAreas[0].AxisY.Interval = x2H;
                chart4.ChartAreas[0].AxisX.Maximum = x1B;
                chart4.ChartAreas[0].AxisX.Minimum = x1A;
                chart4.ChartAreas[0].AxisY.Maximum = x2B;
                chart4.ChartAreas[0].AxisY.Minimum = x2A;
                chart4.Series.Add("Начало x1 = " + x1 + "; x2 = " + x2 + ";").ChartType = SeriesChartType.Bubble;
                chart4.Series.Last().Points.AddXY(x1, x2);
                chart4.Series.Add("Путь").ChartType = SeriesChartType.Line;
                chart4.Series[1].Color = Color.LawnGreen;
                chart4.Series[1].BorderWidth = 5;
                chart4.Series.Last().Points.AddXY(x1, x2);
                double fBazis = f(x1, x2) + Penalty(x1, x2);
                Random rand1 = new Random();
                do      // генерация 1ой ранд точки
                {
                    x1 = rand1.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand1.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                while (x1 > x1B && x2 > x2B);
                double a1 = Convert.ToDouble(textBox2.Text);
                double a2 = Convert.ToDouble(textBox5.Text);
                Stopwatch time = new Stopwatch();
                time.Start();
                while (step >= E && x1 <= a1 && x2 <=a2)       // Расчет остальных точке
                {
                    if (fBazis < (f(x1 + step, x2) + Penalty(x1 + step, x2)) && x1 + step <= x1B && x2 <= x2B)
                    {

                        fBazis = f(x1 + step, x2);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step;
                        continue;
                    }

                    if (fBazis < (f(x1 - step, x2) + Penalty(x1 - step, x2)) && x1 - step <= x1B && x2 <= x2B)
                    {
                        fBazis = f(x1 - step, x2);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step;
                        continue;
                    }
                    if (fBazis < (f(x1, x2 - step) + Penalty(x1, x2 - step)) && x1 <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x2 -= step;
                        continue;
                    }
                    if (fBazis < (f(x1, x2 + step) + Penalty(x1, x2 + step)) && x1 <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x2 += step;
                        continue;
                    }
                    if (fBazis < (f(x1 - step, x2 - step) + Penalty(x1 - step, x2 - step)) && x1 - step <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1 - step, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 -= step;
                        continue;
                    }
                    if (fBazis < (f(x1 + step, x2 - step) + Penalty(x1 + step, x2 - step)) && x1 + step <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1 + step, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 -= step;
                        continue;
                    }
                    if (fBazis < (f(x1 - step, x2 + step) + Penalty(x1 - step, x2 + step)) && x1 - step <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1 - step, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 += step;
                        continue;
                    }
                    if (fBazis < (f(x1 + step, x2 + step) + Penalty(x1 + step, x2 + step)) && x1 + step <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1 + step, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 += step;
                    }
                    else
                    {
                        step /= 2;
                    }
                }
                time.Stop();
                chart4.Series.Add("Решение f(x1, x2) = " + fBazis.ToString("0.###")).ChartType = SeriesChartType.Bubble;
                chart4.Series.Last().Points.AddXY(x1, x2);
                label8.Text = "Ответ: x1 = " + x1.ToString("0.###") + "; x2 = " + x2.ToString("0.###") + ";\nF(x1,x2) = " + fBazis.ToString("0.###") + "; " +
               "\nВремя выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            }
        }
        public void MethodOfPenaltyFuncMin(double x1A, double x1B, double x2A, double x2B, double x1H, double x2H, double E, double step)
        {
            if (radioButton1.Checked == true)
            {
                double x1, x2;
                Random rand = new Random();
                do      // генерация 1ой ранд точки
                {
                    x1 = rand.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                while (CheckOgr(x1, x2) == false);
                chart4.ChartAreas[0].AxisX.Title = "X1";
                chart4.ChartAreas[0].AxisY.Title = "X2";
                chart4.ChartAreas[0].AxisX.Interval = x1H;
                chart4.ChartAreas[0].AxisY.Interval = x2H;
                chart4.ChartAreas[0].AxisX.Maximum = x1B;
                chart4.ChartAreas[0].AxisX.Minimum = x1A;
                chart4.ChartAreas[0].AxisY.Maximum = x2B;
                chart4.ChartAreas[0].AxisY.Minimum = x2A;
                chart4.Series.Add("Начало x1 = " + x1 + "; x2 = " + x2 + ";").ChartType = SeriesChartType.Bubble;
                chart4.Series.Last().Points.AddXY(x1, x2);
                chart4.Series.Add("Путь").ChartType = SeriesChartType.Line;
                chart4.Series[1].Color = Color.LawnGreen;
                chart4.Series[1].BorderWidth = 5;
                chart4.Series.Last().Points.AddXY(x1, x2);
                double fBazis = f(x1, x2) + Penalty1(x1, x2);
                double a1 = Convert.ToDouble(textBox2.Text);
                double a2 = Convert.ToDouble(textBox5.Text);
                Stopwatch time = new Stopwatch();
                time.Start();
                while (step >= E && x1 <= a1 && x2<=a2)       // Расчет остальных точке
                {
                    if (fBazis > (f(x1 + step, x2) + Penalty1(x1 + step, x2)) && CheckOgr(x1 + step, x2) == true)
                    {

                        fBazis = f(x1 + step, x2);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step;
                        continue;
                    }

                    if (fBazis > (f(x1 - step, x2) + Penalty1(x1 - step, x2)) && CheckOgr(x1 - step, x2) == true)
                    {
                        fBazis = f(x1 - step, x2);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step;
                        continue;
                    }
                    if (fBazis > (f(x1, x2 - step) + Penalty1(x1, x2 - step)) && CheckOgr(x1, x2 - step) == true)
                    {
                        fBazis = f(x1, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x2 -= step;
                        continue;
                    }
                    if (fBazis > (f(x1, x2 + step) + Penalty1(x1, x2 + step)) && CheckOgr(x1, x2 + step) == true)
                    {
                        fBazis = f(x1, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x2 += step;
                        continue;
                    }
                    if (fBazis > (f(x1 - step, x2 - step) + Penalty1(x1 - step, x2 - step)) && CheckOgr(x1 - step, x2 - step) == true)
                    {
                        fBazis = f(x1 - step, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 -= step;
                        continue;
                    }
                    if (fBazis > (f(x1 + step, x2 - step) + Penalty1(x1 + step, x2 - step)) && CheckOgr(x1 + step, x2 - step) == true)
                    {
                        fBazis = f(x1 + step, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 -= step;
                        continue;
                    }
                    if (fBazis > (f(x1 - step, x2 + step) + Penalty1(x1 - step, x2 + step)) && CheckOgr(x1 - step, x2 + step) == true)
                    {
                        fBazis = f(x1 - step, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 += step;
                        continue;
                    }
                    if (fBazis > (f(x1 + step, x2 + step) + Penalty1(x1 + step, x2 + step)) && CheckOgr(x1 + step, x2 + step) == true)
                    {
                        fBazis = f(x1 + step, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 += step;
                    }
                    else
                    {
                        step /= 2;
                    }
                }
                time.Stop();
                chart4.Series.Add("Решение f(x1, x2) = " + fBazis.ToString("0.###")).ChartType = SeriesChartType.Bubble;
                chart4.Series.Last().Points.AddXY(x1, x2);
                label8.Text = "Ответ: x1 = " + x1.ToString("0.###") + "; x2 = " + x2.ToString("0.###") + ";\nF(x1,x2) = " + fBazis.ToString("0.###") + "; " +
               "\nВремя выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            }
            if (radioButton2.Checked == true)
            {
                double x1, x2;
                Random rand = new Random();
                do      // генерация 1ой ранд точки
                {
                    x1 = rand.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                while (x1 > x1B && x2 > x2B);
                chart4.ChartAreas[0].AxisX.Title = "X1";
                chart4.ChartAreas[0].AxisY.Title = "X2";
                chart4.ChartAreas[0].AxisX.Interval = x1H;
                chart4.ChartAreas[0].AxisY.Interval = x2H;
                chart4.ChartAreas[0].AxisX.Maximum = x1B;
                chart4.ChartAreas[0].AxisX.Minimum = x1A;
                chart4.ChartAreas[0].AxisY.Maximum = x2B;
                chart4.ChartAreas[0].AxisY.Minimum = x2A;
                chart4.Series.Add("Начало x1 = " + x1 + "; x2 = " + x2 + ";").ChartType = SeriesChartType.Bubble;
                chart4.Series.Last().Points.AddXY(x1, x2);
                chart4.Series.Add("Путь").ChartType = SeriesChartType.Line;
                chart4.Series[1].Color = Color.LawnGreen;
                chart4.Series[1].BorderWidth = 5;
                chart4.Series.Last().Points.AddXY(x1, x2);
                double fBazis = f(x1, x2) + Penalty1(x1, x2);
                Random rand1 = new Random();
                if (x1 <= x1B && x2 <= x2B)   // генерация 1ой ранд точки
                {
                    x1 = rand1.Next((int)((x1B - x1A) / x1H)) * x1H - x1H;
                    x2 = rand1.Next((int)((x2B - x2A) / x2H)) * x2H - x2H;
                }
                else MessageBox.Show("Точка вне границ!");
                double a1 = Convert.ToDouble(textBox2.Text);
                double a2 = Convert.ToDouble(textBox5.Text);
                Stopwatch time = new Stopwatch();
                time.Start();
                while (step >= E)       // Расчет остальных точке
                {
                    if (fBazis > (f(x1 + step, x2) + Penalty1(x1 + step, x2)) && x1 + step <= x1B && x2 <= x2B)
                    {

                        fBazis = f(x1 + step, x2);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step;
                        continue;
                    }

                    if (fBazis > (f(x1 - step, x2) + Penalty1(x1 - step, x2)) && x1 - step <= x1B && x2 <= x2B)
                    {
                        fBazis = f(x1 - step, x2);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step;
                        continue;
                    }
                    if (fBazis > (f(x1, x2 - step) + Penalty1(x1, x2 - step)) && x1 <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x2 -= step;
                        continue;
                    }
                    if (fBazis > (f(x1, x2 + step) + Penalty1(x1, x2 + step)) && x1 <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x2 += step;
                        continue;
                    }
                    if (fBazis > (f(x1 - step, x2 - step) + Penalty1(x1 - step, x2 - step)) && x1 - step <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1 - step, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 -= step;
                        continue;
                    }
                    if (fBazis > (f(x1 + step, x2 - step) + Penalty1(x1 + step, x2 - step)) && x1 + step <= x1B && x2 - step <= x2B)
                    {
                        fBazis = f(x1 + step, x2 - step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 -= step;
                        continue;
                    }
                    if (fBazis > (f(x1 - step, x2 + step) + Penalty1(x1 - step, x2 + step)) && x1 - step <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1 - step, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 -= step; x2 += step;
                        continue;
                    }
                    if (fBazis > (f(x1 + step, x2 + step) + Penalty1(x1 + step, x2 + step)) && x1 + step <= x1B && x2 + step <= x2B)
                    {
                        fBazis = f(x1 + step, x2 + step);
                        dataGridView5.Rows.Add(x1, x2, fBazis.ToString("0.###"));
                        chart4.Series.Last().Points.AddXY(x1, x2);
                        x1 += step; x2 += step;
                    }
                    else
                    {
                        step /= 1.5;
                        continue;
                    }
                }
                time.Stop();
                chart4.Series.Add("Решение f(x1, x2) = " + fBazis.ToString("0.###")).ChartType = SeriesChartType.Bubble;
                chart4.Series.Last().Points.AddXY(x1, x2);
                label8.Text = "Ответ: x1 = " + x1.ToString("0.###") + "; x2 = " + x2.ToString("0.###") + ";\nF(x1,x2) = " + fBazis.ToString("0.###") + "; " +
               "\nВремя выполнения метода: " + (time.ElapsedMilliseconds) + " мс";
            }
        }
        private void button1_Click(object sender, EventArgs e)
            {
            label21.Text = "";
            if (comboBox2.Text == "Метод Сеток")
            {
                tabControl1.SelectedIndex = 0;
                chart1.Series.Clear();
                try
                {
                    label8.Text = "";
                    dataGridView1.Rows.Clear();
                    dataGridView3.Rows.Clear();
                    double x1A = Convert.ToDouble(textBox1.Text);
                    double x1B = Convert.ToDouble(textBox2.Text);
                    double x1H = Convert.ToDouble(textBox3.Text);
                    double x2A = Convert.ToDouble(textBox4.Text);
                    double x2B = Convert.ToDouble(textBox5.Text);
                    double x2H = Convert.ToDouble(textBox6.Text);
                    if (comboBox1.Text == "Max") MethodPereboraMax(x1A, x1B, x1H, x2A, x2B, x2H);
                    if (comboBox1.Text == "Min") MethodPereboraMin(x1A, x1B, x1H, x2A, x2B, x2H);
                }
                catch (System.FormatException)
                {
                    MessageBox.Show("Неверный формат ввода или имеются пустые поля");
                }
            }
            if (comboBox2.Text == "Метод Монте-Карло")
            {
                chart2.Series.Clear();
                chart1.Series.Clear();
                tabControl1.SelectedIndex = 1;
                try
                {
                    label8.Text = "";
                    dataGridView2.Rows.Clear();
                    dataGridView3.Rows.Clear();
                    double x1A = Convert.ToDouble(textBox1.Text);
                    double x1B = Convert.ToDouble(textBox2.Text);
                    double x1H = Convert.ToDouble(textBox3.Text);
                    double x2A = Convert.ToDouble(textBox4.Text);
                    double x2B = Convert.ToDouble(textBox5.Text);
                    double x2H = Convert.ToDouble(textBox6.Text);
                    int kol = Convert.ToInt32(textBox8.Text);
                    if (comboBox1.Text == "Max") MethodPereboraMax(x1A, x1B, x1H, x2A, x2B, x2H);
                    if (comboBox1.Text == "Min") MethodPereboraMin(x1A, x1B, x1H, x2A, x2B, x2H);
                    if (comboBox1.Text == "Max") MonteCarloMax(x1A, x1B, x2A, x2B, kol, x1H, x2H);
                    if (comboBox1.Text == "Min") MonteCarloMin(x1A, x1B, x2A, x2B, kol, x1H, x2H);
                }
                catch (System.FormatException)
                {
                    MessageBox.Show("Неверный формат ввода");
                    return;
                }
               
            }
            if (comboBox2.Text == "Метод Хука-Дживса")
            {
                chart3.Series.Clear();
                dataGridView4.Rows.Clear();
                tabControl1.SelectedIndex = 2;
                try
                {
                    label8.Text = "";
                    double E = Convert.ToDouble(textBox10.Text);
                    double step = Convert.ToDouble(textBox16.Text);
                    double x1A = Convert.ToDouble(textBox1.Text);
                    double x1B = Convert.ToDouble(textBox2.Text);
                    double x1H = Convert.ToDouble(textBox3.Text);
                    double x2A = Convert.ToDouble(textBox4.Text);
                    double x2B = Convert.ToDouble(textBox5.Text);
                    double x2H = Convert.ToDouble(textBox6.Text);
                    if (comboBox1.Text == "Max") MethodHukaMax(x1A, x1B, x2A, x2B, x1H, x2H, E, step);
                    if (comboBox1.Text == "Min") MethodHukaMin(x1A, x1B, x2A, x2B, x1H, x2H, E, step);
                }
                catch (Exception o)
                {
                    MessageBox.Show("Ошибка: " + o.Message);
                    return;
                }
            }
            if (comboBox2.Text == "Метод Штрафных Функций")
            {
                chart4.Series.Clear();
                dataGridView5.Rows.Clear();
                tabControl1.SelectedIndex = 3;
                try
                {
                    label8.Text = "";
                    double E = Convert.ToDouble(textBox10.Text);
                    double step = Convert.ToDouble(textBox16.Text);
                    double x1A = Convert.ToDouble(textBox1.Text);
                    double x1B = Convert.ToDouble(textBox2.Text);
                    double x1H = Convert.ToDouble(textBox3.Text);
                    double x2A = Convert.ToDouble(textBox4.Text);
                    double x2B = Convert.ToDouble(textBox5.Text);
                    double x2H = Convert.ToDouble(textBox6.Text);
                    if (comboBox1.Text == "Max") MethodOfPenaltyFuncMax(x1A, x1B, x2A, x2B, x1H, x2H, E, step);
                    if (comboBox1.Text == "Min") MethodOfPenaltyFuncMin(x1A, x1B, x2A, x2B, x1H, x2H, E, step);
                }
                catch (Exception o)
                {
                    MessageBox.Show("Ошибка: " + o.Message);
                    return;
                }
            }
        }
    }
}
