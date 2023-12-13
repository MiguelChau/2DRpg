using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotKey : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackholeSkillController blackHole;

    //Método usado para configurar as propriedades iniciais da tecla de atalho.
    //Recebe a tecla de atalho, uma referência ao inimigo associado e uma referência ao controlador do skill
    public void SetupHotKey(KeyCode _myNewHotKey, Transform _myEnemy, BlackholeSkillController _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(myHotKey)) 
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }

}
