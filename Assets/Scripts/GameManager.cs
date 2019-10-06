using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    int levelNum;

    public GameObject characterPrefab, messageGO;

    CharacterController character;

    public CameraFollow mainCam;

    public static GameManager Instance
    {
        get; 
        set;
    }

    // Start is called before the first frame update
    void Awake()
    {
        //if no instance is set yet, this one can be it
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(messageGO.transform.parent);
        }
        //if there is already an instance, destroy this object, don't use it!
        else
        {
            Destroy(gameObject);
        }
        levelNum = 0;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneLoaded:" + scene.name);
        SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        Debug.Log("In Spawn code");
        //spawn tile is tile tagged "spawn"
        GameObject spawnTile = GameObject.FindGameObjectWithTag("spawn");
        //instantiate player object at spawn tile
        GameObject characterGO = Instantiate(characterPrefab, spawnTile.transform.position, Quaternion.identity);
        character = characterGO.GetComponent<CharacterController>();
        mainCam.target = characterGO.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        levelNum += 1;
        SceneManager.LoadScene("level" + levelNum);
    }

    public IEnumerator ShowFlavorText(string flavorText)
    {
        string currentMessageText = messageGO.GetComponent<TextMeshProUGUI>().text;
        messageGO.GetComponent<TextMeshProUGUI>().text = flavorText;
        messageGO.SetActive(true);
        yield return new WaitForSeconds(2f);
        messageGO.SetActive(false);
        messageGO.GetComponent<TextMeshProUGUI>().text = currentMessageText;

    }


}
