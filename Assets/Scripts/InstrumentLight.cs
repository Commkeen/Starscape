using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class InstrumentLight : MonoBehaviour {

    private static int _info = 0;
    private static int _warn = 1;
    private static int _err = 2;

    private static string[] _unlitDefault = { "lights_0", "lights_1", "lights_2" };
    private static string[] _litDefault = { "lights_3", "lights_4", "lights_5" };

    private bool _hasPower;

    public int color;
    public bool active;
    public float flash;

    public SpriteAtlas lights;

    private Image _renderer;
	// Use this for initialization
	void Start () {
        var lightArray = new Sprite[lights.spriteCount];
        lights.GetSprites(lightArray);
        _renderer = GetComponent<Image>();
        color = GetColorFromSpriteName(_renderer.sprite.name);
        //SetSprite(0, true);
        ShipInternals.Instance.RegisterInstrumentLight(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPower(bool hasPower)
    {
        _hasPower = hasPower;
        SetSprite(color, active && _hasPower);
    }

	public void Toggle(bool active, float flash=0){
		this.active = active;
        SetSprite(color, active && _hasPower);
	}

	private void SetSprite(int type, bool lit){
		if (lit) {
            _renderer.sprite = lights.GetSprite(_litDefault[type]);
		} else {
            _renderer.sprite = lights.GetSprite(_unlitDefault[type]);
        }
	}

    private int GetColorFromSpriteName(string name)
    {
        int color = System.Array.IndexOf(_unlitDefault, name);
        if (color < 0)
            color = System.Array.IndexOf(_litDefault, name);
        if (color < 0)
            throw new System.ArgumentOutOfRangeException(name);
        return color;
    }
}
