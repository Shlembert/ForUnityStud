using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CatFighter
{
    public string Name { get; private set; }
    public int Agility { get; private set; }     // Ловкость
    public int Accuracy { get; private set; }    // Меткость
    public int Intelligence { get; private set; } // Интеллект (Урон)
    public int Health { get; private set; }      // Бодрость (Очки здоровья)
    public bool IsAlive => Health > 0;           // Проверка, жив ли котик

    public CatFighter(string name, int agility, int accuracy, int intelligence, int health)
    {
        Name = name;
        Agility = agility;
        Accuracy = accuracy;
        Intelligence = intelligence;
        Health = health;
    }

    // Метод для получения урона
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0;
    }

    // Метод атаки
    public void Attack(CatFighter target)
    {
        if (!IsAlive) return;

        Debug.Log($"{Name} атакует {target.Name}!");

        int dodgeRoll = Random.Range(0, 100);
        int hitRoll = Random.Range(0, 100);

        if (hitRoll < Accuracy)
        {
            Debug.Log($"{Name} попадает!");

            if (dodgeRoll >= target.Agility)
            {
                target.TakeDamage(Intelligence);
                Debug.Log($"{target.Name} получает {Intelligence} урона. Осталось здоровья: {target.Health}.");

                if (!target.IsAlive)
                {
                    Debug.Log($"{target.Name} выбыл из сражения!");
                }
            }
            else
            {
                Debug.Log($"{target.Name} уклонился от атаки!");
            }
        }
        else
        {
           // Debug.Log($"{Name} промахивается!");
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

    // Проверка, есть ли живые котики в команде
    public bool HasAliveFighters()
    {
        return Fighters.Any(fighter => fighter.IsAlive);
    }

    // Получение случайного живого котика из команды
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
        Debug.Log("Битва начинается!");

        while (_team1.HasAliveFighters() && _team2.HasAliveFighters())
        {
            TakeTurn(_team1, _team2);
            if (!_team2.HasAliveFighters())
            {
                Debug.Log($"{_team1.TeamName} победила!");
                break;
            }

            TakeTurn(_team2, _team1);
            if (!_team1.HasAliveFighters())
            {
                Debug.Log($"{_team2.TeamName} победила!");
                break;
            }
        }

        Debug.Log("Битва закончилась!");
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
        // Создание команд котиков
        var team1 = new Team("Команда Котиков 1", new List<CatFighter>
        {
            new CatFighter("Котик1", 17, 9, 5, 100),
            new CatFighter("Котик2", 15, 12, 5, 10),
            new CatFighter("Котик3", 20, 8, 5, 10),
            new CatFighter("Котик4", 13, 10, 5, 10),
            new CatFighter("Котик5", 18, 14, 5, 10),
        });

        var team2 = new Team("Команда Противников", new List<CatFighter>
        {
            new CatFighter("Враг1", 11, 9, 5, 10),
            new CatFighter("Враг2", 14, 10, 5, 10),
            new CatFighter("Враг3", 12, 11, 5, 10),
            new CatFighter("Враг4", 10, 12, 5, 10),
            new CatFighter("Враг5", 16, 13, 5, 10),
        });

        // Запуск битвы
        BattleSystem battleSystem = new BattleSystem(team1, team2);
        battleSystem.StartBattle();
    }
}
