#nullable enable
using ActionSample.Components.Unit;
using UnityEngine;
using UnityEngine.Events;

namespace ActionSample.Domain.EntityManager
{
    public interface IEntityManager
    {
        UnityEvent<EntityAddEvent> EntityAddEvent { get; }

        GameObject? FindObjectWithTag(string tag, bool useCache = false);


        void AddEntity(IUnit entity);

        void Update(GameContext context);
    }
}
