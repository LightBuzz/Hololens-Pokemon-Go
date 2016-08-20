using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;

public class PlayerInput : MonoBehaviour
{
    public Pokemon pokemon;
    public Transform initialPokemonPoint;

    public Transform pokeball;
    public Vector3 forceLeft;
    public Vector3 forceRight;

    bool shooting = false;
    bool collidedWithPokemon = false;

#if !UNITY_EDITOR

    GestureRecognizer gr;

    void Awake()
    {
        gr = new GestureRecognizer();

        gr.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold | GestureSettings.ManipulationTranslate);

        gr.TappedEvent += Gr_TappedEvent;
        gr.HoldStartedEvent += Gr_HoldStartedEvent;
        gr.HoldCanceledEvent += Gr_HoldCanceledEvent;
        gr.HoldCompletedEvent += Gr_HoldCompletedEvent;

        gr.StartCapturingGestures();
    }

    void OnDestroy()
    {
        gr.TappedEvent -= Gr_TappedEvent;
        gr.HoldStartedEvent -= Gr_HoldStartedEvent;
        gr.HoldCanceledEvent -= Gr_HoldCanceledEvent;
        gr.HoldCompletedEvent -= Gr_HoldCompletedEvent;
    }

#else

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Gr_TappedEvent(InteractionSourceKind.Hand, 0, new Ray());
        }
    }

#endif

    private void Gr_HoldCompletedEvent(InteractionSourceKind source, Ray headRay)
    {

    }

    private void Gr_HoldCanceledEvent(InteractionSourceKind source, Ray headRay)
    {

    }

    private void Gr_HoldStartedEvent(InteractionSourceKind source, Ray headRay)
    {

    }

    private void Gr_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (!shooting)
        {
            pokeball.GetComponent<Rigidbody>().isKinematic = false;
            pokeball.GetComponent<Rigidbody>().velocity = Vector3.Lerp(forceLeft, forceRight, Random.Range(0, 100) / 100f);

            StartCoroutine(Coroutine_ReturnPokeball());

            shooting = true;
        }
    }

    IEnumerator Coroutine_ReturnPokeball()
    {
        yield return new WaitUntil(() => { return collidedWithPokemon; });

        pokemon.GetComponent<SphereCollider>().enabled = false;

        yield return StartCoroutine(pokemon.Collect(pokeball));

        yield return new WaitForSeconds(1);

        pokeball.GetComponent<Rigidbody>().isKinematic = true;
        pokeball.position = transform.position;
        pokeball.rotation = transform.rotation;

        pokemon.transform.position = initialPokemonPoint.position;
        pokemon.transform.rotation = initialPokemonPoint.rotation;
        pokemon.transform.localScale = initialPokemonPoint.localScale;
        pokemon.GetComponent<SphereCollider>().enabled = true;

        shooting = false;
        collidedWithPokemon = false;
    }

    public void OnPokemonCollided()
    {
        collidedWithPokemon = true;
    }
}
