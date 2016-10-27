using UnityEngine;
using System.Collections;

public class Pokemon : MonoBehaviour
{
    const float MOVE_TO_POKEBALL_SPEED = 3;

    Vector3 initialPosition;
    Quaternion initialRotation;
    Vector3 initialScale;

    public bool CanBeCaptured
    {
        get;
        private set;
    }

    void Awake()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialScale = transform.localScale;
    }

    public void Capture(Transform pokeball)
    {
        GetComponent<Collider>().enabled = false;

        StartCoroutine(Coroutine_Capture(pokeball));
    }

    IEnumerator Coroutine_Capture(Transform pokeball)
    {
        CanBeCaptured = false;

        float delta = 0;

        while (delta < 1f)
        {
            delta = Mathf.Min(1f, delta + Time.deltaTime * MOVE_TO_POKEBALL_SPEED);

            transform.position = Vector3.Lerp(initialPosition, pokeball.position, delta);
            transform.localScale = new Vector3(1f - delta, 1f - delta, 1f - delta);

            yield return null;
        }

        CanBeCaptured = true;
    }

    public void ResetPokemon()
    {
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
        transform.localScale = initialScale;

        GetComponent<Collider>().enabled = true;
    }
}
