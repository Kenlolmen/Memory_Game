using Microsoft.VisualBasic;
using System.Windows.Forms.PropertyGridInternal;

namespace FruitMemoryGame
{
    public partial class Form1 : Form
    {

        bool allowClick = false;
        PictureBox firstguess;
        Random rnd = new Random();
        System.Windows.Forms.Timer clickTimer = new System.Windows.Forms.Timer();
        int time = 60;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer() { Interval = 1000 }; // 1000 miliseconds -> 1 second

        private PictureBox[] pictureBoxes
        {
            get { return Controls.OfType<PictureBox>().ToArray(); } // we will add pictureboxes to array
        }

        private static IEnumerable<Image> images
        {
            get
            {
                return new Image[] {
                Resources.img1,
                Resources.img2,
                Resources.img3,
                Resources.img4,
                Resources.img5,
                Resources.img6,
                Resources.img7,
                Resources.img8
                };
            }
        }

        private void HideImages()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Image = Resources.question;

            }
        }
        private PictureBox getFreeSlot()
        {
            int num;

            do
            {
                num = rnd.Next(0, pictureBoxes.Count());
            }
            while (pictureBoxes[num].Tag != null);
            return pictureBoxes[num];

        }
        private void setRandomImages()
        {
            foreach (var image in images)
            {
                getFreeSlot().Tag = image;
                getFreeSlot().Tag = image;
            }
        }

        private void ResetImages()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;
            }
            HideImages();
            setRandomImages();
            time = 60;
            timer.Start();
        }

        private void StartGameTimer()
        {
            timer.Start();
            timer.Tick += delegate
            {
                time--;
                if (time < 0)
                {
                    timer.Stop();
                    MessageBox.Show("out of time");
                    ResetImages();
                }
                var ssTime = TimeSpan.FromSeconds(time);
                label1.Text = "00: " + time.ToString();
            };
        }

        private void CLICKTIMER_TICK(object sender, EventArgs e)
        {
            HideImages();

            allowClick = true;
            clickTimer.Stop();
        }


        private void clickImage(object sender, EventArgs e)
        {
            if (!allowClick) return;
            var pic = (PictureBox)sender;

            if (firstguess == null)
            {
                firstguess = pic;
                pic.Image = (Image)pic.Tag;
                return;
            }
            pic.Image = (Image)pic.Tag;

            if (pic.Image == firstguess.Image && pic != firstguess)
            {
                pic.Visible = firstguess.Visible = false;
                {
                    firstguess = pic;
                }
                HideImages();
            }
            else
            {
                allowClick = false;
                clickTimer.Start();

            }
            firstguess = null;
            if (pictureBoxes.Any(p => p.Visible)) return;
            MessageBox.Show("YOU WIN NOW TRY AGAIN");
            ResetImages();
        }
        private void startGame(object sender, EventArgs e)
        {
            allowClick=true;
            setRandomImages();
            HideImages();
            StartGameTimer();
            clickTimer.Interval = 1000;
            clickTimer.Tick += CLICKTIMER_TICK;
            button1.Enabled = false;
        }

        public Form1()
        {
            InitializeComponent();
        }

    
    }
}
