using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderPosition : MonoBehaviour
{
    public Vector2 baseRadius = new Vector2(1f, 1f);
    public ParticleSystem idleEffect;
    public ParticleSystem InteractiveEffect;
    public ParticleSystem fireExpand;
    public ParticleSystem fireColapse;

    public float step = 5f; // Valor fixo de incremento/diminui��o
    public float speed = 5f;
    public float minStep = -50f;                     
    public float maxStep = 50f;

    private Vector2 currentRadius;
    private Vector2 targetRadius;
    private float stepOffset = 0f;

    void Awake()
    {
        if (idleEffect == null || InteractiveEffect == null || fireExpand == null || fireColapse == null)
            Debug.LogError("ParticleSystem n�o foi atribu�do no Inspector!", this);
    }

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
            PlayParticle(fireExpand);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            stepOffset = Mathf.Clamp(stepOffset - step, minStep, maxStep);
            UpdateTargetRadius();
            PlayParticle(fireColapse);
        }

        // Interpola suavemente o raio atual em dire��o ao alvo
        currentRadius = Vector2.Lerp(currentRadius, targetRadius, Time.deltaTime * speed);

        // Refer�ncia da posi��o e Raio do Shader
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalVector("_Radius", new Vector4(currentRadius.x, currentRadius.y, 0f, 0f));

        // Atualiza o shape do Particle System
        if (idleEffect != null)
        {
            var shape = idleEffect.shape;
            shape.scale = new Vector3(currentRadius.x, currentRadius.y, 1f);            
        }
        if (InteractiveEffect != null)
        {
            var shape2 = InteractiveEffect.shape;
            shape2.scale = new Vector3(currentRadius.x, currentRadius.y, 1f);
        }
    }

    void PlayParticle(ParticleSystem fx)
    {
        fx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        fx.Play();

        InteractiveEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        InteractiveEffect.Play();

    }

    void UpdateTargetRadius()
    {
        // Aplica o offset proporcionalmente ao raio base
        float factor = stepOffset / step;
        targetRadius = baseRadius + new Vector2(step, step) * factor;
        //Debug.Log(" New Radius: " + targetRadius); //Teste GUS TEMP
    }
}
