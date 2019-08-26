using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGame
{
    /// <summary>
    /// Класс астероида
    /// </summary>
    class Asteroid : GameObject, IDisposable
    {
        #region fields

        private Image _asteroid = Image.FromFile("asteroid1.png");

        #endregion

        #region constructors
        /// <summary>
        /// Конструктор для генерации астероида
        /// </summary>
        /// <param name="pow">Сила и размер астероида (от 3 до 7)</param>
        public Asteroid(Point pos, int pow) : base(pos, new Point (1, 0), new Size(1, 0))
        {
            try
            {
                if (pow < 3 || pow > 7)
                    throw new ArgumentException("Недопустимый размер астероида.");
            }
            catch (ArgumentException)
            {
                pow = pow % 5 + 3;
            }
            finally
            {
                this._dir.X = ((pow - 8) * Game.Width) / 250;
                this._size.Width = (pow * Game.Width) / 150;
                this._size.Height = (_asteroid.Height * this._size.Width) / _asteroid.Width;
                this.Power = (pow - 2) * 10;
            }
        }

        #endregion

        #region methods
        
        /// <summary>
        /// Отрисовывает астероид в буфер игры
        /// </summary>
        public override void Draw()
        {
            Game.buffer.Graphics.DrawImage(_asteroid, _pos.X, _pos.Y, _size.Width, _size.Height);
        }

        /// <summary>
        /// Обновляет положение астероида
        /// </summary>
        public override void Update()
        {
            _pos.X = _pos.X + _dir.X;
        }

        public void Dispose()
        {
            _asteroid = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
