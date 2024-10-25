using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRandomizer : MonoBehaviour
{
    public List<GameObject> tiles;
    public Material[] materials;
    public Material[] gem;
    public GameObject indicator;

    public List<GameObject> red;
    public List<GameObject> yellow;
    public List<GameObject> blue;
    public List<GameObject> green;
    public List<GameObject> purple;

    public int safe;
    public float timer = 0.5f;
    public float deathInterval = 5f;
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
                int integer = Random.Range(0, materials.Length);
                item.GetComponent<MeshRenderer>().material = materials[integer];
                switch (integer)
                {
                    case 0:
                        red.Add(item);
                        break;
                    case 1:
                        yellow.Add(item);
                        break;
                    case 2:
                        blue.Add(item);
                        break;
                    case 3:
                        green.Add(item);
                        break;
                    case 4:
                        purple.Add(item);
                        break;
                    default:
                        break;
                }
            }
            safe = Random.Range(0, materials.Length);
            indicator.GetComponent<MeshRenderer>().material = gem[safe];
            yield return new WaitForSeconds(deathInterval);
            if (safe != 0) red.ForEach((g) => g.SetActive(false));
            if (safe != 1) yellow.ForEach((g) => g.SetActive(false));
            if (safe != 2) blue.ForEach((g) => g.SetActive(false));
            if (safe != 3) green.ForEach((g) => g.SetActive(false));
            if (safe != 4) purple.ForEach((g) => g.SetActive(false));
            yield return new WaitForSeconds(switchInterval);
            deathInterval *= 0.95f;
            tiles.ForEach((g) => g.SetActive(true));
            timer = 0.5f;
            red.Clear();
            yellow.Clear();
            blue.Clear();
            green.Clear();
            purple.Clear();
        }
    }
}
