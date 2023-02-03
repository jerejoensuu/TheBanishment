using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour
{
    private RawImage[] pictures;
    [SerializeField] private RawImage picture1;
    [SerializeField] private RawImage picture2;
    [SerializeField] private RawImage picture3;
    private int currentPicture;

    private bool fadedIn = true;
    [SerializeField] private float fadeTime;
    [SerializeField] private float fadeSpeed = 0.1f;
    private float t;

    private void Start()
    {
        currentPicture = 0;

        pictures = new RawImage[3];

        pictures[0] = picture1;
        pictures[1] = picture2;
        pictures[2] = picture3;
    }

    private void Update()
    {
        if (Input.GetKey("space") && fadedIn)
        {
            if (currentPicture == 3)
            {
                SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            } else 
            {
                StartCoroutine(FadingIn());
            }
        }

        if (!fadedIn)
        {
            Color color = pictures[currentPicture].color;

            t += fadeSpeed * Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t);

            pictures[currentPicture].color = color;
        }
    }

    private IEnumerator FadingIn()
    {
        fadedIn = false;

        yield return new WaitForSeconds(fadeTime);
        
        currentPicture++;
        t = 0f;
        fadedIn = true;
    }
}
