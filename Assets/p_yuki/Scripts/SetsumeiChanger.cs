using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetsumeiChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;

    // Start is called before the first frame update
    void Start()
    {
        text1.alpha = 0;
        text2.alpha = 0;
        StartCoroutine(FirstShow());
    }

    private IEnumerator FirstShow()
    {


        if (text2.alpha > 0)
        {
            for (int i = 20; i >= 0; i--)
            {
                Debug.Log("text2.alpha" + text2.alpha);

                text2.alpha = 0.05f * i;
                yield return null;
            }
        }


        for (int i = 0;i <= 10; i++)
        {
            Debug.Log("i" + i);

            text1.alpha = 0.1f * i;
            yield return null;
        }

        yield return new WaitForSeconds(4);
        StartCoroutine(SecndShow());
    }

    private IEnumerator SecndShow()
    {
        if (text1.alpha > 0)
        {
            for (int i = 20; i >= 0; i--)
            {
                text1.alpha = 0.05f * i;
                yield return null;
            }
        }

        for (int i = 0; i <= 10;i++)
        {
            text2.alpha = 0.1f * i;
            yield return null;
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(FirstShow());
    }
}
