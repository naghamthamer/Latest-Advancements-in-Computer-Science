using UnityEngine;

public class AnimationEvent : MonoBehaviour // Class definition (AnimationEvent is a class that inherits from MonoBehaviour)
{
    public CharacterMovement charMove; // Object (Reference to the CharacterMovement class to trigger attack actions)

    [SerializeField] ParticleSystem bloodParticlePrefab; // Object (Reference to the ParticleSystem component for blood effects)

    // Method (Triggered when the player attack animation reaches a specific frame)
    public void PlayerAttack()
    {
        Debug.Log("player attacked"); // Method (Logs a message to the console)
        charMove.DoAttack(); // Method (Calls the DoAttack method from the CharacterMovement class to perform the attack)
    }

    // Method (Triggered when the player takes damage during an animation)
    public void playerDamage()
    {
        transform.GetComponent<EnemyController>().DamagePlayer(); // Object (Calls the DamagePlayer method from the EnemyController class to apply damage to the player)
        bloodParticlePrefab.Play(); // Method (Plays the blood particle effect)
    }
}
