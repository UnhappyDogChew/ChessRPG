using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class SpriteAnimation : ITask
    {
        public int repr_x { get { return x_array[index]; } }
        public int repr_y { get { return y_array[index]; } }
        public float repr_xScale { get { return scaleX_array[index]; } }
        public float repr_yScale { get { return scaleY_array[index]; } }
        public float repr_rotation { get { return rotation_array[index]; } }
        public Color repr_color { get { return color_array[index]; } }
        public int repr_index { get { return index_array[index]; } }

        int[] x_array;
        int[] y_array;
        float[] scaleX_array;
        float[] scaleY_array;
        float[] rotation_array;
        Color[] color_array;
        int[] index_array;

        int index;
        int length;
        int timespan;
        int interval;

        public bool Stopped;
        public bool Finished { get; protected set; }

        public event TaskFinishedHandler TaskFinished;

        public SpriteAnimation(int length, int interval, int[] x = null, int[] y = null, float[] scaleX = null, 
            float[] scaleY = null, float[] rotation = null, Color[] color = null, int[] index = null)
        {
            this.interval = interval;

            this.length = length;

            Stopped = true;

            if (x == null)
                x_array = Enumerable.Repeat<int>(0, length).ToArray<int>();
            else
                x_array = x;

            if (y == null)
                y_array = Enumerable.Repeat<int>(0, length).ToArray<int>();
            else
                y_array = y;

            if (scaleX == null)
                scaleX_array = Enumerable.Repeat<float>(1.0f, length).ToArray<float>();
            else
                scaleX_array = scaleX;

            if (scaleY == null)
                scaleY_array = Enumerable.Repeat<float>(1.0f, length).ToArray<float>();
            else
                scaleY_array = scaleY;

            if (rotation == null)
                rotation_array = Enumerable.Repeat<float>(0.0f , length).ToArray<float>();
            else
                rotation_array = rotation;

            if (color == null)
                color_array = Enumerable.Repeat<Color>(Color.White, length).ToArray<Color>();
            else
                color_array = color;

            if (index == null)
                index_array = Enumerable.Repeat<int>(0, length).ToArray<int>();
            else
                index_array = index;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!Stopped)
            {
                timespan++;
                if (timespan >= interval)
                {
                    timespan = 0;
                    if (index + 1 >= length)
                        Finished = true;
                    else
                        index++;
                }
            }

            if (Finished)
                TaskFinished?.Invoke(this);
        }

        public virtual void StartTask()
        {
            Stopped = false;
        }

        // ------------------------ Factory Method --------------------------- //
        public static SpriteAnimation GetSpriteAnimation(string name)
        {
            int length = 0;
            int interval = 1;
            int[] x = null; int[] y = null;
            float[] xScale = null; float[] yScale = null;
            float[] rotation = null;
            Color[] color = null;
            int[] index = null;

            switch(name)
            {
                case "Shake": length = 6; x = new int[] { -4, 4, -2, 2, -1, 1 }; interval = 5; break;
                case "MeleeAttackUp": length = 8; y = new int[] { 0, -3, -7, -13, -20, -13, -7, -3, }; interval = 2; break;
                case "MeleeAttackDown": length = 8; y = new int[] { 0, 3, 7, 13, 20, 13, 7, 3, }; interval = 2; break;
                default: throw new Exception("Invalid animation name.");
            }

            return new SpriteAnimation(length, interval, x, y, xScale, yScale, rotation, color, index);
        }
    }
}
