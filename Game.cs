﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HelloWorld
{
    class Game
    {
        //setting Items = to new item is initializing them then and there no need for InitItems function
        private Items _junk = new Items();
        public Items _EmptySlot = new Items(true);
        private Items _damageNecklace = new Items("Necklace of Harm", 0, 2, 10);
        private Items healthnecklace = new Items("Necklace of increase health", 25, 0, 5);
        private Items _sword = new Items("Another Sword", 0, 5, 10);
        private Shop _shop = new Shop();
        private Player _player = new Player(); //player declared but not defined to allow user to choose character later in code
        private bool _gameOver = false;
        private bool _useOldSave;
        //Run the game

        public void Run()
        {
            Start();
            while (_gameOver == false)
            {
                Update();
            }
            End();
        }

        //Performed once when the game begins
        public void Start()
        {
            InitStore();
            InitInventory(_player);
            ControlIntro();
            MainMenu();
            if (_useOldSave == false)
            {
                ChooseCharacter();
                Introduction();
                FarEndOfThePit();
                if (_gameOver == false)
                {
                    MeetTheCamp();
                }
            }
            else if (_useOldSave == true)
            {
                Load();
            }
        }

        //Repeated until the game ends
        public void Update()
        {
            CampLife();
        }

        //Performed once when the game ends
        public void End()
        {
            Console.Clear();
            Console.WriteLine("Thank you for playing my game!");
        }
        private void InitStore()
        {
            _shop.SetItem(_sword, 0);
            _shop.SetItem(healthnecklace, 1);
            _shop.SetItem(_damageNecklace, 2);
        }
        //Allows user to select one of 4 characters each with defining features
        private void ChooseCharacter()
        {
            Console.Clear();
            Console.WriteLine("Please select a character from below!");
            Console.WriteLine("1. Mouse Man, thief of the night\n2. Merlin" +
                " master of the arcane arts [coolest or strongest]\n3. WolfGang deaf musical bard\n.4 " +
                "Professer Eisenburg raiser of the dead");
            char input = ' ';
            while (input != '1' && input != '2' && input != '3' && input != '4')
            {
                input = Console.ReadKey(true).KeyChar;
                switch (input)
                {
                    case '1':
                        _player = new Player(1);
                        break;
                    case '2':
                        _player = new Player(2);
                        break;
                    case '3':
                        _player = new Player(3);
                        break;
                    case '4':
                        _player = new Player(4);
                        break;
                    default:
                        Console.WriteLine("Error please select a valid input");
                        break;
                }
                _player.InitInventory();
            }
            Console.Clear();
        }
        private void MainMenu()
        {
            Console.Clear();
            TestForSaves();
            if (_useOldSave == false)
            {
                Console.Clear();
            }
            else if (_useOldSave == true)
            {
                Console.Clear();
            }

        }

        private void Introduction()
        {
            //small bit of plot introduced
            Console.Clear();
            Console.WriteLine(_player.GetName() + ": You wake up in a pit of the infected bodies of Castle Snositi");
            Console.WriteLine("Around you is nothing but inanimate bodies smelling of rotten flesh, the pit is nothing more than\n" +
                "a divet in the Earth around you.");
            //castle wall no let them in because of plague
            char input = GetInput("Go to castle wall", "Go to far end of the pit", "You notice only two real places to go");
            while (input != '2')
            {
                Console.Clear();
                Console.WriteLine(_player.GetName() + ": You approace the great stone wall, its significantly larger than you,\n" +
                    "and appears to still be guarded, you get the feeling you're not invited back in.");
                Console.WriteLine("Press 2 to go to the far end of the pit");
                input = Console.ReadKey(true).KeyChar;
                if (input != '2')
                {
                    Console.WriteLine("just press 2");
                    Console.WriteLine("but press any key to try again");
                    Console.ReadKey(true);
                }
            }
        }
        private void TestForSaves()
        {
            //Could be a bool but i prefer this, allows me to either introduce new players to the game or allow
            //old players to continue at the campfire where they left off.
            Console.WriteLine("Hello and welcome to a zombie based pvp grinding game! Please select and option from below");
            char input = ' ';
            while (input != '1' && input != '2')
            {
                bool saveExists = false;
                Console.WriteLine("1. New Game");
                if (File.Exists("SaveData.txt") == true) // tests to see if there is in fact a saved game file
                {
                    Console.WriteLine("2. Load Game");   // Visualizes option to select load game if above statement true
                    saveExists = true;
                }
                if (saveExists == true)
                {
                    input = Console.ReadKey(true).KeyChar;
                    if (input == '1')
                    {
                        _useOldSave = false;
                    }
                    else if (input == '2')
                    {
                        _useOldSave = true;
                    }
                    else
                    {
                        Console.WriteLine("Error Invalid Option");
                    }
                }
                else
                {
                    input = Console.ReadKey(true).KeyChar;
                    if (input == '1')
                    {
                        _useOldSave = false;
                    }
                    else
                    {
                        Console.WriteLine("Error Invalid Option");
                    }
                }

            }
        }
        private void ControlIntro()//should help clear up any actual question
        {
            Console.WriteLine("This game consists of very few controls, use numpad or number row to\n" +
                "select options, if you enter an invalid option you will be told, often you may be prompted\n" +
                "to press any key to continue, please read each screen thouroughly before deciding");
            Console.WriteLine("\n\n\nPress any key to continue!");
            Console.ReadKey(true);
        }


        private void FarEndOfThePit()//Just use to hold specific text looks neater
        {
            Console.Clear();
            Console.WriteLine(_player.GetName() + ": upon arriving at the far end of the gate, you notice an undead peasant\n" +
                "just standing there. But unfortunately it notices you and begins to approach quickly\n" +
                "Press any key  begin battle introduction");
            Console.ReadKey(true);
            BattleLoop(_player);
        }
        private void BattleLoop(Player player) //player fights against a zombie
        {
            int enemynumber = GenerateNumber(1, 4, true);
            Enemy enemy = new Enemy(enemynumber);
            //test for both players being alive
            while (player.GetHealth() > 0 && enemy.GetHealth() > 0)
            {
                Console.Clear();
                player.PrintStats();
                Console.WriteLine();
                enemy.PrintStats();
                if (player.GetHealth() > 0)//makes sure player is alive before attacking
                {
                    if (player.HasMana() == false)
                    {
                        char input = GetInput("Attack soft yet sure", "Attack hard yet blind", "What move will you choose?");
                        if (input == '1')
                        {
                            player.Attack(enemy);
                        }
                        else if (input == '2')
                        {
                            player.BlindAttack(enemy);
                        }
                    }
                    else
                    {
                        char input = GetInput("Magic Attack", "Rest For Mana", "What move will you choose?");
                        if (input == '1')
                        {
                            player.ManaAttack(enemy);
                        }
                        else if (input == '2')
                        {
                            player.ManaFromRest(5);
                        }
                    }
                }
                if (enemy.GetHealth() > 0)//makes sure enemy is alive before attacking
                {
                    float EnemyChoice = GenerateNumber(1, 10);//adds randomized attacks
                    if (enemy.HasMana() == false)
                    {
                        if (EnemyChoice >= 6)
                        {
                            enemy.Attack(player);
                        }
                        else if (EnemyChoice == 5) //Just a good RNG possibility
                        {
                            Console.WriteLine(enemy.GetName() + " doesn't seem interested");
                            Console.ReadKey(true);
                        }
                        else
                        {
                            enemy.BlindAttack(player);
                        }
                    }
                    else
                    {
                        if (EnemyChoice >= 6)
                        {
                            enemy.ManaAttack(player);
                        }
                        else
                        {
                            enemy.Attack(player);
                        }
                    }
                }
            }
            if (player.GetHealth() > 0)
            {
                Console.Clear();
                Console.WriteLine(_player.GetName() + " has defeated " + enemy.GetName() + "!");
                //after battle if player was the last alive they gain experience
                Console.WriteLine("You gained " + player.GainExperience(enemy) + " experience!");
                _player.GoldEarned(5);
                Console.WriteLine("You gained 5 gold!");
                //after battle the enemy may drop an item
                int lootchance = GenerateNumber(1, 5, true);
                switch (lootchance)
                {
                    case 1:
                    case 2://case 1 and 2 will yeild nothing found
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Unfortunately " + enemy.GetName() + " dropped nothing!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;
                    case 3://case 3 will drop a "junk"
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("The foe dropped a " + _junk.GetName());
                        Console.WriteLine("Press any key to continue");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadLine();
                        _player.EquipItem(_junk);
                        break;
                    case 4: //case 4 will drop damage increase necklace
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("The foe dropped a " + _damageNecklace.GetName());
                        Console.WriteLine("Press any key to continue");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadLine();
                        _player.EquipItem(_damageNecklace);
                        break;
                    case 5: //case 5 will drop health increase necklace
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("The foe dropped a" + healthnecklace.GetName());
                        Console.WriteLine("Press any key to continue");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadLine();
                        _player.EquipItem(healthnecklace);
                        break;

                }
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                Console.Clear();
                if (_player.GetExperience() >= 100)
                {
                    _player.LevelUP();
                }
                player.PrintStats();
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
            }
            else if (enemy.GetHealth() > 0)
            {
                Death();
            }
        }
        private void HuntAnimal(Player player) //allows player to possibly find an easy source of xp.
        {
            int number = GenerateNumber(1, 3, true);
            WildLife animal = new WildLife(number);
            Console.WriteLine(player.GetName() + " has stumbled upon a " + animal.GetName());
            while (player.GetHealth() > 0 && animal.GetHealth() > 0)
            {
                if (player.GetHealth() > 0)
                {
                    char input = GetInput("Attack soft", "Attack Hard", "What does " + player.GetName() + " do?");
                    if (input == '1')
                    {
                        player.Attack(animal);
                    }
                    else
                    {
                        player.BlindAttack(animal);
                    }
                }
                if (animal.GetHealth() > 0)
                {
                    float hitchoice = GenerateNumber(1, 10);
                    if (hitchoice >= 5)
                    {
                        animal.Attack(player);
                    }
                    else
                    {
                        animal.Attack(player);
                    }
                }
            }
            if (player.GetHealth() <= 0)
            {
                Death();
            }
            else
            {
                Console.WriteLine(player.GetName() + " has proven to be stronger than " + animal.GetName());
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                Console.Clear();
                player.GainExperience(animal);
                if (_player.GetExperience() >= 100)
                {
                    _player.LevelUP();
                }
                player.PrintStats();
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
            }
        }
        public float GenerateNumber(int min, int max) //takes in min and max to make generating numbers easy and variables non permanent
        {
            Random r = new Random();
            float number = r.Next(min, max);
            return number;
        }
        public int GenerateNumber(int min, int max, bool integer)
        {
            Random r = new Random();
            int number = r.Next(min, max);
            return number;
        }
        public void Death() //Just dialogue and background info
        {
            Console.Clear();
            Console.WriteLine("You have succumbed to that of a fungi, neither alive nor dead \n" +
                    "forever growing and forever rotting, but what is death to someone whos never lived?");
            Console.WriteLine("Final Stats");
            _player.PrintStats();
            Console.WriteLine("Press any key to continue");
            _gameOver = true;
            Console.ReadKey(true);
        }

        private void MeetTheCamp() // just dialogue and background info
        {
            Console.WriteLine("You notice a young girl has been watching the whole time from just beyond a few shrubs\n" +
                "she urges you to follow.\n Press any key to continue");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine(_player.GetName() + " follows the young girl into the woods for what seems like only a few minutes");
            Console.WriteLine("She suddenly stops near a clearing revealing a small society embedded deep in the woods.");
            Console.WriteLine("Press any key to continue to the center");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine("Once you are positioned near the center camp fire, the girl explains that this is a refugee camp for \n" +
                "those like you who have been kicked from the castle. This is your new home for the forseeable future");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
            Console.Clear();
        }
        private void CampLife() // The main loop that tha player can save from or load to
        {
            while (_gameOver == false)
            {
                Console.Clear();
                char input = GetInput("Camp Shop", "Camp Rest Area", "Wilderness Scavenge", "Save", "What will you do for now?");
                switch (input)
                {
                    case '1':
                        CampShop();
                        break;
                    case '2':
                        RestArea();
                        break;
                    case '3':
                        float enemychance = GenerateNumber(1, 10);
                        if (enemychance >= 8)
                        {
                            HuntAnimal(_player);
                        }
                        else
                        {
                            BattleLoop(_player);
                        }
                        break;
                    case '4':
                        Save();
                        break;
                }
            }
        }
        private void CampShop() // allows player to buy 3 different items or as many items as wanted.
        {
            bool leave = false;
            while (leave == false)
            {
                Console.Clear();
                _player.PrintStats();
                char input = GetInput(_shop.GetItem(0), _shop.GetItem(1), _shop.GetItem(2), "Leave", "sell", "What do ya need little one?");
                int itemintSlot = 0;
                switch (input)
                {
                    case '1':
                        _player.BuyItem(_shop, 0);
                        break;
                    case '2':
                        _player.BuyItem(_shop, 1);
                        break;
                    case '3':
                        _player.BuyItem(_shop, 2);
                        break;
                    case '4':
                        Console.WriteLine("Alright Ill Be Seeing You Around Then\nPress any key to continue");
                        leave = true;
                        Console.ReadKey(true);
                        break;
                    case '5':
                        char itemSlot = GetInput(_player, "cancel", "Please press the number that corresponds with the item you wish to sell");
                        if (itemSlot == 4)
                        {
                            break;
                        }
                        switch(itemSlot)
                        {
                            case '1': 
                                itemintSlot = 0;
                                break;
                            case '2':
                                itemintSlot = 1;
                                break;
                            case '3':
                                itemintSlot = 2;
                                break;
                        }
                            _player.SellItem(_shop, itemintSlot);
                        break;

                }
            }
        }
        private void RestArea() //Allows player to heal as much as needed
        {
            Console.Clear();
            char input = GetInput("heal", "leave", "The camp fire looks comfy, do you wish to heal?");
            if (input == '1')
            {
                _player.HealFromRest(25);
                if (_player.HasMana() == true)
                { _player.ManaFromRest(25); }                
            }
            else
            {
                Console.WriteLine("yea maybe another time");
            }
        }

        public void Save() // calls for player to save their important stats;
        {
            StreamWriter writer = new StreamWriter("SaveData.txt");
            _player.Save(writer);
            writer.Close();
            Console.WriteLine("Saved, press any key to continue");
            Console.ReadKey(true);
        }
        public void Load()//loads players important stats
        {
            StreamReader reader = new StreamReader("SaveData.txt");
            _player.Load(reader);
            reader.Close();
        }
        private char GetInput(Player player, string option4, string query)
        {
            char input = ' ';
            Console.WriteLine(query);

            Console.WriteLine("1. " + player.Inventory(0).GetName());
            Console.WriteLine("2. " + player.Inventory(1).GetName());
            Console.WriteLine("3. " + player.Inventory(2).GetName());
            Console.WriteLine("4. " + option4);
            while (input != '1' && input != '2' && input != '3' && input != '4')
            {
                input = Console.ReadKey(true).KeyChar;
                if (input != '1' && input != '2' && input != '3' && input != '4')
                {
                    Console.WriteLine("Please select a valid option");
                }
            }
            return input;
        }

        private char GetInput(string option1, string option2, string option3, string option4, string query) //gets either 1 2 3 or 4 as a char input and prints a query
        {
            char input = ' ';
            Console.WriteLine(query);

            Console.WriteLine("1. " + option1);
            Console.WriteLine("2. " + option2);
            Console.WriteLine("3. " + option3);
            Console.WriteLine("4. " + option4);
            while (input != '1' && input != '2' && input != '3' && input != '4')
            {
                input = Console.ReadKey(true).KeyChar;
                if (input != '1' && input != '2' && input != '3' && input != '4')
                {
                    Console.WriteLine("Please select a valid option");
                }
            }
            return input;
        }
        private void InitInventory(Player player) // Sets all items in players inventory as Empty Slot
        {
            player.InitInventory();
        }
        private char GetInput(Items option1, Items option2, Items option3, string option4, string option5, string query) //gets either 1 2 3 or 4 as a char input and prints a query
        {
            char input = ' ';
            Console.WriteLine(query);

            Console.WriteLine("1. " + option1.GetName() + " costs " + option1.GetValue());
            Console.WriteLine("2. " + option2.GetName() + " costs " + option2.GetValue());
            Console.WriteLine("3. " + option3.GetName() + " costs " + option3.GetValue());
            Console.WriteLine("4. " + option4);
            Console.WriteLine("5. " + option5);
            while (input != '1' && input != '2' && input != '3' && input != '4' && input != '5')
            {
                input = Console.ReadKey(true).KeyChar;
                if (input != '1' && input != '2' && input != '3' && input != '4' && input !='5')
                {
                    Console.WriteLine("Please select a valid option");
                }
            }
            return input;
        }
        private char GetInput(string option1, string option2, string query) //This is used to get a simple 1 or 2 char from a 2 answer question
        {
            Console.WriteLine(query);
            Console.WriteLine("1. " + option1);
            Console.WriteLine("2. " + option2);
            char input = ' ';
            while (input != '1' && input != '2')
            {
                input = Console.ReadKey(true).KeyChar;
                if (input != '1' && input != '2')
                {
                    Console.WriteLine("Please select a valid option");
                }
            }
            return input;
        }
    }
}
