using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour
{
    public ParticleSystem kam;

    public void DoDialog()
    {
        DialogManager.Instance.PopUpDialog("AMBATAKAM", gameObject.transform.position);
        kam.Play();
    }
}
