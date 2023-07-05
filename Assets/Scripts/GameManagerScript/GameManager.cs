using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("Player Object")]
    public GameObject Player;
    bool beginMatch = false;

    [Header("Back Ground")]
    public Sprite BG_Image;
    public GameObject BackGroundObj;

    [Header("Group Enemies Attribute")]
    public GameObject GroupEnemies;
    public float EnemyWaveRate = 75;
    public List<EnemiesShape> enemiesShapes;
    public bool randomShape = true;
    public int fixShapeIndex = 0;
    public List<EnemyPathData> enemyPaths;
    public bool randomPath = true;
    public int fixPathIndex = 0;

    [Header("Meteorite Attribute")]
    public GameObject Obstacles;
    public GameObject MeteoritePrefab;
    public float MeteoriteGenSpeed = 0.1f;
    public float maxInterval = 10f;
    public float minInterval = 20f;
    public float maxSize = 1f;
    public float minSize = 0.25f;
    public float CurMeteo = 0;

    [Header("Bosses Attribute")]
    public List<GameObject> Bosses;
    public GameObject BossHealthBar;
    public bool isBossExist = false;
    public bool isBossDead = false;
    
    [Header("Buffs Attribute")]
    public float PowerUpRate = 15;
    public float ChangeSpaceShipRate = 15;
    public float HealthRate = 10;
    public float MoneyRate = 50;
    public List<GameObject> ItemsPickUp;
    public List<Sprite> SpaceShipList;

    [Header("Game Audio")]
    public GameObject GameMusic;

    [Header("UI Canvas")]
    public GameObject UI_Canvas;
    public GameObject UI_Hubs;
    public GameObject Boudaries;

    [Header("Wave Manager")]
    public bool usingCustomLevel = false;
    public LevelSetting Level;
    public int waveNumber = 0;
    public bool isSurvivalMode = true;
    public int MaxwaveNumber = 0;

    [Tooltip("For Debug or Survival Mode Only")]
    public int BossEveryWave = 10;
    public bool startNewWave = false;
    [Tooltip("Set Base Stat For Enemies")]
    public float statMultipler = 1.0f;
    [Tooltip("Enemies Stronger Each Wave")]
    public bool isStrongerEachWave = false;

    public int targetFramRate = 60;
    float deltaTime = 0.0f;

    void Awake() 
    {
        if(!usingCustomLevel)
        {
            EnemiesShape[] result1 = Array.ConvertAll(Resources.LoadAll("EnemiesShapes", typeof(EnemiesShape)), asset => (EnemiesShape)asset);
            EnemyPathData[] result2 = Array.ConvertAll(Resources.LoadAll("EnemiesPaths", typeof(EnemyPathData)), asset => (EnemyPathData)asset);
            enemiesShapes = result1.ToList();
            enemyPaths = result2.ToList();
        }
        else if(usingCustomLevel && Level != null)
        {
            MaxwaveNumber = Level.WaveList.Count;
        }
        else
        {
            Debug.LogError("Format Wave Manager Error");
            Time.timeScale = 0;
        }
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFramRate;
    }
    public float SetMultipiler(float statMultipler, bool isStrongerEachWave)
    {
        return Mathf.Pow(statMultipler, isStrongerEachWave? waveNumber : 1);
    }

    public void OnEnable()
    {
        SingleEnemy.callWhenDead += SetItemsPickUp;
        Meteorite.callWhenDead += CountMeteo;
        HealthManager.callWhenDead += GameOver;
        Boss_HealthManager.callWhenDead += WhenBossDead;
        PlayerController.firstTouch += BeginMatch;
    }

    public void OnDisable()
    {
        SingleEnemy.callWhenDead -= SetItemsPickUp;
        Meteorite.callWhenDead -= CountMeteo;
        HealthManager.callWhenDead -= GameOver;
        Boss_HealthManager.callWhenDead -= WhenBossDead;
        PlayerController.firstTouch -= BeginMatch;
    }
    public void BeginMatch()
    {
        beginMatch = true;
    }
    public void WhenBossDead(Vector3 spawnPosition)
    {
        if(isBossExist)
        {
            isBossDead = true;
            isBossExist = false;
            SetItemsPickUp(-2, true, spawnPosition);
            GameMusic.GetComponent<AudioSource>().PlayOneShot(GameMusic.GetComponent<GameAudio>().BossDeadShound);
        }
    }
    public void SetItemsPickUp(int ID, bool isDestroyedbyPlayer, Vector3 spawnPosition)
    {
        if(isDestroyedbyPlayer && ID >= 0)
        {    
            float i = UnityEngine.Random.Range(0.0f, 100.0f);
            if(i < PowerUpRate)
            {
                //Debug.Log("PowerUp");
                //ItemPrefab.GetComponent<SpriteRenderer>().sprite = 
                Instantiate(ItemsPickUp[0], spawnPosition, Quaternion.identity);
            }
            else if(i >= PowerUpRate && i < PowerUpRate+ChangeSpaceShipRate)
            {
                //Debug.Log("ChangeSpaceShip");
                int j = UnityEngine.Random.Range(0, SpaceShipList.Count);
                GameObject item = Instantiate(ItemsPickUp[1], spawnPosition, Quaternion.identity);
                item.transform.GetComponent<SpriteRenderer>().sprite = SpaceShipList[j];
                item.transform.GetComponent<ItemChangeShip>().ShipIndex = j;
                if(j == 0)
                {
                    item.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                }
                else
                {
                    item.transform.localScale = new Vector3(0.3f, 0.3f, 1);
                }
            }
            else if(i >= PowerUpRate+ChangeSpaceShipRate && i < PowerUpRate+ChangeSpaceShipRate+HealthRate)
            {
                Instantiate(ItemsPickUp[2], spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.Log("Coin Drop");
            }
        }
        else if(isDestroyedbyPlayer && ID < 0)
        {
            float i = UnityEngine.Random.Range(0.0f, 100.0f);
            if(ID == -2)
            {
                Instantiate(ItemsPickUp[2], spawnPosition, Quaternion.identity);
            }
            else if(ID == -1 && i < HealthRate)
            {
                Instantiate(ItemsPickUp[2], spawnPosition, Quaternion.identity);
            }
        }
    }
    public void CountMeteo(Vector3 spawnPosition, bool isDestroyedbyPlayer)
    {
        SetItemsPickUp(-1, isDestroyedbyPlayer, spawnPosition);
        CurMeteo--;
    }

    public void CreateObstacles()
    {
        float min = Boudaries.transform.GetChild(0).GetComponent<BoundaryLeft>().leftbound;
        float max = Boudaries.transform.GetChild(0).GetComponent<BoundaryLeft>().rightbound;
        float height = Boudaries.transform.GetChild(0).GetComponent<BoundaryLeft>().upperbound + 5;
        float interval = !usingCustomLevel? UnityEngine.Random.Range(minInterval, maxInterval) : UnityEngine.Random.Range(Level.minInterval, Level.maxInterval);
        StartCoroutine(GenerateMeteorites(height, interval, min, max));
    }
    IEnumerator GenerateMeteorites(float height, float time, float min, float max)
    {
        while(time > 0)
        {
            float pos = UnityEngine.Random.Range(min, max);
            float scale = !usingCustomLevel? UnityEngine.Random.Range(minSize, maxSize) : UnityEngine.Random.Range(Level.minSize, Level.maxSize);
            GameObject meteorite = Instantiate(MeteoritePrefab, new Vector3(pos, height, -5), Quaternion.identity);
            meteorite.transform.localScale = new Vector3(scale, scale, 1);
            meteorite.transform.SetParent(Obstacles.transform);
            if(!usingCustomLevel)
            {
                meteorite.GetComponent<Meteorite>().SetMaxHealth(scale, SetMultipiler(statMultipler, isStrongerEachWave));
            }
            else
            {
                meteorite.GetComponent<Meteorite>().SetMaxHealth(scale, SetMultipiler(Level.statMultipler, Level.isStrongerEachWave));
            }            
            time -= !usingCustomLevel? MeteoriteGenSpeed : Level.MeteoriteGenSpeed;
            CurMeteo += 1;
            yield return new WaitForSeconds(!usingCustomLevel? MeteoriteGenSpeed : Level.MeteoriteGenSpeed);
        }

    }
    public int GenerateEnemyShape(List<EnemiesShape> shapes = null)
    {
        int i;
        if(!usingCustomLevel)
        {
            if(randomShape)
                i = UnityEngine.Random.Range(0, enemiesShapes.Count);
            else
                i = fixShapeIndex;
        }
        else
        {
            i = UnityEngine.Random.Range(0, shapes.Count);
        }
        Debug.Log("SHAPE NUMBER: " + i); 
        return i;
    }
    public int GenerateEnemyPath(List<EnemyPathData> paths = null)
    {
        int i;
        if(!usingCustomLevel)
        {
            if(randomPath)
                i = UnityEngine.Random.Range(0, enemyPaths.Count);
            else
                i = fixPathIndex;
        }
        else
        {
            i = UnityEngine.Random.Range(0, paths.Count);
        }
        Debug.Log("PATH NUMBER: " + i); 
        return i;
    }
    public List<Vector3> GenerateEnemySpawnPoint(List<int> numOfPos)
    {
        List<Vector3> returnValue = new List<Vector3>();
        /*
            Order: Left -> Right -> Low -> Up -> Mid
        */
        foreach(int i in numOfPos)
        {
            if(i >= 0 && i <= 9)
            {
                returnValue.Add(Boudaries.transform.GetChild(0).GetChild(i).position);
            }
            else if(i >= 10 && i <= 19)
            {
                returnValue.Add(Boudaries.transform.GetChild(1).GetChild(i-10).position);
            }
            else if(i >= 20 && i <= 25)
            {
                returnValue.Add(Boudaries.transform.GetChild(2).GetChild(i-20).position);
            }
            else if(i >= 26 && i <= 31)
            {
                returnValue.Add(Boudaries.transform.GetChild(3).GetChild(i-26).position);
            }
            else
            {
                returnValue.Add(GroupEnemies.transform.GetChild(i-32).position);
            }
        }      
        return returnValue;
    }
    public List<Vector3> GeneratePathPoints(List<int> index)
    {
        List<Vector3> returnValue = new List<Vector3>();
        /*
            Order: Left -> Right -> Low -> Up -> Mid
        */
        foreach(int i in index)
        {
            if(i >= 0 && i <= 9)
            {
                returnValue.Add(Boudaries.transform.GetChild(0).GetChild(i).position);
            }
            else if(i >= 10 && i <= 19)
            {
                returnValue.Add(Boudaries.transform.GetChild(1).GetChild(i-10).position);
            }
            else if(i >= 20 && i <= 25)
            {
                returnValue.Add(Boudaries.transform.GetChild(2).GetChild(i-20).position);
            }
            else if(i >= 26 && i <= 31)
            {
                returnValue.Add(Boudaries.transform.GetChild(3).GetChild(i-26).position);
            }
            else
            {
                returnValue.Add(GroupEnemies.transform.GetChild(i-32).position);
            }
        }      
        return returnValue;
    }

    public void SpawnBoss()
    {
        isBossExist = true;
        isBossDead = false;
        int i = UnityEngine.Random.Range(0, Bosses.Count);
        var x = new List<int>();
        x.Add(28);
        var ThisBoss = new GameObject();
        if(!usingCustomLevel)
        {
            ThisBoss = Instantiate(Bosses[i], GenerateEnemySpawnPoint(x)[0], Quaternion.identity);
            ThisBoss.GetComponent<Boss_HealthManager>().SetHealthBar(BossHealthBar, SetMultipiler(statMultipler, isStrongerEachWave));
        }
        else
        {
            ThisBoss = Instantiate(Level.WaveList[waveNumber-1].ObjectsForThisWave[0], GenerateEnemySpawnPoint(x)[0], Quaternion.identity);
            ThisBoss.GetComponent<Boss_HealthManager>().SetHealthBar(BossHealthBar, SetMultipiler(Level.statMultipler, Level.isStrongerEachWave));
        }        
        ThisBoss.GetComponent<Boss_SpawnAttr>().SetPath(GroupEnemies.transform.GetChild(12).position);    
        ThisBoss.GetComponent<Boss_SpawnAttr>().MainTarget = Player;
        for(int j = 0; j < GroupEnemies.transform.childCount; j++)
        {
            ThisBoss.GetComponent<Boss_SpawnAttr>().MovePoints.Add(GroupEnemies.transform.GetChild(j).position);
        }
    }

    public void SpawnEnemies()
    {
        if(!usingCustomLevel)
        {    
            int i = GenerateEnemyShape();
            int j = GenerateEnemyPath();

            List<Vector3> spawnPos = GenerateEnemySpawnPoint(enemyPaths[j].firstPoint);
            List<Vector3> middelPoints = GeneratePathPoints(enemyPaths[j].middlePointIndex);

            GroupEnemies.transform.GetComponent<GroupEnemy>().SpawnEnemies(
                enemiesShapes[i].ID_of_Pos, 
                spawnPos, 
                middelPoints, 
                enemyPaths[j].time,
                SetMultipiler(statMultipler, isStrongerEachWave));
        }
        else
        {
            GroupEnemies.GetComponent<GroupEnemy>().SetEnemyPrefabs(Level.WaveList[waveNumber-1].ObjectsForThisWave);
            int i = GenerateEnemyShape(Level.WaveList[waveNumber-1].enemiesShapes);
            int j = GenerateEnemyPath(Level.WaveList[waveNumber-1].enemyPathDatas);

            List<Vector3> spawnPos = GenerateEnemySpawnPoint(
                Level.WaveList[waveNumber-1].enemyPathDatas[j].firstPoint);
            List<Vector3> middelPoints = GeneratePathPoints(
                Level.WaveList[waveNumber-1].enemyPathDatas[j].middlePointIndex);

            GroupEnemies.transform.GetComponent<GroupEnemy>().SpawnEnemies(
                Level.WaveList[waveNumber-1].enemiesShapes[i].ID_of_Pos, 
                spawnPos, 
                middelPoints, 
                Level.WaveList[waveNumber-1].enemyPathDatas[j].time, 
                SetMultipiler(Level.statMultipler, Level.isStrongerEachWave));
        }
    }

    public void WaveManager(int WaveType)
    {
        switch(WaveType)
        {
            case 0:
                //Debug.Log("Boss Time !!!");
                GroupEnemies.transform.GetComponent<GroupEnemy>().canMove = false;
                GroupEnemies.transform.GetComponent<GroupEnemy>().ResetPos();
                SpawnBoss();
                break;
            case 1:
                GroupEnemies.transform.GetComponent<GroupEnemy>().canMove = false;
                SpawnEnemies();
                break;
            case 2:
                //Debug.Log("Obstacle Confirmed");
                GroupEnemies.transform.GetComponent<GroupEnemy>().canMove = false;
                GroupEnemies.transform.GetComponent<GroupEnemy>().ResetPos();
                CreateObstacles();
                break;
        }
        startNewWave = false;
    }

    public void SetWaveUI(int wave)
    {
        string MaxWave = (isSurvivalMode && !usingCustomLevel)? "" : "/" + MaxwaveNumber;
        UI_Canvas.transform.GetChild(1).GetComponent<TMP_Text>().text = "Wave: " + wave + MaxWave;
    }

    IEnumerator GetNewWave()
    {
        if(waveNumber >= MaxwaveNumber && (!isSurvivalMode || usingCustomLevel))
        {
            GameOver(true);
        }
        else
        {
            startNewWave = true;
            int WaveType = 0;
            SetWaveUI(++waveNumber);

            if(!usingCustomLevel)
            {
                Debug.Log(waveNumber % BossEveryWave);
                float i = UnityEngine.Random.Range(0, 100f);
                if(waveNumber % BossEveryWave == 0 || (waveNumber == MaxwaveNumber && !isSurvivalMode))
                {
                    WaveType = 0;
                    UI_Hubs.transform.GetChild(6).GetChild(0).GetComponent<TMP_Text>().text = "Boss Incomming";
                }
                else if(waveNumber % BossEveryWave != 0 || (waveNumber != MaxwaveNumber && !isSurvivalMode))
                { 
                    if(i <= EnemyWaveRate)
                    {
                        WaveType = 1;
                        UI_Hubs.transform.GetChild(6).GetChild(0).GetComponent<TMP_Text>().text = "Wave " + waveNumber + "\nDestroy Enemies";
                    }
                    else if(i > EnemyWaveRate)
                    {
                        WaveType = 2;
                        UI_Hubs.transform.GetChild(6).GetChild(0).GetComponent<TMP_Text>().text = "Wave " + waveNumber + "\nBeware Meteorites";
                        Debug.Log("Obstacle Wave");
                    }
                }
            }
            else
            {
                switch(Level.WaveList[waveNumber-1].ObjectsForThisWave[0].tag)
                {
                    case "Meteorite":
                        WaveType = 2;
                        UI_Hubs.transform.GetChild(6).GetChild(0).GetComponent<TMP_Text>().text = "Wave " + waveNumber + "\nBeware Meteorites";
                        Debug.Log("Obstacle Wave");
                        break;
                    case "Enemy":
                        WaveType = 1;
                        UI_Hubs.transform.GetChild(6).GetChild(0).GetComponent<TMP_Text>().text = "Wave " + waveNumber + "\nDestroy Enemies";
                        break;
                    case "Boss":
                        WaveType = 0;
                        UI_Hubs.transform.GetChild(6).GetChild(0).GetComponent<TMP_Text>().text = "Boss Incomming";
                        Debug.Log("Boss Time !!!");
                        break;
                }
            } 

            UI_Hubs.transform.GetChild(6).DOScale(new Vector3(2,2,2), 0.5f).OnComplete(()=>
            {
                
            }).SetLink(gameObject);
            yield return new WaitForSeconds(2f);
            UI_Hubs.transform.GetChild(6).DOScale(new Vector3(0,0,0), 0.01f).OnComplete(()=>
            {   
                WaveManager(WaveType); 
            }).SetLink(gameObject);           
        }        
    }

    public void GameOver(bool isWin = false)
    {
        UI_Hubs.transform.GetChild(6).GetChild(0).GetComponent<TMP_Text>().text = isWin? "You Win" : "You Lose";
        UI_Hubs.transform.GetChild(6).DOScale(new Vector3(2,2,2), 0.5f).OnComplete(()=>
        {
            GroupEnemies.SetActive(false);
            Obstacles.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Player.GetComponent<PlayerController>().allowPlayerControl = false;
            UI_Hubs.transform.GetChild(6).GetChild(0).GetComponent<TMP_Text>().text = isWin? "You Win" : "You Lose";
            Time.timeScale = 0;
        }).SetLink(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(GroupEnemies.transform.GetComponent<GroupEnemy>().allEnemiesDestroyed && CurMeteo <= 0 && isBossExist == false && !startNewWave)
        {
            if(beginMatch)
                StartCoroutine(GetNewWave());         
        }

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        UI_Canvas.transform.GetChild(2).GetComponent<TMP_Text>().text = "FPS: " + 1/deltaTime;
    }
}
