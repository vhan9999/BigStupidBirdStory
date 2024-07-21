#define TEAM
// #define BUILDING

#define TEST

using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private static readonly List<CharaBehaviour> charaList = new();
        private static readonly List<TeamBehavier> teamList = new();
        private static GameObject charaBehavierContainer;
        public NavMeshSurface surface2D;
        [SerializeField] private CharaBehaviour charaBehaviourPrefab;
        [SerializeField] private TeamBehavier teamBehavierPrefab;
        [SerializeField] private EnemyBehaviour enemyBehaviourPrefab;
        private GameObject buildingContainer;
        private GameObject enemyBehavierContainer;
        private EnemySpawnManage enemySpawnManage;
        private IInput input;
        private GameObject mountainHole;
        private GameObject teamBehavierContainer;
        private TouchManage touchManage;

        private void Awake()
        {
            var isMobile = Application.platform == RuntimePlatform.Android;
            input = false && isMobile
                ? gameObject.AddComponent<MobileInput>()
                : gameObject.AddComponent<ComputerInput>();
            charaBehaviourPrefab = Resources.Load<CharaBehaviour>("Chara/CharaPrefab");
            teamBehavierPrefab = Resources.Load<TeamBehavier>("Chara/TeamPrefab");
            enemyBehaviourPrefab = Resources.Load<EnemyBehaviour>("Enemy/EnemyPrefab");
            var grassPrefab = Resources.Load<GrassLand>("Map/GrassLand");

            var grass = FindObjectOfType<GrassLand>();
            if (grass == null) grass = Instantiate(grassPrefab);

            if (surface2D != null)
                surface2D = GameObject.Find("MapContainer/MapNavMesh").GetComponent<NavMeshSurface>();
            mountainHole = GameObject.Find("MapContainer/MountainHole");
            buildingContainer = GameObject.Find("BuildingContainer");
            if (buildingContainer == null) buildingContainer = new GameObject("BuildingContainer");
            charaBehavierContainer = GameObject.Find("CharaBehavierContainer");
            if (charaBehavierContainer == null) charaBehavierContainer = new GameObject("CharaBehavierContainer");
            teamBehavierContainer = GameObject.Find("TeamBehavierContainer");
            if (teamBehavierContainer == null) teamBehavierContainer = new GameObject("TeamBehavierContainer");
            enemyBehavierContainer = GameObject.Find("EnemyBehavierContainer");
            if (enemyBehavierContainer == null) enemyBehavierContainer = new GameObject("EnemyBehavierContainer");

            enemySpawnManage = FindObjectOfType<EnemySpawnManage>();
            if (enemySpawnManage == null)
            {
                var manage = new GameObject("EnemySpawnManage");
                enemySpawnManage = manage.AddComponent<EnemySpawnManage>();
                enemySpawnManage.transform.SetParent(grass.transform);
                var go = new GameObject();
                go.transform.position = new Vector2(-12, 0);
                go.transform.SetParent(manage.transform);
            }

            enemySpawnManage.SetContainer(enemyBehavierContainer);
            enemySpawnManage.SetPrefab(enemyBehaviourPrefab);
#if BUILDING
            Debug.Log("building is defined");
            touchManage = GetComponent<TouchManage>();
            if (touchManage == null) touchManage = gameObject.AddComponent<TouchManage>();

            touchManage.AddOnUpdateNavMesh(() =>
                surface2D.UpdateNavMesh(surface2D.navMeshData)
            );

            input.AddOnClick(touchManage);
            input.AddOnMove(touchManage);
            input.AddOnLongPress(touchManage);
            input.AddStartTouch(touchManage);
            input.AddEndTouch(touchManage);
#endif
#if TEAM
#endif

            #region init

            #region todo-load

            // load chara data
            //load team data
            // load building data

            #endregion

            #endregion
        }

        private void Update()
        {
#if TEST

            if (Input.GetKeyDown(KeyCode.A))
            {
                var team = Instantiate(teamBehavierPrefab, mountainHole.transform.position,
                    charaBehavierContainer.transform.rotation, teamBehavierContainer.transform);
                var chara = Instantiate(charaBehaviourPrefab, mountainHole.transform.position,
                    charaBehavierContainer.transform.rotation, charaBehavierContainer.transform);
                team.AddMember(chara);
                chara.team = team;
                teamList.Add(team);
                charaList.Add(chara);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
            }
#endif
        }

        public static GameObject GetCharaContainer()
        {
            return charaBehavierContainer;
        }


        public static List<TeamBehavier> GetTeamList()
        {
            return teamList;
        }

        private TeamBehavier CreateTeamBehavier()
        {
            var teamBehavier = Instantiate(teamBehavierPrefab, teamBehavierContainer.transform);
            return teamBehavier;
        }
    }
}