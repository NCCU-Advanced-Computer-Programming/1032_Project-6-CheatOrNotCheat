using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Final_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        static int _level = 1;
        int _width;
        int _height;
        int cheatingRow;
        int cheatingCol;
        bool timeStop = false;
        private int moveStatus = 0;
        Shape view;
        TextBlock pause = new TextBlock();
        Image tcher = new Image();
        internal DispatcherTimer timer;
        _background[,] _classroom;

        public Window1()
        {
            //初始化老師圖片並置入
            InitializeComponent();
            tcher.Source = new BitmapImage(new Uri("images/ITO-01.gif", UriKind.Relative));
            tcher.Width = 84;
            tcher.Height = 77;
            this.teacherLayer.Children.Add(tcher);
            this.teacherLayer.Children.Add(pause);
            //計時器用以移動老師和判斷是否作弊失敗
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.04);
            timer.Tick += moveTeacher;
            timer.Start();
            _level = 1;
            gmStart();
        }

        //開關
        void gmStart()
        {
            //根據層數設定教室大小
            _width = 5 + 2 * _level;
            _height = 5 + 2 * _level;
            this.ugdBoard.Children.Clear();
            this.ugdBoard.Columns = _width;
            this.ugdBoard.Rows = _height;
            //設定教室內容
            _classroom = new _background[_height, _width];
            for (int row = 0; row < _height; row++)
                for (int col = 0; col < _width; col++)
                {
                    _background s;
                    if (row % 2 == 1 && col % 2 == 1)
                    {
                        s = new _background(false);
                    }
                    else
                    {
                        s = new _background(true);
                    }
                    //設定紙條位置
                    if (row == _width - 2 && col == _height - 2)
                    {
                        s.Cheat = true;
                        s.Done = true;
                        cheatingRow = row;
                        cheatingCol = col;
                    }
                    s.Row = row;
                    s.Col = col;
                    _classroom[row, col] = s;
                    this.ugdBoard.Children.Add(s);
                }
            //設定視窗大小
            this.Width = (_level * 2 + 5) * 88;
            this.Height = (_level * 2 + 5) * 74;
            //設定老師
            setTeacher();
        }

        //以上下左右控制紙條位置
        private void Window_previewKeyDown(object sender, KeyEventArgs e)
        {
            Key kPressed;
            if (e.Key == Key.ImeProcessed)
            {
                kPressed = e.ImeProcessedKey;
            }
            else
            {
                kPressed = e.Key;
            }
            switch (kPressed)
            {
                case Key.Up:
                    if (!timeStop)
                    {
                        for (int i = cheatingRow - 1; i >= 0; i--)
                        {
                            if (!_classroom[i, cheatingCol].isFloor)
                            {
                                _classroom[cheatingRow, cheatingCol].Cheat = false;
                                _classroom[cheatingRow, cheatingCol].Done = true;
                                cheatingRow = i;
                                _classroom[cheatingRow, cheatingCol].Cheat = true;
                                _classroom[cheatingRow, cheatingCol].Done = true;
                                //getCheater();
                                return;
                            }
                        }
                    }
                    return;

                case Key.Right:
                    if (!timeStop)
                    {
                        for (int i = cheatingCol + 1; i <= _width - 1; i++)
                        {
                            if (!_classroom[cheatingRow, i].isFloor)
                            {
                                _classroom[cheatingRow, cheatingCol].Cheat = false;
                                _classroom[cheatingRow, cheatingCol].Done = true;
                                cheatingCol = i;
                                _classroom[cheatingRow, cheatingCol].Cheat = true;
                                _classroom[cheatingRow, cheatingCol].Done = true;
                                //getCheater();
                                return;
                            }
                        }
                    }
                    return;

                case Key.Left:
                    if (!timeStop)
                    {
                        for (int i = cheatingCol - 1; i >= 0; i--)
                        {
                            if (!_classroom[cheatingRow, i].isFloor)
                            {
                                _classroom[cheatingRow, cheatingCol].Cheat = false;
                                _classroom[cheatingRow, cheatingCol].Done = true;
                                cheatingCol = i;
                                _classroom[cheatingRow, cheatingCol].Cheat = true;
                                _classroom[cheatingRow, cheatingCol].Done = true;
                                //getCheater();
                                return;
                            }
                        }
                    }
                    return;

                case Key.Down:
                    if (!timeStop)
                    {
                        for (int i = cheatingRow + 1; i <= _height - 1; i++)
                        {
                            if (!_classroom[i, cheatingCol].isFloor)
                            {
                                _classroom[cheatingRow, cheatingCol].Cheat = false;
                                _classroom[cheatingRow, cheatingCol].Done = true;
                                cheatingRow = i;
                                _classroom[cheatingRow, cheatingCol].Cheat = true;
                                _classroom[cheatingRow, cheatingCol].Done = true;
                                //getCheater();
                                return;
                            }
                        }
                    }
                    return;

                //暫停判定
                case Key.Space:
                    if (timeStop)
                    {
                        pause.Foreground = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                        timeStop = false;
                        timer.Start();
                    }
                    else
                    {
                        pause.Text = "PAUSE!!";
                        pause.FontSize = 70;
                        pause.HorizontalAlignment = HorizontalAlignment.Center;
                        pause.VerticalAlignment = VerticalAlignment.Center;
                        pause.Foreground = new SolidColorBrush(Color.FromArgb(255, 183, 112, 72));
                        pause.Margin = new Thickness((_level + 1) * 88, (_level + 2) * 74 - 10, 0, 0);
                        timeStop = true;
                        timer.Stop();
                    }
                    return;

                default:
                    return;
            }
        }

        //判定是否過關
        void checkClear()
        {
            //預設為過關
            bool clear = true;
            //若有任意學生未作弊，clear為false
            foreach (_background s in _classroom)
            {
                if (!s.isFloor && !s.Done)
                {
                    clear = false;
                }
            }
            //若已過第三關，結束遊戲
            if (clear && _level >= 3)
            {
                MessageBox.Show("All Clear");
                this.Close();
                timer.Stop();
                return;
            }
            //載入下一關
            else if (clear)
            {
                MessageBox.Show("Stage " + _level + " Clear.");
                _level += 1;
                view.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                gmStart();
                return;
            }
        }

        //設定老師
        void setTeacher()
        {
            //設定圖片位置
            tcher.Margin = new Thickness(0, 0, 0, 0);
            //視野為圓形
            view = new Ellipse();
            view.Width = (_level * 2 + 1) * 88;
            view.Height = (_level * 2 + 1) * 88;
            view.Margin = new Thickness(-_level * 88, -_level * 74 - 37, 0, 0);
            BrushConverter convter = new BrushConverter();
            view.Fill = new SolidColorBrush(Color.FromArgb(128, 255, 255, 0));
            //視野放入畫布
            this.teacherLayer.Children.Add(view);

        }

        private void moveTeacher(object sender, EventArgs e)
        {
            getCheater();
            //當老師處於一塊地磚中間，決定移動位置
            if ((view.Margin.Top + 407) % 74 <= 6 && (view.Margin.Left + 352) % 88 <= 6)
            {
                bool directionDeciding = true;//尚未決定移動方向為true
                //判斷老師所在的格子
                int curR = (int)(((view.Margin.Top + 37 + 74 * _level) - (view.Margin.Top + 37 + 74 * _level) % 74) / 74);
                int curC = (int)(((view.Margin.Left + 44 + 88 * _level) - (view.Margin.Left + 44 + 88 * _level) % 88) / 88);
                //MessageBox.Show(curR.ToString() + " " + curC.ToString());
                //用以儲存老師前後左右的格子
                _background[] target = new _background[4];
                //預設為學生(不能行走)
                _background def = new _background(false);
                target[0] = def;
                target[1] = def;
                target[2] = def;
                target[3] = def;
                //若該格為走道，將其放入陣列
                if (curR + 1 < _height)
                {
                    if (_classroom[curR + 1, curC].isFloor)
                    {
                        target[0] = _classroom[curR + 1, curC];
                    }
                }
                if (curC + 1 < _width)
                {
                    if (_classroom[curR, curC + 1].isFloor)
                    {
                        target[1] = _classroom[curR, curC + 1];
                    }
                }
                if (curR - 1 >= 0)
                {
                    if (_classroom[curR - 1, curC].isFloor)
                    {
                        target[2] = _classroom[curR - 1, curC];
                    }
                }
                if (curC - 1 >= 0)
                {
                    if (_classroom[curR, curC - 1].isFloor)
                    {
                        target[3] = _classroom[curR, curC - 1];
                    }
                }
                while (directionDeciding)
                {
                    //隨機決定移動方向
                    Random random = new Random();
                    int rand = random.Next(0, 4);
                    //若移動方向為走道，放入moveStatus，已決定方向directionDeciding為false
                    if (target[rand].isFloor)
                    {
                        moveStatus = rand;
                        directionDeciding = false;
                    }
                }
                move();
                return;
            }
            else
            {
                move();
            }
        }

        //移動老師
        void move()
        {
            switch (moveStatus)
            {
                case 0://下
                    view.Margin = new Thickness(view.Margin.Left, view.Margin.Top + 10, 0, 0);
                    tcher.Margin = new Thickness(tcher.Margin.Left, tcher.Margin.Top + 10, 0, 0);
                    return;

                case 1://右
                    view.Margin = new Thickness(view.Margin.Left + 10, view.Margin.Top, 0, 0);
                    tcher.Margin = new Thickness(tcher.Margin.Left + 10, tcher.Margin.Top, 0, 0);
                    return;

                case 2://上
                    view.Margin = new Thickness(view.Margin.Left, view.Margin.Top - 10, 0, 0);
                    tcher.Margin = new Thickness(tcher.Margin.Left, tcher.Margin.Top - 10, 0, 0);
                    return;

                case 3://左
                    view.Margin = new Thickness(view.Margin.Left - 10, view.Margin.Top, 0, 0);
                    tcher.Margin = new Thickness(tcher.Margin.Left - 10, tcher.Margin.Top, 0, 0);
                    return;

                default:
                    return;
            }
        }

        //判定紙條是否進入視線範圍
        void getCheater()
        {
            if (Math.Pow(((view.Margin.Top + (_level + 0.5) * 88) - cheatingRow * 74 - 37), 2) + Math.Pow(((view.Margin.Left + (_level + 0.5) * 88) - cheatingCol * 88 - 44), 2) <= Math.Pow((_level + 0.5) * 88, 2))
            {
                MessageBox.Show("fail");
                this.Close();
                timer.Stop();
                return;
            }
            checkClear();
        }
    }
}
