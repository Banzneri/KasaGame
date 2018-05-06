using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour, IActionObject {

    #region Settings

    [Header("General")]

    // Reference to the Fireball object
    [SerializeField]
    private Fireball _Fireball;

    // Whether spawner is activated on start
    [SerializeField]
    private bool _ActivateOnStart;



    [Header("Spawning")]

    // Rate of spawning
    [SerializeField]
    private float _SpawnRateMin;
        
    [SerializeField]
    private float _SpawnRateMax;

    // Whether balls are spawned from the center only
    [SerializeField]
    private bool _FromCenterOnly;

    // Area of spawning
    [SerializeField]
    private float _SpawnAreaX, _SpawnAreaZ;



    [Header("Fireball")]

    // Size of Fireball
    [SerializeField]
    private float _BallSizeMin;

    [SerializeField]
    private float _BallSizeMax;

    // Speed of fireballs
    [SerializeField]
    private float _BallSpeedMin, _BallSpeedMax;

    // Lifetime of fireballs
    [SerializeField]
    private float _BallLifeMin, _BallLifeMax;

    #endregion

    #region Private Variables

    // Tracks time
    private float _Timer;

    // Between _SpawnRateMin & _SpawnRateMax
    private float _NextSpawn;

    // Is Spawner Activated
    private bool _Activated;

    #endregion

    #region Methods

    // Use this for initialization
    void Start () {
        _Activated = _ActivateOnStart;
        _Timer = 0;
        _NextSpawn = Random.Range(_SpawnRateMin, _SpawnRateMax);
	}
	
	// Update is called once per frame
	void Update () {

        if (_Activated)
        {
            _Timer += Time.deltaTime;
            if(_Timer >= _NextSpawn)
            {
                SpawnFireball();
            }
        }
	}

    // Activates the Spawner
    public void Action()
    {
        _Activated = !_Activated;
    }

    // Spawns new fireball
    private void SpawnFireball()
    {
        // Reset timer
        _Timer = 0;
        _NextSpawn = Random.Range(_SpawnRateMin, _SpawnRateMax);

        // Instantiate object and set Spawner as its parent
        Fireball _Object = Instantiate(_Fireball, new Vector3(0, 0, 0), _Fireball.transform.rotation);
        _Object.transform.parent = this.transform;

        // Calculate position and other variables
        Vector3 _SpawnPos = new Vector3(0, -1, 0);

        if (!_FromCenterOnly)
        {
            _SpawnPos.Set(Random.Range(-_SpawnAreaX / 2, _SpawnAreaX / 2), -1, Random.Range(-_SpawnAreaZ / 2, _SpawnAreaZ / 2));
        }

        float _Speed = Random.Range(_BallSpeedMin, _BallSpeedMax);
        float _LifeTime = Random.Range(_BallLifeMin, _BallLifeMax);
        float _Size = Random.Range(_BallSizeMin, _BallSizeMax);

        // Initialize Fireball
        _Object.InitializeObject(_SpawnPos, _Speed, _LifeTime, _Size);
    }

    // Show the spawn area
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        if (_FromCenterOnly)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(1.5f, 5, 1.5f));
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(_SpawnAreaX, 1, _SpawnAreaZ));
        }
    }

    #endregion

}
