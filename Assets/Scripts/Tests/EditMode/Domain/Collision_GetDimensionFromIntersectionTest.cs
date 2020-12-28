using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Collision_GetDimensionFromIntersectionTest
    {
        // A Test behaves as an ordinary method
        [Test, TestCaseSource("ForCollision_GetDimensionFromIntersection")]
        public void Collision_GetDimensionFromIntersection(Bounds self, Bounds intersection, ActionSample.Domain.Collision.Dimension? expected)
        {
            ActionSample.Domain.Collision.Dimension? result = ActionSample.Domain.Collision.GetDimensionFromIntersection(self, intersection);
            Assert.That(result, Is.EqualTo(expected));
        }


        public static IEnumerable<TestCaseData> ForCollision_GetDimensionFromIntersection()
        {
            string name;
            TestCaseData t;

            name = "左方向からの接触";
            t = new TestCaseData(
                new Bounds(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                new Bounds(
                    new Vector3(0, 0, 0),
                    new Vector3(5, 100, 20)
                ),
                ActionSample.Domain.Collision.Dimension.LEFT

            );
            t.SetName(name);
            yield return t;

            name = "右方向からの接触";
            t = new TestCaseData(
                new Bounds(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                new Bounds(
                    new Vector3(45, 0, 0),
                    new Vector3(5, 100, 20)
                ),
                ActionSample.Domain.Collision.Dimension.RIGHT

            );
            t.SetName(name);
            yield return t;

            name = "上方向からの接触";
            t = new TestCaseData(
                new Bounds(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                new Bounds(
                    new Vector3(0, 95, 0),
                    new Vector3(50, 5, 20)
                ),
                ActionSample.Domain.Collision.Dimension.TOP

            );
            t.SetName(name);
            yield return t;

            name = "下方向からの接触";
            t = new TestCaseData(
                new Bounds(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                new Bounds(
                    new Vector3(0, 5, 0),
                    new Vector3(50, 5, 20)
                ),
                ActionSample.Domain.Collision.Dimension.BOTTOM

            );
            t.SetName(name);
            yield return t;

            name = "手前方向からの接触";
            t = new TestCaseData(
                new Bounds(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                new Bounds(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 5)
                ),
                ActionSample.Domain.Collision.Dimension.FRONT

            );
            t.SetName(name);
            yield return t;

            name = "奥方向からの接触";
            t = new TestCaseData(
                new Bounds(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                new Bounds(
                    new Vector3(0, 0, 15),
                    new Vector3(50, 100, 5)
                ),
                ActionSample.Domain.Collision.Dimension.REAR

            );
            t.SetName(name);
            yield return t;

            name = "接触していない場合";
            t = new TestCaseData(
                new Bounds(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                new Bounds(
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0)
                ),
                null

            );
            t.SetName(name);
            yield return t;

            name = "わずかな差で左方向と検出される場合";
            t = new TestCaseData(
                new Bounds(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 50, 20)
                ),
                new Bounds(
                    new Vector3(14, 15, 0),
                    new Vector3(14, 15, 20)
                ),
                ActionSample.Domain.Collision.Dimension.LEFT
            );
            t.SetName(name);
            yield return t;

        }
    }
}
