using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarSemicircle : MonoBehaviour
{

    public float maxHp;
    public float currentHp;
    public float criticalHpPercent;
    public Image ContentCircle;
    public RectTransform ContentLine;
    private float width;
    public Text HpText;
    private float currentHpForText;
    private float criticalHp;
    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>(); // Determination the animator
        width = ContentLine.rect.width; // Define the object width "Content"
    }


    void Update()
    {

        HpText.text = ((int)currentHpForText).ToString(); // The output value "currentHpForText" in the text box
        currentHpForText = currentHp / maxHp * 100;

        criticalHp = maxHp / 100 * criticalHpPercent; // Calculation of criticalHP

        Vector3 position = ContentLine.anchoredPosition; // Move the object "Content" Y-axis
        position.x = width * currentHp / maxHp; // Calculation of displacement
        ContentLine.anchoredPosition = position;

        ContentCircle.fillAmount = currentHp / (maxHp / 2); // Converting object "ContentCircle"

        if (currentHp < 50) ContentLine.gameObject.SetActive(false);
        else ContentLine.gameObject.SetActive(true);

        if (currentHp < criticalHp) // Conditions start the animation "critical"
        {
            animator.SetBool("critical", true);
        }

        else
        {
            animator.SetBool("critical", false);
        }
    }
    void HealthControl(int value) // Limitation periods values "currentHp"
    {
        currentHp = Mathf.Clamp(value, 0, maxHp);

    }

}

