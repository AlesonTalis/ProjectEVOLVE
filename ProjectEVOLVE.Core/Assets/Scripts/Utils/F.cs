using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectEVOLVE.Utils
{
    public static class F
    {
        #region EspiralLoop

        public static List<Espiral> EspiralLoop(int width, int height = 0)
        {
            if (height == 0) height = width;

            int x = 0, y = 0;
            int dx = 0, dy = -1;

            int t = Mathf.Max(width, height);
            int maxI = t * t;

            List<Espiral> espiral = new List<Espiral>();

            for (int i = 0; i < maxI; i++)
            {
                if ((-width / 2 <= x) && (x <= width / 2) && (-height / 2 <= y) && (y <= height / 2))
                {
                    //p += x + " " + y + ", ";
                    espiral.Add(new Espiral() { x = x, y = y });
                }
                if ((x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1 - y)))
                {
                    t = dx;
                    dx = -dy;
                    dy = t;
                }

                x += dx;
                y += dy;
            }


            return espiral;
            //Debug.Log(p);
        }

        public static bool Distance(Vector3 p1, Vector3 p2, float dst)
        {
            bool x = Mathf.Abs(p1.x - p2.x) > dst;
            bool y = Mathf.Abs(p1.y - p2.y) > dst;
            bool z = Mathf.Abs(p1.z - p2.z) > dst;

            return x || y || z;// se qualquer um for true...
        }

        public class Espiral
        {
            public int x = 0, y = 0;

            public Vector2Int GetV2I(int multiplier = 1) => new Vector2Int()
            {
                x = x * multiplier,
                y = y * multiplier
            };
        }

        #endregion

        public static string GetNewGUID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString("N");
        }

        public static bool InsideRange(this float value, Vector2 range) => value > range.x && value < range.y;
    }
}
