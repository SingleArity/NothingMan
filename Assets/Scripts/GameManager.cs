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

    public AudioClip powerUpSound, levelEndSound, levelStartSound, dedSound;

    public bool characterSpawned;
    public static bool alreadyEnabled = false;

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
        if (!alreadyEnabled)
        {
            alreadyEnabled = true;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneLoaded:" + scene.name);
        //if(!characterSpawned)
            SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        mainCam = GameObject.FindObjectOfType<CameraFollow>();
        characterSpawned = true;
        Debug.Log("In Spawn code");
        //spawn tile is tile tagged "spawn"
        GameObject spawnTile = GameObject.FindGameObjectWithTag("spawn");
        //instantiate player object at spawn tile
        GameObject characterGO = Instantiate(characterPrefab, spawnTile.transform.position, Quaternion.identity);
        character = characterGO.GetComponent<CharacterController>();
        mainCam.target = characterGO.transform;
        PlaySound(levelStartSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        if (levelNum != 6)
        {
            levelNum += 1;
            SceneManager.LoadScene("level" + levelNum);
        }
        else
        {
            //last level
            levelNum = 0;
            Instance.characterSpawned = false;
            SceneManager.LoadScene("title");
        }
    }

    public IEnumerator ShowFlavorText(string flavorText)
    {
        string currentMessageText = messageGO.GetComponent<TextMeshProUGUI>().text;
        Instance.messageGO.GetComponent<TextMeshProUGUI>().text = flavorText;
        Instance.messageGO.SetActive(true);
        yield return new WaitForSeconds(2f);
        Instance.messageGO.SetActive(false);
        Instance.messageGO.GetComponent<TextMeshProUGUI>().text = currentMessageText;

    }

    //audio effects
    public void PlaySound(AudioClip sound)
    {
        //StartCoroutine(PlaySoundClip(sound));
        Instance.GetComponent<AudioSource>().clip = sound;
        Instance.GetComponent<AudioSource>().Play();
    }

    IEnumerator PlaySoundClip(AudioClip sound)
    {
        yield return null;
    }

    public void RestartLevel()
    {
        Debug.Log("GM RestartLevel");
        Instance.characterSpawned = false;
        SceneManager.LoadScene("level" + levelNum);
    }
}
