
using ActionSample.Components.Ui;
using ActionSample.Parameters;
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

        InstallStageSetting();
    }


    private void InstallStageSetting()
    {
        Container.Bind<StageSetting>().FromResource("StageSetting").AsCached();
    }
}
