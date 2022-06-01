using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterSpawn : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> sprites;
    [SerializeField] private List<Behaviour> comps;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        comps = GetComponents<Behaviour>().Concat(GetComponentsInChildren<Behaviour>()).ToList();

        DisableAllComponents();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Request a spawn fx
        PoolObject poolObj = ObjectPooler.GetInstance().RequestObject(PoolObjectType.SPAWN_FX);
        // Activate fetched spawn fx & fetch animator
        Animator spawnFX = poolObj.Activate(transform.position, Quaternion.identity).GetComponentInChildren<Animator>();
        // Set delay (spawn fx length - 2 frames (spawnfx anim is on 12fps))
        float delay = spawnFX.GetCurrentAnimatorClipInfo(0)[0].clip.length - (2f / 12f);
        // Enable all components after delay
        StartCoroutine(EnableAllCompWithDelay(delay));
    }

    private IEnumerator EnableAllCompWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnableAllComponents();
    }

    private void EnableAllComponents()
    {
        foreach (SpriteRenderer spr in sprites)
        {
            spr.enabled = true;
        }

        foreach (Behaviour comp in comps)
        {
            if (!(comp is CharacterSpawn))
            {
                comp.enabled = true;
            }
        }

        this.enabled = false;
    }

    private void DisableAllComponents()
    {
        foreach (Behaviour comp in comps)
        {
            if (!(comp is CharacterSpawn))
            {
                comp.enabled = false;
            }
        }

        foreach (SpriteRenderer spr in sprites)
        {
            spr.enabled = false;
        }
    }
}
