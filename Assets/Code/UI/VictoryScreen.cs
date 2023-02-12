using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Level;
using UnityEngine.UI;

namespace Code.UI
{
    public class VictoryScreen : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;

        [SerializeField] private GameObject panel;
        [SerializeField] private RawImage victoryImage;
        private Image screenBG;
        private float t;
        private float t2;
        [SerializeField] private float fadeSpeed = 0.1f;
        private bool fadedIn;

        void Update()
        {
            if (screenBG.color.a < 1)
            {
                Color tempcolor = screenBG.color;
                
                tempcolor.a = Mathf.Lerp(0, 1, Mathf.SmoothStep(0, 1, t));
                t += fadeSpeed * Time.fixedDeltaTime;

                screenBG.color = tempcolor;
            } else if (screenBG.color.a == 1 && victoryImage.color.a < 1)
            {
                Color tempcolor = victoryImage.color;
                
                tempcolor.a = Mathf.Lerp(0, 1, Mathf.SmoothStep(0, 1, t2));
                t2 += fadeSpeed * Time.fixedDeltaTime;
                
                victoryImage.color = tempcolor;
            } else if (screenBG.color.a == 1 && victoryImage.color.a == 1)
            {
                fadedIn = true;
            }

            if (fadedIn & (Input.GetKey("escape") || Input.GetKey("space")))
            {
                levelManager.EndLevel();
            }
        }

        private void Awake()
        {
            fadedIn = false;
            screenBG = panel.gameObject.GetComponent<Image>();
            StartFade();
        }

        public void StartFade()
        {
            // Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }
}
