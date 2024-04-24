using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun; 
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class playerUI :  MonoBehaviour
{
    public static playerUI instance;
    public TMP_Text timerText; 
  private void Awake(){

    instance = this; 
  }
    void Start()
    {
      
      
      
    
    }

    void Update()
    {
     
    }
}
