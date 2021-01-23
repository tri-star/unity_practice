#nullable enable
using ActionSample.Components;
using ActionSample.Domain.BehaviourTree;

namespace ActionSample.Domain.Behaviour.Plans
{
    public class NoOpPlan : IBehaviourPlan
    {
        private Timer timer;

        private float timeoutSec;

        public string name { get { return "何もしない"; } }

        public NoOpPlan(float timeoutSec)
        {
            this.timeoutSec = timeoutSec;
            timer = new Timer(timeoutSec);
            timer.Init(timeoutSec);
        }


        public void Init()
        {
            timer.Init(timeoutSec);
        }

        public void Execute(GameContext context, IUnit unit)
        {
            unit.StopForce();
            timer.Update(context.TimeManager.GetDeltaTime());
        }

        public bool isDone()
        {
            return timer.IsDone();
        }
    }
}
