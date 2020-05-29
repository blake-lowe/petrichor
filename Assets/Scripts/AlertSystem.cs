using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSystem : MonoBehaviour
{

	public bool AlarmTrigger = false;
	private float TimeToDisableAlarm = 0;
	public float AlarmDelay = 10;

    // Start is called before the first frame update
    /*
    private void Start()
    {
        
    }
    */

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (AlarmTrigger == true)
        {
        	if (Time.time > TimeToDisableAlarm)
        	{
        		AlarmTrigger = false;
        	}
        }
    }

    public void SoundAlarm()
    {
    	AlarmTrigger = true;
    	TimeToDisableAlarm = Time.time + AlarmDelay;
    }
}
