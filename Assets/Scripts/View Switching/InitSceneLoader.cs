using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneLoader : MonoBehaviour
{
    private void Awake()
    {
        if (SceneManager.sceneCount == 1)
        {
            SceneManager.LoadScene(ViewConfig.Views.Init);
            SceneManager.UnloadSceneAsync(gameObject.scene);
            gameObject.SetActive(false);
        }
    }
}
