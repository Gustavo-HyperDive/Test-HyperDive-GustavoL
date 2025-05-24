using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderPosition : MonoBehaviour
{
    public float radius = 1f;
    public ParticleSystem particleSystem;

    void Update()
    {
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_Radius", radius);

        // Atualiza o raio do shape do Particle System
        if (particleSystem != null)
        {
            var shape = particleSystem.shape;
            shape.radius = radius - 0.5f;
        }

    }
}
