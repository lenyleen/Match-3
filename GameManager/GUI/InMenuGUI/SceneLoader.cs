using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
        public static int lvlNumber { get; private set; }
        private Animator animator => this.GetComponent<Animator>();
        private void Start() 
        {
                FadeOut();
        }

        public void LoadLvl(int lvlNum,int sceneNumber)
        {
                lvlNumber = lvlNum;
                StartCoroutine(ScreenFade(sceneNumber));
        }

        private IEnumerator ScreenFade(int sceneNumber)
        {
                animator.SetTrigger("FadeIn");
                yield return new WaitForSeconds(1.20f);
                SceneManager.LoadScene(sceneNumber);
        }
        private void FadeOut()
        {
              animator.SetTrigger("FadeOut");
        }
}
