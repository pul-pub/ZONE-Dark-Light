using UnityEngine;

public class Artifact : MonoBehaviour
{
    public ArtifactObject ArtifactObject;
    public bool IsFinded = false;

    [SerializeField] private GameObject[] objsOnChange;

    public void SetFinded()
    {
        IsFinded = true;

        for (int i = 0; i < objsOnChange.Length; i++)
            objsOnChange[i].SetActive(true);
    }
}
