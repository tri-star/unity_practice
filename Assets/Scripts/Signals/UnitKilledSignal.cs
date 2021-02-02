
using ActionSample.Components.Unit;

namespace ActionSample.Signals
{
    public class UnitKilledSignal
    {
        // @TODO: ユニットの種類を特定できるようにする
        public IUnit Unit { get; private set; }

        // @TODO: 死亡した原因の武器を特定できるようにする

        public UnitKilledSignal(IUnit unit)
        {
            Unit = unit;
        }
    }
}
