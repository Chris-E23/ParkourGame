using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;


public class playerUI : MonoBehaviour
{
    public playerUI instance;
    public TMP_Text timerText; 
    // Start is called before the first frame update
    void Start()
    {
      instance = this;
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
