using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionInputHandler : MonoBehaviour
{
    public void OnLeave()
    {
        Destroy(gameObject);
    }
}
