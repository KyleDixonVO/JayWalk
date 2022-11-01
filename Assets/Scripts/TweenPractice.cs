using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TweenPractice : MonoBehaviour
{
    public GameObject tweenTarget;
    Sequence sequence;

    // Start is called before the first frame update
    void Start()
    {
        sequence = DOTween.Sequence();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sequence.Append(tweenTarget.GetComponent<SpriteRenderer>().DOFade(0, 1))
            .Insert(0, tweenTarget.GetComponent<SpriteRenderer>().DOColor(Color.white, (float)0.25));
            sequence.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            tweenTarget.GetComponent<SpriteRenderer>().DOFade(1, 1);
            tweenTarget.GetComponent<SpriteRenderer>().DOColor(Color.red, (float)0.25);
        }
    }
}
