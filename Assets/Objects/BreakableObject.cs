using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamagable
{
    [SerializeField] private int _experience_reward;
    [SerializeField] private int _required_level;
    [SerializeField] private float _respawn_interval_min = 3;
    [SerializeField] private float _respawn_interval_max = 10;

    private void Awake()
    {
        gameObject.SetActive(false);
        Invoke("ActivateGameObject", Random.Range(_respawn_interval_min, _respawn_interval_max));
    }

    private void ActivateGameObject()
    {
        gameObject.SetActive(true);
    }

    public void hit(float energy, System.Action<int> callback)
    {
        //Test if enough Energy
        if (_required_level > energy) return;

        gameObject.SetActive(false);
        Invoke("ActivateGameObject", Random.Range(_respawn_interval_min, _respawn_interval_max));
        callback(_experience_reward);
    }
}
