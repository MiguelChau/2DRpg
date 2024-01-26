using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private float sfxMinDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    private int bgmIndex;

    private bool canPlayerSFX;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;

        Invoke("AllowSFX", 1f);
    }

    private void Update()
    {
        if(!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }

    public void PlaySFX(int _sfxIndex, Transform _sourceSound)
    {
        //if (sfx[_sfxIndex].isPlaying)
        //  return;

        if (canPlayerSFX == false)
            return;

        if (_sourceSound != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _sourceSound.position) > sfxMinDistance)
            return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolumePlataform(sfx[_index]));

    IEnumerator DecreaseVolumePlataform(AudioSource _audio)
    {
        float dafaultVol = _audio.volume;

        while(_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;

            yield return new WaitForSeconds(.6f);

            if(_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = dafaultVol;
                break;
            }
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }
    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[_bgmIndex].Play();  
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX() => canPlayerSFX = true;
}
