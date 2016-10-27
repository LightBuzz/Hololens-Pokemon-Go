using UnityEngine;
using System.Collections;

public class Pokeball : MonoBehaviour
{
    /// <summary>
    /// We don't have a real pokeball so it can't open for the pokemon to go in
    /// </summary>
    public Transform pokemonEnterPoint;

    Pokemon hitPokemon = null;

    [HideInInspector]
    public bool readyToThrow = true;
    bool isWaitingBeforePokemonCapture = false;

    new public Rigidbody rigidbody;
    Vector3 initialPosition;
    Quaternion initialRotation;

    void Awake()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void OnCollisionEnter(Collision col)
    {
        if (hitPokemon != null)
        {
            return;
        }

        if (col.transform.tag == "Pokemon")
        {
            hitPokemon = col.transform.GetComponent<Pokemon>();

            isWaitingBeforePokemonCapture = true;
            StartCoroutine(Coroutine_CaptureSequence());
        }
    }

    public void Throw(Vector3 velocity, Vector3 angularVelocity)
    {
        readyToThrow = false;

        transform.parent = null;
        rigidbody.isKinematic = false;
        rigidbody.velocity = velocity;
        rigidbody.angularVelocity = angularVelocity;

        StartCoroutine(Coroutine_WaitForPokemonReset());
    }

    IEnumerator Coroutine_WaitForPokemonReset()
    {
        yield return new WaitForSeconds(2);

        if (hitPokemon != null)
        {
            yield return new WaitUntil(() => { return !isWaitingBeforePokemonCapture; });

            yield return new WaitForSeconds(1);

            hitPokemon.ResetPokemon();
            hitPokemon = null;
        }

        ResetPokeball();
    }

    IEnumerator Coroutine_CaptureSequence()
    {
        Vector3 lastVelocity = rigidbody.velocity;
        if (lastVelocity.z < 0)
        {
            lastVelocity.z = -lastVelocity.z;
        }

        rigidbody.velocity = lastVelocity;
        lastVelocity *= 0.5f;
        Vector3 angularVelocity = rigidbody.angularVelocity;

        yield return new WaitForSeconds(0.25f);

        rigidbody.isKinematic = true;

        Vector3 eulerAngles = Quaternion.LookRotation(hitPokemon.transform.position - transform.position, Vector3.up).eulerAngles;
        if (eulerAngles.x > 180)
        {
            eulerAngles.x -= 360;
        }
        transform.rotation = Quaternion.Euler(Mathf.Lerp(eulerAngles.x, 0, 0.75f), Mathf.Lerp(eulerAngles.y, 180, 0.5f), 0);

        yield return new WaitForSeconds(0.15f);

        hitPokemon.Capture(pokemonEnterPoint);

        yield return new WaitUntil(() => { return !hitPokemon.CanBeCaptured; });

        yield return new WaitForSeconds(0.5f);

        rigidbody.isKinematic = false;
        rigidbody.velocity = lastVelocity;
        rigidbody.angularVelocity = angularVelocity;

        isWaitingBeforePokemonCapture = false;
    }

    public void ResetPokeball()
    {
        transform.parent = Camera.main.transform;
        rigidbody.isKinematic = true;
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;

        readyToThrow = true;
    }
}