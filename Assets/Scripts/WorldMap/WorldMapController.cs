using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Com.LuisPedroFonseca.ProCamera2D;

public class WorldMapController : MonoBehaviour
{
    Gamepad gamepad;
    
    public Transform playerAvatar;
    public WorldMapLevel currentLevel;

    List<MapLevelDirectionPair> canMoveTo;

    bool takingInput;

    //used for camera offset calculations
    float xMargin, yMargin;
    ProCamera2D camera;

    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;
        takingInput = true;
        canMoveTo = currentLevel.neighbors;
        //center of screen is 12.5 x 9.375 world units in
        //because the map is at 0, 0 these are our margins
        xMargin = 12.5f;
        yMargin = 9.375f;
        camera = GameObject.FindObjectOfType<ProCamera2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (takingInput) {
            if (Input.GetKeyDown(KeyCode.UpArrow) && CanMove(Direction.UP))
                StartCoroutine(Move(Direction.UP));
            if (Input.GetKeyDown(KeyCode.DownArrow) && CanMove(Direction.DOWN))
                StartCoroutine(Move(Direction.DOWN));
            if (Input.GetKeyDown(KeyCode.LeftArrow) && CanMove(Direction.LEFT))
                StartCoroutine(Move(Direction.LEFT));
            if (Input.GetKeyDown(KeyCode.RightArrow) && CanMove(Direction.RIGHT))
                StartCoroutine(Move(Direction.RIGHT));

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                EnterLevel(currentLevel);
        }

        //camera offset
        //left of margin
        if(playerAvatar.position.x < (-1 * xMargin))
        {
            camera.OffsetX = (-1 * xMargin) - playerAvatar.position.x;
        }
        //right of margin
        else if(playerAvatar.position.x > xMargin)
        {
            camera.OffsetX = xMargin - playerAvatar.position.x;
        }
        else
        {
            camera.OffsetX = 0f;
        }
        //TODO y margin offset
    }

    public void AddLevel(WorldMapLevel l, Direction d)
    {
        canMoveTo.Add(new MapLevelDirectionPair(l, d));
    }

    //true if we have the ability to move in direction d
    bool CanMove(Direction d)
    {
        return canMoveTo.Exists(x => x.direction == d);
    }

    IEnumerator Move(Direction dir)
    {
        Debug.Log("moving!" + dir);
        takingInput = false;
        WorldMapLevel toMoveTo = canMoveTo.Find(x => x.direction == dir).level;
        //tween the player object
        Vector3 deltaPos = toMoveTo.transform.position - playerAvatar.transform.position;
        float distanceToMove = deltaPos.magnitude;
        LeanTween.move(playerAvatar.gameObject, toMoveTo.transform.position, (distanceToMove / 10f));
        while (playerAvatar.position != toMoveTo.transform.position)
        {        
            yield return null;
        }
        //done moving
        Debug.Log("Done moving! " + dir);
        currentLevel = toMoveTo;
        canMoveTo = currentLevel.neighbors;
        takingInput = true;

    }

    void EnterLevel(WorldMapLevel lvl)
    {
        SceneManager.LoadScene(lvl.sceneName);
    }

    public void AddGamepad(InputDevice d)
    {
        gamepad = Gamepad.current;
    }
}

[System.Serializable]
public class MapLevelDirectionPair
{
    public MapLevelDirectionPair(WorldMapLevel l, Direction d)
    {
        level = l;
        direction = d;
    }

    [SerializeField]
    public WorldMapLevel level;
    [SerializeField]
    public Direction direction;
}

public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
