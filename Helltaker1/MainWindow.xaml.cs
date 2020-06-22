using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Media;
using WMPLib;
using System.Collections;
using System.IO;

namespace Helltaker1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap original;
        Bitmap[] frames = new Bitmap[12];
        ImageSource[] imgFrame = new ImageSource[12];
        string bitmapPath = "Resources/Lucifer.png";
        int frame = -1;

        //string[] filePaths = Directory.GetFiles("Resources", "*.png");

        WindowsMediaPlayer wplayer;

        SoundPlayer playSound = new SoundPlayer();

        /*for release bitmap*/
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public MainWindow()
        {
            InitializeComponent();
            #region 캐릭터
            var Azazel = new System.Windows.Forms.MenuItem
            {
                Index = 8,
                Text = "Azazel",
            };
            var Cerberus = new System.Windows.Forms.MenuItem
            {
                Index = 7,
                Text = "Cerberus",
            };
            var Judgement = new System.Windows.Forms.MenuItem
            {
                Index = 6,
                Text = "Judgement",
            };
            var Justice = new System.Windows.Forms.MenuItem
            {
                Index = 5,
                Text = "Justice",
            };
            var Lucifer = new System.Windows.Forms.MenuItem
            {
                Index = 4,
                Text = "Lucifer",
            };
            var Malina = new System.Windows.Forms.MenuItem
            {
                Index = 3,
                Text = "Malina",
            };
            var Modeus = new System.Windows.Forms.MenuItem
            {
                Index = 2,
                Text = "Modeus",
            };
            var Pandemonica = new System.Windows.Forms.MenuItem
            {
                Index = 1,
                Text = "Pandemonica",
            };
            var Zdrada = new System.Windows.Forms.MenuItem
            {
                Index = 0,
                Text = "Zdrada",
            };
            #endregion

            #region 캐릭터 선택
            Azazel.Click += (object o, EventArgs e) =>
            {
                bitmapPath = "Resources/Azazel.png";
                //Console.WriteLine("아자젤");
                Animation(bitmapPath);
            };
            Cerberus.Click += (object o, EventArgs e) =>
            {
                bitmapPath = "Resources/Cerberus.png";
                //Console.WriteLine("케로베로스");
                Animation(bitmapPath);
            };
            Judgement.Click += (object o, EventArgs e) =>
            {
                bitmapPath = "Resources/Judgement.png";
                //Console.WriteLine("저지먼트");
                Animation(bitmapPath);
            };
            Justice.Click += (object o, EventArgs e) =>
            {
                bitmapPath = "Resources/Justice.png";
                //Console.WriteLine("저스티스");
                Animation(bitmapPath);
            };
            Lucifer.Click += (object o, EventArgs e) =>
            {
                bitmapPath = "Resources/Lucifer.png";
                //Console.WriteLine("루시퍼");
                Animation(bitmapPath);
            };
            Malina.Click += (object o, EventArgs e) =>
            {
                bitmapPath = "Resources/Malina.png";
                //Console.WriteLine("말리나");
                Animation(bitmapPath);
            };
            Modeus.Click += (object o, EventArgs e) =>
            {
                bitmapPath = "Resources/Modeus.png";
                //Console.WriteLine("모데우스");
                Animation(bitmapPath);
            };
            Pandemonica.Click += (object o, EventArgs e) =>
            {
                bitmapPath = "Resources/Pandemonica.png";
                //Console.WriteLine("판데모니카");
                Animation(bitmapPath);
            };
            Zdrada.Click += (object o, EventArgs e) =>
            {
                bitmapPath = "Resources/Zdrada.png";
                //Console.WriteLine("즈드라다");
                Animation(bitmapPath);
            };
            #endregion

            Animation(bitmapPath);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.0167*3); //60FPS
            timer.Tick += NextFrame;
            timer.Start();
            this.Topmost = true;
            MouseDown += MainWindow_MouseDown;

            wplayer = new WindowsMediaPlayer();
            wplayer.URL = "Resources/Helltaker.mp3";
            wplayer.settings.setMode("loop", true);

            /*for notify icon*/
            var menu = new System.Windows.Forms.ContextMenu();

            var noti = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.Icon.FromHandle(frames[0].GetHicon()),
                Visible = true,
                Text = "HellTaker",
                ContextMenu = menu,
            };
            var item = new System.Windows.Forms.MenuItem
            {
                Index = 1,
                Text = "끄기",

            };
            var overlay = new System.Windows.Forms.MenuItem
            {
                Index = 0,
                Text = "오버레이",
            }; overlay.Checked = true;
            var bgm = new System.Windows.Forms.MenuItem
            {
                Index = 2,
                Text = "브금",
            }; bgm.Checked = true;
            var CharSelect = new System.Windows.Forms.MenuItem
            {
                Index = 3,
                Text = "캐릭터 선택",
            };

           


            item.Click += (object o, EventArgs e) =>
            {
                Application.Current.Shutdown();
            };
            overlay.Click += (object o, EventArgs e) =>
            {
                overlay.Checked = !overlay.Checked;
                if (overlay.Checked)
                    this.Topmost = true;
                else
                    this.Topmost = false;
            };
            bgm.Click += (object o, EventArgs e) =>
            {           
                bgm.Checked = !bgm.Checked;
                if (bgm.Checked)
                    Play();
                else
                    Stop();
            };

           
            menu.MenuItems.Add(item);
            menu.MenuItems.Add(overlay);
            menu.MenuItems.Add(bgm);
            menu.MenuItems.Add(CharSelect);

            #region 리스트에 캐릭터 추가
            CharSelect.MenuItems.Add(Azazel);
            CharSelect.MenuItems.Add(Cerberus);
            CharSelect.MenuItems.Add(Judgement);
            CharSelect.MenuItems.Add(Justice);
            CharSelect.MenuItems.Add(Lucifer);
            CharSelect.MenuItems.Add(Malina);
            CharSelect.MenuItems.Add(Modeus);
            CharSelect.MenuItems.Add(Pandemonica);
            CharSelect.MenuItems.Add(Zdrada);
            #endregion

            noti.ContextMenu = menu;
        }

        private void NextFrame(object sender, EventArgs e)
        {
            frame = (frame + 1) % 12;
            Lucifer.Source = imgFrame[frame];
            original = System.Drawing.Image.FromFile(bitmapPath) as Bitmap;
            Console.WriteLine(bitmapPath);
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

   
        private void Play()
        {
            wplayer.controls.play();
        }
        private void Stop()
        {
            wplayer.controls.stop();
        }
        private void Animation(string _path)
        {
            original = System.Drawing.Image.FromFile(bitmapPath) as Bitmap;
            for (int i = 0; i < 12; i++)
            {
                frames[i] = new Bitmap(100, 100);
                using (Graphics g = Graphics.FromImage(frames[i]))
                {
                    g.DrawImage(original, new System.Drawing.Rectangle(0, 0, 100, 100),
                        new System.Drawing.Rectangle(i * 100, 0, 100, 100),
                        GraphicsUnit.Pixel);
                }
                var handle = frames[i].GetHbitmap();
                try
                {
                    imgFrame[i] = Imaging.CreateBitmapSourceFromHBitmap(handle, 
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(handle);
                }
            }
        }
    }
}
