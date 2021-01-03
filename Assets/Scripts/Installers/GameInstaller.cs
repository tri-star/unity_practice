
using ActionSample.Components.Ui;
using ActionSample.Signals;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<PlayerAttackSignal>();
        Container.DeclareSignal<UnitStateChangeSignal>();
        Container.DeclareSignal<UnitDamageSignal>();

        Container.Bind<ActionButton>().AsTransient();
    }
}
