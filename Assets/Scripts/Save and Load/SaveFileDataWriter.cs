using UnityEngine;
using System;
using System.IO;
using System.Linq.Expressions;

public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    //before creating a new save file, we must check to see if this character slot is already filled (max slots =  )
    public bool CheckToSeeIfFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //use to delete save files
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath,saveFileName));
    }

    //Used to create a save file upon starting a new game
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        //make a path to save the file (on the machine)
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            //create the directory the file will be written to, if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("CREATING SAVE FILE, AT SAVE PATH: " + savePath);

            //serialize the c# game data object to JSON
            string dataToStore = JsonUtility.ToJson(characterData, true);

            //Write file to the system
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {

            Debug.LogError("Error WHILST TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED" + savePath + "\n" + ex);
        }
    }

    //Used to lead a save file upon loading a previous game

    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;

        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {

                    using (StreamReader reader = new StreamReader(stream))
                    {

                        dataToLoad = reader.ReadToEnd();
                    }


                }
                //Deserialize the data from JSON back to UNITY c#
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);

            }
            catch //(Exception ex)
            {
                Debug.Log("FILE IS BLANK");
            }
        
        }
         
        return characterData;
    }
}
