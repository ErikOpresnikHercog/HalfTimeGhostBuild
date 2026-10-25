using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    [Multiline(4)]
    public List<string> DialogueStrings = new List<string>();
    
}
