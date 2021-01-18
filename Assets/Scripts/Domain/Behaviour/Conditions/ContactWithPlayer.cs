using ActionSample.Components;
using ActionSample.Domain.BehaviourTree;
using UnityEngine;

namespace ActionSample.Domain.Behaviour.Conditions
{
    public class ContactWithPlayer : IBehaviourCondition
    {
        private float distance;

        public ContactWithPlayer(float distance)
        {
            this.distance = distance;
        }

        public bool isSatisfied(GameContext context, IUnit unit)
        {
            var player = context.GetPlayer();
            bool isSatisfiedX = Mathf.Abs(player.transform.position.x - unit.Transform.position.x) <= distance;
            bool isSatisfiedZ = Mathf.Abs(player.transform.position.z - unit.Transform.position.z) <= distance;

            return isSatisfiedX && isSatisfiedZ;
        }
    }
}
