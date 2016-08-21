using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;

public class PlayerInput : MonoBehaviour
{
    public Pokeball pokeball;

    public Vector3 forceLeft;
    public Vector3 forceRight;

    bool canThrow = true;

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
        if (canThrow)
        {
            canThrow = false;

            pokeball.Throw(Camera.main.transform.rotation * Vector3.Lerp(forceLeft, forceRight, Random.Range(0f, 1f)),
                new Vector3(Random.Range(0f, 50f), Random.Range(-2f, 2f), Random.Range(-1f, 1f)));

            StartCoroutine(Coroutine_ReturnPokeball());
        }
    }

    IEnumerator Coroutine_ReturnPokeball()
    {
        yield return new WaitUntil(() =>
        {
            return pokeball.ready;
        });

        canThrow = true;
    }
}
