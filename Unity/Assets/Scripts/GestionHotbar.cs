using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.Environment;
using YounGenTech.HealthScript;

public class GestionHotbar : MonoBehaviour
{
    private Totem totemJugador;
    public GameObject inventory;
    private Inventory mainInventory;
    private Tooltip toolTip;

    private InputManager inputManagerDatabase;

    private float ataqueJugadorActual;
    private float vidaJugadorActual;
    private float previoAtaqueJugador;
    private float previoVidaJugador;

    int normalSize = 3;
    private GameManager gestorJuego;
    
    public void OnEnable()
    {
        Inventory.ItemConsumed += OnConsumeItem;
    }

    public void OnDisable()
    {
        Inventory.ItemConsumed -= OnConsumeItem;
    }

    void Start()
    {
        if (inputManagerDatabase == null)
            inputManagerDatabase = (InputManager)Resources.Load("InputManager");



        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
            toolTip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
        if (inventory != null)
            mainInventory = inventory.GetComponent<Inventory>();
      

    }

    public void OnConsumeItem(Item item)
    {
        Totem totemActual = GameManager.Instance.totemActual;
        for (int i = 0; i < item.itemAttributes.Count; i++)
        {

            if (item.itemAttributes[i].attributeName == "Vida")
            {
                totemActual.aumentarVida(item.itemAttributes[i].attributeValue);
                Debug.Log("Objeto de vida");

            }


            if (item.itemAttributes[i].attributeName == "Ataque")
            {
                ataqueJugadorActual += item.itemAttributes[i].attributeValue;
            }

            if (item.itemAttributes[i].attributeName == "Cohete")
            {
                totemActual.GetComponent<MovimientoController>().saltar(item.itemAttributes[i].attributeValue);
                Debug.Log("Objeto de Cohete salto " + item.itemAttributes[i].attributeValue);
                GameObject cohete = Instantiate(item.itemModel, totemActual.transform.position, Quaternion.identity) as GameObject;

                cohete.transform.GetChild(0).gameObject.AddComponent<Cohete>();
                cohete.transform.GetChild(0).gameObject.AddComponent<CircleCollider2D>();
                cohete.transform.parent = totemActual.transform;
            }

            // Raig: permet moure's més ràpid i tenir més passos límit.

            if (item.itemAttributes[i].attributeName == "Movimiento")
            {
                totemActual.gameObject.GetComponent<MovimientoController>().VelocidadMovimiento += item.itemAttributes[i].attributeValue;
                Debug.Log("Aumento la velocidad de movimiento en  " + item.itemAttributes[i].attributeValue);

            }

            //Coet: propulsa cap a dalt, mantenint pres el botó de salt es manté més en l'aire (air control).
            //  if (item.itemID== Global.TIPO_OBJETOS.objetoCohete)
            //  {
            //ataqueJugadorActual += item.itemAttributes[i].attributeValue;
            //      totemActual.gameObject.transform.position+=new Vector3(0,50,0); 
            //   }
        }

        switch (item.itemID)
        {
            // Bola de teletransport: disparar-la per teletransportar-se a una posició més endavant.
            case Global.TIPO_OBJETOS.objetoTeletransporte:

                //Debug.Log("Objeto de Cohete teletransporte " + item.itemAttributes[i].attributeValue);
                Debug.Log("Objeto de Cohete teletransporte");
                GameObject teletransporte = Instantiate(item.itemModel, totemActual.transform.position, Quaternion.identity) as GameObject;

                teletransporte.transform.GetChild(0).gameObject.AddComponent<Teletransporte>();
                teletransporte.transform.GetChild(0).gameObject.AddComponent<CircleCollider2D>();
                teletransporte.transform.parent = totemActual.transform;
                break;



            case Global.TIPO_OBJETOS.objetoAngel:

                //Debug.Log("Objeto Angel de la Guarda " + item.itemAttributes[i].attributeValue);
                GameObject angel = Instantiate(item.itemModel, totemActual.transform.position, Quaternion.identity) as GameObject;
                angel.GetComponent<SpriteRenderer>().enabled = false;
                //angel.SetActive(false);
                angel.gameObject.AddComponent<Angel>();
                angel.transform.parent = totemActual.transform;
                totemActual.ActivarAngelGuarda();
                break;


            case Global.TIPO_OBJETOS.objetoIglu:

                //Debug.Log("Objeto Iglu " + item.itemAttributes[i].attributeValue);
                GameObject iglu = Instantiate(item.itemModel, totemActual.transform.position, Quaternion.identity) as GameObject;

                iglu.transform.GetChild(0).gameObject.AddComponent<Iglu>();
                iglu.transform.GetChild(0).gameObject.AddComponent<CircleCollider2D>();
                iglu.transform.parent = totemActual.transform;
                break;


            case Global.TIPO_OBJETOS.objetoEscudoSimple:

                //Debug.Log("Objeto Escudo " + item.itemAttributes[i].attributeValue);
                GameObject escudo = Instantiate(item.itemModel, totemActual.transform.position, Quaternion.identity) as GameObject;
                escudo.transform.Rotate(transform.rotation.x, 90, transform.rotation.z);
                escudo.transform.GetChild(0).gameObject.AddComponent<Escut>();
                escudo.transform.GetChild(0).gameObject.AddComponent<CircleCollider2D>();
                escudo.transform.parent = totemActual.transform;
                break;


            case Global.TIPO_OBJETOS.objetoEscudoDoble:

                //Debug.Log("Objeto Escudo Doble " + item.itemAttributes[i].attributeValue);
                GameObject escudo_doble = Instantiate(item.itemModel, totemActual.transform.position, Quaternion.identity) as GameObject;
                escudo_doble.transform.Rotate(transform.rotation.x, 90, transform.rotation.z);
                escudo_doble.transform.GetChild(0).gameObject.AddComponent<EscutDoble>();
                escudo_doble.transform.GetChild(0).gameObject.AddComponent<CircleCollider2D>();
                escudo_doble.transform.parent = totemActual.transform;
                break;

        }
        GameManager.Instance.eliminarItem(item);

    }
    // Update is called once per frame
    void Update()
    {
    }

}