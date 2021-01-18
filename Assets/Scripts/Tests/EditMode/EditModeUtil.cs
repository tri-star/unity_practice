using System.Collections.Generic;
using ActionSample.Domain;
using ActionSample.Domain.RandomGenerator;
using ActionSample.Infrastructure.EntityManager;
using ActionSample.Infrastructure.RandomGenerator;
using ActionSample.Infrastructure.TimeManager;

namespace Tests.EditMode
{
    public class EditModeUtil
    {
        public static GameContext CreateGameContext(Dictionary<string, IRandomGenerator> randomGeneratorMap = null, ITimeManager timeManager = null)
        {
            if (timeManager == null)
            {
                timeManager = new TimeManagerUnity();
            }

            var context = new GameContext(entityManager: new EntityManagerUnity(), timeManager);
            if (randomGeneratorMap != null)
            {
                foreach (var entry in randomGeneratorMap)
                {
                    context.RandomGeneratorManager.Add(entry.Key, entry.Value);
                }
            }
            else
            {
                context.RandomGeneratorManager.Add("Game", new RandomGeneratorUnity(0));
            }
            return context;
        }
    }
}
