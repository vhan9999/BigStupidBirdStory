// #define TEAM
// #define BUILDING

#define TEST

using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public NavMeshSurface surface2D;
        [SerializeField] private CharaBehaviour charaBehaviourPrefab;
        [SerializeField] private TeamBehavier teamBehavierPrefab;
        private readonly List<TeamBehavier> teamList = new();
        private GameObject buildingContainer;
        private GameObject charaBehavierContainer;
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

            surface2D = GameObject.Find("MapContainer/MapNavMesh").GetComponent<NavMeshSurface>();
            mountainHole = GameObject.Find("MapContainer/MountainHole");
            buildingContainer = GameObject.Find("BuildingContainer");
            if (buildingContainer == null) buildingContainer = new GameObject("BuildingContainer");
            charaBehavierContainer = GameObject.Find("CharaBehavierContainer");
            if (charaBehavierContainer == null) charaBehavierContainer = new GameObject("CharaBehavierContainer");
            teamBehavierContainer = GameObject.Find("TeamBehavierContainer");
            if (teamBehavierContainer == null) teamBehavierContainer = new GameObject("TeamBehavierContainer");
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
            charaBehavierContainer = GameObject.Find("CharaBehavierContainer");
            if (charaBehavierContainer == null) charaBehavierContainer = new GameObject("CharaBehavierContainer");
            var charaBehavier = Instantiate(charaBehaviourPrefab, charaBehavierContainer.transform);

            teamBehavierContainer = GameObject.Find("TeamBehavierContainer");
            if (teamBehavierContainer == null) teamBehavierContainer = new GameObject("TeamBehavierContainer");

            var teamBehavier = CreateTeamBehavier();
            teamBehavier.AddMember(charaBehavier);
            teamList.Add(teamBehavier);

            var battleManage = GetComponent<BattleManage>();
            if (battleManage == null) battleManage = gameObject.AddComponent<BattleManage>();
            teamBehavier.AddOnBattleStart(battleManage.OnBattleStart);
            battleManage.AddOnBattleEnd(teamBehavier.OnBattleEnd);
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
                var team = Instantiate(teamBehavierPrefab, teamBehavierContainer.transform);
                var chara = Instantiate(charaBehaviourPrefab, mountainHole.transform.position,
                    charaBehavierContainer.transform.rotation, charaBehavierContainer.transform);
                team.AddMember(chara);
                chara.team = team;
            }

#endif
        }

        // private TeamBehavier CreateTeamBehavier()
        // {
        //     var teamBehavier = Instantiate(teamBehavierPrefab, teamBehavierContainer.transform);
        //     return teamBehavier;
        // }
    }
}