using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderPosition : MonoBehaviour
{
    public Vector2 baseRadius = new Vector2(1f, 1f);
    public ParticleSystem particleSystem;

    public float step = 5f;                            // Valor fixo de incremento/diminuição
    public float speed = 5f;                           // Velocidade do Lerp
    public float minStep = -50f;                       // Limite inferior do deslocamento
    public float maxStep = 50f;

    private Vector2 currentRadius;
    private Vector2 targetRadius;
    private float stepOffset = 0f;

    void Start()
    {
        // Inicializa com base no raio original
        currentRadius = baseRadius;
        targetRadius = baseRadius;
    }

    void Update()
    {
        // Detecta input e ajusta stepOffset dentro dos limites
        if (Input.GetKeyDown(KeyCode.F))
        {
            stepOffset = Mathf.Clamp(stepOffset + step, minStep, maxStep);
            UpdateTargetRadius();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            stepOffset = Mathf.Clamp(stepOffset - step, minStep, maxStep);
            UpdateTargetRadius();
        }

        // Interpola suavemente o raio atual em direção ao alvo
        currentRadius = Vector2.Lerp(currentRadius, targetRadius, Time.deltaTime * speed);

        // Envia dados para o shader
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalVector("_Radius", new Vector4(currentRadius.x, currentRadius.y, 0f, 0f));

        // Atualiza o shape do Particle System
        if (particleSystem != null)
        {
            var shape = particleSystem.shape;
            shape.scale = new Vector3(currentRadius.x, currentRadius.y, 1f);
        }
    }

    void UpdateTargetRadius()
    {
        // Aplica o offset proporcionalmente ao raio base
        float factor = stepOffset / step;
        targetRadius = baseRadius + new Vector2(step, step) * factor;
        Debug.Log(" New Radius: " + targetRadius);
    }
}
