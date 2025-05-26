using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderPosition : MonoBehaviour
{
    public Vector2 radius = new Vector2(1f, 1f);
    public ParticleSystem particleSystem;

    void Update()
    {
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalVector("_Radius", new Vector4(radius.x, radius.y, 0f, 0f));

        // Atualiza o raio do shape do Particle System
        if (particleSystem != null)
        {
            var shape = particleSystem.shape;
            shape.scale = new Vector3(radius.x, radius.y, 1f);
        }

    }
}
