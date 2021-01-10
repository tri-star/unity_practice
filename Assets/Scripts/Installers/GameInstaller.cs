
using ActionSample.Components.Ui;
using ActionSample.Parameters;
using ActionSample.Signals;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<PlayerAttackSignal>().OptionalSubscriber();
        Container.DeclareSignal<UnitStateChangeSignal>().OptionalSubscriber();
        Container.DeclareSignal<UnitDamageSignal>().OptionalSubscriber();

        Container.Bind<ActionButton>().AsTransient();

        InstallStageSetting();
    }


    private void InstallStageSetting()
    {
        Container.Bind<StageSetting>().FromResource("StageSetting").AsCached();
    }
}
