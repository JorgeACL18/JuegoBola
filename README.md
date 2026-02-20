# Juego Roll-a-Ball
Para esta tarea tuvimos que seguir el tutorial oficial de Unity "Roll-a-Ball", con el cual somos capaces de crear un juego donde tú (la bola) tienes que recoger unos ítems para ganar mientras que un enemigo te persigue.

## Primeros pasos: Plano y Bola (Jugador)
Lo primero que nos manda a hacer el tutorial es crear un plano de juego que tenga paredes, obstáculos y algunas rampas.

Después, creamos la bola que controlaremos. Tenemos que cambiar el material y el color de la misma, añadirle componentes como `RigidBody` para que se vea afectado por las leyes de la física y, lo más importante, modificarla utilizando código. Esto último nos permitirá controlar la bola como también los textos de la IU que vamos a añadir más adelante.

También, tenemos que configurar la ubicación de la luz y de la cámara para el juego. La cámara también tendrá un pequeño bloque de código ya que su función será seguir al jugador desde la perspectiva en la que la pusimos.

<img width="1103" height="629" alt="Captura desde 2026-02-20 10-25-12" src="https://github.com/user-attachments/assets/9579ffd8-12aa-454a-8301-c3ae2c13eb12" />

<img width="1103" height="629" alt="Captura desde 2026-02-20 10-30-30" src="https://github.com/user-attachments/assets/379f3911-31e5-442c-8ba5-d53cfead7a88" />

Así quedaría nuestro plano con la bola y la cámara. 

El código, debería verse así (por ahora):

Bola (Jugador)
~~~ c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BolaGuaoController : MonoBehaviour
{
    public float speed = 0;
    private Rigidbody rb;
    private float moveX;
    private float moveY;

    void Start()
    {
        rb = GetComponent <Rigidbody>();
    }

    void OnMove (InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); 

        moveX = movementVector.x;
        moveY = movementVector.y;
    }

     void FixedUpdate() 
    {
        Vector3 movement = new Vector3(moveX, 0.0f, moveY);
        rb.AddForce(movement * speed); 
    }
}
~~~

Cámara:
~~~ c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject bola;
    private Vector3 offset;
    
    void Start()
    {
        offset = transform.position - bola.transform.position;
    }
    
    void LateUpdate()
    {
        transform.position = bola.transform.position + offset;
    }
}
~~~

---

## Coleccionables
En el tutorial de Unity, nos pide que creemos unos coleccionables, en este caso unos pequeños cubos que rotan constantemente.

Para esto tenemos que utilizar `Prefab`, lo cual se reduce a simplemente tener el objeto dentro de una carpeta que hayamos creado. Este objeto se puede duplicar para añadir la cantidad de cubos que queramos, por mi parte solo añadí 8 cubos.

Como se puede suponer, estos cubos, para que roten, constan de un pequeño bloque de código, el cual es el siguiente:

~~~ c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{ 
    
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}

~~~

En el plano se verá de la siguiente forma:

<img width="1103" height="629" alt="Captura desde 2026-02-20 10-36-29" src="https://github.com/user-attachments/assets/a4913699-8909-4e28-acbe-d546b4faf0ab" />

Por útlimo, al ser coleccionables,tenemos que crear una etiqueta y ponérsela al Prefab que creamos y, además, tenemos que habiliar la función `is Trigger` para que, cuando la bola toque el cubo, el coleccionable desaparece.

Como es de esperarse, hay que modificar el código de la bola para que, con el uso de la etiqueta que funciona como variable, podamos hacer que el cubo desaparezca cuando lo tocamos:

~~~ c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BolaGuaoController : MonoBehaviour
{
    public float speed = 0;
    public GameObject winText;
    private Rigidbody rb;
    private float moveX;
    private float moveY;

    void Start()
    {
        rb = GetComponent <Rigidbody>();
    }

    void OnMove (InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); 

        moveX = movementVector.x;
        moveY = movementVector.y;
    }

     void FixedUpdate() 
    {
        Vector3 movement = new Vector3(moveX, 0.0f, moveY);
        rb.AddForce(movement * speed); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            other.gameObject.SetActive(false);
        }
    }
}

~~~

---

## Interfaz:
Como dije antes en el apartado de `Primeros pasos`, el tutorial nos manda a añadir una IU, el cual contiene dos textos. El primero se trata de el contador de cubos que recolectamos y el segudno es el texto de si ganaste o perdiste (este segundo texto se verá más adelante)

Bien, para añadir esta IU solo tenemos que añadir, como si fuera un objeto, una UI, ya una vez introducido este objeto solo queda añadir los dos textos que queremos. Quedaría así:

<img width="1103" height="629" alt="Captura desde 2026-02-20 10-47-33" src="https://github.com/user-attachments/assets/21b15133-d361-4645-b912-9381cfe18f33" />

El texto "Count text" funciona como 'placeholder' para lo que queremos mostrar. Para esto, solo tenemos que modificar el código de la bola para que salga el contador y el texto "You Win!" de esta forma:

~~~ c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BolaGuaoController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winText;
    private Rigidbody rb;
    private int count;
    private float moveX;
    private float moveY;

    void Start()
    {
        rb = GetComponent <Rigidbody>();
        count = 0;

        SetCountText();
        winText.SetActive(false);
    }

    void OnMove (InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); 

        moveX = movementVector.x;
        moveY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count == 7){
            countText.text = "Count: " + count.ToString() + " Only one left!";
        }

        if (count >= 8)
        {
            winText.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

     void FixedUpdate() 
    {
        Vector3 movement = new Vector3(moveX, 0.0f, moveY);
        rb.AddForce(movement * speed); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }
}
~~~

---

## Enemigo
Para crear un enemigo, primero, tenemos que añadir un objeto (cualquiera), para que funcione de manera que nos persiga y, cuando entre en contacto con la bola, hayamos perdido y desaparece la bola.

En mi caso, decidí importar un modelo de Internet (el caparazón rojo de Mario Kart 7 para la 3DS) para que sea mi enemigo. Lo ponemos a una distancia que esté algo alejado de la bola, le añadimos las colisiones necesarias, el RigidBody y, por último, pasar al código. El código del enemigo debería verse así y está diseñado para que persiga la bola en todo momento:

~~~ c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShell : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        if (player != null)
       {    
           navMeshAgent.SetDestination(player.position);
       }
        
    }
}
~~~

Lo que pasa en con el enemigo es algo curioso ya que no solo vale con ponerle código, sino que tenemos que añadir dos componentes: `NavMeshAgent`, el cual se encarga de la IA que utiliza la bola, y `NavMeshSurface`, este se pone directamente al plano para que la IA sepa por donde puede moverse. Por lo que nuestro plano debería verse así ahora:

<img width="1103" height="629" alt="Captura desde 2026-02-20 11-00-19" src="https://github.com/user-attachments/assets/e33f0c09-50bc-4b1d-8042-f330b8a5ab0d" />

Como último paso, modificamos el código de nuestra bola para que, cuando el enemigo nos atrape, aparezca el texto de "You Lose!":

~~~ c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BolaGuaoController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winText;
    private Rigidbody rb;
    private int count;
    private float moveX;
    private float moveY;

    void Start()
    {
        rb = GetComponent <Rigidbody>();
        count = 0;

        SetCountText();
        winText.SetActive(false);
    }

    void OnMove (InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); 

        moveX = movementVector.x;
        moveY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count == 7){
            countText.text = "Count: " + count.ToString() + " Only one left!";
        }

        if (count >= 8)
        {
            winText.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

     void FixedUpdate() 
    {
        Vector3 movement = new Vector3(moveX, 0.0f, moveY);
        rb.AddForce(movement * speed); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject); 
            winText.gameObject.SetActive(true);
            winText.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
}
~~~

