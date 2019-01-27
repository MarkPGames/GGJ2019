using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : BaseInteractable
{
    public AnimationCurve animCurve;
    public override void Interact()
    {
        //TODO end the game
        //  throw new System.NotImplementedException();
        PlayerController player = FindObjectOfType<PlayerController>();
        player.ChangeState(PlayerState.DISABLED, Vector3.zero);
        StartCoroutine(WinRoutine());
        StartCoroutine(heartRoutine());
    }

    private IEnumerator heartRoutine()
    {
        bool heartState = true;
        float maxTime = .5f;
        float timer = 0;
        //super disgusting but time 
        PlayerController player = FindObjectOfType<PlayerController>();
        UIManager.Instance.HeartImage.gameObject.SetActive(true);
        Vector3 midPoint = (player.transform.position + this.transform.position) / 2f;
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(midPoint);
        while (true)
        {
            Debug.Log("Heart routine");
            Debug.Log("Screenspace: " + screenSpace);
            // heartSprite.gameObject.SetActive((heartState = !heartState) );
            while (timer <= maxTime)
            {
                timer += Time.deltaTime;
                if (UIManager.Instance.HeartImage.gameObject.activeInHierarchy)
                {
                    screenSpace = Camera.main.WorldToScreenPoint(midPoint);
                    //UIManager.Instance.HeartImage.rectTransform.anchoredPosition = new Vector2(100, 100) * Random.Range(50,700);
                    UIManager.Instance.HeartImage.transform.position = screenSpace + (Vector3.up * 50) * Mathf.Lerp(0, 2, (timer / maxTime));
                    //heartSprite.transform.position = midPoint + Vector3.up * Mathf.Lerp(0, 2, timer / maxTime);
                }
                yield return null;
            }
            timer = 0;
            yield return null;
        }
    }

    private IEnumerator WinRoutine()
    {
        float maxTIme = 2;
        float timer = 0;
        while (timer <= maxTIme)
        {
            UIManager.Instance.SetBlackoutProgress(timer / maxTIme);
            timer += Time.deltaTime;
            yield return null;
        }
        //UIManager.Instance.WinObject.gameObject.SetActive(true);
        yield return null;
        timer = 0;
        while (timer <= 3)
        {
            timer += Time.deltaTime;
            yield return null;
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        //UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
