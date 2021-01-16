using NUnit.Framework;
using ActionSample.Infrastructure.EntityManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests.Infrastructure.EntityManager
{
    class EntityManagerTest
    {
        private EntityManagerUnity entityManager;

        [SetUp]
        public void SetUp()
        {
            entityManager = new EntityManagerUnity();
            SceneManager.LoadScene("EntityManagerTest");

            foreach (var gameObject in GameObject.FindObjectsOfType<GameObject>())
            {
                GameObject.DestroyImmediate(gameObject);
            }
        }

        [Test]
        public void _指定したタグのオブジェクトを検索できる()
        {
            GameObject g = new GameObject();
            g.tag = "obstacle";
            g.name = "obj";

            var result = entityManager.FindObjectWithTag("obstacle");
            Assert.That(result, Is.EqualTo(g));
        }


        [Test]
        public void _キャッシュした場合はキャッシュしたオブジェクトを返す()
        {
            GameObject g = new GameObject();
            g.tag = "obstacle";
            g.name = "obj1";
            entityManager.FindObjectWithTag("obstacle", useCache: true);
            GameObject.DestroyImmediate(g);

            GameObject g2 = new GameObject();
            g2.tag = "obstacle";
            g2.name = "obj2";

            var result = entityManager.FindObjectWithTag("obstacle", useCache: true);
            Assert.That(result, Is.EqualTo(g));
        }

        [Test]
        public void _キャッシュしない場合は常に対象を検索する()
        {
            GameObject g = new GameObject();
            g.tag = "obstacle";
            g.name = "obj1";
            entityManager.FindObjectWithTag("obstacle", useCache: false);
            GameObject.DestroyImmediate(g);

            GameObject g2 = new GameObject();
            g2.tag = "obstacle";
            g2.name = "obj2";

            var result = entityManager.FindObjectWithTag("obstacle", useCache: false);
            Assert.That(result, Is.EqualTo(g2));
        }


    }
}
