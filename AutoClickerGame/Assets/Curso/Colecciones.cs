using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Colecciones : MonoBehaviour
{

    [Header("List")]
    public int[] myContent;
    public List<int> weaponsList; //Lista
    [Header("Dictionary")]
    public GameObject[] content;
    public Dictionary<string, GameObject> characterDictionary; //Dicionario
    [Header("Stack")]
    public Stack<GameObject> cardsStack; //Stack
    [Header("Queue")]
    public Queue<GameObject> playersQueue; //Queue


    private void Start()
    {
        /*
        #region List

        //Init
        weaponsList = new List<int>();

        //Add
        for (int i = 0; i < myContent.Length; i++)
        {
            weaponsList.Add(myContent[i]);
        }
        weaponsList.AddRange(myContent);

        //Remove
        weaponsList.Remove(myContent[2]);


        //Read
        int tempValue = weaponsList[2];

        //Amount
        int amount = weaponsList.Count;

        //Clear
        weaponsList.Clear();
        #endregion
        #region Dictionary
        //Init
        characterDictionary = new Dictionary<string, GameObject>();
        //Add
        for (int i = 0; i < content.Length; i++)
        {
            characterDictionary.Add(content[i].name, content[i]);
        }
        //Remove
        characterDictionary.Remove("Orc");
        //Contains
        bool contains = characterDictionary.ContainsKey("Orc");
        //Read
        GameObject myvalue = characterDictionary["Orc"];
        //Amount
        int amounts = characterDictionary.Count;
        //Clear
        characterDictionary.Clear();
        #endregion
        #region Stack
        //Init
        cardsStack = new Stack<GameObject>();
        //Add
        for (int i = 0;i < myContent.Length; i++)
        {
            cardsStack.Push(content[i]);
        }
        //Return First and Remove
        GameObject myGameObjectPop = cardsStack.Pop();
        //Return First
        GameObject myGameObjectPeek = cardsStack.Peek();

        //Contains
        bool contiene = cardsStack.Contains(myGameObjectPeek);
        //Amnount
        int  cuantos = cardsStack.Count;
        //Clear
        cardsStack.Clear();


        #endregion
        #region Queue
        //Init
        playersQueue = new Queue<GameObject>();
        //Add
        for (int i = 0; i < content.Length; i++)
        {
            playersQueue.Enqueue(content[i]);
        }
        //Remove
        GameObject myGameObject = playersQueue.Dequeue();
        //Contains
        bool contienen = playersQueue.Contains(myGameObject);
        #endregion */
    }
}

