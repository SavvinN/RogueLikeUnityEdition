using rogueLike;
using rogueLike.GameObjects;
using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityManager;

public class UnityManager : MonoBehaviour
{
    private Game currentGame;
    private rogueLike.GameObjects.GameObject[,] gridToRender;
    public World myWorld;
    private readonly float _entityHeight = 0;
    private readonly float _groundHeight = -0.495f;
    private UnityEngine.GameObject _currentPlayer;
    private UnityEngine.GameObject[] _zombies;
    private UnityEngine.GameObject[] _archers;
    private List<UnityArrow> _arrows;

    [SerializeField] private float _playerVelocity = 8;
    [SerializeField] private float _enemyVelocity = 4;
    [SerializeField] private float _cameraVelocity = 20;
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
        _arrows = new();
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

            currentGame.HandleEnemyBehavior(zombies, archers);
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
            if (rogueLike.World.CompareObjects(obj, new Wall()))
            {
                AddWorldObject(obj, Wall, _entityHeight);
            }
            else
            {
                AddWorldObject(obj, Ground, _groundHeight);
            }
        }
        InitializeEntity(currentGame.MyWorld.GetPlayer(), currentGame.MyWorld.GetZombies(), currentGame.MyWorld.GetArchers());
    }

    public void AddWorldObject(rogueLike.GameObjects.GameObject obj, UnityEngine.GameObject uObject, float height)
    {
        var spawnedObj = Instantiate(uObject);
        spawnedObj.transform.position = new Vector3(obj.Position.X, height, obj.Position.Y);
        spawnedObj.transform.parent = World.transform;
    }

    public void DestroyWorld()
    {
        for (var i = World.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(World.transform.GetChild(i).gameObject);
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
            Time.deltaTime * _cameraVelocity);

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
        List<UnityArrow> arrowsToRemove = new();

        TryAddArrow(arrows);

        foreach (var arrow in _arrows)
        {
            if (!arrows.Contains(arrow.rArrow))
            {
                arrow.DestroyMash();
                arrowsToRemove.Add(arrow);
            }
        }

        RemoveDestroyedArrows(arrowsToRemove);

        UpdateArrowsMashPosition();
    }

    public void RemoveDestroyedArrows(List<UnityArrow> arrows)
    {
        foreach (var arrow in arrows)
        {
            _arrows.Remove(arrow);
        }
    }

    public void UpdateArrowsMashPosition()
    {
        for (int i = 0; i < _arrows.Count; i++)
        {
            _arrows[i].mash.transform.position = Vector3.MoveTowards(_arrows[i].mash.transform.position,
                new Vector3(_arrows[i].rArrow.Position.X, _entityHeight, _arrows[i].rArrow.Position.Y),
                Time.deltaTime * _arrowVelocity);
        }
    }

    public void TryAddArrow(List<Arrow> arrows)
    {
        while (arrows.Count > _arrows.Count)
        {
            foreach (var arrow in arrows)
            {
                if (!CheckToContentsArrow(arrow))
                    _arrows.Add(new UnityArrow(arrow, Arrow, _entityHeight));
            }
        }
    }

    public bool CheckToContentsArrow(Arrow arrow)
    {
        foreach (var arr in _arrows)
        {
            if (arrow == arr.rArrow)
                return true;
        }
        return false;
    }

    public struct UnityArrow
    {
        public Arrow rArrow;
        public UnityEngine.GameObject mash;

        public UnityArrow(Arrow arrow, UnityEngine.GameObject obj, float height)
        {
            rArrow = arrow;
            mash = Instantiate(obj);
            mash.transform.position = new Vector3(arrow.Position.X, height, arrow.Position.Y);
        }

        public void DestroyMash()
        {
            Destroy(mash);
        }
    }
}
