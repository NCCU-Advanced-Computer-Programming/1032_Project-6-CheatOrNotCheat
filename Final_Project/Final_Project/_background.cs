using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace Final_Project
{
    class _background : Image
    {
        private int _row;
        private int _col;//設定所在位置
        private bool _chting = false;//是否正在作弊
        private bool _isDone = false;//是否已經完成考試
        private bool _isfloor;//true為走道，false為學生
        //圖片
        public Image ready;//未作弊
        public Image done;//作弊完成
        public Image cheating;//作弊中
        public Image floor;//走道

        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public int Col
        {
            get { return _col; }
            set { _col = value; }
        }

        public bool Cheat
        {
            get { return _chting; }
            set { _chting = value;
            checkStatus();
            }
        }

        public bool Done
        {
            get { return _isDone; }
            set { _isDone = value;
            checkStatus();    
            }
        }

        public bool isFloor
        {
            get { return _isfloor; }
            set { _isfloor = value; }
        }

        public _background(bool isfloor)
            : base()
        {
            //設定是否為學生
            _isfloor = isfloor;
            //將圖片初始化
            ready = new Image();
            ready.Source = new BitmapImage(new Uri("images/ready.jpg", UriKind.Relative));
            done = new Image();
            done.Source = new BitmapImage(new Uri("images/done.jpg", UriKind.Relative));
            cheating = new Image();
            cheating.Source = new BitmapImage(new Uri("images/cheating.jpg", UriKind.Relative));
            floor = new Image();
            floor.Source = new BitmapImage(new Uri("images/floor.jpg", UriKind.Relative));
            //設定圖片寬與高
            this.Width = 88;
            this.Height = 74;
            //設定圖片內容
            checkStatus();
        }

        public void checkStatus()
        {
            if(_isfloor){
                this.Source = floor.Source;
            }
            else if (_chting)
            {
                this.Source = cheating.Source;
            }
            else if (_isDone)
            {
                this.Source = done.Source;
            }
            else
            {
                this.Source = ready.Source;
            }
        }
    }
}
