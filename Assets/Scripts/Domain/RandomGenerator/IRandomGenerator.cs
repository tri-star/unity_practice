
namespace ActionSample.Domain.RandomGenerator
{
    public interface IRandomGenerator
    {
        void Init(int seed);
        float FromRange(float min, float max);
    }
}
