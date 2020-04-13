using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapController : MonoBehaviour
{

    public Transform playerAvatar;
    public WorldMapLevel currentLevel;

    List<MapLevelDirectionPair> canMoveTo;

    bool takingInput;

    // Start is called before the first frame update
    void Start()
    {
        takingInput = true;
        canMoveTo = currentLevel.neighbors;
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
