using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;

public class PlayerInput : MonoBehaviour
{
    public Pokeball pokeball;

    public Vector3 throwForceLeft = new Vector3(-1, 4, 3.5f);
    public Vector3 throwForceRight = new Vector3(1, 4, 3.5f);

    bool canThrowPokeball = true;

#if !UNITY_EDITOR

    GestureRecognizer gestureRecognizer;

    void Awake()
    {
        gestureRecognizer = new GestureRecognizer();

        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold | GestureSettings.ManipulationTranslate);

        gestureRecognizer.TappedEvent += Gr_TappedEvent;
        gestureRecognizer.HoldStartedEvent += Gr_HoldStartedEvent;
        gestureRecognizer.HoldCanceledEvent += Gr_HoldCanceledEvent;
        gestureRecognizer.HoldCompletedEvent += Gr_HoldCompletedEvent;

        gestureRecognizer.StartCapturingGestures();
    }

    void OnDestroy()
    {
        gestureRecognizer.TappedEvent -= Gr_TappedEvent;
        gestureRecognizer.HoldStartedEvent -= Gr_HoldStartedEvent;
        gestureRecognizer.HoldCanceledEvent -= Gr_HoldCanceledEvent;
        gestureRecognizer.HoldCompletedEvent -= Gr_HoldCompletedEvent;
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
        if (canThrowPokeball)
        {
            canThrowPokeball = false;

            Quaternion cameraRotation = Camera.main.transform.rotation;
            Vector3 randomThrowForce = Vector3.Lerp(throwForceLeft, throwForceRight, Random.Range(0f, 1f));
            Vector3 randomThrowAngularForce = new Vector3(Random.Range(0f, 50f), Random.Range(-2f, 2f), Random.Range(-1f, 1f));

            pokeball.Throw(cameraRotation * randomThrowForce, randomThrowAngularForce);

            StartCoroutine(Coroutine_ReturnPokeball());
        }
    }

    IEnumerator Coroutine_ReturnPokeball()
    {
        yield return new WaitUntil(() => { return pokeball.readyToThrow; });

        canThrowPokeball = true;
    }
}
