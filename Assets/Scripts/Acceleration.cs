using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Acceleration : MonoBehaviour
{
    
    
    [SerializeField, Tooltip("Nombre de charge maximum d'acceleration")]
    private int nombreChargeMax;

    [SerializeField, Tooltip("Nombre de charge maximum d'acceleration")]
    private float forceAcceleration;

    [SerializeField, Tooltip("Nombre de secondes avant de pouvoir réactiver l'acceleration")]
    private float delai;

    [SerializeField, Tooltip("Image des charges")]
    private Image charge1;
    [SerializeField, Tooltip("Image des charges")]
    private Image charge2;
    [SerializeField, Tooltip("Image des charges")]
    private Image charge3;

    // Nombre de charge d'acceleration disponible
    private int chargeAcceleration;

    // Référence au Rigidbody de la boule pour appliquer la physique.
    private Rigidbody rigidbody;

    // Décide si la boule peut acceler ou pas
    private bool peutAcellerer = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charge1.enabled = false;
        charge2.enabled = false;
        charge3.enabled = false;
        rigidbody = GetComponent<Rigidbody>();
        ControleurJeu.Instance.Controles.actions.FindAction("Acceleration").performed += CommencerAcceleration;
    }

    /// <summary>
    /// Donne une charge d'acceleration quand entre en contact avec l'objet d'acceleration
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (chargeAcceleration <= nombreChargeMax && chargeAcceleration > 0)
        {
            chargeAcceleration++;
        }
    }

    /// <summary>
    /// Accelere la balle quand la touche w est appuyer
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void CommencerAcceleration(InputAction.CallbackContext context)
    {
        if (peutAcellerer == true)
        {
            Vector3 direction = transform.forward;

            peutAcellerer = false;

            rigidbody.AddForce(direction * forceAcceleration, ForceMode.Acceleration);

            chargeAcceleration--;

            if (chargeAcceleration == 0)
            {
                charge1.enabled = false;
                charge2.enabled = false;
                charge3.enabled = false;
            }
            else if (chargeAcceleration == 1)
            {
                charge1.enabled = true;
                charge2.enabled = false;
                charge3.enabled = false;
            }
            else if (chargeAcceleration == 2)
            {
                charge1.enabled = true;
                charge2.enabled = true;
                charge3.enabled = false;
            }
            else
            {
                charge1.enabled = true;
                charge2.enabled = true;
                charge3.enabled = true;
            }

            Coroutine delaiAvantAcceleration = StartCoroutine(CommencerCollisionCoroutine());
        }
    }


    private IEnumerator CommencerCollisionCoroutine()
    {
        yield return new WaitForSeconds(delai);
        peutAcellerer = true;
    }
}
