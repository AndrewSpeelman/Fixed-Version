using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void LoadNextScene()
    {
        int current_scene_index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current_scene_index + 1);
    }
	
	public void SkipScene()
    {
        int current_scene_index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current_scene_index + 2);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void goToLS()
    {
        SceneManager.LoadScene("Level Select");
    }
}
