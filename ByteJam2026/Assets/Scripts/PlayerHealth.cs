using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float Health;

    void Start()
    {
        Health = 100;
    }

    public void TakeDamage(float damage){
        Health -= damage;
        
        if(Health < 0){
            Health = 0;
        }
    }
}
