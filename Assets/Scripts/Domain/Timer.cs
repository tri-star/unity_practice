namespace ActionSample.Domain
{
    public class Timer
    {
        private float timeOutSec;

        private float elapsedSec;


        public Timer(float timeOutSec)
        {
            this.timeOutSec = timeOutSec;
        }

        public void Init(float timeOutSec)
        {
            this.timeOutSec = timeOutSec;
            elapsedSec = 0;
        }

        /// <summary>
        /// タイマーを進める
        /// </summary>
        /// <param name="deltaTime">経過時間(単位：秒)</param>
        public void Update(float deltaTime)
        {
            elapsedSec += deltaTime;
        }


        public void Reset()
        {
            elapsedSec = 0;
        }

        public bool IsDone()
        {
            return elapsedSec >= timeOutSec;
        }
    }
}
