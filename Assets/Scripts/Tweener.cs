using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener: MonoBehaviour
{
    public Tween activeTween;
    public float speed = 35f;
    void Start()
    {
    }

    void Update()
    {
            if (activeTween != null)
            { 
                if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f)
                {
                    float elapsedTime = Time.time - activeTween.StartTime;
                    float t = elapsedTime / activeTween.Duration;
                    t = Mathf.Clamp01(t);
                    activeTween.Target.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, t);
                }
                else
                {
                    activeTween.Target.position = activeTween.EndPos;
                }


        }


    }


    public void AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos)
    {
        float distance = Vector3.Distance(startPos, endPos);
        float duration = distance / speed;
        activeTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
    }
}
