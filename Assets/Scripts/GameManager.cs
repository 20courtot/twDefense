using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum gameStatus {
    next, play, gameover, win
}

public class GameManager : Singleton<GameManager> {
    const float waitingTime = 1f;

    [SerializeField]
    private int totalWaves = 10;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private Enemy[] enemies;
    [SerializeField]
    private int totalEnemies = 3;
    [SerializeField]
    private int enemiesPerSpawn;
    [SerializeField]
    public Text totalMoneyLabel;
    [SerializeField]
    public Image GameStatusImage;
    [SerializeField]
    public Text nextWaveBtnLabel;
    [SerializeField]
    public Text escapedLabel;
    [SerializeField]
    public Text waveLabel;
    [SerializeField]
    public Text GameStatusLabel;
    [SerializeField]
    private int waveNumber = 0;
    private int totalMoney = 10; 
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int enemiesToSpawn = 0;
    private IGameState currentState;
    private AudioSource audioSource;

    public List<Enemy> EnemyList = new List<Enemy>();

    public int WaveNumber {
        get {
            return waveNumber;
        } set {
            waveNumber = value;
        }
    }
    public int TotalEscaped {
        get {
            return totalEscaped;
        }
        set {
            totalEscaped = value;
        }
    }
    public int RoundEscaped {
        get {
            return roundEscaped;
        }
        set {
            roundEscaped = value;
        }
    }
    public int TotalKilled {
        get {
            return totalKilled;
        }
        set {
            totalKilled = value;
        }
    }
    public int TotalMoney {
        get {
            return totalMoney;
        }
        set {
            totalMoney = value;
            totalMoneyLabel.text = totalMoney.ToString();
        }
    }
    public AudioSource AudioSource {
        get {
            return audioSource;
        }
    }

    protected override void Awake() {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            Debug.LogError("AudioSource component missing from this game object");
        }
    }

    void Start() {

        GameStatusImage.gameObject.SetActive(false);
        ChangeState(new PlayState());
    }

    void Update() {
        handleEscape();
        currentState?.UpdateState(this);
    }

    IEnumerator spawn() {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies) {
            for(int i = 0; i < enemiesPerSpawn; i++) {
                if(EnemyList.Count < totalEnemies) {
                    Enemy newEnemy = Instantiate(enemies[ Random.Range( 0, enemiesToSpawn)]) as Enemy;
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }
            yield return new WaitForSeconds(waitingTime);
            StartCoroutine(spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy) {
        EnemyList.Add(enemy);
    }

    public void UnRegister(Enemy enemy) {
        EnemyList.Remove(enemy);
        Destroy (enemy.gameObject);
        isWaveOver();
    }

    public void DestroyAllEnemies() {
        foreach (Enemy enemy in EnemyList) {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    private void handleEscape() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            TowerManager.Instance.disableDragSprite();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }

    public void addMoney(int amount){
        TotalMoney += amount;
    }

    public void subtractMoney(int amount) {
        TotalMoney -= amount;
    }

    public void isWaveOver() {
        escapedLabel.text = "Escaped " + TotalEscaped + "/10";
        if ((roundEscaped + TotalKilled) == totalEnemies){
            if(waveNumber <= enemies.Length){
                enemiesToSpawn = waveNumber;
            } 
            setCurrentGameState();
        }
    }

    public void setCurrentGameState(){
        if (TotalEscaped >= 10){
            ChangeState(new GameOverState());
        } else if (waveNumber >= totalWaves) {
            ChangeState(new WinState());
        } else {
            ChangeState(new NextState());
        }
    }

    public void nextWavePressed() {
        switch (currentState)
        {
            case NextState _:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            default:
                totalEnemies = 3;
                TotalEscaped = 0;
                waveNumber = 0;
                enemiesToSpawn = 0;
                TotalMoney = 10;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagsBuildSites();
                totalMoneyLabel.text = TotalMoney.ToString();
                escapedLabel.text = "Escaped " + TotalEscaped + "/10";
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);
                break;
        }
        DestroyAllEnemies();
        TotalKilled = 0;
        roundEscaped = 0;
        waveLabel.text = "Wave " + (waveNumber + 1);
        StartCoroutine(spawn());
        GameStatusImage.gameObject.SetActive(false);
    }

    public void showMenu() {
        Debug.Log("Showing menu with current state: " + currentState.GetType().Name);
        switch (currentState)
        {
            case GameOverState _:
                GameStatusLabel.text = "Gameover";
                audioSource.PlayOneShot(SoundManager.Instance.Gameover);
                nextWaveBtnLabel.text = "Play again";
                break;
            case NextState _:
                nextWaveBtnLabel.text = "Next Wave";
                GameStatusLabel.text = "Wave " + (waveNumber + 2) + " next.";
                break;
            case PlayState _:
                nextWaveBtnLabel.text = "Play";
                break;
            case WinState _:
                nextWaveBtnLabel.text = "Play";
                GameStatusLabel.text = "You Won!";
                break;
        }
        GameStatusImage.gameObject.SetActive(true);
    }

    public void ChangeState(IGameState newState) {
        Debug.Log("Changing state to " + newState.GetType().Name);
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
}
