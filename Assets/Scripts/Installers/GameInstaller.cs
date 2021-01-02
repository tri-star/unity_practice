
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        base.InstallBindings();

        SignalBusInstaller.Install(Container);
    }
}
