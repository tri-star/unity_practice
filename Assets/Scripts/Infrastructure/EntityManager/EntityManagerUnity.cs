#nullable enable
using UnityEngine;
using ActionSample.Domain;
using System.Collections.Generic;

namespace ActionSample.Infrastructure.EntityManager
{
    public class EntityManagerUnity : IEntityManager
    {
        Dictionary<string, GameObject> taggedObjectCache;

        public EntityManagerUnity()
        {
            taggedObjectCache = new Dictionary<string, GameObject>();
        }

        public GameObject? FindObjectWithTag(string tag, bool useCache = false)
        {
            if (useCache && taggedObjectCache.ContainsKey(tag))
            {
                return taggedObjectCache[tag];
            }
            var gameObject = GameObject.FindGameObjectWithTag(tag);

            if (gameObject && useCache)
            {
                taggedObjectCache.Add(tag, gameObject);
            }

            return gameObject;
        }
    }

}
