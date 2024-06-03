using UnityEngine;

public enum proType
{
    rock, arrow, fireball
};

public class Projectile : MonoBehaviour, ICloneable<Projectile>
{
    [SerializeField]
    private int attackStrength;
    [SerializeField]
    private proType projectileType;

    public int AttackStrength
    {
        get { return attackStrength; }
    }

    public proType ProjectileType
    {
        get { return projectileType; }
    }

    public Projectile Clone()
    {
        return Instantiate(this);
    }
}
