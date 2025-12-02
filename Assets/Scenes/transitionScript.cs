using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class transitionScript : MonoBehaviour
{
    public static string scene;
    public float time = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Transition());
    }

    // Update is called once per frame
    private IEnumerator Transition()
    {
        yield return new WaitForSeconds(time);

        if (!string.IsNullOrEmpty(scene))
        {
            SceneManager.LoadScene(scene);
        }
    }

    public static void ToScene(string theScene)
    {
        scene = theScene;
        SceneManager.LoadScene("TransitionScreen");
    }

}
