using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CatFighter
{
    public string Name { get; private set; }
    public int Agility { get; private set; }     // ��������
    public int Accuracy { get; private set; }    // ��������
    public int Intelligence { get; private set; } // ��������� (����)
    public int Health { get; private set; }      // �������� (���� ��������)
    public bool IsAlive => Health > 0;           // ��������, ��� �� �����

    public CatFighter(string name, int agility, int accuracy, int intelligence, int health)
    {
        Name = name;
        Agility = agility;
        Accuracy = accuracy;
        Intelligence = intelligence;
        Health = health;
    }

    // ����� ��� ��������� �����
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0;
    }

    // ����� �����
    public void Attack(CatFighter target)
    {
        if (!IsAlive) return;

        Debug.Log($"{Name} ������� {target.Name}!");

        int dodgeRoll = Random.Range(0, 100);
        int hitRoll = Random.Range(0, 100);

        if (hitRoll < Accuracy)
        {
            Debug.Log($"{Name} ��������!");

            if (dodgeRoll >= target.Agility)
            {
                target.TakeDamage(Intelligence);
                Debug.Log($"{target.Name} �������� {Intelligence} �����. �������� ��������: {target.Health}.");

                if (!target.IsAlive)
                {
                    Debug.Log($"{target.Name} ����� �� ��������!");
                }
            }
            else
            {
                Debug.Log($"{target.Name} ��������� �� �����!");
            }
        }
        else
        {
           // Debug.Log($"{Name} �������������!");
        }
    }
}

public class Team
{
    public string TeamName { get; private set; }
    public List<CatFighter> Fighters { get; private set; }

    public Team(string teamName, List<CatFighter> fighters)
    {
        TeamName = teamName;
        Fighters = fighters;
    }

    // ��������, ���� �� ����� ������ � �������
    public bool HasAliveFighters()
    {
        return Fighters.Any(fighter => fighter.IsAlive);
    }

    // ��������� ���������� ������ ������ �� �������
    public CatFighter GetRandomAliveFighter()
    {
        var aliveFighters = Fighters.Where(fighter => fighter.IsAlive).ToList();
        if (aliveFighters.Count == 0) return null;
        int randomIndex = Random.Range(0, aliveFighters.Count);
        return aliveFighters[randomIndex];
    }
}

public class BattleSystem
{
    private Team _team1;
    private Team _team2;

    public BattleSystem(Team team1, Team team2)
    {
        _team1 = team1;
        _team2 = team2;
    }

    public void StartBattle()
    {
        Debug.Log("����� ����������!");

        while (_team1.HasAliveFighters() && _team2.HasAliveFighters())
        {
            TakeTurn(_team1, _team2);
            if (!_team2.HasAliveFighters())
            {
                Debug.Log($"{_team1.TeamName} ��������!");
                break;
            }

            TakeTurn(_team2, _team1);
            if (!_team1.HasAliveFighters())
            {
                Debug.Log($"{_team2.TeamName} ��������!");
                break;
            }
        }

        Debug.Log("����� �����������!");
    }

    private void TakeTurn(Team attackingTeam, Team defendingTeam)
    {
        var attacker = attackingTeam.GetRandomAliveFighter();
        var defender = defendingTeam.GetRandomAliveFighter();

        if (attacker != null && defender != null)
        {
            attacker.Attack(defender);
        }
    }
}

public class BattleTest : MonoBehaviour
{
    private void Start()
    {
        // �������� ������ �������
        var team1 = new Team("������� ������� 1", new List<CatFighter>
        {
            new CatFighter("�����1", 17, 9, 5, 100),
            new CatFighter("�����2", 15, 12, 5, 10),
            new CatFighter("�����3", 20, 8, 5, 10),
            new CatFighter("�����4", 13, 10, 5, 10),
            new CatFighter("�����5", 18, 14, 5, 10),
        });

        var team2 = new Team("������� �����������", new List<CatFighter>
        {
            new CatFighter("����1", 11, 9, 5, 10),
            new CatFighter("����2", 14, 10, 5, 10),
            new CatFighter("����3", 12, 11, 5, 10),
            new CatFighter("����4", 10, 12, 5, 10),
            new CatFighter("����5", 16, 13, 5, 10),
        });

        // ������ �����
        BattleSystem battleSystem = new BattleSystem(team1, team2);
        battleSystem.StartBattle();
    }
}
