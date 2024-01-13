using rogueLike;
using rogueLike.GameObjects;
using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UnityManager : MonoBehaviour
{
    private Game currentGame;
    private rogueLike.GameObjects.GameObject[,] gridToRender;
    public World myWorld;
    private int _entityHeight = 0;
    private UnityEngine.GameObject _currentPlayer;
    private UnityEngine.GameObject[] _zombies;
    private UnityEngine.GameObject[] _archers;
    private List<UnityEngine.GameObject> _arrows;

    [SerializeField] private float _playerVelocity = 8;
    [SerializeField] private float _enemyVelocity = 4;
    [SerializeField] private float _cameraVelociy = 20;
    [SerializeField] private float _arrowVelocity = 9;
    [SerializeField] private int _cameraHeight = 20;

    [Space]
    public UnityEngine.GameObject World;
    public UnityEngine.GameObject Camera;
    public TextMeshProUGUI LifeCountText;
    public TextMeshProUGUI LevelCountText;
    public Canvas GameOverCanvas;

    [Space]
    [Header("Префабы")]
    public UnityEngine.GameObject Wall;
    public UnityEngine.GameObject Ground;
    public UnityEngine.GameObject Zombie;
    public UnityEngine.GameObject Archer;
    public UnityEngine.GameObject Player;
    public UnityEngine.GameObject Arrow;
    public ParticleSystem HitParticle;

    private void Awake()
    {
        currentGame = new Game();
    }

    void Start()
    {
        PlayerController._myGame = currentGame;
        _arrows = new List<UnityEngine.GameObject>();
        _currentPlayer = Instantiate(Player);
        HitParticle = Instantiate(HitParticle);
        PlayerController._attackParticle = HitParticle;
        UpdateWorld();
    }

    void Update()
    {
        if (!currentGame.IsGameOver)
        {
            if (currentGame.IsGoal())
            {
                DestroyWorld();
                UpdateWorld();
            }

            var currentPlayer = currentGame.MyWorld.GetPlayer();
            var zombies = currentGame.MyWorld.GetZombies();
            var archers = currentGame.MyWorld.GetArchers();
            var arrows = currentGame.MyWorld.GetAllArrows();

            currentGame.HandleEnemyAction(zombies, archers);

            currentGame.HandleArrowsAction();

            UpdateEntityAction(currentPlayer, zombies, archers, arrows);

            DestroyEnemyPrefabs(_zombies, zombies);
            DestroyEnemyPrefabs(_archers, archers);

            currentGame.frameCount += Time.deltaTime * 100;
        }
        else
        {
            DestroyWorld();
            Destroy(_currentPlayer);
            GameOverCanvas.gameObject.SetActive(true);
        }
    }

    public void DestroyEnemyPrefabs(UnityEngine.GameObject[] enemysPrefabs, Enemy[] enemys)
    {
        for (int i = 0; i < enemysPrefabs.Length; i++)
        {
            TryToDestroyEntity(enemysPrefabs[i], enemys[i]);
        }
    }

    public void UpdateWorld()
    {

        LifeCountText.text = "Life " + currentGame.life.ToString();
        LevelCountText.text = "Level " + currentGame.level.ToString();
        gridToRender = currentGame.MyWorld.GetGameObjectGrid();

        foreach (var obj in gridToRender)
        {
            if (obj.GetType() == new Wall().GetType())
            {
                var spawnedObj = Instantiate(Wall);
                spawnedObj.transform.position = new Vector3(obj.Position.X, _entityHeight, obj.Position.Y);
                spawnedObj.transform.parent = World.transform;
            }
            else
            {
                var spawnedObj = Instantiate(Ground);
                spawnedObj.transform.position = new Vector3(obj.Position.X, -0.495f, obj.Position.Y);
                spawnedObj.transform.parent = World.transform;
            }
        }
        InitializeEntity(currentGame.MyWorld.GetPlayer(), currentGame.MyWorld.GetZombies(), currentGame.MyWorld.GetArchers());
    }

    public void DestroyWorld()
    {
        for (var i = World.transform.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(World.transform.GetChild(i).gameObject);
        }
    }

    public void TryToDestroyEntity(UnityEngine.GameObject unityObj, rogueLike.GameObjects.GameObject gameObj)
    {
        if (gameObj.Position == rogueLike.Vector2.Zero)
            unityObj.SetActive(false);
    }

    public void InitializeEntity(Player player, Zombie[] zombies, Archer[] archers)
    {
        _currentPlayer.transform.position = new Vector3(player.Position.X, _entityHeight, player.Position.Y);

        Camera.transform.position = new Vector3(player.Position.X, _cameraHeight, player.Position.Y);

        _zombies = new UnityEngine.GameObject[zombies.Length];
        _archers = new UnityEngine.GameObject[archers.Length];

        InitializeEnemy(_zombies, zombies, Zombie);
        InitializeEnemy(_archers, archers, Archer);
    }

    public void InitializeEnemy(UnityEngine.GameObject[] enemysPrefabs, Enemy[] enemys, UnityEngine.GameObject enemyType)
    {
            for (int i = 0; i < enemys.Length; i++)
            {
                enemysPrefabs[i] = Instantiate(enemyType);
                enemysPrefabs[i].transform.position = new Vector3(enemys[i].Position.X, _entityHeight, enemys[i].Position.Y);
                enemysPrefabs[i].transform.parent = World.transform;
            }
    }

    public void UpdateEntityAction(Player player, Zombie[] zombies, Archer[] archers, List<Arrow> arrows)
    {
        _currentPlayer.transform.position = Vector3.MoveTowards(_currentPlayer.transform.position,
                new Vector3(player.Position.X, _entityHeight, player.Position.Y),
                Time.deltaTime * _playerVelocity);

        Camera.transform.position = Vector3.Lerp(Camera.transform.position, 
            _currentPlayer.transform.position + new Vector3(0, _cameraHeight, 0), 
            Time.deltaTime * _cameraVelociy);
        
        UpdateEnemy(_zombies, zombies);
        UpdateEnemy(_archers, archers);
        UpdateArrows(arrows);
    }

    public void UpdateEnemy(UnityEngine.GameObject[] enemyPrefabs, Enemy[] enemy)
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            enemyPrefabs[i].transform.position = Vector3.MoveTowards(enemyPrefabs[i].transform.position,
                new Vector3(enemy[i].Position.X, _entityHeight, enemy[i].Position.Y),
                Time.deltaTime * _enemyVelocity);
        }
    }

    public void UpdateArrows(List<Arrow> arrows)
    {
        while (arrows.Count > _arrows.Count)
        {
            var arrow = Instantiate(Arrow);
            _arrows.Add(arrow);
            arrow.transform.position = new Vector3(arrows.Last().Position.X, _entityHeight, arrows.Last().Position.Y);
        }

        while (arrows.Count < _arrows.Count)
        {
            Destroy(_arrows[0]);
            _arrows.RemoveAt(0);
        }

        if (arrows.Count == _arrows.Count)
        {
            for (int i = 0; i < arrows.Count; i++)
            {
                _arrows[i].transform.position = Vector3.MoveTowards(_arrows[i].transform.position,
                    new Vector3(arrows[i].Position.X, _entityHeight, arrows[i].Position.Y),
                    Time.deltaTime * _arrowVelocity);
            }
        }
    }

}
