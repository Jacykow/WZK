using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{
    public static ViewManager main;

    private string _currentView = null;
    public string CurrentView
    {
        get => _currentView;
        set
        {
            if (_currentView != null)
            {
                SceneManager.UnloadSceneAsync(_currentView);
            }
            SceneManager.LoadScene(value, LoadSceneMode.Additive);
            _currentView = value;
        }
    }

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        CurrentView = ViewConfig.Views.Menu;
    }
}
