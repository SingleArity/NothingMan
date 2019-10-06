using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{

    GameManager gm;

    GameObject messageGO;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        messageGO = GameManager.Instance.messageGO;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController player = collision.gameObject.GetComponent<CharacterController>();
        if (player)
        {
            player.Exit();
            StartCoroutine(ExitLevel());
        }
    }

    public IEnumerator ExitLevel()
    {
        DisplayMessage();
        while(!Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            yield return null;
        HideMessage();
        gm.NextLevel();
    }

    public void DisplayMessage()
    {
        messageGO.SetActive(true);
    }

    public void HideMessage()
    {
        messageGO.SetActive(false);
    }
}
