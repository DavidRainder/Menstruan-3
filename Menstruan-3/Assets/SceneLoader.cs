using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region Singleton

    private static SceneLoader instance = null;
    public static SceneLoader Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    public static void LoadScene(string sceneName)
    {
        instance.StartCoroutine(Load(sceneName));
    }

    private static IEnumerator Load(string scene)
    {
        yield return new WaitForSeconds(1.0f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone) { 
            yield return null;
        }

        FadeManager.Instance.FadeIn();
    }
}
