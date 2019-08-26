using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGame
{
    class Ship : GameObject
    {
        #region fields

        private Image _ship = Image.FromFile("Ship.png");
        public static event Message MessageDie;

        #endregion

        #region constructors

        public Ship(Point pos, Point dir) : base(pos, dir, new Size(1, 1))
        {
            this._size.Height = (4 * Game.Height) / 100;
            this._size.Width = (_ship.Width * this._size.Height) / _ship.Height;
            Power = 100;
        }

        #endregion

        #region methods

        /// <summary>
        /// Отрисовывает корабль в буфер игры
        /// </summary>
        public override void Draw()
        {
            Game.buffer.Graphics.DrawImage(_ship, _pos.X, _pos.Y, _size.Width, _size.Height);
        }

        /// <summary>
        /// Обновляет положение корабля
        /// </summary>
        public override void Update()
        { }

        /// <summary>
        /// Двигает корабль вверх
        /// </summary>
        public void Up()
        {
            if (_pos.Y > 0)
                _pos.Y = _pos.Y - _dir.Y;
        }

        /// <summary>
        /// Двигает корабль вниз
        /// </summary>
        public void Down()
        {
            if (_pos.Y < Game.Height)
                _pos.Y = _pos.Y + _dir.Y;
        }

        public void Dispose()
        {
            _ship = null;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Вызывает смерть корабля и конец игры
        /// </summary>
        public void Die()
        {
            MessageDie?.Invoke();
            Dispose();
        }

        #endregion
    }

}
