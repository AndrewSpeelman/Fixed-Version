using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public void LoadNextLevel()
    {
        if(GameplayController.Level == GameplayController.NUM_LEVELS)
        {
            SceneManager.LoadScene(4);
        }
        else
        {
            GameplayController.Level = GameplayController.Level + 1;
            //Five is the offset between the title screen and level 1
            SceneManager.LoadScene(GameplayController.Level + 5);
        }
    }

    public void LoadCurrentLevel()
    {
        //Five is the offset between the title screen and level 1
        SceneManager.LoadScene(GameplayController.Level + 5);
    }

    public void LoadLevel(int lvl)
    {
        GameplayController.Level = lvl;
        //Five is the offset between the title screen and level 1
        SceneManager.LoadScene(GameplayController.Level + 5);
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
        SceneManager.LoadScene(5);
    }
}
