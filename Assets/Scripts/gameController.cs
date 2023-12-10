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
        Debug.Log("ids Count" + ids.Count);
    }
    public int getId(int id){
        return ids[id];
    }
    public int getLength(){
        return ids.Count;
    }
}
