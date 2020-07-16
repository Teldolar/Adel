using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adel_Practice
{
    public partial class Form1 : Form
    {
        Pirate[] pirates;
        Boat[] boats;
        MainMenu MnMen1;
        MenuItem pnkt1;
        MenuItem pnkt2;
        MenuItem info;
        public int hehe;
        public Form1()
        {
            InitializeComponent();
            //создание меню
            pnkt1 = new MenuItem("Пуск", new EventHandler(timer1_Tick), Shortcut.Alt1);
            pnkt2 = new MenuItem("Стоп", new EventHandler(stop), Shortcut.Alt2);
            info = new MenuItem("Информация о разработчике", new EventHandler(InfoORazrabotchik), Shortcut.Alt3);
            MnMen1 = new MainMenu(new MenuItem[] { pnkt1, pnkt2, info });
            this.Menu = MnMen1;
            //создание массива из объектов класса boat(лодки) в количестве 2 штук
            boats = new Boat[2];
            //создание объектов класса boat 
            boats[0] = new Boat(new Point(0, 200), Brushes.Red, 2,1,1);
            boats[1] = new Boat(new Point(0, 300), Brushes.Red, 2,2,0);
            //создание массива из объектов класса pirate(пираты) в количестве 4 штук
            pirates = new Pirate[4];
            //создание объектов класса pirate 
            pirates[0] = new Pirate(new Point(0, 200),Brushes.Red,4,1,1,5);
            pirates[1] = new Pirate(new Point(0, 225), Brushes.Red, 4,1,0,5);
            pirates[2] = new Pirate(new Point(0, 300), Brushes.Red, 4,2,0,5);
            pirates[3] = new Pirate(new Point(0, 325), Brushes.Red, 4,2,0,1);
            //двойная буферизация
            DoubleBuffered = true;
            //событие для подсчета прибывших лодок
            boats[1].plus_count += boats[0].Check_count;
            //событие для старта движения пиратов, после остановки лодок
            boats[0].startmoving += pirates[0].First_moving;
            boats[0].startmoving += pirates[1].First_moving;
            boats[0].startmoving += pirates[2].First_moving;
            boats[0].startmoving += pirates[3].First_moving;
            //событие для подсчета прибывших пиратов
            pirates[1].plus_count += pirates[0].Check_count;
            pirates[2].plus_count += pirates[0].Check_count;
            pirates[3].plus_count += pirates[0].Check_count;
            //события для начала движения лодок в обратнуюсторону
            pirates[0].startmoving += boats[0].Second_moving;
            pirates[0].startmoving += boats[1].Second_moving;
        }
        private void stop(object sender, EventArgs e)
        {
            //остановка таймера
            timer1.Enabled = false;
        }
        private void InfoORazrabotchik(object sender, EventArgs e)
        {
            //о разработчике
            MessageBox.Show("Выполнил: студент группы 4208\nГареев А.И.\nВариант работы: 3", "Информация о разработчике");
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //отрисовка всех фигур
            boats[0].draw(e.Graphics);
            boats[1].draw(e.Graphics);
            pirates[0].draw(e.Graphics);
            pirates[1].draw(e.Graphics);
            pirates[2].draw(e.Graphics);
            pirates[3].draw(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //старт таймера
            timer1.Enabled = true;
            //движение всех фигур
            boats[0].Moving();
            boats[1].Moving();
            pirates[0].Moving();
            pirates[1].Moving();
            pirates[2].Moving();
            pirates[3].Moving();
            Invalidate();
        }
    }
    class Boat
    {
        //всякие переменные (по названию понятно за что отвечают)
        public Point position;
        Brush color;
        Size size;
        //состояние в котором находятся лодки
        int step;
        //количество прибывших лодок
        int check_count=0;
        //количество лодок, которое должно прибыть
        int count;
        int speed;
        //является ли объект главным
        int main;
        //создание событий (по названиям понятно)
        public event StartMovingDelegate startmoving;
        public delegate void StartMovingDelegate();
        public event plus_count_delegate plus_count;
        public delegate void plus_count_delegate();
        public Boat(Point beginposition, Brush Color,int Count,int Speed,int Main)
        {
            //присваивание переменным значений, присоздании объекта
            main = Main;
            speed = Speed;
            count = Count;
            step = 1;
            color = Color;
            size = new Size(50, 50);
            position = beginposition;
            //подписываем самогосебя на событие, которое считает количество прибывших лодок
            plus_count += Check_count;
        }
        public void draw(Graphics context)
        {
            //отрисовка лодок
            context.FillRectangle(color, new Rectangle(position, size));
        }
        //функция движения лодок
        public void Moving()
        {
            if(step==1)
            {
                //состояние движения вправо
                position.X += speed;
                if(position.X==500)
                {
                    plus_count();
                    step = 2;
                }
            }
            if(step == 2 && check_count==count&&main==1)
            {
                //остановка и запуск пиратов
                startmoving();
                step = 0;
            }
            if(step == 3)
            {
                //движение влево
                position.X -= speed;
                if (position.X == 0)
                {
                    step = 0;
                }
            }
            
        }
        public void Second_moving()
        {
            //старт движения влево
            step = 3;
        }
        public void Check_count()
        {
            //подсчет лодок
            check_count++;
        }
    }
    class Pirate
    {
        //снова переменные
        public Point position;
        Brush color;
        Size size;
        int step;
        int check_count;
        int count=0;
        //скорость лодки, нужна, чтобы вместе с лодкой дошли до центра
        int speed;
        int main;
        //скорость пиратов
        int new_speed;
        //снова события
        public event plus_count_delegate plus_count;
        public delegate void plus_count_delegate();
        public event StartMovingDelegate startmoving;
        public delegate void StartMovingDelegate();
        public Pirate(Point beginposition, Brush Color,int Count,int Speed,int Main,int New_speed)
        {
            //снова присваивание значений
            main = Main;
            speed = Speed;
            count = Count;
            step = 1;
            color = Color;
            size = new Size(25, 25);
            position = beginposition;
            //снова подсчет пиратов
            plus_count += Check_count;
            new_speed = New_speed;
        }
        public void draw(Graphics context)
        {
            //отрисовка
            context.FillEllipse(color, new Rectangle(position, size));
        }
        public void Moving()
        {

            if(step==1)
            {
                //положение движения с лодкой
                position.X += speed;
                if (position.X == 500)
                {
                    step = 2;
                }
                
            }
            if(step==2 && check_count == count&&main==1)
            {
                //остановка и запуск лодок
               startmoving();
                step = 0;
            }
            if(step==3)
            {
                //движение до конца экрана
                position.X += new_speed;
                if (position.X >= 1000)
                {
                    step = 2;
                    plus_count();
                }
            }
        }
        public void First_moving()
        {
            //старт движения до конца экрана
            step = 3;
        }
        public void Check_count()
        {
            //подсчет пиратов
            check_count++;
        }
    }
}
