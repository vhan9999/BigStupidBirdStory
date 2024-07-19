#define TEAM
// #define BUILDING
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public NavMeshSurface surface2D;
        private GameObject charaBehavierContainer;
        private CharaBehaviour charaBehaviourPrefab;
        private IInput input;
        private GameObject teamBehavierContainer;
        private TeamBehavier teamBehavierPrefab;
        private TouchManage touchManage;
        List<TeamBehavier> teamList = new List<TeamBehavier>();

        private void Awake()
        {
            var isMobile = Application.platform == RuntimePlatform.Android;
            input = false && isMobile
                ? gameObject.AddComponent<MobileInput>()
                : gameObject.AddComponent<ComputerInput>();
            charaBehaviourPrefab = Resources.Load<CharaBehaviour>("Chara/CharaPrefab");
            teamBehavierPrefab = Resources.Load<TeamBehavier>("Chara/TeamPrefab");
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
        }

        private TeamBehavier CreateTeamBehavier()
        {
            var teamBehavier = Instantiate(teamBehavierPrefab, teamBehavierContainer.transform);
            return teamBehavier;
        }

        private void Update()
        {
        }
        
    }
}