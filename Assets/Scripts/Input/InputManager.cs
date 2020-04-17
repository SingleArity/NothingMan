using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{

    public static Gamepad gamepad;

    // Start is called before the first frame update
    void Start()
    {
    }

    public static void InitializeGamePad()
    {
        gamepad = Gamepad.current;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AddDevice(InputDevice d)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        gamepad = Gamepad.current;
        switch (sceneName)
        {
            case "title":
                Debug.Log("titlescreen gamepadd add");
                GameObject.FindObjectOfType<TitleScreen>().AddGamepad(d);
                break;
            case "worldmap":
                GameObject.FindObjectOfType<WorldMapController>().AddGamepad(d);
                break;
            case "demothankyou":
                break;
            default:
                GameObject.FindObjectOfType<CharacterController>().AddGamepad(d);
                break;
        }
    }

    public static void RemoveDevice(InputDevice d)
    {

    }
}
