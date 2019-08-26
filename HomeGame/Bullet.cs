using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGame
{
    /// <summary>
    /// Класс выстрела
    /// </summary>
    class Bullet : GameObject
    {
        #region constructors

        public Bullet(Point pos) : base(pos, new Point(Game.Width / 50, 0), new Size((Game.Height / 100) * 2, Game.Height / 100))//speed = 100
        {
            Power = 10;
        }

        #endregion

        #region metods

        /// <summary>
        /// Отрисовывает пулю в буфер игры
        /// </summary>
        public override void Draw()
        {
            Game.buffer.Graphics.FillRectangle(Brushes.OrangeRed, _pos.X, _pos.Y, _size.Width, _size.Height);
        }

        /// <summary>
        /// Обновляет положение пули
        /// </summary>
        public override void Update()
        {
            _pos.X = _pos.X + _dir.X;
        }
        
        #endregion
    }
}
