using UnityEngine;

public class AISensorPoint : MonoBehaviour
{
    [Header("AI Sensor Point")]
    [SerializeField] GameObject _parentObject;

    Collider _collider;

    public GameObject ParentObject => _parentObject;

    private void OnValidate()
    {
        OnChange();
    }

    // this is a really nice way to set default values for other components outside of runtime
    private void Reset()
    {
        OnChange();
    }

    private void OnChange()
    {
        _collider = GetComponent<Collider>();

        if (_parentObject == null)
            Debug.LogWarning(gameObject.name + "'s '_parentObject' has not been set in its AI Sensor Point script.");

        if (_collider == null)
            Debug.LogWarning(gameObject.name + "'s '_collider' has not been set in its AI Sensor Point script.");
        else
            _collider.isTrigger = true;

        if (gameObject.layer == (int)Layers.Default)
            Debug.LogWarning(gameObject.name + "'s Layer is still default. The AI Sensor Point script needs a more specific layer.");
    }
}
