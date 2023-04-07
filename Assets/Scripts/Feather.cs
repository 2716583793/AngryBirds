using Unity.Mathematics;
using UnityEngine;

public class Feather : MonoBehaviour
{
    public GameObject effect;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Instantiate(effect, transform.position, quaternion.identity);
        Destroy(this);
    }
}
