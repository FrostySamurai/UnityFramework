namespace Samurai.UnityFramework.Pooling
{
    public interface IPoolable
    {
        void OnRetrievedFromPool();
        void OnReturnedToPool();
    }
}