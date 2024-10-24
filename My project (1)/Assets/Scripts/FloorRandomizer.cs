using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRandomizer : MonoBehaviour
{
    public List<GameObject> tiles;
    public Material[] materials;

    public float timer = 0.5f;
    public float switchInterval;

    private void Start()
    {
        tiles.AddRange(GameObject.FindGameObjectsWithTag("Tile"));
        StartCoroutine(Randomize());
    }

    public IEnumerator Randomize()
    {
        while (true)
        {
            while (timer > 0)
            {
                foreach (GameObject item in tiles)
                {
                    item.GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
                }
                timer -= 0.06667f;
                yield return new WaitForSeconds(0.06667f);
            }
            foreach (GameObject item in tiles)
            {
                item.GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
            }
            yield return new WaitForSeconds(switchInterval);
            timer = 0.5f;
        }
    }
}
