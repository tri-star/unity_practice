using System.Collections.Generic;
using ActionSample.Domain;
using ActionSample.Domain.RandomGenerator;
using ActionSample.Infrastructure.RandomGenerator;

namespace Tests.EditMode
{
    public class EditModeUtil
    {
        public static GameContext CreateGameContext(Dictionary<string, IRandomGenerator> randomGeneratorMap = null)
        {
            var context = new GameContext();
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
