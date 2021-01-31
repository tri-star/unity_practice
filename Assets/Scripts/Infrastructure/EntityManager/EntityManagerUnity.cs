#nullable enable
using UnityEngine;
using ActionSample.Domain;
using System.Collections.Generic;
using ActionSample.Components.Unit;
using ActionSample.Domain.EntityManager;
using UnityEngine.Events;

namespace ActionSample.Infrastructure.EntityManager
{
    public class EntityManagerUnity : IEntityManager
    {
        Dictionary<string, GameObject> taggedObjectCache;

        // TODO: 後でIEntityに変更する
        Dictionary<int, IUnit> entities;

        public UnityEvent<EntityAddEvent> EntityAddEvent
        {
            get;
            private set;
        }

        public EntityManagerUnity()
        {
            taggedObjectCache = new Dictionary<string, GameObject>();
            entities = new Dictionary<int, IUnit>();
            EntityAddEvent = new UnityEvent<EntityAddEvent>();
        }

        public void AddEntity(IUnit entity)
        {
            entities.Add(entity.GetHashCode(), entity);
            EntityAddEvent.Invoke(new EntityAddEvent(entity));
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

        public void Update(GameContext context)
        {
        }
    }

}
