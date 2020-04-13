using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Com.LuisPedroFonseca.ProCamera2D;

public class GameManager : MonoBehaviour
{

    int levelNum;

    public GameObject characterPrefab, messageGO;

    CharacterController character;

    public ProCamera2D mainCam;

    public AudioClip powerUpSound, levelEndSound, levelStartSound, dedSound;

    public bool characterSpawned, flavorTextCRRunning = false;
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
            mainCam = GameObject.FindObjectOfType<ProCamera2D>();
            DontDestroyOnLoad(mainCam.gameObject);
        }
        //if there is already an instance, destroy this object, don't use it!
        else
        {
            Destroy(messageGO.transform.parent.gameObject);
            Destroy(gameObject);
            //also destroy other camera
            foreach (ProCamera2D cam in GameObject.FindObjectsOfType<ProCamera2D>())
                if (cam != mainCam) Destroy(cam.gameObject);
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
        //reset global lift movement loop timer
        LiftPiece.loopTime = 0f;

        Debug.Log("SceneLoaded:" + scene.name);
        //if(!characterSpawned)
        if(scene.name.Contains("level"))
            SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        //do we need this? -> mainCam = GameObject.FindObjectOfType<ProCamera2D>();
        characterSpawned = true;
        Debug.Log("In Spawn code");
        Debug.Log("level:" + SceneManager.GetActiveScene().name);
        //spawn tile is tile tagged "spawn"
        GameObject spawnTile = GameObject.FindGameObjectWithTag("spawn");
        //instantiate player object at spawn tile
        GameObject characterGO = Instantiate(characterPrefab, spawnTile.transform.position + new Vector3(0f,.5f,0f), Quaternion.identity);
        character = characterGO.GetComponent<CharacterController>();
        mainCam.RemoveAllCameraTargets();
        mainCam.AddCameraTarget(character.transform);
        mainCam.Reset();
        ProCamera2DCinematics cinematic = mainCam.GetComponent<ProCamera2DCinematics>();
        cinematic.OnCinematicFinished.RemoveAllListeners();
        PlaySound(levelStartSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        if (levelNum != 9)
        {
            levelNum += 1;
            SceneManager.LoadScene("level" + levelNum);
        }
        else
        {
            //last level
            levelNum = 0;
            Instance.characterSpawned = false;
            Destroy(GameObject.FindObjectOfType<CameraFollow>().gameObject);
            SceneManager.LoadScene("demothankyou");
        }
    }

    public void BackToMap()
    {
        SceneManager.LoadScene("worldmap");
    }

    public IEnumerator ShowFlavorText(string flavorText)
    {
        //if another instance of this flavor text routine is running, wait
        while (flavorTextCRRunning) yield return null;

        //now start this one
        flavorTextCRRunning = true;
        //always return to this text as the base text
        string returnToText = "You were something!\n\n\n\nNow you are nothing";
        Instance.messageGO.GetComponent<TextMeshProUGUI>().text = flavorText;
        Instance.messageGO.SetActive(true);
        yield return new WaitForSeconds(2f);
        Instance.messageGO.SetActive(false);
        Instance.messageGO.GetComponent<TextMeshProUGUI>().text = returnToText;
        //now done, set flag back
        flavorTextCRRunning = false;

    }

    public void DeltaGain(Delta d)
    {
        Debug.Log("Found a delta");
        d.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + 1.5f, 0f);
        d.isFloating = false;
        character.moveDisabled = true;
        DeltaCinematic(d);
    }

    public void DeltaCinematic(Delta d)
    {
        ProCamera2DCinematics cinematic = mainCam.GetComponent<ProCamera2DCinematics>();
        cinematic.AddCinematicTarget(d.transform);
        Debug.Log("delta objct? " +d);
        Debug.Log("delta gameobj " + d.gameObject);
        cinematic.OnCinematicFinished.AddListener(delegate { Debug.Log("d? "+d); DestroyObject(d.gameObject); });
        cinematic.OnCinematicFinished.AddListener(EnablePlayer);
        cinematic.OnCinematicFinished.AddListener(RemoveCinematicEventListeners);
        //this is for parallax sub cameras
        /*foreach (PixelPerfectCamera cam in mainProCam.GetComponentsInChildren<PixelPerfectCamera>())
        {

            cam.enabled = false;
        }*/
        cinematic.Play();
    }

    public void EnablePlayer()
    {
        Debug.Log("enable player second?");
        character.moveDisabled = false;
    }

    public void DestroyObject(GameObject o)
    {
        Debug.Log("destroying object, first?");
        Destroy(o);
    }

    public void RemoveCinematicEventListeners()
    {
        Debug.Log("remove listeners third?");
        ProCamera2DCinematics cinematic = mainCam.GetComponent<ProCamera2DCinematics>();
        cinematic.OnCinematicFinished.RemoveAllListeners();
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
