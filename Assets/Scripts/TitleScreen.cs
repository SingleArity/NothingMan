using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour
{

    public int state = 0;
    Gamepad gamepad;

    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;       
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(gamepad);
        if (state == 0)
        {
            if (gamepad != null)
            {
                if (Mathf.Abs(gamepad.leftStick.ReadValue().x) >= .7f || Mathf.Abs(gamepad.leftStick.ReadValue().y) >= .7f ||
                    gamepad.dpad.left.wasPressedThisFrame || gamepad.dpad.right.wasPressedThisFrame)
                //(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) ||
                // Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.LogError("left stick!");
                    GetComponent<Animator>().SetBool("Bloop", true);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) ||
                 Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.LogError("left stick!");
                    GetComponent<Animator>().SetBool("Bloop", true);
                }
            }
        }else if(state == 1)
        {
            if (gamepad != null)
            {
                if (gamepad.buttonSouth.wasPressedThisFrame)

                {
                    GetComponent<Animator>().SetBool("Prompt", true);
                    SetState(2);
                    StartCoroutine(PlayGame());
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GetComponent<Animator>().SetBool("Prompt", true);
                    SetState(2);
                    StartCoroutine(PlayGame());
                }
            }

        }
    }

    public void SetState(int newState)
    {
        state = newState;
    }

    IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("level0");
    }

    public void AddGamepad(InputDevice device)
    {
        gamepad = Gamepad.current;
    }
}
