using System;

namespace ActionSample.Domain.BehaviourTree
{
    public class PlanNotFoundException : Exception
    {
        public PlanNotFoundException(string message) : base(message)
        {
        }
    }
}
