using UnityEngine;

public class CharacterActivator : MonoBehaviour
{
    public GameObject[] charactersInScene;

    void Start()
    {
        int index = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

        for (int i = 0; i < charactersInScene.Length; i++)
        {
            charactersInScene[i].SetActive(i == index);
        }

        Debug.Log("kich hoat nhan vat: " + index);
    }
}
