using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void selectlevel(int level_index)
    {
        SceneManager.LoadScene(level_index);
    }
}

