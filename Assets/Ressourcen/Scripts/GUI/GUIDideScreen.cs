using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIDideScreen : MonoBehaviour
{
    [SerializeField] private GameObject screen;
    [SerializeField] private Image screenUp;
    [Space]
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textGroup;
    #region IMGS ESSENCE
    [SerializeField] private Image imgLight;
    [SerializeField] private Image imgMask;
    [SerializeField] private Image imgFace;
    [SerializeField] private Image imgBody;
    [SerializeField] private Image imgBody2;
    [SerializeField] private Image imgBackpack;
    [SerializeField] private Image imgGun;
    [SerializeField] private Image imgPistol;
    [SerializeField] private Image imgHandR;
    [SerializeField] private Image imgHandL;
    [SerializeField] private Image imgLeg;
    #endregion

    public void ToMenu() => SceneManager.LoadScene(0, LoadSceneMode.Single);
    public void Restart()
    {
        SaveHeandler.StartSession();
        SceneManager.LoadScene(SaveHeandler.SessionNow.idScene, LoadSceneMode.Single);
    }

    public void OnDeath(IMetaEssence _meta)
    {
        screen.SetActive(true);

        textName.text = _meta.Name;
        switch (_meta.Group)
        {
            case TypeGroup.millitary:
                textGroup.text = "�������";
                break;
            case TypeGroup.scientist:
                textGroup.text = "������";
                break;
            case TypeGroup.Mutant:
                textGroup.text = "�������";
                break;
            case TypeGroup.bandut:
                textGroup.text = "�������";
                break;
            case TypeGroup.clearSky:
                textGroup.text = "������ ����";
                break;
            case TypeGroup.svoboda:
                textGroup.text = "�������";
                break;
            case TypeGroup.dolg:
                textGroup.text = "����";
                break;
            case TypeGroup.stalker:
                textGroup.text = "��������";
                break;
        }

        ViewEssence viv = _meta.Visual;

        imgLight.gameObject.SetActive(viv.Light);
        imgMask.gameObject.SetActive(viv.Mask);
        imgBody2.gameObject.SetActive(viv.Body2);
        imgBackpack.gameObject.SetActive(viv.Backpack);
        imgGun.gameObject.SetActive(viv.Gun);
        imgPistol.gameObject.SetActive(viv.Pistol);
        imgBody.gameObject.SetActive(viv.Body);
        imgFace.gameObject.SetActive(viv.Face);
        imgHandL.gameObject.SetActive(viv.Hand);
        imgHandR.gameObject.SetActive(viv.Hand);
        imgLeg.gameObject.SetActive(viv.Leg);

        imgLight.sprite = viv.Light;
        imgMask.sprite = viv.Mask;
        imgFace.sprite = viv.Face;

        imgBody.sprite = viv.Body;
        imgBody2.sprite = viv.Body2;
        imgBackpack.sprite = viv.Backpack;
        imgGun.sprite = viv.Gun;
        imgPistol.sprite = viv.Pistol;

        imgHandL.sprite = viv.Hand;
        imgHandR.sprite = viv.Hand;

        imgLeg.sprite = viv.Leg;

        StartCoroutine(AnimDeth());
    }

    IEnumerator AnimDeth()
    {
        byte a = 0;
        while (a < 255)
        {
            a += 4;
            screenUp.color += new Color32(0, 0, 0, 4);
            yield return new WaitForSeconds(a / 100);
        }
    }
}
