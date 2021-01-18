#nullable enable
using ActionSample.Domain.RandomGenerator;
using UnityEngine;

namespace ActionSample.Domain
{
    public class GameContext
    {
        private GameObject? player;

        public RandomGeneratorManager RandomGeneratorManager { get; private set; }

        public IEntityManager EntityManager { get; private set; }

        public ITimeManager TimeManager { get; private set; }

        public GameContext(IEntityManager entityManager, ITimeManager timeManager)
        {
            this.RandomGeneratorManager = new RandomGeneratorManager();
            this.EntityManager = entityManager;
            this.TimeManager = timeManager;
            this.player = null;
        }

        public GameObject GetPlayer()
        {
            if (player == null)
            {
                player = EntityManager.FindObjectWithTag("player");
            }
            return player!;
        }
    }
}
