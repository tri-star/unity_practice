#nullable enable
using ActionSample.Components;
using ActionSample.Domain.BehaviourTree;

namespace ActionSample.Domain.Behaviour.Plans
{
    public class MeleePlan : IBehaviourPlan
    {
        public string name => "近接攻撃";

        private enum STATES
        {
            INITIAL,
            ATTACKING,
            DONE,
        }

        private STATES state;

        public void Init()
        {
            state = STATES.INITIAL;
        }

        public void Execute(GameContext context, IUnit unit)
        {
            switch (state)
            {
                case STATES.INITIAL:
                    var combatUnit = unit.GetComponent<CombatUnit>();
                    combatUnit.Attack();
                    state = STATES.ATTACKING;
                    break;
                case STATES.ATTACKING:
                    if (unit.GetState() != Unit.States.ATTACK)
                    {
                        state = STATES.DONE;
                    }
                    break;
            }
        }

        public bool isDone()
        {
            return state == STATES.DONE;
        }
    }
}
