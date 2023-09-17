namespace SpriteTrail
{
    public interface IPoolable
    {
        void SetReturnToPool(ReturnObjectToPool returnDelegate);
    }
}