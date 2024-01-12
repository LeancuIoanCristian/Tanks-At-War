using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class TankPartsManager
{
    [SerializeField] private Tank tankReference = new Tank();
    [SerializeField] private List<Hull> hull_options;
    [SerializeField] private List<Engine> engine_options;
    [SerializeField] private List<Gun> gun_options;
    [SerializeField] private List<Turret> turret_options;
    [SerializeField] private List<Tracks> tracks_options;
    [SerializeField] private List<Radio> radio_options;
    [SerializeField] private List<Ammo> ammo_options;


    public void RandomizedGivenTank(Tank tank_reference)
    {
        tank_reference.SetHull(hull_options[UnityEngine.Random.Range(0, hull_options.Count -1)]);
        tank_reference.GetHull().SetTracks(tracks_options[UnityEngine.Random.Range(0, tracks_options.Count - 1)]);
        tank_reference.SetTurret(turret_options[UnityEngine.Random.Range(0, turret_options.Count - 1)]);
        tank_reference.GetTurret().SetGun(gun_options[UnityEngine.Random.Range(0, gun_options.Count - 1)]);
        tank_reference.GetTurret().SetRadio(radio_options[UnityEngine.Random.Range(0, radio_options.Count - 1)]);
        tank_reference.GetTurret().GetGun().SetAmmo(ammo_options[UnityEngine.Random.Range(0, radio_options.Count - 1)]);
    }

    public void CreateNewTank()
    {
        RandomizedGivenTank(tankReference);
    }


}

