using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGame
{
    class MedKit: GameObject
    {
        #region fields

        private Image _medkit = Image.FromFile("MedKit.png");
        #endregion

        #region constructors

        public MedKit(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = -50;
        }

        #endregion

        #region methods

        /// <summary>
        /// Отрисовывает аптечку в буфер игры
        /// </summary>
        public override void Draw()
        {
            Game.buffer.Graphics.DrawImage(_medkit, _pos.X, _pos.Y, _size.Width, _size.Height);
        }

        /// <summary>
        /// Обновляет положение аптечки
        /// </summary>
        public override void Update()
        {
            _pos.X = _pos.X + _dir.X;
        }

        public void Dispose()
        {
            _medkit = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
