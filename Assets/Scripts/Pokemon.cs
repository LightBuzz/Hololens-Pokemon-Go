using UnityEngine;
using System.Collections;

public class Pokemon : MonoBehaviour
{
    public float collectionSpeed = 5;

    public IEnumerator Collect(Transform pokeball)
    {
        float delta = 0;

        while (delta < 1f)
        {
            delta = Mathf.Min(1f, delta + Time.deltaTime * collectionSpeed);

            transform.localScale = new Vector3(1f - delta, 1f - delta, 1f - delta);

            transform.position = Vector3.Lerp(transform.position, pokeball.position, delta);

            yield return null;
        }
    }
}
