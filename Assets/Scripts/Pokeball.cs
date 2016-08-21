using UnityEngine;
using System.Collections;

public class Pokeball : MonoBehaviour
{
    public Transform enterPoint;

    Pokemon hitPokemon = null;

    bool isWaitingBeforeCapture = false;
    public bool ready = true;

    Rigidbody Rigidbody
    {
        get
        {
            return GetComponent<Rigidbody>();
        }
    }

    Vector3 startPosition;
    Quaternion startRotation;

    void Awake()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
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

            isWaitingBeforeCapture = true;
            StartCoroutine(Coroutine_WaitAndCapture());
        }
    }

    public void Throw(Vector3 velocity, Vector3 angularVelocity)
    {
        ready = false;

        transform.parent = null;
        Rigidbody.isKinematic = false;
        Rigidbody.velocity = velocity;
        Rigidbody.angularVelocity = angularVelocity;

        StartCoroutine(Coroutine_WaitForPokemonReset());
    }

    IEnumerator Coroutine_WaitForPokemonReset()
    {
        yield return new WaitForSeconds(2);

        if (hitPokemon != null)
        {
            yield return new WaitUntil(() =>
            {
                return !isWaitingBeforeCapture;
            });

            yield return new WaitForSeconds(1);

            hitPokemon.ResetPokemon();
            hitPokemon = null;
        }

        ResetPokeball();
    }

    IEnumerator Coroutine_WaitAndCapture()
    {
        Vector3 lastVelocity = Rigidbody.velocity;
        if (lastVelocity.z < 0)
        {
            lastVelocity.z = -lastVelocity.z;
        }

        Rigidbody.velocity = lastVelocity;
        lastVelocity *= 0.5f;
        Vector3 angularVelocity = Rigidbody.angularVelocity;

        yield return new WaitForSeconds(0.25f);

        Rigidbody.isKinematic = true;

        Vector3 eulerAngles = Quaternion.LookRotation(hitPokemon.transform.position - transform.position, Vector3.up).eulerAngles;
        if (eulerAngles.x > 180)
        {
            eulerAngles.x -= 360;
        }
        transform.rotation = Quaternion.Euler(Mathf.Lerp(eulerAngles.x, 0, 0.75f), Mathf.Lerp(eulerAngles.y, 180, 0.5f), 0);

        yield return new WaitForSeconds(0.15f);

        hitPokemon.Capture(enterPoint);

        yield return new WaitUntil(() =>
        {
            return !hitPokemon.IsBeingCaptured;
        });

        yield return new WaitForSeconds(0.5f);

        Rigidbody.isKinematic = false;
        Rigidbody.velocity = lastVelocity;
        Rigidbody.angularVelocity = angularVelocity;

        isWaitingBeforeCapture = false;
    }

    public void ResetPokeball()
    {
        transform.parent = Camera.main.transform;
        Rigidbody.isKinematic = true;
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;

        ready = true;
    }
}