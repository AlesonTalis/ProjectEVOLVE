using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEVOLVE.Utils
{
    public interface vec
    {
        
    }

    public class vec2 : vec
    {
        public float x, y;


        public vec2() { }
        public vec2(float x)
        {
            this.x = x;
        }
        public vec2(float x, float y) : this(x)
        {
            this.y = y;
        }



        public static vec2 one = new vec2(1, 1);
    }

    public class vec3 : vec
    {
        public float x, y, z;

        public vec3() { }
        public vec3(float x)
        {
            this.x = x;
        }
        public vec3(float x, float y) : this(x)
        {
            this.y = y;
        }
        public vec3(float x, float y, float z) : this(x,y)
        {
            this.z = z;
        }




        public static vec3 one = new vec3(1, 1, 1);
    }
}
