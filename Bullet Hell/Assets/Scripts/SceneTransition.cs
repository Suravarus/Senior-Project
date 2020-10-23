using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    //private int[] roomlist = {1,2,3,4,5,6,7,8,9,10};
    
    //public GameObject player;
    //public GameObject cameras;
    //Scene currentScene = SceneManager.GetActiveScene();
    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        //foreach (var room in roomlist)
        //{
        //    Debug.Log("Rooms" + room);
        //}
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) ///&& !other.isTrigger)
        {
            //SceneManager.LoadScene(sceneToLoad);
            
            SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
            //SceneManager.MoveGameObjectToScene(cameras, SceneManager.GetSceneByName(sceneToLoad));
            //SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(sceneToLoad));
            //SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}
