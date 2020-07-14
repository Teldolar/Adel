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
        public Form1()
        {
            InitializeComponent();
            pnkt1 = new MenuItem("Пуск", new EventHandler(timer1_Tick), Shortcut.Alt1);
            pnkt2 = new MenuItem("Стоп", new EventHandler(stop), Shortcut.Alt2);
            info = new MenuItem("Информация о разработчике", new EventHandler(InfoORazrabotchik), Shortcut.Alt3);
            MnMen1 = new MainMenu(new MenuItem[] { pnkt1, pnkt2, info });
            this.Menu = MnMen1;
            boats = new Boat[2];
            boats[0] = new Boat(new Point(0, 200), Brushes.Red, 2,1,1);
            boats[1] = new Boat(new Point(0, 300), Brushes.Red, 2,2,0);
            pirates = new Pirate[4];
            pirates[0] = new Pirate(new Point(0, 200),Brushes.Red,4,1,1,1);
            pirates[1] = new Pirate(new Point(0, 225), Brushes.Red, 4,1,0,2);
            pirates[2] = new Pirate(new Point(0, 300), Brushes.Red, 4,2,0,5);
            pirates[3] = new Pirate(new Point(0, 325), Brushes.Red, 4,2,0,10);
            DoubleBuffered = true;
            boats[1].plus_count += boats[0].Check_count;
            boats[0].startmoving += pirates[0].First_moving;
            boats[0].startmoving += pirates[1].First_moving;
            boats[0].startmoving += pirates[2].First_moving;
            boats[0].startmoving += pirates[3].First_moving;
            pirates[0].stopmoving += boats[0].Stop_moving;
            pirates[0].stopmoving += boats[1].Stop_moving;
            pirates[1].plus_count += pirates[0].Check_count;
            pirates[2].plus_count += pirates[0].Check_count;
            pirates[3].plus_count += pirates[0].Check_count;
            pirates[0].startmoving += boats[0].Second_moving;
            pirates[0].startmoving += boats[1].Second_moving;
            pirates[0].plus_count += boats[0].Stop_moving;
            pirates[2].plus_count += boats[1].Stop_moving;
        }
        private void stop(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
        private void InfoORazrabotchik(object sender, EventArgs e)
        {
            MessageBox.Show("Выполнил: студент группы 4208\nГареев А.И.\nВариант работы: 3", "Информация о разработчике");
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            boats[0].draw(e.Graphics);
            boats[1].draw(e.Graphics);
            pirates[0].draw(e.Graphics);
            pirates[1].draw(e.Graphics);
            pirates[2].draw(e.Graphics);
            pirates[3].draw(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
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
        public Point position;
        Brush color;
        Size size;
        int step;
        int check_count=0;
        int count;
        int speed;
        int main;
        public event StartMovingDelegate startmoving;
        public delegate void StartMovingDelegate();
        public event plus_count_delegate plus_count;
        public delegate void plus_count_delegate();
        public Boat(Point beginposition, Brush Color,int Count,int Speed,int Main)
        {
            main = Main;
            speed = Speed;
            count = Count;
            step = 1;
            color = Color;
            size = new Size(50, 50);
            position = beginposition;
            plus_count += Check_count;
        }
        public void draw(Graphics context)
        {
            context.FillRectangle(color, new Rectangle(position, size));
        }
        public void Moving()
        {
            if(step==1)
            {
                position.X += speed;
                if(position.X==500)
                {
                    plus_count();
                    step = 2;
                }
            }
            if(step == 2 && check_count==count&&main==1)
            {
                startmoving();
            }
            if(step == 3)
            {
                position.X -= speed;
                if (position.X == 0)
                {
                    Stop_moving();
                }
            }
            
        }
        public void Second_moving()
        {
            step = 3;
        }
        public void Check_count()
        {
            check_count++;
        }
        public void Stop_moving()
        {
            step = 0;
        }
    }
    class Pirate
    {
        public Point position;
        Brush color;
        Size size;
        int step;
        int check_count;
        int count=0;
        int speed;
        int main;
        int new_speed;
        public event plus_count_delegate plus_count;
        public delegate void plus_count_delegate();
        public event StartMovingDelegate startmoving;
        public delegate void StartMovingDelegate();
        public event StopMovingDelegate stopmoving;
        public delegate void StopMovingDelegate();
        public Pirate(Point beginposition, Brush Color,int Count,int Speed,int Main,int New_speed)
        {
            main = Main;
            speed = Speed;
            count = Count;
            step = 1;
            color = Color;
            size = new Size(25, 25);
            position = beginposition;
            plus_count += Check_count;
            new_speed = New_speed;
        }
        public void draw(Graphics context)
        {
            context.FillEllipse(color, new Rectangle(position, size));
        }
        public void Moving()
        {
            if(step==1)
            {
                position.X += speed;
                if (position.X == 500)
                {
                    step = 2;
                }
                
            }
            if(step==2 && check_count == count&&main==1)
            {
               startmoving();
                step = 0;
            }
            if(step==3)
            {
                position.X += new_speed;
                if (position.X > 500 && main == 1)
                {
                    stopmoving();
                }
                if (position.X >= 1000)
                {
                    step = 2;
                    plus_count();
                }
            }
        }
        public void First_moving()
        {
            step = 3;
        }
        public void Check_count()
        {
            check_count++;
        }
    }
}
