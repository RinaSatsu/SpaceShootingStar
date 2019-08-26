using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HomeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form
            //{
            //    Width = Screen.PrimaryScreen.Bounds.Width,
            //    Height = Screen.PrimaryScreen.Bounds.Height
            //};
            {
                Width = 800,
                Height = 600
            };
            Game.Init(form);
            form.Show();
            Game.Draw();
            SoundPlayer player = new SoundPlayer();
            //player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "/Music.wav";
            //player.Play();
            Application.Run(form);
        }
    }
}
