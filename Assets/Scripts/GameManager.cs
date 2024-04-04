using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    public float setX;
    private int count;
    private PlayerController playerController;

    private void Start()
    {
        StartCoroutine(Countdown(1));
        cam = Camera.main;
        setX = Camera.main.pixelWidth;
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        StarGame();
    }
    IEnumerator Countdown(float delay)
    {
        count = 4;
        if (count <= 4)
        {
            count--;
            yield return new WaitForSeconds(delay);
        }
        if (count == 3)
        {
            count--;
            yield return new WaitForSeconds(delay);
        }
        if (count == 2)
        {
            count--;
            yield return new WaitForSeconds(delay);
        }
        if (count == 1)
        {
            count--;
            yield return new WaitForSeconds(delay);
        }
        if(count == 0)
        {
            count = 0;
        }
    }
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUI.Label( new Rect(setX-60,100,100,100),count.ToString());
        if (GUI.Button(new Rect(setX-100,160,100,50), "RESTART"))
        {
            count = 3;
            SceneManager.LoadScene("Prototipe");
        }
    }
    private void StarGame()
    {
        if (count <= 0) 
        {
            playerController.FordwardSpeed = 10;
            playerController.Animator.enabled = true;
            playerController.GetSwipe();
        }
    }
}
