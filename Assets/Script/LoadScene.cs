using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public void ChangeScene( string name){
        SceneManager.LoadScene(name);
    }
    // Start is called before the first frame update
    public void paused()
    {
        Time.timeScale=0;
    }

    // Update is called once per frame
    public void resume()
    {
     Time.timeScale =1;   
    }
    
}
