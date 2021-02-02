#nullable enable
using UnityEngine;
using ActionSample.Domain;
using System.Collections.Generic;
using ActionSample.Components.Unit;
using ActionSample.Domain.EntityManager;
using UnityEngine.Events;
using Zenject;
using ActionSample.Signals;

namespace ActionSample.Infrastructure.EntityManager
{
    public class EntityManagerUnity : IEntityManager
    {
        Dictionary<string, GameObject> taggedObjectCache;

        // TODO: 後でIEntityに変更する
        Dictionary<int, IUnit> entities;

        SignalBus signalBus;

        public EntityManagerUnity(SignalBus signalBus)
        {
            taggedObjectCache = new Dictionary<string, GameObject>();
            entities = new Dictionary<int, IUnit>();
            this.signalBus = signalBus;
        }

        public void AddEntity(IUnit entity)
        {
            entities.Add(entity.GetHashCode(), entity);
            signalBus.TryFire(new UnitBornSignal(entity));
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
