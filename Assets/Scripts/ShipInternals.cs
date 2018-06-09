using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInternals : MonoBehaviour {

    public static ShipInternals Instance { get; private set; }

    private float _powerFailDrainRate = 8.0f;

    private float _batteryChargeMax = 2000.0f;
    private float _batteryDrainRate = 200.0f;
    private float _batteryPowerProvided = 20.0f;
    private float _batteryLowThreshold = 500.0f;
	public float BatteryCharge { get; set; }

    private float _solarChargeMax = 100.0f;
    private float _solarChargeRate = 10.0f;
    private float _solarDrainRate = 7.0f;
    private float _solarPowerProvided = 50.0f;
    public float SolarCharge { get; set; }

    private float _solarDeployMax = 100.0f;
    private float _solarDeployRate = 20.0f;
    public float SolarDeployStatus { get; set; }
    

    private float _maxAvailablePower = 100.0f;
    public float UsedPower { get; set; }
	public float AvailablePower { get; set; }
	public int PowerSource{ get; set; }

    private Dictionary<string, InstrumentLight> _lights = new Dictionary<string, InstrumentLight>();
    private Dictionary<string, InstrumentToggle> _toggles = new Dictionary<string, InstrumentToggle>();
    private Dictionary<string, InstrumentGauge> _gauges = new Dictionary<string, InstrumentGauge>();

    public void RegisterInstrumentLight(InstrumentLight light)
    {
        _lights.Add(light.gameObject.name, light);
    }

    public void RegisterInstrumentToggle(InstrumentToggle toggle)
    {
        _toggles.Add(toggle.gameObject.name, toggle);
    }

    public void RegisterInstrumentGauge(InstrumentGauge gauge)
    {
        _gauges.Add(gauge.gameObject.name, gauge);
    }

	// Use this for initialization
	void Awake () {
        Instance = this;
	}

    void Start ()
    {
        PowerSource = -1;
        BatteryCharge = _batteryChargeMax * 0.75f;
        OnZeroPower();
    }

    // Update is called once per frame
    void Update () {
        UpdatePowerBus();
        UpdateBatteryPanel();
        UpdatePowerBusStatusPanel();
        UpdatePowerBusControlPanel();
	}

	public void OnButton_PowerSource(int source)
    {
        ChangePowerSource(source);
    }

    private void UpdatePowerBus()
    {
        var availablePowerThisFrame = AvailablePower;
        if (PowerSource == -1)
        {
            availablePowerThisFrame = AvailablePower - (_powerFailDrainRate * Time.deltaTime);
            if (availablePowerThisFrame <= 0)
            {
                OnZeroPower();
            }
        }
        else if (PowerSource == 0)
        {
            if (BatteryCharge > 0)
            {
                availablePowerThisFrame = _batteryPowerProvided;
                BatteryCharge -= _batteryDrainRate * Time.deltaTime;
            }
            else
            {
                ChangePowerSource(-1);
            }
        }
        else if (PowerSource == 1)
        {
            if (SolarCharge >= _solarChargeMax)
            {
                availablePowerThisFrame = _solarPowerProvided;
            }
            else
            {
                ChangePowerSource(-1);
            }
        }
        else if (PowerSource == 2)
        {
            ChangePowerSource(-1);
        }

        _gauges["i_availablePowerMeter"].MoveToValue(availablePowerThisFrame / _maxAvailablePower);
        if (AvailablePower <= 0 && availablePowerThisFrame > 0)
        {
            OnPowerRestored();
        }
        AvailablePower = availablePowerThisFrame;
    }

    private void UpdateBatteryPanel()
    {
        _lights["l_batteryLow"].Toggle(BatteryCharge <= _batteryLowThreshold);
        _lights["l_batteryCharge"].Toggle(PowerSource > 0);
        _lights["l_batteryDrain"].Toggle(PowerSource == 0);
        _gauges["i_batteryPowerMeter"].MoveToValue(BatteryCharge / _batteryChargeMax);
    }

    private void UpdatePowerBusStatusPanel()
    {
        _lights["l_insufPower"].Toggle(UsedPower > AvailablePower);
        _lights["l_lowPower"].Toggle(AvailablePower < _maxAvailablePower * 0.6f);
        _lights["l_failPower"].Toggle(PowerSource < 0);
    }

    private void UpdatePowerBusControlPanel()
    {
        _lights["l_batteryReady"].Toggle(BatteryCharge > 0);
        _lights["l_auxCharge"].Toggle(SolarDeployStatus >= _solarDeployMax);
        _lights["l_auxReady"].Toggle(SolarCharge >= _solarChargeMax);
    }

    private void ChangePowerSource(int source)
    {
        PowerSource = source;

        _lights["l_batteryActive"].Toggle(source == 0);
        _lights["l_auxActive"].Toggle(source == 1);
        _lights["l_mainActive"].Toggle(source == 2);

        _toggles["btn_batteryPower"].Toggle(source == 0);
        _toggles["btn_auxPower"].Toggle(source == 1);
        _toggles["btn_mainPower"].Toggle(source == 2);
    }

    private void OnZeroPower()
    {
        foreach (var light in _lights)
        {
            light.Value.SetPower(false);
        }

        foreach (var gauge in _gauges)
        {
            gauge.Value.SetPower(false);
        }
    }

    private void OnPowerRestored()
    {
        foreach (var light in _lights)
        {
            light.Value.SetPower(true);
        }

        foreach (var gauge in _gauges)
        {
            gauge.Value.SetPower(true);
        }
    }
}
