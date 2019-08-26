using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGame
{
    abstract class BaseObject
    {
        #region fields

        protected Point _pos;
        protected Point _dir;
        protected Size _size;

        #endregion

        #region constructors

        protected BaseObject()
        { }

        /// <summary>
        /// Стандартный конструктор с параметрами
        /// </summary>
        /// <param name="pos">Стартовая позиция</param>
        /// <param name="dir">Вектор движения</param>
        /// <param name="size">Размер</param>
        protected BaseObject(Point pos, Point dir, Size size)
        {
            this._pos = pos;
            this._dir = dir;
            this._size = size;
        }

        /// <summary>
        /// Конструктор с параметрами для квадратных объектов
        /// </summary>
        /// <param name="pos">Стартовая позиция</param>
        /// <param name="dir">Вектор движения</param>
        /// <param name="size">Размер</param>
        protected BaseObject(Point pos, Point dir, int size)
        {
            this._pos = pos;
            this._dir = dir;
            this._size.Height = size;
            this._size.Width = size;
        }

        #endregion

        #region methods

        /// <summary>
        /// Метод отрисовки объекта в буфер игры
        /// </summary>
        public abstract void Draw();


        /// <summary>
        /// Метод обновления состояния объекта
        /// </summary>
        public abstract void Update();
                
        #endregion
    }

    /// <summary>
    /// Общий класс игровых объектов с силой и хп
    /// </summary>
    abstract class GameObject: BaseObject, ICollision
    {
        #region fields

        public Rectangle Rect => new Rectangle(_pos, _size);
        public delegate void Message();

        #endregion

        #region properties

        public int Power { get; set; }

        #endregion

        #region constructors

        protected GameObject()
        { }

        /// <summary>
        /// Стандартный конструктор с параметрами
        /// </summary>
        /// <param name="pos">Стартовая позиция</param>
        /// <param name="dir">Вектор движения</param>
        /// <param name="size">Размер</param>
        protected GameObject(Point pos, Point dir, Size size)
        {
            this._pos = pos;
            this._dir = dir;
            this._size = size;
        }

        /// <summary>
        /// Конструктор с параметрами для квадратных объектов
        /// </summary>
        /// <param name="pos">Стартовая позиция</param>
        /// <param name="dir">Вектор движения</param>
        /// <param name="size">Размер</param>
        protected GameObject(Point pos, Point dir, int size)
        {
            this._pos = pos;
            this._dir = dir;
            this._size.Height = size;
            this._size.Width = size;
        }

        #endregion

        #region methods

        /// <summary>
        /// Проверяет столкновение текущего объекта с другим объектом
        /// </summary>
        /// <param name="o">Объект, на столкновение с которыи проверяется</param>
        /// <returns>true если объекты столкнулись</returns>
        public bool IsCollide(ICollision o) => o.Rect.IntersectsWith(this.Rect);

        /// <summary>
        /// Реализация столкновения с другим объектом и уменьшение их хп
        /// </summary>
        /// <param name="o">Объект, на столкновение с которыи проверяется</param>
        /// <returns>true если объекты столкнулись</returns>
        public void Collision(GameObject o, bool isPrimary)
        {
            if (this.IsCollide(o))
            {
                this.Power -= o.Power;
                if (isPrimary)
                    o.Collision(this, false);
            }
        }

        #endregion
    }

    /// <summary>
    /// Общий класс звёзд и прочих декоративных объектов
    /// </summary>
    abstract class StarObject : BaseObject
    {
        #region constructors

        protected StarObject()
        { }

        protected StarObject(Point pos, Point dir, Size size)
        {
            this._pos = pos;
            this._dir = dir;
            this._size = size;
        }

        protected StarObject(Point pos, Point dir, int size)
        {
            this._pos = pos;
            this._dir = dir;
            this._size.Height = size;
            this._size.Width = size;
        }

        #endregion
    }
}
