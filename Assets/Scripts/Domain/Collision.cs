using UnityEngine;

namespace ActionSample.Domain
{
    public class Collision
    {

        public enum Dimension
        {
            LEFT,
            RIGHT,
            TOP,
            BOTTOM,
            FRONT,
            REAR
        }


        /// <summary>
        /// 2つの領域の重なった部分のサイズを返す
        /// </summary>
        /// <param name="self">自分の領域サイズ</param>
        /// <param name="other">相手の領域サイズ</param>
        /// <returns>衝突していない場合はnull、衝突している場合は重なった部分のサイズ</returns>
        public static Bounds? GetIntersection(Bounds self, Bounds other)
        {
            if (!self.Intersects(other))
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
                    left + (width / 2),
                    top + (height / 2),
                    front + (depth / 2)
                ),
                new Vector3(width, height, depth)
            );
        }


        /// <summary>
        /// 指定された衝突範囲が自分にとってどの方向の衝突かを返す
        /// </summary>
        /// <param name="self">自分の領域サイズ</param>
        /// <param name="intersection">相手との衝突範囲の交差部分</param>
        /// <returns>衝突の方向</returns>
        public static Dimension? GetDimensionFromIntersection(Bounds self, Bounds intersection)
        {
            if(intersection.size.z < intersection.size.x && intersection.size.z < intersection.size.y)
            {
                if(intersection.min.z >= (self.size.z / 2))
                {
                    return Dimension.REAR;
                }
                else
                {
                    return Dimension.FRONT;
                }
            }
            if (intersection.size.x < intersection.size.z && intersection.size.x < intersection.size.y)
            {
                if (intersection.min.x >= (self.size.x / 2))
                {
                    return Dimension.RIGHT;
                }
                else
                {
                    return Dimension.LEFT;
                }
            }
            if (intersection.size.y < intersection.size.z && intersection.size.y < intersection.size.x)
            {
                if (intersection.min.y >= (self.size.y / 2))
                {
                    return Dimension.BOTTOM;
                }
                else
                {
                    return Dimension.TOP;
                }
            }
            return null;
        }
    }

}