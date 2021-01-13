using ActionSample.Domain.RandomGenerator;

namespace ActionSample.Domain
{
    public class GameContext
    {
        public RandomGeneratorManager RandomGeneratorManager { get; private set; }

        public GameContext()
        {
            this.RandomGeneratorManager = new RandomGeneratorManager();
        }
    }
}
