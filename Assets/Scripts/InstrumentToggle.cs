using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentToggle : MonoBehaviour {

    public Sprite buttonUp;
    public Sprite buttonDown;

    private UnityEngine.UI.Image _renderer;
	// Use this for initialization
	void Start () {
        _renderer = GetComponent<UnityEngine.UI.Image>();
        ShipInternals.Instance.RegisterInstrumentToggle(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void Toggle(bool depressed)
    {
        SetSprite(depressed);
    }

    private void SetSprite(bool depressed)
    {
        if (depressed)
        {
            _renderer.sprite = buttonDown;
        }
        else
        {
            _renderer.sprite = buttonUp;
        }
    }
}
