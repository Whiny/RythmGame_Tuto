using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject stage = null;
    Stage _stage;
    GameObject[] stagePlates;

    [SerializeField] float offsetY = 3;
    [SerializeField] float plateSpeed = 10;

    int stepCount = 0;
    int totalPlateCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _stage = stage.GetComponent("Stage") as Stage;
        stagePlates = _stage.plates;
        totalPlateCount = stagePlates.Length;

        for(int i=0; i<totalPlateCount; i++)
        {
            stagePlates[i].transform.position = new Vector3(stagePlates[i].transform.position.x,stagePlates[i].transform.position.y + offsetY,stagePlates[i].transform.position.z);
        }
    }

    // Update is called once per frame
    public void ShowNextPlate()
    {
        if(stepCount<totalPlateCount)
            StartCoroutine(MovePlateCo(stepCount++));
    }

    IEnumerator MovePlateCo(int p_num)
    {
        stagePlates[p_num].SetActive(true);
        Vector3 t_destPos = new Vector3(stagePlates[p_num].transform.position.x,stagePlates[p_num].transform.position.y - offsetY,stagePlates[p_num].transform.position.z);
    
        while(Vector3.SqrMagnitude(stagePlates[p_num].transform.position - t_destPos) >= 0.001f)
        {
            stagePlates[p_num].transform.position = Vector3.Lerp(stagePlates[p_num].transform.position, t_destPos, plateSpeed * Time.deltaTime);
            yield return null;
        }
        stagePlates[p_num].transform.position = t_destPos;
    }
}
