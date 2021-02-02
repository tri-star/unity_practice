using ActionSample.Components.Unit;

namespace ActionSample.Signals
{
    public class UnitBornSignal
    {
        public IUnit Unit { get; private set; }

        public UnitBornSignal(IUnit unit)
        {
            Unit = unit;
        }
    }
}
