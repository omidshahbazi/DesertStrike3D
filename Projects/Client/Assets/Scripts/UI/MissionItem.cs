using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionItem : MonoBehaviour
{
    public GameObject checkBox;
    public Text description;

    public void SetData(string Text, bool isComplete)
    {
        description.text = Text;
        checkBox.SetActive(isComplete);
    }
}
