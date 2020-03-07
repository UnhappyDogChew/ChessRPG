using System;

using Microsoft.Xna.Framework;
namespace ChessRPGMac
{
    public abstract class Collider
    {
        public bool solid { get; set; }

        public Collider() { }

        public Collider(bool solid)
        {
            this.solid = solid;
        }

        public abstract bool Detect(Collider other);

        public abstract int GetAdjacentDistance(Collider other, Direction direction);
    }

    public class SquareCollider : Collider
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int originWidth { get; set; }
        public int originHeight { get; set; }

        public int bboxLeft { get { return x - (int)originWidth; } }
        public int bboxRight { get { return x + width - 1 - (int)originWidth; } }
        public int bboxTop { get { return y - (int)originHeight; } }
        public int bboxBottom { get { return y + height - 1 - (int)originHeight; } }

        public SquareCollider(int x, int y, int width, int height, int originWidth, int originHeight, bool solid = true) : base(solid)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.originWidth = originWidth;
            this.originHeight = originHeight;
        }

        public override bool Detect(Collider other)
        {
            if (other is SquareCollider)
            {
                SquareCollider sq = (SquareCollider)other;
                if (sq.bboxTop > this.bboxBottom || sq.bboxLeft > this.bboxRight ||
                    sq.bboxBottom < this.bboxTop || sq.bboxRight < this.bboxLeft)
                {
                    return false;
                }
                return true;
            }
            else if (other is NullCollider)
                return false;

            throw new NotImplementedException("Not implemented collider.");
        }
        /// <summary>
        /// Gets distance to be adjacent with given collider in given direction.
        /// If colliders are met already, returns negetive value.
        /// </summary>
        /// <returns>The adjacent distance.</returns>
        /// <param name="other">Other collider.</param>
        /// <param name="direction">Direction.</param>
        public override int GetAdjacentDistance(Collider other, Direction direction)
        {
            if (other is SquareCollider)
            {
                switch (direction)
                {
                    case Direction.Up: return bboxTop - ((SquareCollider)other).bboxBottom - 1;
                    case Direction.Down: return ((SquareCollider)other).bboxTop - bboxBottom - 1;
                    case Direction.Left: return bboxLeft - ((SquareCollider)other).bboxRight - 1;
                    case Direction.Right: return ((SquareCollider)other).bboxLeft - bboxRight - 1;
                }
            }
            else if (other is NullCollider)
                return 0;

            throw new NotImplementedException("Not implemented collider.");
        }
    }

    /// <summary>
    /// This collider are not detected.
    /// </summary>
    public class NullCollider : Collider
    {
        public override bool Detect(Collider other)
        {
            return false;
        }

        public override int GetAdjacentDistance(Collider other, Direction direction)
        {
            return 0;
        }
    }
}
