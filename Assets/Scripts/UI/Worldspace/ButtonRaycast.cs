/**
 * @brief ≈‰≥¢ ¡÷πŒ
 * @details ≈‰≥¢ ¡÷πŒ (∑£¥˝«— Ω√∞£∏∂¥Ÿ ¥Á±Ÿ Get, AI)
 * @author ±ËπÃº∫
 * @date 22-04-26
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRaycast : MonoBehaviour
{
    
    protected virtual void Touched()
    {

    }
    void DetectTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
            {
                if (hit.collider.CompareTag("WorldSpaceUI") && hit.collider.name == this.name)
                {
                    Touched();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectTouch();
    }
}
