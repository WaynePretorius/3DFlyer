using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class LevelManager : MonoBehaviour
{
    
    private void Awake()
    {
        MakeSingleton();
    }

    private void MakeSingleton()
    {
        int numberOfLevelManagers = FindObjectsOfType<LevelManager>().Length;

        if(numberOfLevelManagers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadGetNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        int lastSceneIndex = SceneManager.sceneCountInBuildSettings;

        if (nextSceneIndex >= lastSceneIndex)
        {
            RestartLevel();
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
