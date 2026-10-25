using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void ChangeText(string x)
    {
        text.text = x;
    }
}
