using Player.save;
using UnityEngine;

internal enum CharaState
{
    Idle,
    Walk,
    Hungry,
    Tried,

    // Run,
    // Attack,
    // Skill,
    Die
}

public class CharaBehavier : MonoBehaviour
{
    [SerializeField] private CharaData charaData;

    [SerializeField] private CharaState state;

    [SerializeField] private Vector2 walkTarget;

    private void Start()
    {
    }

    private void Update()
    {
        if (charaData.hp.now <= 0)
        {
            SetState(CharaState.Die);
        }
        else if (charaData.hunger.now <= 0)
        {
            SetState(CharaState.Hungry);
        }
        else if (charaData.energy.now <= 0)
        {
            SetState(CharaState.Tried);
        }
        else
        {
            if (state == CharaState.Idle)
            {
                var r = Random.Range(0, 2f);
                if (r < 1)
                    SetState(CharaState.Walk);
            }
        }

        if (state == CharaState.Walk)
        {
            if (Vector2.Distance(transform.position, walkTarget) < 0.1f)
                SetState(CharaState.Idle);
            else
                transform.position = Vector2.MoveTowards(transform.position, walkTarget, 1f * Time.deltaTime);
        }
    }

    public void SetWalkTo(Vector2 pos)
    {
        walkTarget = pos;
    }

    public void SetCharaData(CharaData charaData)
    {
        this.charaData = charaData;
    }

    private void SetState(CharaState state)
    {
        switch (state)
        {
            case CharaState.Idle:
                break;
            case CharaState.Walk:
                break;
            case CharaState.Hungry:
                break;
            case CharaState.Tried:
                break;
            case CharaState.Die:
                break;
        }
    }
}