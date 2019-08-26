using System.Drawing;

namespace HomeGame
{
    interface ICollision
    {
        bool IsCollide(ICollision obj);

        Rectangle Rect { get; }
    }
}
