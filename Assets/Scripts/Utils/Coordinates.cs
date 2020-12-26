using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionSample.Utils.Coordinates
{

    class BoundsUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="center">左上の座標</param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Bounds CreateFromLTWH(Vector3 position, Vector3 size)
        {
            return new Bounds(
                new Vector3(
                    position.x - (size.x / 2),
                    position.y - (size.y / 2),
                    position.z - (size.z / 2)
                ),
                size
            );
        }
    }

}
