using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.LevelMaking
{
    public enum LevelTypes
    {
        Entrance = 0,
        Middle = 1,
        Final = 2
    }

    public class LevelMaking : MonoBehaviour
	{
		public int CurrentDifficulty = 1;
		public Vector3 EndOfCurrentLevelPosition = new Vector3(86.69f, 0f, 35f);
        public List<GameObject> CreatedLevelsList;
        public List<Level> AvaiableLevels;

        public void Awake()
        {
            AvaiableLevels = (new DirectoryInfo(Application.dataPath + "//Resources//LevelsPrefab"))
                .GetFiles()
                .Where(x => !x.Name.Contains("meta"))
                .Select(x => new Level
                {
                    Type = (LevelTypes)Enum.Parse(typeof(LevelTypes), x.Name.Split('.')[0].Split('_')[0]),
                    Difficulty = int.Parse(x.Name.Split('.')[0].Split('_')[1]),
                    RandomNumber = int.Parse(x.Name.Split('.')[0].Split('_')[2])
                }).ToList();
        }

        public GameObject GetLevelePrefab(LevelTypes type, int difficulty)
		{
            var avaiableSelection = AvaiableLevels.Where(x => 
                x.Type == type && 
                x.Difficulty == difficulty
            ).ToList();

            if(avaiableSelection.Count == 0)
                return Resources.Load<GameObject>("LevelsPrefab/Middle_1_0");

            var randomNumber = UnityEngine.Random.Range(
                0,
                avaiableSelection.Count - 1
            );

            var selected = avaiableSelection[randomNumber];

			return Resources.Load<GameObject>(selected.GetCompleteName());
		}

		public bool InstantiateNextLevel(LevelTypes type, int? difficulty = null)
		{
			try
			{
				CurrentDifficulty = 1; //Implementare lo scalo di velocità	
				var levelGameObject = GetLevelePrefab(type, CurrentDifficulty);
				
				CreatedLevelsList.Add(Instantiate(
					levelGameObject,
					new Vector3(
						EndOfCurrentLevelPosition.x,
						EndOfCurrentLevelPosition.y,
						EndOfCurrentLevelPosition.z
					),
					Quaternion.identity
				));

                var xSize = levelGameObject
                            .GetComponent<BoxCollider2D>()
                            .size
                            .x;

                EndOfCurrentLevelPosition = new Vector3(
					EndOfCurrentLevelPosition.x + (xSize),
					EndOfCurrentLevelPosition.y,
					EndOfCurrentLevelPosition.z
				);

                if (CreatedLevelsList.Count > 2)
                    Utils.PopAdnDestroy(CreatedLevelsList, 0);

				return true;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public void LevelMakerReset()
		{
			CurrentDifficulty = 0;
			EndOfCurrentLevelPosition = new Vector3(86.69f, 0f, 35f);
		}
	
	}
}
