using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace HomeGame
{
    class Game
    {
        #region fields

        private static BufferedGraphicsContext _context;
        private static List<StarObject> _stars;
        private static List<Bullet> _bullets;
        private static List<Asteroid> _asteroids;
        private static int astrs = 3;
        private static Func<double, double> AstrGenFunc;

        private static List<MedKit> _medkits;
        private static Ship _ship;
        private static Timer _timer = new Timer { Interval = 100 };

        private static Random rnd = new Random();
        private static event Action<string> LogEvent;

        public static BufferedGraphics buffer;

        #endregion

        #region properties

        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Score { get; set; } = 0;

        #endregion

        #region constructors

        static Game()
        { }

        #endregion

        #region methods

        /// <summary>
        /// Инициализирует поле с игрой
        /// </summary>
        /// <param name="form">Форма, в которой вызывается игра</param>
        public static void Init(Form form)
        {
            LogEvent += (msg) => { Console.WriteLine($"{DateTime.Now}: {msg}"); };
            _context = BufferedGraphicsManager.Current;
            Graphics g = form.CreateGraphics();
            form.KeyDown += Form_KeyDown;

            try
            {
                Width = form.ClientSize.Width;
                Height = form.ClientSize.Height;
                if (Width > 1000 || Height > 1000)
                    throw new ArgumentOutOfRangeException();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception)
            {  }
            finally
            {
                buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
                Load();
                
                _timer.Start();
                _timer.Tick += TimerTick;
            }
        }
        
        /// <summary>
        /// Генерирует графические объекты
        /// </summary>
        private static void Load()
        {
            _ship = new Ship(new Point(0, 400), new Point(10, 10));
            _stars = new List<StarObject>();
            _bullets = new List<Bullet>();
            _asteroids = new List<Asteroid>();
            _medkits = new List<MedKit>();

            //generation of stars
            bool flag;
            for (int i = 0; i < Width / 40; i++)
            {
                if (rnd.Next(100) > 50)
                    flag = true;
                else
                    flag = false;
                _stars.Add(new GlowingStar(new Point(i * 40, rnd.Next(Height)), rnd.Next(2, 4) * 5, flag));
            }
            for (int i = 0; i < Height / 50; i++)
            {
                _stars.Add(new SpeedStar(new Point(rnd.Next(Width), i * 50), new Point(rnd.Next(1, 10) * 5, 0), rnd.Next(2, 4) * 5));
            }
            for (int i = 0; i < Height * Width / 25000; i++)
            {
                _stars.Add(new Star(new Point(rnd.Next(Width), rnd.Next(Height)), new Point(rnd.Next(1, 10) * 5, rnd.Next(1, 10) * 5), rnd.Next(1, 3) * 5));
            }

            GenerateAsteroids(astrs);

            //generation of medkits
            for (var i = 0; i < 3; i++)
            {
                _medkits.Add(new MedKit(new Point(Game.Width + rnd.Next(100, 1000), rnd.Next(0, Game.Height)), new Point(-10, 0), new Size(20, 20)));
                LogEvent($"Created medkit at ({_medkits[i].Rect.X}; {_medkits[i].Rect.Y})");
            }

            Ship.MessageDie += Finish;
        }

        /// <summary>
        /// Выводит все графические объекты в буфер
        /// </summary>
        public static void Draw()
        {
            buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _stars)
                obj.Draw();
            foreach (Asteroid astr in _asteroids)
                astr?.Draw();
            foreach (MedKit med in _medkits)
                med?.Draw();
            foreach (Bullet b in _bullets)
                b.Draw();
            _ship?.Draw();
            if (_ship != null)
                buffer.Graphics.DrawString($"Energy: {_ship.Power} Score: {Score}", SystemFonts.DefaultFont, Brushes.White, 0, 0);
            buffer.Render();
        }

        /// <summary>
        /// Обновляет состояние всех графических объектов
        /// </summary>
        private static void Update()
        {
            foreach (StarObject obj in _stars)
                obj.Update();

            foreach (Bullet b in _bullets)
                b.Update();

            for (int i = 0; i < _asteroids.Count; i++)
            {
                if (_asteroids[i] == null)
                    continue;
                _asteroids[i].Update();

                if (_asteroids[i].Rect.Right < 0)
                {
                    _asteroids[i].Dispose();
                    _asteroids[i] = null;
                    LogEvent($"Asteroid {i} was destroyed.");
                    continue;
                }

                for (int j = 0; j < _bullets.Count; j++)
                {
                    if (_bullets[j] == null)
                        continue;

                    if (_bullets[j].Rect.X > Game.Width)
                    {
                        _bullets[j] = null;
                        LogEvent($"Bullet {j} was destroyed.");
                        continue;
                    }

                    if (!_bullets[j].IsCollide(_asteroids[i]))
                        continue;

                    System.Media.SystemSounds.Beep.Play();
                    Score += _bullets[j].Power;
                    _asteroids[i].Collision(_bullets[j], true);

                    LogEvent($"Asteroid {i} was hit. Now its power is {_asteroids[i].Power}.");

                    if (_bullets[j].Power <= 0)
                    {
                        _bullets[j] = null;
                        LogEvent($"Bullet {j} was destroyed.");
                    }

                    if (_asteroids[i].Power > 0)
                        continue;
                    _asteroids[i].Dispose();
                    _asteroids[i] = null;
                    LogEvent($"Asteroid {i} was destroyed.");
                    break;
                }

                if (_asteroids[i] == null)
                    continue;

                if (!_ship.IsCollide(_asteroids[i]))
                    continue;

                LogEvent($"Asteroid {i} hit the ship. Lost {_asteroids[i].Power} energy.");
                _ship?.Collision(_asteroids[i], true);
                System.Media.SystemSounds.Asterisk.Play();
                _asteroids[i]?.Dispose();
                _asteroids[i] = null;

                if (_ship.Power <= 0)
                {
                    _ship?.Die();
                    LogEvent($"Game finished.");
                }
            }

            for (int i = 0; i < _medkits.Count; i++)
            {
                if (_medkits == null)
                    continue;
                _medkits[i].Update();

                if (_medkits[i].Rect.Right < 0)
                {
                    _medkits[i].Dispose();
                    _medkits[i] = null;
                    LogEvent($"Medkit {i} was destroyed.");
                    continue;
                }

                if (!_ship.IsCollide(_medkits[i]))
                    continue;

                LogEvent($"Medkit {i} was used. Regenerated {-_medkits[i].Power} energy.");
                _ship?.Collision(_medkits[i], true);
                System.Media.SystemSounds.Asterisk.Play();
                _medkits[i].Dispose();
                _medkits[i] = null;
            }

            _asteroids.RemoveAll(a => a == null);
            _bullets.RemoveAll(b => b == null);
            _medkits.RemoveAll(m => m == null);

            if (_asteroids.Count == 0)
                GenerateAsteroids(astrs);

            if (_medkits.Count == 0)
            {
                _medkits.Add(new MedKit(new Point(Game.Width + rnd.Next(1000, 5000), rnd.Next(0, Game.Height)), new Point(-10, 0), new Size(20, 20)));
                LogEvent($"Created medkit at ({_medkits[0].Rect.X}; {_medkits[0].Rect.Y})");
            }
                
        }

        /// <summary>
        /// Обработчик события тика таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TimerTick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            //LogEvent($"Form_KeyDown is called. {e.KeyCode} is pressed.");
            if (e.KeyCode == Keys.ControlKey)
                _bullets.Add(new Bullet(new Point(_ship.Rect.Right, _ship.Rect.Y + _ship.Rect.Height / 2)));
            if (e.KeyCode == Keys.Up)
                _ship.Up();
            if (e.KeyCode == Keys.Down)
                _ship.Down();
        }


        /// <summary>
        /// Генерирует астероиды
        /// </summary>
        /// <param name="num">Количество астероидов, которое будет сгененрировано</param>
        private static void GenerateAsteroids(int num)
        {
            AstrGenFunc = Math.Sin;
            _asteroids.Add(new Asteroid(new Point(Game.Width + 200, rnd.Next(0, Game.Height)), rnd.Next(3, 7)));
            LogEvent($"Created asteroid at ({_asteroids[0].Rect.X}; {_asteroids[0].Rect.Y}) with {_asteroids[0].Power} power.");
            for (var i = 1; i < num; i++)
            {
                _asteroids.Add(new Asteroid(new Point(Game.Width + 100 * i + 200,
                                                      (int)(_asteroids[0].Rect.Y + 100 * Math.Sin((100 * i * 3.14) / 400))), 
                                            rnd.Next(3, 7)));
                LogEvent($"Created asteroid at ({_asteroids[i].Rect.X}; {_asteroids[i].Rect.Y}) with {_asteroids[i].Power} power.");
            }
            astrs++;
        }
        
        /// <summary>
        /// Процедура окончания игры
        /// </summary>
        public static void Finish()
        {
            _timer.Stop();
            buffer.Graphics.DrawString("You died", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.Red, 200, 100);
            buffer.Render();
        }

        #endregion
    }
}
