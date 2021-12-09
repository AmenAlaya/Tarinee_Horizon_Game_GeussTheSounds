using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManagerScript : MonoBehaviour
{
    public Text textInfo;
    public GameObject textDocuInfo;

    // Start is called before the first frame update
    void Start()
    {
        textDocuInfo.SetActive(false);
        textInfo.text = "COLORS!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
