using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	//public void LoadNextScene()
 //   {
 //       int current_scene_index = SceneManager.GetActiveScene().buildIndex;
 //       SceneManager.LoadScene(current_scene_index + 1);
 //   }
	
	//public void SkipScene()
 //   {
 //       int current_scene_index = SceneManager.GetActiveScene().buildIndex;
 //       SceneManager.LoadScene(current_scene_index + 2);
 //   }

 //   public void LoadStartScene()
 //   {
 //       SceneManager.LoadScene(0);
 //   }

    public void LoadNextLevel()
    {
 
            GameplayController.Level = GameplayController.Level + 1;
            //Five is the offset between the title screen and level 1
            SceneManager.LoadScene(GameplayController.Level + 3);
        
    }

    public void LoadCurrentLevel()
    {
        //Five is the offset between the title screen and level 1
        SceneManager.LoadScene(GameplayController.Level + 3);
    }

    public void LoadLevel(int lvl)
    {
        GameplayController.Level = lvl;
        //Five is the offset between the title screen and level 1
        SceneManager.LoadScene(GameplayController.Level + 3);
    }

    public void LoadAttackerVictory()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadDefenderVictory()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadStartLevel()
    {
        GameplayController.Level = 0;
        SceneManager.LoadScene(0);
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene(8);
    }
}
