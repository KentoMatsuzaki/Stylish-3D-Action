using UnityEngine;

public interface IAttacker
{
    void EnableCollider();
    void DisableCollider();
    void OnTriggerEnter(Collider other);
}
