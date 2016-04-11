using UnityEngine;
using System.Collections;

namespace PE
{ 

    public class UnityAdapter {

        public static Vector3 UnityVector(Vec3 v)
        {
            
            return new Vector3((float)v.x, (float)v.y, (float)v.z);
        }

        public static Vec3 PEVector(Vector3 v)
        {
            return new Vec3((double)v.x, (double)v.y, (double)v.z);
        }

    }
}