using System;
using System.Collections.Generic;
using System.Drawing;
using f=System.Windows.Forms;

namespace arkanoidVideogioco
{
    public partial class GameForm : Form
    {
       

        private void GameForm_Load(object sender, EventArgs e)
        {

        }
        private f.Timer timer;
        private Rectangle paddle;
        private Rectangle ball;
        private List<Rectangle> bricks;
        private int ballDX = 4;
        private int ballDY = -4;
        private const int paddleSpeed = 10;
        private bool sinistra, destra;

        public GameForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Width = 800;
            this.Height = 800;
            this.Text = "Arkanoid Base";
            this.KeyPreview = true;

            this.Paint += Disegna;
            this.KeyDown += TastoPremuto;
            this.KeyUp += TastoRilasciato;

            timer = new f.Timer();
            timer.Interval = 20;
            timer.Tick += Aggiorna;
            timer.Start();

            paddle = new Rectangle(350, 550, 100, 15);
            ball = new Rectangle(390, 300, 20, 20);
            InizializzaMattoni();
        }

        private void InizializzaMattoni()
        {
            bricks = new List<Rectangle>();
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    bricks.Add(new Rectangle(60 + c * 65, 50 + r * 25, 60, 20));
                }
            }
        }

        private void Disegna(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.FillRectangle(Brushes.Blue, paddle);
            g.FillEllipse(Brushes.Red, ball);

            foreach (var brick in bricks)
            {
                g.FillRectangle(Brushes.Green, brick);
                g.DrawRectangle(Pens.Black, brick);
            }
        }

        private void Aggiorna(object sender, EventArgs e)
        {
            if (sinistra && paddle.X > 0) paddle.X -= paddleSpeed;
            if (destra && paddle.X + paddle.Width < ClientSize.Width) paddle.X += paddleSpeed;

            ball.X += ballDX;
            ball.Y += ballDY;

            if (ball.X < 0 || ball.X + ball.Width > ClientSize.Width) ballDX = -ballDX;
            if (ball.Y < 0) ballDY = -ballDY;
            if (ball.IntersectsWith(paddle)) ballDY = -Math.Abs(ballDY);

            for (int i = 0; i < bricks.Count; i++)
            {
                if (ball.IntersectsWith(bricks[i]))
                {
                    bricks.RemoveAt(i);
                    ballDY = -ballDY;
                    break;
                }
            }

            if (ball.Y > ClientSize.Height)
            {
                timer.Stop();
                MessageBox.Show("Game Over!");
                Application.Exit();
            }

            Invalidate();
        }

        private void TastoPremuto(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) sinistra = true;
            if (e.KeyCode == Keys.Right) destra = true;
        }

        private void TastoRilasciato(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) sinistra = false;
            if (e.KeyCode == Keys.Right) destra = false;
        }




    }
}
