using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitCitizen : MonoBehaviour
{
    private Animator anim;
    private WaitForSeconds wait;
    private GameManager gameManager;

    [SerializeField]
    private string carrot = "100.0A";


    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        // ≈‰≥¢ Material º≥¡§
        int rand = Random.Range(0, 12);
        anim.SetInteger("SantaIndex", rand);


        wait = new WaitForSeconds(3f);

        StartCoroutine(GetCarrot());
    }

    IEnumerator GetCarrot()
    {
        while(true)
        {
           gameManager.MyCarrots += GoldManager.UnitToBigInteger(carrot);
           
            yield return wait;

        }
    }
}
