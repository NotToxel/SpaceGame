using UnityEngine;

public class OxygenTrigger : MonoBehaviour
{
    [SerializeField] private bool isOxygenArea = true; // Whether this area has oxygen

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OxygenManager.Instance.SetOxygenState(isOxygenArea);
        }
    }
}
