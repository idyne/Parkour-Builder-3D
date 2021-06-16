using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Text>().text = "LEVEL " + Singleton.GM.level;
    }
}
