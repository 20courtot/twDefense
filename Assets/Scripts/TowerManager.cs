using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerManager : Singleton<TowerManager>
{
    public TowerButton towerBtnPressed { get; set; }
    public List<GameObject> TowerList = new List<GameObject>();
    public List<Collider2D> BuildList = new List<Collider2D>();

    private SpriteRenderer spriteRenderer;
    private Collider2D buildTile;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "BuildSite")
            {
                buildTile = hit.collider;
                buildTile.tag = "BuildSiteFull";
                RegisterBuildSite(buildTile);
                placeTower(hit);
            }
        }
        if (spriteRenderer.enabled)
        {
            followMouse();
        }
    }

    public void RegisterBuildSite(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }

    public void RenameTagsBuildSites()
    {
        foreach (Collider2D buildTag in BuildList)
        {
            buildTag.tag = "BuildSite";
        }
        BuildList.Clear();
    }

    public void RegisterTower(GameObject tower)
    {
        TowerList.Add(tower);
    }

    public void DestroyAllTowers()
    {
        foreach (GameObject tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }

    public void placeTower(RaycastHit2D hit){
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            GameObject towerPrefab = towerBtnPressed.TowerObject;
            ITower newTower = TowerFactory.CreateTower(towerPrefab, null); // Instancie la tour sans stratégie pour le moment
            if (newTower != null)
            {
                GameObject newTowerObject = (newTower as MonoBehaviour).gameObject;
                newTowerObject.transform.position = hit.transform.position;
                RegisterTower(newTowerObject);

                // Définir la stratégie d'attaque après l'instanciation
                IAttackStrategy attackStrategy = TowerFactory.GetAttackStrategy(newTower.Projectile.ProjectileType);
                newTower.SetAttackStrategy(attackStrategy);

                buyTower(towerBtnPressed.TowerPrice);
                disableDragSprite();
            }
        }
    }

    public void selectedTower(TowerButton towerBtn)
    {
        if (towerBtn.TowerPrice <= GameManager.Instance.TotalMoney)
        {
            towerBtnPressed = towerBtn;
            enableDragSprite(towerBtn.DragSprite);
        }
    }

    public void buyTower(int price)
    {
        GameManager.Instance.subtractMoney(price);
        GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.BuildTower);
    }

    private void followMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void enableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void disableDragSprite()
    {
        spriteRenderer.enabled = false;
        towerBtnPressed = null;
    }
}
