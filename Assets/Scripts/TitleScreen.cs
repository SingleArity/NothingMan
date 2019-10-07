using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
                    Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Animator>().SetBool("Bloop", true);
            }
        }else if(state == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
                    Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Animator>().SetBool("Prompt", true);
                SetState(2);
                StartCoroutine(PlayGame());
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
}
