using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GAME
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Theoryofgame Game = new Theoryofgame();
        double c1 = 0, c2 = 0;
        int n, nmax;

        private void button2_Click(object sender, EventArgs e)
        {

            bool repit = false;
            bool print = false;

            if (readConst())
            {
                repit = true;
                print = true;
            } //читаем константы


            if (repit)
            {
                Game.CreateGame(n, n); // создаем игру

                for (int i = 0; i < n; i++)
                {
                    double V = c1 / c2;
                    double r = c2 * 2;
                    double StopAngle = Math.Asin(V);
                    for (int j = 0; j < n; j++)
                    {

                        double Psi = -StopAngle + j * ((StopAngle * 2)/n);
                        double Fi = -Math.PI / 2 +  i * (Math.PI / n);
                        if ((double)i == (double)n / 2) { Fi = -Math.PI / 2 + (i + 1) * (Math.PI / n); }
                        if ((double)j == (double)n / 2) { Psi = -StopAngle + (j + 1) * ((StopAngle * 2) / n); }
                        if (Math.Acos(Math.Cos(Psi) / V) >= Fi)
                        { Game.SetPrise(i, j, r);}
                        else
                        { Game.SetPrise(i, j, ((r * Math.Abs(Math.Sin(Psi) - Math.Sin(Fi))) / Math.Sqrt(1 + V * V - 2 * V * Math.Cos(Fi - Psi)))); }
                    }

                } // заполняем элементы

                Game.Play(nmax); // Рассчет Nmax раундов игры

                if (print){Print();} // печать результатов

            }

        } //генерация и вычисление


        void Print()
        {     
            textBox9.Text = "Вычисленные значения:" + Environment.NewLine;
            double[] play = Game.EazyPlay();
            textBox9.Text += "Среднее = " + (String.Format("{0:0.00}", play[2])) + "; MaxMin = " + (String.Format("{0:0.00}", play[1])) + "; MinMax = " + (String.Format("{0:0.00}", play[0])) + Environment.NewLine; 
            textBox9.Text += "Стратегии: " + (Game.GetFirstStrategyEazy()+1) + " столбец; " + (Game.GetSecondtStrategyEazy() + 1) + " строка. Цена игры: " + String.Format("{0:0.00}", Game.Price_game_Eazy()) + Environment.NewLine;
            textBox9.Text += "Цена игры в смешанных стратегиях: " + String.Format("{0:0.00}", Game.Price_game()) + Environment.NewLine;

            this.chart1.Series["Series1"].Points.Clear();
            this.chart2.Series["Series1"].Points.Clear();
            this.chart1.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();
            this.chart2.ChartAreas["ChartArea1"].AxisX.RoundAxisValues();
            Queue<double> X1 = Game.GetFirstStrategy(); 
            Queue<double> X2 = Game.GetSecondStrategy();
            double StopAngle = Math.Asin(c1 / c2);
            for (int i = 0; i < n; i++)
            {
                double Psi = -StopAngle + i * ((StopAngle * 2) / n);
                double Fi = -Math.PI / 2 + i * (Math.PI / n);
                if ((double)i == (double)n / 2) { Fi = -Math.PI / 2 + (i + 1) * (Math.PI / n); }
                if ((double)i == (double)n / 2) { Psi = -StopAngle + (i + 1) * ((StopAngle * 2) / n); }
                this.chart1.Series["Series1"].Points.AddXY( (Fi) *360/(2*Math.PI), X1.Dequeue());
                 this.chart2.Series["Series1"].Points.AddXY(Psi * 360 / (2 * Math.PI), X2.Dequeue());
            }
            this.chart1.Legends.Clear();
            this.chart2.Legends.Clear();
            
        } // печать результатов 
        bool readConst()
        {
            bool b = true;
            try { n = int.Parse(textBox1.Text); }
            catch { MessageBox.Show(("Ошибка введения размера матрицы." + Environment.NewLine + "Используйте целые числа.")); b = false; }
            try { nmax = int.Parse(textBox2.Text); }
            catch { MessageBox.Show(("Ошибка введения тактов вычисления." + Environment.NewLine + "Используйте целые числа.")); b = false; }
            try { c1 = double.Parse(textBox4.Text); }
            catch { MessageBox.Show(("Ошибка введения расчетного параметра Скорости E" + Environment.NewLine + "Проверьте знак разделения целой и дробной части числа.")); b = false; }
            try { c2 = double.Parse(textBox6.Text); }
            catch { MessageBox.Show(("Ошибка введения расчетного параметра Скорости P" + Environment.NewLine + "Проверьте знак разделения целой и дробной части числа.")); b = false; }
            return (b);
        } // чтение констант
    }
}
