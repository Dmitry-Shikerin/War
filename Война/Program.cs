using System;
using System.Collections.Generic;

namespace Война
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlatoonFactory platoonFactory = new PlatoonFactory();

            int platoonSize = 5;
            string platoonName1 = "Отряд 1";
            string platoonName2 = "Отряд 2";

            Platoon platoon1 = platoonFactory.CreateRandom(platoonSize, platoonName1);
            Platoon platoon2 = platoonFactory.CreateRandom(platoonSize, platoonName2);

            Arena arena = new Arena();

            arena.Work(platoon1, platoon2);
        }
    }

    abstract class Fighter
    {
        protected int Armor;

        public Fighter(string name, int health, int armor, int damage)
        {
            Name = name;
            Health = health;
            Armor = armor;
            Damage = damage;
        }

        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public bool IsAlive => Health > 0;
        public int Damage { get; private set; }

        public virtual void TakeDamage(int damage)
        {
            if(IsAlive == false) 
            {
                return;
            }

            if (Armor > damage)
            {
                damage = Armor;
            }

            Health -= damage - Armor;

            if (IsAlive == false)
            {
                Health = 0;
            }
        }

        public virtual void Attack(Fighter enemy)
        {
            int currentDamege = DealDamage();
            enemy.TakeDamage(currentDamege);
        }

        protected virtual int DealDamage()
        {
            return Damage;
        }

        public virtual void ShowInfo()
        {
            Console.Write($"Здоровье: {Health}, Броня: {Armor}, Урон: {Damage}");
        }

        public abstract Fighter Clone();
    }

    class Warriour : Fighter
    {
        public int _rageCount = 0;

        public Warriour() : base("Воин", 150, 12, 30) { }

        protected override int DealDamage()
        {
            int ragePerBloW = 7;
            int ragePerFuriousBlow = 20;
            int damagePerFurriousBlow = 10;

            Console.WriteLine($"{Name} накапливает {ragePerBloW} ярости");
            _rageCount += ragePerBloW;

            if (_rageCount >= ragePerFuriousBlow)
            {
                Console.WriteLine($"{Name} наносит яростный удар");
                _rageCount -= ragePerFuriousBlow;
                int damage = Damage + damagePerFurriousBlow;

                return damage;
            }

            return Damage;
        }

        public override void ShowInfo()
        {
            Console.WriteLine(Name);
            base.ShowInfo();
            Console.WriteLine($", Ярости: {_rageCount}");
        }

        public override Fighter Clone()
        {
            return new Warriour();
        }
    }

    class DeathKnight : Fighter
    {
        public DeathKnight() : base("Рыцарь смерти", 150, 15, 25) { }

        public override void TakeDamage(int damage)
        {
            int minValueSlope = 1;
            int maxValueSlope = 100;
            int slopeValue = 25;

            int slope = Utils.CreateRandomValue(minValueSlope, maxValueSlope);

            if (slope <= slopeValue)
            {
                Console.WriteLine($"{Name} уклонился");
                return;
            }

            base.TakeDamage(damage);
        }

        public override void ShowInfo()
        {
            Console.WriteLine(Name);
            base.ShowInfo();
        }

        public override Fighter Clone()
        {
            return new DeathKnight();
        }
    }

    class Priest : Fighter
    {
        private int _mana = 20;

        public Priest() : base("Жрец", 110, 8, 33) { }

        public override void TakeDamage(int damage)
        {
            int manaPerPrayerRestoration = 20;
            int healthPerPrayerRestoration = 25;

            base.TakeDamage(damage);

            if (_mana >= manaPerPrayerRestoration)
            {
                Health += healthPerPrayerRestoration;
                Console.WriteLine($"{Name} восстанавливает {healthPerPrayerRestoration} здоровья");
                _mana -= manaPerPrayerRestoration;
            }
        }

        protected override int DealDamage()
        {
            int manaPerBlow = 8;

            _mana += manaPerBlow;
            Console.WriteLine($"{Name} восстанавливает {manaPerBlow} маны");
            return Damage;
        }

        public override void ShowInfo()
        {
            Console.WriteLine(Name);
            base.ShowInfo();
            Console.WriteLine($", Мана: {_mana}");
        }

        public override Fighter Clone()
        {
            return new Priest();
        }
    }

    class Warlock : Fighter
    {
        private int _mana = 20;

        public Warlock() : base("Чернокнижник", 105, 8, 35) { }

        protected override int DealDamage()
        {
            int manaPerBlow = 8;
            int manaPerBurnOfFilth = 20;
            int damagePerBurnOfFilt = 25;

            _mana += manaPerBlow;

            Console.WriteLine($"{Name} восстанавливает {manaPerBlow} маны");

            if (_mana >= manaPerBurnOfFilth)
            {
                Console.WriteLine($"{Name} использует ожог скверны");

                int damage = Damage + damagePerBurnOfFilt;

                return damage;
            }

            return Damage;
        }

        public override void ShowInfo()
        {
            Console.WriteLine(Name);
            base.ShowInfo();
            Console.WriteLine($", Мана: {_mana}");
        }

        public override Fighter Clone()
        {
            return new Warlock();
        }
    }

    class Magician : Fighter
    {
        private int _mana = 20;
        private int _iceArmor = 0;

        public Magician() : base("Маг", 100, 8, 40) { }

        public override void TakeDamage(int damage)
        {
            int damagePerIceArmor = 7;

            if (_iceArmor > 0)
            {
                _iceArmor--;
                damage -= damagePerIceArmor;
                Console.WriteLine($"{Name} блокирует {damagePerIceArmor} урона");
            }

            base.TakeDamage(damage);
        }

        protected override int DealDamage()
        {
            int manaPerBlow = 8;
            int manaPerIceArmor = 15;
            int stacsPerIceArmor = 4;
            int manaPerFayerBoll = 20;
            int damagePerFayerBoll = 20;

            _mana += manaPerBlow;

            Console.WriteLine($"{Name} восстанавливаееет {manaPerBlow} маны");

            if (_mana >= manaPerIceArmor && _iceArmor == 0)
            {
                _iceArmor = stacsPerIceArmor;
                _mana -= manaPerIceArmor;
                Console.WriteLine($"{Name} активирует Ледяной доспех");
            }

            if (_mana >= manaPerFayerBoll && _iceArmor > 0)
            {
                Console.WriteLine($"{Name} использует огненный шар и наносит {Damage + damagePerFayerBoll} урона");
                _mana -= manaPerFayerBoll;
                int damage = Damage + damagePerFayerBoll;

                return damage;
            }

            return Damage;
        }

        public override void ShowInfo()
        {
            Console.WriteLine(Name);
            base.ShowInfo();
            Console.WriteLine($", Мана: {_mana}, Заряды Ледяного доспеха: {_iceArmor}.");
        }

        public override Fighter Clone()
        {
            return new Magician();
        }
    }

    class Arena
    {
        public void Work(Platoon platoon1, Platoon platoon2)
        {
            ShowPlatoons(platoon1, platoon2);

            ConductBattle(platoon1, platoon2);

            ShowResults(platoon1, platoon2);
        }

        private static void ConductBattle(Platoon platoon1, Platoon platoon2)
        {
            while (platoon1.Fighters.Count > 0 && platoon2.Fighters.Count > 0)
            {
                ConductAttacPlatoon(platoon1, platoon2);
                ConductAttacPlatoon(platoon2, platoon1);

                Console.ReadKey();
            }
        }

        private static void ConductAttacPlatoon(Platoon attackingSquad, Platoon takingDamageSquad)
        {
            Console.WriteLine($"Атакует команда {attackingSquad.Name}");

            attackingSquad.Attack(takingDamageSquad.Fighters);
            attackingSquad.ShowInfo();
            attackingSquad.RemoveDeadFighters();
            Console.WriteLine();
        }

        private static void ShowPlatoons(Platoon platoon1, Platoon platoon2)
        {
            Console.WriteLine(platoon1.Name);
            platoon1.ShowInfo();
            Console.WriteLine(platoon2.Name);
            platoon2.ShowInfo();
        }

        private static void ShowResults(Platoon platoon1, Platoon platoon2)
        {
            if (platoon1.Fighters.Count == 0 && platoon2.Fighters.Count == 0)
                Console.WriteLine("Ничья");
            else if (platoon2.Fighters.Count == 0)
                Console.WriteLine($"Победил {platoon1.Name}");
            else if (platoon1.Fighters.Count == 0)
                Console.WriteLine($"Победил {platoon2.Name}");
        }
    }

    class FighterFactory
    {
        public Fighter CreateRandom()
        {
            List<Fighter> fighters = new List<Fighter>()
            {
                new Warriour(),
                new DeathKnight(),
                new Priest(),
                new Warlock(),
                new Magician()
            };

            int fighterIndex = Utils.CreateRandomValue(fighters.Count);

            return fighters[fighterIndex];
        }
    }

    class PlatoonFactory
    {
        private FighterFactory _fighterFactory = new FighterFactory();

        public Platoon CreateRandom(int groupSize, string name)
        {
            List<Fighter> fighters = new List<Fighter>();

            for (int i = 0; i < groupSize; i++)
            {
                Fighter fighter = _fighterFactory.CreateRandom();
                fighters.Add(fighter);
            }

            return new Platoon(fighters, name);
        }
    }

    class Platoon
    {
        private List<Fighter> _fighters = new List<Fighter>();

        public Platoon(List<Fighter> fighters, string name)
        {
            _fighters = fighters;
            Name = name;
        }
        
        public string Name { get; private set; }

        public IReadOnlyList<Fighter> Fighters => _fighters;

        public void ShowInfo()
        {
            for (int i = 0; i < _fighters.Count; i++)
            {
                Console.WriteLine($"Имя: {_fighters[i].Name} Здоровье: {_fighters[i].Health}, Урон: {_fighters[i].Damage}");
            }
        }

        public void RemoveDeadFighters()
        {
            for (int i = _fighters.Count - 1; i >= 0; i--)
            {
                if (_fighters[i].IsAlive == false)
                {
                    Console.WriteLine($"Боец {_fighters[i].Name} погиб");
                    _fighters.RemoveAt(i);
                }
            }
        }

        public void Attack(IReadOnlyList<Fighter> enemies)
        {
            foreach (Fighter fighter in _fighters)
            {
                if (enemies.Count == 0)
                    return;

                int enemyIndex = Utils.CreateRandomValue(enemies.Count);

                Fighter enemy = enemies[enemyIndex];

                fighter.Attack(enemy);
            }
        }
    }

    public static class Utils
    {
        private static Random s_random = new Random();

        public static int CreateRandomValue(int minValue, int maxValue)
        {
            return s_random.Next(minValue, maxValue);
        }

        public static int CreateRandomValue(int value)
        {
            return s_random.Next(value);
        }
    }
}
