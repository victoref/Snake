/*Realizada:
 * Victor Emiliano Fernandez Rubio
 * Antonio Luis Suarez Solis*/

//Profesor: Jaime Sanchez Hernandez

/*Practica de la asignatura FUNDAMENTOS DE LA PROGRAMACION de la UCM*/

using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Dir { Norte, Sur, Este, Oeste, Null} //enumeracion de todas las direcciones posibles

class ListaEnlazada
{
    
    public class Nodo 
    {
        public int x, y;


        public Nodo sig; // enlace al siguiente nodo

        // constructor 
        public Nodo(int i, int j)
        {
            x = i; y = j;
        }
    }

    // atributos de la lista enlazada: referencia al primero y al último		
    public Nodo pri, ult, act;

    // constructora de listas
    public ListaEnlazada()
    {
        pri = ult = act = null;
    }

    // obtener primer elto de la lista
    public void primero(out int i, out int j)
    {
        if (pri == null) throw new Exception("Error primero: lista vacia");
        i = pri.x; j = pri.y;
    }

    // obtener ultimo elto de la lista
    public void ultimo(out int i, out int j)
    {
        if (ult == null) throw new Exception("Error ultimo: lista vacia");
        i = ult.x; j = ult.y;
    }

    // ver si un elto esta en la lista
    public bool esta(int i, int j)
    {
        // iniciamos con aux al ppio
        Nodo aux = pri;

        // busqueda: avanzazmos con aux mientras no llegemoa al final y no encontremos elto
        while (aux != null && (aux.x != i || aux.y != j)) aux = aux.sig;

        // si no hemos llegado el final, es pq el elto está en la lista
        return (aux != null);
    }

    // insertar elto al ppio de la lista
    public void insertaIni(int x, int y)
    {
        // si la lista es vacia creamos nodo y apuntamos a el pri y ult
        if (pri == null)
        {
            pri = new Nodo(x, y);
            pri.sig = null;
            ult = pri;
        }
        else
        { // si no es vacia creamos nodo y lo enganchamos al ppio
            Nodo aux = new Nodo(x, y);
            aux.sig = pri;
            pri = aux;
        }
    }


    // insertar elto al final de la lista
    public void insertaFin(int x, int y)
    {
        // si es vacia creamos nodo y apuntamos a el ppi y ult
        if (pri == null)
        {
            pri = new Nodo(x, y);
            pri.sig = null;
            ult = pri;
        }
        else
        { // si no, creamos nodo apuntado por ult.sig y enlazamos
            ult.sig = new Nodo(x, y);
            ult = ult.sig;
            ult.sig = null;
        }
    }



    // elimina elto dado de la lista, si esta
    public void eliminaElto(int x, int y)
    {
        // lista vacia
        if (pri == null) throw new Exception("Error eliminaElto: lista vacia");
        else
        {
            // eliminar el primero
            if (x == pri.x && y == pri.y)
            {
                // si solo tiene un elto
                if (pri == ult)
                    pri = ult = null;
                // si tiene más de uno
                else
                    pri = pri.sig;
            }
            // eliminar otro distinto al primero
            else
            {
                // busqueda. aux al ppio
                Nodo aux = pri;
                // recorremos lista buscando el ANTERIOR al que hay que eliminar (para poder luego enlazar)
                while (aux.sig != null && (x != aux.sig.x || y != aux.sig.y))
                    aux = aux.sig;

                // si lo encontramos
                if (aux.sig != null)
                {
                    // si es el ultimo cambiamos referencia al ultimo
                    if (aux.sig == ult)
                        ult = aux;
                    // puenteamos
                    aux.sig = aux.sig.sig;
                }
            }
        }
    }

    // elimina primer elto de la lista
    public void eliminaIni()
    {
        if (pri == null) throw new Exception("Error eliminaIni: lista vacia");
        if (pri == ult) pri = ult = null;
        else pri = pri.sig;
    }



    // inicizlización del iterador. Lo colocamos al ppio
    public void iniciaRecorrido()
    {
        act = pri;
    }

    public bool dame_actual_y_avanza(out int x, out int y)
    {
        x = y = 0;
        // si estamos al final, ya no hay actual y devolvemos false
        if (act == null)
            return false;
        else
        { // si no, info del nodo, avanzamos act y devolvemos true
            x = act.x;
            y = act.y;
            act = act.sig;
            return true;
        }

    }

    public void verLista()
    {//Este método estaba en comentarios porque es auxiliar, así que no se tiene que usar
        Console.Write("Lista: ");
        Nodo aux = pri;
        while (aux != null)
        {
            Console.Write("(" + aux.x + "," + aux.y + ") ");
            aux = aux.sig;
        }
        Console.Write("\n\n");
    }
}

class Estado
{
    Random rnd = new Random();
    //Ancho y el alto que tendra el rectangulo de juego
    private int fils;
    private int cols;

    //Las direccion de la serpiente.
    private int dirx;
    private int diry;

   
    public ListaEnlazada serp;
    public ListaEnlazada frutas;

    public Estado(int ancho, int alto)
    {
        cols = ancho;
        fils = alto;

        dirx = 1;//le damos a la serpiente una direccion de partida 
        diry = 0;
        
        serp = new ListaEnlazada();//inicializamos las listas, una para serpiente y otra para fruta
        frutas = new ListaEnlazada();

        serp.insertaFin(cols / 2, fils / 2);

    }


    public bool colision(int x, int y)  //controla si colisiona con los margenes del tablero
    {
        return (x < 1 || x > cols || y < 1 || y > fils || serp.esta(x, y));
    }


    //Metodo para hacer avanzar a la serpiente
    public void avanza()
    {
        int x; //declaramos un x y un y 
        int y;
        serp.ultimo(out x, out y);//los valores de x e y seran modificados el ultimo elmento de serp

        x += dirx; //le añadimos la dirreccion 
        y += diry;

        //añadimos siempre un nuevo nodo y dependiendo del tablero se realiza una opcion u otra
        if (!colision(x, y))
        {
            serp.insertaFin(x, y);//avanzamos en uno a la serp

            if (frutas.esta(x, y))//si no hay colision pero si hay fruta, eliminamos fruta
                frutas.eliminaElto(x, y);
            else//sino eliminamos el primer elemento de la lista que realmeente en el juego es la cola, es decir el que hemos añadido al principio
				serp.eliminaIni();
        }
        else
            throw new Exception("Colisión");


    }


    public void cambiaDir(Dir dir)
    {
        switch (dir) //un swicth con tantos casos como direcciones hay
        {
            //para cada una de las posibles direcciones modificamos la x y la y añadiendo o restando 1
            case Dir.Norte:
                dirx = 0;
                diry = -1;
                break;
            case Dir.Sur:
                dirx = 0;
                diry = 1;
                break;
            case Dir.Este:
                dirx = 1;
                diry = 0;
                break;
            case Dir.Oeste:
                dirx = -1;
                diry = 0;
                break;
            case Dir.Null:
                break;
        }
    }


    public void ponFruta()
    {
        int x;
        int y;
        do
        {          
            x = rnd.Next(1, cols);//generamos dos numeros randoms que se encuentren dentro del tablero
            y = rnd.Next(1, fils);//y sera en esa posicion donde coloquemos una fruta si el hueco esta libre

        }while (serp.esta(x, y));
        frutas.insertaFin(x, y);
    }


    public void dibuja()
    {
        //medieante dos bucles pintamos tanto los margenes superior e inferior
        //como el derecho y el izquierdo

        for (int x = 0; x < cols + 2; x++)
        {
            Console.SetCursorPosition(x * 2, 0);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write("  ");

            Console.SetCursorPosition(x * 2, fils + 1);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write("  ");
        }

        for (int y = 0; y < fils + 2; y++)
        {
            Console.SetCursorPosition(0, y);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write("  ");

            Console.SetCursorPosition((cols + 1) * 2, y);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write("  ");

        }

        int i;
        int j;
        
        serp.iniciaRecorrido();//iniciamos la lista de serp
    
        while (serp.dame_actual_y_avanza(out i, out j)) //pintamos el cuerpo de la serpiente
        {
            Console.SetCursorPosition(i * 2, j);
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("  ");
        }
        serp.ultimo(out i, out j); //modificamos la i y la j con la ultima posicion de la lista (la cabeza de serp)

        Console.SetCursorPosition(i * 2, j);//y la dibujamos 
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.Write(":>");


        frutas.iniciaRecorrido();//iniciamos la lista de frutas

        while (frutas.dame_actual_y_avanza(out i, out j))
        {
            Console.SetCursorPosition(i * 2, j);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("  ");

        }

    }

}

namespace snake
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            int i = 0;
            int j = 0;

            int ancho;
            int alto;
            int velocidad;

            int cont = 0;


            //preguntamos al jugador las dimensiones que quiere que tenga su juego y la velocidad a la que se ejecute
            Console.WriteLine("Introduce las dimensiones del rectángulo de juego: ");

            Console.Write("Ancho: ");
            ancho = int.Parse(Console.ReadLine());

            Console.Write("Alto: ");
            alto = int.Parse(Console.ReadLine());

            Console.Write("Velocidad del juego (ms): ");
            velocidad = int.Parse(Console.ReadLine());
            Console.Clear();

            Estado estado = new Estado(ancho, alto);

            bool colision = false;

            estado.serp.ultimo(out i, out j);

            while (!colision)//mientras colision se falso (bucle del juego)
            {
                if (cont == 7)//cada vez que cont llegue a la cifra predeterminada se añadira una fruta
                {
                    estado.ponFruta();
                    cont = 0;
                    
                }   
                      
                estado.cambiaDir(leeEntrada());
                estado.serp.ultimo(out i, out j);
                try
                {
                    estado.avanza();
                }
                catch {
                    colision = true;
                   // Console.WriteLine("vaya te has chocado!!");
                    Console.ReadLine();

                }
                estado.dibuja();
                Thread.Sleep(velocidad);
                 cont++;

                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
            }

        }
        static Dir leeEntrada() //metodo que lee las entradas de teclado y que devolvera una dirreccion
        {
            Dir d = Dir.Null;
            if (Console.KeyAvailable)
            {
                string tecla = Console.ReadKey().Key.ToString();
                switch (tecla)
                {
                    case "LeftArrow":
                        d = Dir.Oeste;
                        break;
                    case "UpArrow":
                        d = Dir.Norte;
                        break;
                    case "RightArrow":
                        d = Dir.Este;
                        break;
                    case "DownArrow":
                        d = Dir.Sur;
                        break;
                }
                while (Console.KeyAvailable)
                    (Console.ReadKey(false)).KeyChar.ToString();
            }

            return d;
        }
    }
}