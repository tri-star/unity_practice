
using ActionSample.Components;
using ActionSample.Components.Ui;
using ActionSample.Domain;
using ActionSample.Infrastructure.EntityManager;
using ActionSample.Infrastructure.RandomGenerator;
using ActionSample.Infrastructure.TimeManager;
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
        Container.Bind<JimmyUnit>().ToSelf().AsTransient();

        InstallStageSetting();
        InstallGameContext();
    }


    private void InstallGameContext()
    {
        Container.Bind<ITimeManager>().To<TimeManagerUnity>().AsSingle();
        Container.Bind<IEntityManager>().To<EntityManagerUnity>().AsSingle();
        Container.Bind<GameContext>().ToSelf().AsSingle();
        var context = Container.Resolve<GameContext>();
        context.RandomGeneratorManager.Add("Game", new RandomGeneratorUnity(100));
    }

    private void InstallStageSetting()
    {
        Container.Bind<StageSetting>().FromResource("StageSetting").AsCached();
    }
}
