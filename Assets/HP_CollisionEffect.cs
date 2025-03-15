using UnityEngine;

public class HP_CollisionEffect : MonoBehaviour
{
    public int HP = 100; // Example HP stat
    public Light effectLight; // Assign a light in the inspector or instantiate dynamically
    private int previousHP;

    private void Start()
    {
        previousHP = HP;

        // If no light is assigned, create one dynamically
        if (effectLight == null)
        {
            GameObject lightObj = new GameObject("EffectLight");
            lightObj.transform.SetParent(transform);
            lightObj.transform.localPosition = Vector3.zero;
            effectLight = lightObj.AddComponent<Light>();
            effectLight.color = Color.white;
            effectLight.intensity = 0;
            effectLight.range = 5;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HP_CollisionEffect otherObject = collision.gameObject.GetComponent<HP_CollisionEffect>();

        if (otherObject != null)
        {
            // Check whose HP changed
            bool selfHPChanged = (HP != previousHP);
            bool otherHPChanged = (otherObject.HP != otherObject.previousHP);

            if (selfHPChanged)
            {
                StartCoroutine(FlashLightEffect());
            }

            if (otherHPChanged)
            {
                otherObject.StartCoroutine(otherObject.FlashLightEffect());
            }

            // Update HP values for future comparisons
            previousHP = HP;
            otherObject.previousHP = otherObject.HP;
        }
    }

    private System.Collections.IEnumerator FlashLightEffect()
    {
        effectLight.intensity = 5; // Bright flash
        yield return new WaitForSeconds(0.3f); // Duration of the flash
        effectLight.intensity = 0; // Back to normal
    }
}
