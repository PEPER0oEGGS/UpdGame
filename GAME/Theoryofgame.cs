using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME
{
    class Theoryofgame
    {
        // игрок 1 максимум I Строки
        // игрок 2 минимум j Столбцы

        int I;// Количество стратегий первого игрока
        int J;// Количество стратегий второго игрока
        double[,] Game; //Матрица игры
        bool ExistenMatrix = false;
        bool eazyplay = false; 
        double maxmin = 0, minmax = 0;
        double price_game = 0;
        int[] strategy = new int[2];
        Queue<double> Strategy1 = new Queue<double>();
        Queue<double> Strategy2 = new Queue<double>();

        public void CreateGame(int i, int j)
        {
            I = i;
            J = j;
            Game = new double[I, J];
            ExistenMatrix = true;
            eazyplay = false;
        }//Инициализация матрицы игры.
        public void SetPrise(int i, int j, double price)
        {
            if (ExistenMatrix)
            {
                Game[i, j] = price;
                eazyplay = false;
            }

        }//Задание элемента матрицы
        void Maxmin()
        {
            
            double local_min_j = 0;
            for (int i = 0; i < I; i++)
            {
                for (int j = 0; j < J; j++)
                {
                    if (j == 0) local_min_j = Game[i, j];
                    if (local_min_j > Game[i, j]) { local_min_j = Game[i, j]; }
                }
                if (i == 0) { maxmin = local_min_j; strategy[0] = i; }
                if (maxmin < local_min_j) { maxmin = local_min_j; strategy[0] = i; }
            }
        }//поиск наибольшего минимума
        void Minmax()
        {
            double local_max_i = 0;
            for (int j = 0; j < J; j++)
            {
                for (int i = 0; i < I; i++)
                {
                    if (i == 0) local_max_i = Game[i, j];
                    if (local_max_i < Game[i, j]){ local_max_i = Game[i, j]; }
            }
                if (j == 0) { minmax = local_max_i; strategy[1] = j; }
                if (minmax > local_max_i) { minmax = local_max_i; strategy[1] = j; }
            }
        }//поиск наименьшего максимума
        public double[] EazyPlay()
        {
            if (!ExistenMatrix) return (new double[]{-1,0,-1});
            if (!eazyplay)
            {
            Maxmin();
            Minmax();
            eazyplay = true;
            }
            return (new double[] { maxmin, minmax, ((maxmin + minmax) / 2) });
        } // расчет минимакса и максимина в простых стратегиях
        public void Play(int MaxRound)
        {
            if (ExistenMatrix) {
            double[] x = new double[I];
            double[] y = new double[J];
            double[] pc = new double[I];
            double[] fi = new double[J];
            
            double omax = 1000000; //~ max V(n))/n 
            double omin = -1000000; //~(min U(n))/n 
            int round = 1; // такт вычисления
            int Collum = 0;  // случайно выбраный столбец (+ его инициализация) J
            int Line = 0; // инициализация константы со строкой. I

            if (!eazyplay) { EazyPlay(); }
            Collum = strategy[1];
            price_game = 0;

            for (bool repit  =true; repit;)
            {
                    
                    Line = 0; // Выбрали первый элемент столбца         
                    for (int i = 0; i < I; i++)
                    {
                        pc[i] = pc[i] + Game[Collum, i]; // Вектор = вектор + элемент J строки 
                        if (pc[i] >= pc[Line]) { Line = i; }// если I (элемент столбца) не минимальный, то I=i   pc - вектор первого игрока
                    }// перебираем столбец и ищем минимальный
                    
                    x[Line] = x[Line] + 1; // x имеет размерность количества строк и показывает сколько раз была выбранна I-я строка 

                    Collum = 0; //запомнили первый столбец
                
                for (int j = 0; j < J; j++)
                {
                    fi[j] = fi[j] + Game[j, Line]; // Вектор = вектор + элементы I стотбца 
                    if (fi[j] <= fi[Collum]) { Collum = j; } // если J (элемент строки) не мax, то J=j   Fi - вектор второго игрока
                } //перебираем строку и ищем максимальное

                    y[Collum] = y[Collum] + 1; // x имеет размерность количества столбцов и показывает сколько раз был выбранн J-й столбец 

                if (Math.Abs(omax - pc[Collum] / round) >= 0) { omax = pc[Collum] / round; }
                if (Math.Abs(omin - fi[Collum] / round) >= 0) { omin = fi[Collum] / round; }

                round++;

                price_game += Game[Line, Collum];//цена игры

                if (round > MaxRound) { repit = false; round--; }
                //if (Math.Abs(omax - omin) < eps) { if (ai > 100) { repit = false; ai--; } }
            } //Расчет игры.
              price_game = price_game / MaxRound;
            for (int i = 0,j = 0; i < I||j<J; i++,j++)
            {
                if(i < I) Strategy1.Enqueue(x[i] / round);

                if (j < J) Strategy2.Enqueue(y[j] / round);

            }
            }
        }// просчитать смешанные стратегии
        public int GetFirstStrategyEazy()
        {
            return strategy[1];
        }
        public int GetSecondtStrategyEazy()
        {
            return strategy[0];
        }
        public double Price_game()
        {
            return price_game;
        }
        public double Price_game_Eazy()
        {
            return Game[strategy[0], strategy[1]];
        }
        public Queue<double> GetFirstStrategy()
        {
            return Strategy1;
        }
        public Queue<double> GetSecondStrategy()
        {
            return Strategy2;
        }

    }
}
