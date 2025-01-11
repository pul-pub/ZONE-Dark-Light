using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIDideScreen : MonoBehaviour
{
    [SerializeField] private GameObject screen;
    [Space]
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private Image imgAnomaly;
    [SerializeField] private Image imgMutant;
    [SerializeField] private Image[] imgPeople;

    public void ToMenu() => SceneManager.LoadScene(0, LoadSceneMode.Single);
    public void Restart()
    {
        SaveHeandler.StartSession();
        SceneManager.LoadScene(SaveHeandler.SessionSave.idScene, LoadSceneMode.Single);
    }

    public void OnDide(IMetaEnemy _meta)
    {
        screen.SetActive(true);

        textName.text = _meta.Name;
        if (_meta.visualEnemy.ContainsKey("Face"))
        {
            for (int i = 0; i < 5; i++)
                imgPeople[i].gameObject.SetActive(true);

            imgPeople[3].sprite = _meta.visualEnemy["Face"];
            imgPeople[0].sprite = _meta.visualEnemy["Body"];
            imgPeople[1].sprite = _meta.visualEnemy["Hand"];
            imgPeople[2].sprite = _meta.visualEnemy["Hand"];
            imgPeople[4].sprite = _meta.visualEnemy["Leg"];
        }
        else if (_meta.visualEnemy.ContainsKey("Body"))
        {
            imgMutant.gameObject.SetActive(true);
            imgMutant.sprite = _meta.visualEnemy["Body"];
        }
        else if (_meta.visualEnemy.ContainsKey("Anomaly"))
        {
            imgAnomaly.gameObject.SetActive(true);
            imgAnomaly.sprite = _meta.visualEnemy["Anomaly"];
        }
    }
}
