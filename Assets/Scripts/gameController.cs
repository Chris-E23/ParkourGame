using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    
    [SerializeField] private List<int> ids;
   
    public static gameController instance;

    private void Awake()
    {
        instance = this;
        ids = new List<int>();
    }

    public void addToList(int id)
    {
        ids.Add(id);
    }
}
