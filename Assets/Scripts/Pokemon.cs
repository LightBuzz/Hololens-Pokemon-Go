using UnityEngine;
using System.Collections;

public class Pokemon : MonoBehaviour
{
    public float moveToPokeballSpeed = 2;

    public bool IsBeingCaptured
    {
        get;
        private set;
    }

    Vector3 startPosition;
    Quaternion startRotation;

    void Awake()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    public void Capture(Transform target)
    {
        IsBeingCaptured = true;

        GetComponent<SphereCollider>().enabled = false;

        StartCoroutine(Coroutine_Capture(target));
    }

    IEnumerator Coroutine_Capture(Transform target)
    {
        float delta = 0;

        while (delta < 1f)
        {
            delta = Mathf.Min(1f, delta + Time.deltaTime * moveToPokeballSpeed);

            transform.localScale = new Vector3(1f - delta, 1f - delta, 1f - delta);

            transform.position = Vector3.Lerp(startPosition, target.position, delta);

            yield return null;
        }

        IsBeingCaptured = false;
    }

    public void ResetPokemon()
    {
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
        transform.localScale = Vector3.one;

        GetComponent<SphereCollider>().enabled = true;
    }
}
