using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprits;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = sprits[Random.Range(0, sprits.Length)];
        Destroy(this);
    }
}
