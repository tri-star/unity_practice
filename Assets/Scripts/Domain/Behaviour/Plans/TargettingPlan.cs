#nullable enable
using ActionSample.Components.Unit;
using ActionSample.Domain.BehaviourTree;
using UnityEngine;

namespace ActionSample.Domain.Behaviour.Plans
{
    public class TargettingPlan : IBehaviourPlan
    {
        private enum STATES
        {
            INITIAL,
            MOVING,
            DONE
        }

        private STATES state;

        private Timer timer;

        private float timeoutSec;

        public string Name { get { return "プレイヤーに近づく"; } }

        public TargettingPlan(float timeoutSec)
        {
            this.timeoutSec = timeoutSec;
            timer = new Timer(timeoutSec);
        }


        public void Init()
        {
            state = STATES.INITIAL;
            timer.Init(timeoutSec);
        }

        public void Execute(GameContext context, IUnit unit)
        {
            switch (state)
            {
                case STATES.INITIAL:
                    timer.Init(timeoutSec);
                    state = STATES.MOVING;
                    break;
                case STATES.MOVING:
                    GameObject player = context.GetPlayer();
                    timer.Update(context.TimeManager.GetDeltaTime());
                    if (timer.IsDone() || Collision.IsIntersect(unit.Bounds, player.GetComponent<IUnit>().Bounds))
                    {
                        state = STATES.DONE;
                        break;
                    }
                    float vectorX = IsPlayerStatesLeft(player, unit) ? -1 : 1;
                    float vectorZ = IsPlayerStatesFront(player, unit) ? 1 : -1;
                    unit.MoveToward(vectorX, vectorZ);
                    break;
                case STATES.DONE:

                    break;
            }
        }

        public bool IsDone()
        {
            return state == STATES.DONE;
        }


        private bool IsPlayerStatesFront(GameObject player, IUnit unit)
        {
            return player.transform.position.z > unit.Transform.position.z;
        }


        private bool IsPlayerStatesLeft(GameObject player, IUnit unit)
        {
            return player.transform.position.x < unit.Transform.position.x;
        }

    }
}
