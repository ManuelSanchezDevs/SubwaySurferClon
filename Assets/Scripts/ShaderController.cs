using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] private float curveX;
    [SerializeField, Range(-1, 1)] private float curveY;
    [SerializeField] private Material[] materials;

    [SerializeField] private float delayRandom = 3f;
    [SerializeField] private int myRandom = -1;
    [SerializeField] private bool check;
    [SerializeField] private bool stopDelayRandom = false;

    private void Start()
    {
        check = false;
    }

    private void Update()
    {
        foreach (var m in materials)
        {
            m.SetFloat(Shader.PropertyToID("_Curve_X"), curveX);
            m.SetFloat(Shader.PropertyToID("_Curve_Y"), curveY);
        }
        CurveGenerator();
        CountDown();
    }

    public void CurveGenerator()
    {
        if (myRandom == -1)
        {
            stopDelayRandom = true;
            curveX -= Time.deltaTime;
            if (curveX <= -1)
            {
                curveX = -1;
                stopDelayRandom = false;
            }
            check = true;
        }
        if (myRandom == 1)
        {
            stopDelayRandom = true;
            if (curveX > 0)
            {
                curveX -= Time.deltaTime;
                stopDelayRandom = false;
            }
            if(curveX < 0)
            {
                curveX += Time.deltaTime;
                stopDelayRandom = false;
            }
            check = true;
        }
        if (myRandom == 2)
        {
            stopDelayRandom = true;
            curveX += Time.deltaTime;
            if (curveX >= 1)
            {
                curveX = 1;
                stopDelayRandom = false;
            }
            check = true;
        }
        if (myRandom == 3)
        {
            stopDelayRandom = true;
            curveY -= Time.deltaTime;
            if (curveY <= -1)
            {
                curveY = -1;
                stopDelayRandom = false;
            }
            check = true;
        }
        if (myRandom == 4)
        {
            stopDelayRandom = true;
            curveY += Time.deltaTime;
            if (curveY >= 1)
            {
                curveY = 1;
                stopDelayRandom = false;
            }
            check = true;
        }
    }

    private void CountDown()
    {   
        if (!stopDelayRandom)
        {
            delayRandom -= Time.deltaTime;
        }
        if (delayRandom <= 0)
        {
            if (!check)
            {
                myRandom = -1;
                delayRandom = 3f;
            }
            else if (check)
            {
                delayRandom = 3f;
                myRandom = Random.Range(-1,5);
            }
        }
    }
}


