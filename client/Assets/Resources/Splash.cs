using UnityEngine;
using System.Collections;



public class Splash : MonoBehaviour
{

    public void OnFinish()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }

}
