using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleWithLight : MonoBehaviour
{
    [SerializeField]
    public GameObject _prefab;

    private ParticleSystem _particleSystem;
    private readonly List<GameObject> _instances = new List<GameObject>();
    private ParticleSystem.Particle[] _particles;
    private float _diff;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        _diff = transform.position.y;
    }

    void LateUpdate()
    {
        int count = _particleSystem.GetParticles(_particles);

        while (_instances.Count < count)
            _instances.Add(Instantiate(_prefab, _particleSystem.transform));

        for (int i = 0; i < _instances.Count; i++)
        {
            if (i < count)
            {
                Vector3 particlePos = _particles[i].position;
                particlePos.y += _diff;
                _instances[i].transform.position = particlePos;

                _instances[i].SetActive(true);
            }
            else
            {
                _instances[i].SetActive(false);
            }
        }
    }
}