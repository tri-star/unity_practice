using System.Collections.Generic;
using ActionSample.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.EditMode.Utils
{
    public class CollisionUtilTest
    {
        // A Test behaves as an ordinary method
        [Test, TestCaseSource("ForGetIntersection")]
        public void _接触している場合は範囲_接触していない場合はnullを返すこと(Bounds self, Bounds other, Bounds? expected)
        {
            Bounds? result = CollisionUtils.GetIntersection(self, other);
            Assert.That(result, Is.EqualTo(expected));
        }


        public static IEnumerable<TestCaseData> ForGetIntersection()
        {
            TestCaseData t;
            string name;

            name = "接触なし__X軸";
            t = new TestCaseData(
                createBoundsFromLTWH(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                createBoundsFromLTWH(
                    new Vector3(51, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                null
            );
            t.SetName(name);
            yield return t;

            name = "接触なし__Y軸";
            t = new TestCaseData(
                createBoundsFromLTWH(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                createBoundsFromLTWH(
                    new Vector3(0, 101, 0),
                    new Vector3(50, 100, 20)
                ),
                null
            );
            t.SetName(name);
            yield return t;

            name = "接触なし__Z軸";
            t = new TestCaseData(
                createBoundsFromLTWH(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                createBoundsFromLTWH(
                    new Vector3(0, 0, 21),
                    new Vector3(50, 100, 20)
                ),
                null
            );
            t.SetName(name);
            yield return t;

            name = "接触あり__X軸";
            t = new TestCaseData(
                createBoundsFromLTWH(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                createBoundsFromLTWH(
                    new Vector3(49, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                createBoundsFromLTWH(
                    new Vector3(49, 0, 0),
                    new Vector3(1, 100, 20)
                )
            );
            t.SetName(name);
            yield return t;

            name = "接触あり__Y軸";
            t = new TestCaseData(
                createBoundsFromLTWH(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                createBoundsFromLTWH(
                    new Vector3(0, 99, 0),
                    new Vector3(50, 100, 20)
                ),
                createBoundsFromLTWH(
                    new Vector3(0, 99, 0),
                    new Vector3(50, 1, 20)
                )
            );
            t.SetName(name);
            yield return t;

            name = "接触あり__Z軸";
            t = new TestCaseData(
                createBoundsFromLTWH(
                    new Vector3(0, 0, 0),
                    new Vector3(50, 100, 20)
                ),
                createBoundsFromLTWH(
                    new Vector3(0, 0, 19),
                    new Vector3(50, 100, 20)
                ),
                createBoundsFromLTWH(
                    new Vector3(0, 0, 19),
                    new Vector3(50, 100, 1)
                )
            );
            t.SetName(name);
            yield return t;

        }


        private static Bounds createBoundsFromLTWH(Vector3 position, Vector3 size)
        {
            return new Bounds(
                new Vector3(
                    position.x + (size.x / 2),
                    position.y + (size.y / 2),
                    position.z + (size.z / 2)
                ),
                size
            );
        }
    }
}
