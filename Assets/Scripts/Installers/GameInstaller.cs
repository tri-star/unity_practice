
using ActionSample.Components.Ui;
using ActionSample.Domain;
using ActionSample.Infrastructure.RandomGenerator;
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
        Container.DeclareSignal<UnitKilledSignal>().OptionalSubscriber();

        Container.Bind<ActionButton>().AsTransient();

        InstallStageSetting();
        InstallGameContext();
    }


    private void InstallGameContext()
    {
        GameContext context = new GameContext();
        context.RandomGeneratorManager.Add("Game", new RandomGeneratorUnity(100));
        Container.Bind<GameContext>().FromInstance(context).AsSingle();
    }

    private void InstallStageSetting()
    {
        Container.Bind<StageSetting>().FromResource("StageSetting").AsCached();
    }
}
