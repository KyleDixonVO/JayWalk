using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHoverListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OverwriteActiveButton()
    {
        UI_Manager.ui_manager.caller = gameObject;
    }
}
