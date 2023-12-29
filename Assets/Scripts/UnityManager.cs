using rogueLike;
using rogueLike.GameObjects;
using rogueLike.GameObjects.Enemys;
using rogueLike.GameObjects.MazeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.WebCam;

public class UnityManager : MonoBehaviour
{
    private Game currentGame;
    private rogueLike.GameObjects.GameObject[,] gridToRender;
    public World myWorld;

    [SerializeField] private float _playerVelocity = 8;
    [SerializeField] private float _enemyVelocity = 4;
    [SerializeField] private float _cameraVelociy = 20;
    [SerializeField] private float _arrowVelocity = 9;
    [SerializeField] private int _cameraHeight = 20;

    private int _entityHeight = 0;
    private UnityEngine.GameObject _currentPlayer;
    private UnityEngine.GameObject[] _zombies;
    private UnityEngine.GameObject[] _archers;
    private List<UnityEngine.GameObject> _arrows;

    [Space]
    public UnityEngine.GameObject World;
    public UnityEngine.GameObject Camera;
    public TextMeshProUGUI LifeCountText;
    public TextMeshProUGUI LevelCountText;
    public TextMeshProUGUI YouDiedText;

    [Space]
    [Header("Префабы")]
    public UnityEngine.GameObject Wall;
    public UnityEngine.GameObject Ground;
    public UnityEngine.GameObject Zombie;
    public UnityEngine.GameObject Archer;
    public UnityEngine.GameObject Player;
    public UnityEngine.GameObject Arrow;


    // Start is called before the first frame update
    void Start()
    {
        currentGame = new Game();
        _arrows = new List<UnityEngine.GameObject>();
        _currentPlayer = Instantiate(Player);
        UpdateWorld();
    }

    // Update is called once per frame
    void Update()
    {
        var currentPlayer = currentGame.MyWorld.GetPlayer();
        var zombies = currentGame.MyWorld.GetZombies();
        var archers = currentGame.MyWorld.GetArchers();
        var arrows = currentGame.MyWorld.GetAllArrows();
        
        currentGame.HandleEnemyAction(zombies, archers);
        currentGame.HandleMoveInput(currentPlayer, CatchInput());
        currentGame.HandleAttackInput(currentPlayer, CatchInput());
        currentGame.HandleArrowsAction();
        
        UpdateEntityAction(currentPlayer, zombies, archers, arrows);

        for (int i = 0; i < _zombies.Length; i++)
        {
            TryToDestroyEntity(_zombies[i], zombies[i]);
        }

        for (int i = 0; i < _archers.Length; i++)
        {
            TryToDestroyEntity(_archers[i], archers[i]);
        }

        if (currentGame.IsGoal())
        {
            DestroyWorld();
            UpdateWorld();
        }

        
        currentGame.FrameCount += Time.deltaTime * 100;
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
        if (gameObj.Position == System.Numerics.Vector2.Zero)
            unityObj.SetActive(false);
    }

    public void InitializeEntity(Player player, Zombie[] zombies, Archer[] archers)
    {
        _currentPlayer.transform.position = new Vector3(player.Position.X, _entityHeight, player.Position.Y);

        Camera.transform.position = new Vector3(player.Position.X, _cameraHeight, player.Position.Y);


        _zombies = new UnityEngine.GameObject[zombies.Length];
        _archers = new UnityEngine.GameObject[archers.Length];

        for (int i = 0; i < zombies.Length; i++)
        {
            _zombies[i] = Instantiate(Zombie);
            _zombies[i].transform.position = new Vector3(zombies[i].Position.X, _entityHeight, zombies[i].Position.Y);
            _zombies[i].transform.parent = World.transform;
        }

        for (int i = 0; i < archers.Length; i++)
        {
            _archers[i] = Instantiate(Archer);
            _archers[i].transform.position = new Vector3(archers[i].Position.X, _entityHeight, archers[i].Position.Y);
            _archers[i].transform.parent = World.transform;
        }

    }

    public void UpdateEntityAction(Player player, Zombie[] zombies, Archer[] archers, List<Arrow> arrows)
    {
        _currentPlayer.transform.position = Vector3.MoveTowards(_currentPlayer.transform.position,
                new Vector3(player.Position.X, _entityHeight, player.Position.Y),
                Time.deltaTime * _playerVelocity);

        Camera.transform.position = Vector3.Lerp(Camera.transform.position, _currentPlayer.transform.position + new Vector3(0, _cameraHeight, 0), Time.deltaTime * _cameraVelociy);

        for (int i = 0; i < zombies.Length; i++)
        {
            _zombies[i].transform.position = Vector3.MoveTowards(_zombies[i].transform.position,
                new Vector3(zombies[i].Position.X,_entityHeight, zombies[i].Position.Y),
                Time.deltaTime * _enemyVelocity);
        }

        for (int i = 0; i < archers.Length; i++)
        {
            _archers[i].transform.position = Vector3.MoveTowards(_archers[i].transform.position,
                new Vector3(archers[i].Position.X, _entityHeight, archers[i].Position.Y),
                Time.deltaTime * _enemyVelocity);
        }

        while(arrows.Count > _arrows.Count)
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
            for(int i = 0; i < arrows.Count; i++)
            {
                _arrows[i].transform.position = Vector3.MoveTowards(_arrows[i].transform.position,
                    new Vector3 (arrows[i].Position.X,_entityHeight, arrows[i].Position.Y),
                    Time.deltaTime * _arrowVelocity);
            }
        }
    }

    ConsoleKey CatchInput()
    {
        ConsoleKey moveKey = ConsoleKey.Spacebar;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveKey = ConsoleKey.UpArrow;
        }
        else
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveKey = ConsoleKey.DownArrow;
        }
        else
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveKey = ConsoleKey.RightArrow;
        }
        else
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveKey = ConsoleKey.LeftArrow;
        }
        else
            if (Input.GetKey(KeyCode.W))
        {
            moveKey = ConsoleKey.W;
        }
        else
        if (Input.GetKey(KeyCode.S))
        {
            moveKey = ConsoleKey.S;
        }
        else
        if (Input.GetKey(KeyCode.D))
        {
            moveKey = ConsoleKey.D;
        }
        else
        if (Input.GetKey(KeyCode.A))
        {
            moveKey = ConsoleKey.A;
        }

        return moveKey;
    }
}
