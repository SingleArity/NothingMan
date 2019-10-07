using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class See : Ability
{


    private void Awake()
    {
        GameObject.FindObjectOfType<MapController>().EnableFullSight();    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleAbility()
    {
        
    }

    public override void HandleAnimation()
    {


    }
}
