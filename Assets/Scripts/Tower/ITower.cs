public interface ITower
{
    void SetAttackStrategy(IAttackStrategy strategy);
    Enemy TargetEnemy { get; }
    Projectile Projectile { get; }
}
