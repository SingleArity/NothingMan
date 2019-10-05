using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected CharacterController character;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacter(CharacterController cc)
    {
        character = cc;
    }

    public abstract void HandleAbility();

    public void HandleAnimation()
    {

    }
}
