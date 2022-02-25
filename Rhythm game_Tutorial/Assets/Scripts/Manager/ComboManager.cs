using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] GameObject goGomboImage = null;
    [SerializeField] UnityEngine.UI.Text txtCombo = null;

    int currentCombo = 0;
    int maxCombo = 0;
    Animator animator;
    string animComboUp = "ComboUp";

    void Start()
    {
        txtCombo.gameObject.SetActive(false);
        goGomboImage.SetActive(false);
        animator = GetComponent("Animator") as Animator;
    }

   public void InmcreaseCombo(int p_num = 1)
   {
       currentCombo += p_num;
       txtCombo.text = string.Format("{0:#,##0}",currentCombo);

       if(currentCombo >2)
       {
            if(maxCombo<currentCombo)
                maxCombo = currentCombo;

           txtCombo.gameObject.SetActive(true);
           goGomboImage.SetActive(true);
           animator.SetTrigger(animComboUp);
       }
   }

   public void ResetCombo()
   {
        currentCombo = 0;
        txtCombo.text = "0";
        txtCombo.gameObject.SetActive(false);
        goGomboImage.SetActive(false);
   }

   public int GetCurrentCombo()
   {
       return currentCombo;
   }

   public int GetMaxCombo()
   {
       return maxCombo;
   }
}
