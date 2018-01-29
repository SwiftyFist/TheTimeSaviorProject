using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelMaking
{
	public class LevelMaking : MonoBehaviour
	{
		public int CurrentDifficulty = 1;
		public Vector3 EndOfCurrentLevelPosition = new Vector3(86.69f, 0f, 35f);
        public List<GameObject> LevelsList;
		
		public enum LevelTypes
		{
			Entrance = 0,
			Middle = 1,
			Final = 2
		}
		
		public static GameObject GetLevelePrefab(LevelTypes type, int difficulty)
		{
			var livello = string.Format(
				"LevelsPrefab/{0}_{1}_{2}",
				type,
				difficulty,
				0 //Random.Range(0, 5); Dovranno esserci 5 prefab per ogni tipo
			);
			return Resources.Load<GameObject>(livello);
		}

		public bool InstantiateNextLevel(LevelTypes type, int? difficulty = null)
		{
			try
			{
				CurrentDifficulty = 1; //Implementare lo scalo di velocità	
				var levelGameObject = GetLevelePrefab(type, CurrentDifficulty);
				
				LevelsList.Add(Instantiate(
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

                if (LevelsList.Count > 2)
                    Utils.PopAdnDestroy(LevelsList, 0);

				return true;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public void Reset()
		{
			CurrentDifficulty = 0;
			EndOfCurrentLevelPosition = new Vector3(86.69f, 0f, 35f);
		}
	
	}
}
