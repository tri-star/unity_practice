using UnityEngine;
using ActionSample.Utils.Coordinates;

namespace ActionSample.Utils
{
    public class CollisionUtils
    {
        /// <summary>
        /// 2つの領域の重なった部分のサイズを返す
        /// </summary>
        /// <param name="self">自分の領域サイズ</param>
        /// <param name="other">相手の領域サイズ</param>
        /// <returns>衝突していない場合はnull、衝突している場合は重なった部分のサイズ</returns>
        public static Bounds? GetIntersection(Bounds self, Bounds other)
        {
            if(!self.Intersects(other))
            {
                return null;
            }

            float left = Mathf.Max(self.min.x, other.min.x) - self.min.x;
            float width = Mathf.Min(self.max.x, other.max.x) - Mathf.Max(self.min.x, other.min.x);
            float top = Mathf.Max(self.min.y, other.min.y) - self.min.y;
            float height = Mathf.Min(self.max.y, other.max.y) - Mathf.Max(self.min.y, other.min.y);
            float front = Mathf.Max(self.min.z, other.min.z) - self.min.z;
            float depth = Mathf.Min(self.max.z, other.max.z) - Mathf.Max(self.min.z, other.min.z);

            return new Bounds(
                new Vector3(
                    left + (width/2),
                    top + (height / 2),
                    front + (depth / 2)
                ),
                new Vector3(width, height, depth)
            );
        }
    }

}

