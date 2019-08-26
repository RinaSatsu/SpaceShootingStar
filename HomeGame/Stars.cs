using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HomeGame
{
    /// <summary>
    /// Класс звёзд, которые скачут по экрану и по Х и по У.
    /// </summary>
    class Star : StarObject
    {
        #region constructors

        public Star(Point pos, Point dir, int size) : base(pos, dir, size)
        { }

        #endregion

        #region methods

        /// <summary>
        /// Отрисовывает белую четырехконечную звезду в буфер игры
        /// </summary>
        public override void Draw()
        {
            Point[] curvePoints = { new Point(_pos.X, _pos.Y + _size.Height / 2), new Point(_pos.X + _size.Width * 3 / 8, _pos.Y + _size.Height * 3 / 8),
                                    new Point(_pos.X + _size.Width / 2, _pos.Y), new Point(_pos.X + _size.Width * 5 / 8, _pos.Y + _size.Height * 3 / 8),
                                    new Point(_pos.X + _size.Width, _pos.Y + _size.Height / 2), new Point(_pos.X + _size.Width * 5 / 8, _pos.Y + _size.Height * 5 / 8),
                                    new Point(_pos.X + _size.Width / 2, _pos.Y + _size.Height), new Point(_pos.X + _size.Width * 3 / 8, _pos.Y + _size.Height * 5 / 8)};

            Game.buffer.Graphics.FillPolygon(Brushes.Snow, curvePoints);
        }

        /// <summary>
        /// Обновляет положение объекта
        /// </summary>
        public override void Update()
        {
            _pos.X = _pos.X + _dir.X;
            _pos.Y = _pos.Y + _dir.Y;
            if (_pos.X < 0)
                _dir.X = -_dir.X;
            if (_pos.X + _size.Width > Game.Width)
                _dir.X = -_dir.X;
            if (_pos.Y < 0)
                _dir.Y = -_dir.Y;
            if (_pos.Y + _size.Height > Game.Height)
                _dir.Y = -_dir.Y;
        }
        #endregion
    }

    /// <summary>
    /// Класс звезд, бегущих справа налево
    /// </summary>
    class SpeedStar : StarObject
    {
        #region constructors

        public SpeedStar(Point pos, Point dir, int size) : base(pos, dir, size)
        { }

        #endregion

        #region methods

        /// <summary>
        /// Отрисовывает светло-желтую восьмиконечную звезду в буфер игры
        /// </summary>
        public override void Draw()
        {
            Game.buffer.Graphics.DrawLine(Pens.White, _pos.X, _pos.Y + _size.Height / 2, _pos.X + _size.Width, _pos.Y + _size.Height / 2);
            Game.buffer.Graphics.DrawLine(Pens.White, _pos.X + _size.Width / 2, _pos.Y, _pos.X + _size.Width / 2, _pos.Y + _size.Height);
            Game.buffer.Graphics.DrawLine(Pens.White, _pos.X + _size.Width / 4, _pos.Y + _size.Height / 4,
                                                      _pos.X + _size.Width * 3 / 4 + 1, _pos.Y + _size.Height * 3 / 4 + 1);
            Game.buffer.Graphics.DrawLine(Pens.White, _pos.X + _size.Width * 3 / 4 + 1, _pos.Y + _size.Height / 4,
                                                      _pos.X + _size.Width / 4, _pos.Y + _size.Height * 3 / 4 + 1);
            Game.buffer.Graphics.FillEllipse(Brushes.PapayaWhip, _pos.X + _size.Width / 4, _pos.Y + _size.Height / 4, _size.Height / 2, _size.Width / 2);
        }

        /// <summary>
        /// Обновляет положение звезды
        /// </summary>
        public override void Update()
        {
            _pos.X = _pos.X - _dir.X;
            if (_pos.X < 0)
                _pos.X = Game.Width + _size.Width;
        }

        #endregion
    }

    /// <summary>
    /// Класс звёзд, мерцающих на фоне
    /// </summary>
    class GlowingStar : StarObject
    {
        #region fields

        private Image _star = Image.FromFile("GlowingStar.jpg");

        protected Size _maxSize;
        protected bool _isMax;

        #endregion

        #region constructors

        public GlowingStar(Point pos, int size) : base(pos, new Point (-Game.Width / 750, 0), size)
        {
            this._maxSize = this._size;
            this._isMax = true;
        }

        public GlowingStar(Point pos, int size, int maxSize) : base(pos, new Point(-1, 0), size)
        {
            this._maxSize.Height = maxSize;
            this._maxSize.Width = maxSize;
            this._isMax = false;
        }

        public GlowingStar(Point pos, int size, bool isMax) : base(pos, new Point(-1, 0), size)
        {
            this._isMax = isMax;
            this._maxSize = this._size;
            if (!_isMax)
            {
                this._size.Height /= 2;
                this._size.Width /= 2;
            }

        }

        #endregion

        #region methods

        /// <summary>
        /// Отрисовывает .jpg-картинку звезды в буфер игры
        /// </summary>
        public override void Draw()
        {
            Game.buffer.Graphics.DrawImage(_star, _pos.X, _pos.Y, _size.Width, _size.Height);
        }

        /// <summary>
        /// Обновляет размер и положение звезды
        /// </summary>
        public override void Update()
        {
            if (_isMax)
            {
                _size.Height /= 2;
                _size.Width /= 2;
            }
            else
                _size = _maxSize;
            _isMax = !_isMax;
            _pos.X += _dir.X;
            if (_pos.X < 0)
                _pos.X = Game.Width + _size.Width;
        }

        #endregion
    }
}
