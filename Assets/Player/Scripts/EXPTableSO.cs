using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EXPTable", menuName = "ScriptableObjects/EXPTable")]
public class EXPTableSO : ScriptableObject
{
    public List<int> levels = null;
}
