using ActionSample.Components.Unit;

namespace ActionSample.Domain.EntityManager
{
    public class EntityAddEvent
    {
        public IUnit Unit { get; private set; }

        public EntityAddEvent(IUnit unit)
        {
            Unit = unit;
        }
    }
}
