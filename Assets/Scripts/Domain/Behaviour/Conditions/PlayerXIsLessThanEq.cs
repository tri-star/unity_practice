#nullable enable
using ActionSample.Components.Unit;
using ActionSample.Domain.BehaviourTree;
using UnityEngine;

namespace ActionSample.Domain.Behaviour.Conditions
{
    public class PlayerXIsLessThanEq : BehaviourCondition
    {
        private float distance;

        public PlayerXIsLessThanEq(float distance)
        {
            this.distance = distance;
        }

        public override bool IsSatisfied(GameContext context, IUnit unit)
        {
            var player = context.GetPlayer();
            return Mathf.Abs(player.transform.position.x - unit.Transform.position.x) < distance;
        }
    }
}
