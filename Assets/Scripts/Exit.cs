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
            player.Exit(transform);
            StartCoroutine(ExitLevel());
        }
    }

    public IEnumerator ExitLevel()
    {
        DisplayMessage();
        gm.PlaySound(gm.levelEndSound);
        
        while(!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Space))
            yield return null;
        HideMessage();
        if (Input.GetKeyDown(KeyCode.Return))
            gm.NextLevel();
        else if (Input.GetKeyDown(KeyCode.Space))
            gm.BackToMap();
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
