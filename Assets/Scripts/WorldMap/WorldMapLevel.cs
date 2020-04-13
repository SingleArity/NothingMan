using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldMapLevel : MonoBehaviour
{
    //the scene associated with this level
    public string sceneName;

    [SerializeField]
    public List<MapLevelDirectionPair> neighbors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
