using UnityEngine;

public class Enemy : MonoBehaviour {
    
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int rewardAmt;
    [SerializeField]
    private Transform exitPoint;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private float navigationUpdate;
    [SerializeField]
    private Animator anim;
    private int target = 0;
    private Transform enemy;
    private Collider2D enemyCollider;
    private float navigationTime = 0;
    private bool isDead = false; 

    public bool IsDead {
        get {
            return isDead;
        }
    }

    // Use this for initialization
    void Start () {
        enemy = GetComponent<Transform> ();
        anim = GetComponent<Animator>();    
        enemyCollider = GetComponent<Collider2D>();
        GameManager.Instance.RegisterEnemy(this);    
    }

    // Update is called once per frame
    void Update () {
        if (wayPoints != null && !isDead) {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate) {
                if (target < wayPoints.Length) {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, 0.8f * navigationTime);
                } else {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, 0.8f * navigationTime);
                }
                navigationTime = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "WayPoint")
            target += 1;
        else if (other.tag == "Finish") {
            GameManager.Instance.TotalEscaped += 1;
            GameManager.Instance.RoundEscaped += 1;
            GameManager.Instance.UnRegister(this);
            GameManager.Instance.isWaveOver();
        } else if (other.tag == "Projectile") {
            Projectile newP = other.gameObject.GetComponent<Projectile>();
            enemyHit(newP.AttackStrength);
            Destroy(other.gameObject);
        }
    }

    public void enemyHit(int hitPoints) {
        if (healthPoints - hitPoints > 0) {
            anim.Play("Hurt");
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
            healthPoints -= hitPoints;
        } else {
            die();
        }
    }

    public void die() {
        isDead = true;
        anim.SetTrigger("didDie");
        GameManager.Instance.TotalKilled += 1;
        enemyCollider.enabled = false;
        GameManager.Instance.addMoney(rewardAmt);
        GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Die);
        GameManager.Instance.isWaveOver();
    }
}
