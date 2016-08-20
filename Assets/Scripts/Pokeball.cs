using UnityEngine;
using System.Collections;

public class Pokeball : MonoBehaviour
{
    public PlayerInput playerInput;

    void OnCollisionEnter(Collision col)
    {
        playerInput.OnPokemonCollided();
    }
}