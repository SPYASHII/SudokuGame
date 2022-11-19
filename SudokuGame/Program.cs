using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuGame
{
    class Game
    {
        static Random rnd = new Random();

        static int x = 0, y = 1, border_X = x + 1, border_Y = y + 1, section = 0, num_to_insert = 0;

        static bool show_answer = false, end_game = true, win = false;

        const int field_size = 9;
        static int[,] field = new int[field_size, field_size];
        static int[,] field_for_player = new int[field_size, field_size];

        static List<int> static_elements_coordinates = new List<int> { };
        static List<List<int>> wrong_elm_cord_lists = new List<List<int>> { };
        
        static ConsoleKeyInfo button;
        enum key { UP, DOWN, RIGHT, LEFT, INSERT, ANSWER, END, TRASH }
        static key pressed;
        static void DeleteWrngEl(int y, int x)//Логика удаления из памяти некоректно стоящих елементов
        {
            for (int i = 0; i < wrong_elm_cord_lists.Count; i++)
            {
                for(int j = 0; j < wrong_elm_cord_lists[i].Count; j += 2)
                {
                    if (wrong_elm_cord_lists[i][j] == y && wrong_elm_cord_lists[i][j + 1] == x)
                    {
                        if (j == 0)
                        {
                            wrong_elm_cord_lists.RemoveAt(i);
                            i--;
                            break;
                        }
                        else
                        {
                            wrong_elm_cord_lists[i].RemoveAt(j);
                            wrong_elm_cord_lists[i].RemoveAt(j);
                            break;
                        }
                    }
                }
            }
        }
        static void ShowArr(int[,] arr)
        {
            for (int i = 0; i < field_size; i++)
            {
                for (int j = 0; j < field_size; j++)
                {
                    Console.Write($"{arr[i, j]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        static bool ElExist(int[,]arr, int find)//Существует ли елемент в двухмерном массиве
        {
            bool exist = false;
            for (int i = 0; i < field_size; i++)
            {
                for (int j = 0; j < field_size; j++)
                {
                    if (arr[i, j] == find)
                    {
                        return exist = true;
                    }
                }
            }
            return exist;
        }
        static void Generate()//Генерация
        {
            bool insert;
            int[] memory = new int[field_size];
            for (int i = 0; i < field_size;)
            {
                insert = true;
                int num = rnd.Next(1, 10);
                for (int j = 0; j < field_size; j++)
                {
                    if (num == memory[j])
                    {
                        insert = false;
                        break;
                    }
                }
                if (insert)
                {
                    memory[i] = num;
                    field[0, i] = num;
                    i++;
                }
            }    //Генерация первой строки
            for (int i = 1; i < 3; i++)
            {
                for (int j = 0; j < field_size; j++)
                {
                    if (j < 6)
                        field[i, j] = field[i - 1, j + 3];
                    else
                        field[i, j] = field[i - 1, j - 6];
                }
            }         //Заполнение поля
            for (int i = 3; i < field_size; i++)
            {
                for (int j = 0; j < field_size; j++)
                {
                    if (j < 8)
                        field[i, j] = field[i - 3, j + 1];
                    else
                        field[i, j] = field[i - 3, 0];
                }
            }//Заполнение поля
            for (int k = 0; k < 3; k++)
            {
                int min = 0;
                int max = 0;
                switch (k)
                {
                    case 0:
                        min = 0;
                        max = 3;
                        break;
                    case 1:
                        min = 3;
                        max = 6;
                        break;
                    case 2:
                        min = 6;
                        max = 9;
                        break;
                }
                for (int i = 0; i < 2;)
                {
                    insert = true;
                    int num = rnd.Next(min, max);
                    for (int j = 0; j < 2; j++)
                    {
                        if (num == memory[j])
                        {
                            insert = false;
                            break;
                        }
                    }
                    if (insert)
                    {
                        memory[i] = num;
                        i++;
                    }

                }
                for (int i = 0; i < field_size; i++)
                {
                    int bufer;
                    bufer = field[memory[0], i];
                    field[memory[0], i] = field[memory[1], i];
                    field[memory[1], i] = bufer;
                }//Перетасовка строк
                for (int i = 0; i < field_size; i++)
                {
                    int bufer;
                    bufer = field[i, memory[0]];
                    field[i, memory[0]] = field[i, memory[1]];
                    field[i, memory[1]] = bufer;
                }//Перетасовка столбцов
            }         //Перетасовка
            for (int i = 0; i < field_size; i++)
            {
                for (int j = 0; j < field_size; j++)
                {
                    //System.Convert.ToBoolean(rnd.Next(0, 2))
                    if (System.Convert.ToBoolean(rnd.Next(0, 2)))
                    {
                        field_for_player[i, j] = field[i, j];
                        static_elements_coordinates.Add(i);
                        static_elements_coordinates.Add(j);
                    }
                }
            }
            //ShowArr(field);
            //ShowArr(field_for_player);
        }
        static bool SectionLogic(string type, ref List<int> memory, ref List<int> buffer, bool good, int a, int b)//Логика проверки елементов в зависимости от секции
        {
            List<int> x_mod = new List<int> { };
            List<int> y_mod = new List<int> { };

            switch (section)
            {
                case 1:
                    x_mod.Add(1);
                    x_mod.Add(2);
                    x_mod.Add(1);
                    x_mod.Add(2);

                    y_mod.Add(1);
                    y_mod.Add(1);
                    y_mod.Add(2);
                    y_mod.Add(2);
                    break;
                case 2:
                    x_mod.Add(1);
                    x_mod.Add(-1);
                    x_mod.Add(1);
                    x_mod.Add(-1);

                    y_mod.Add(1);
                    y_mod.Add(1);
                    y_mod.Add(2);
                    y_mod.Add(2);
                    break;
                case 3:
                    x_mod.Add(-1);
                    x_mod.Add(-2);
                    x_mod.Add(-1);
                    x_mod.Add(-2);

                    y_mod.Add(1);
                    y_mod.Add(1);
                    y_mod.Add(2);
                    y_mod.Add(2);
                    break;
                case 4:
                    x_mod.Add(1);
                    x_mod.Add(2);
                    x_mod.Add(1);
                    x_mod.Add(2);

                    y_mod.Add(-1);
                    y_mod.Add(-1);
                    y_mod.Add(1);
                    y_mod.Add(1);
                    break;
                case 5:
                    x_mod.Add(-1);
                    x_mod.Add(1);
                    x_mod.Add(1);
                    x_mod.Add(-1);

                    y_mod.Add(-1);
                    y_mod.Add(1);
                    y_mod.Add(-1);
                    y_mod.Add(1);
                    break;
                case 6:
                    x_mod.Add(-1);
                    x_mod.Add(-2);
                    x_mod.Add(-1);
                    x_mod.Add(-2);

                    y_mod.Add(-1);
                    y_mod.Add(-1);
                    y_mod.Add(1);
                    y_mod.Add(1);
                    break;
                case 7:
                    x_mod.Add(1);
                    x_mod.Add(2);
                    x_mod.Add(1);
                    x_mod.Add(2);

                    y_mod.Add(-1);
                    y_mod.Add(-1);
                    y_mod.Add(-2);
                    y_mod.Add(-2);
                    break;
                case 8:
                    x_mod.Add(1);
                    x_mod.Add(-1);
                    x_mod.Add(1);
                    x_mod.Add(-1);

                    y_mod.Add(-1);
                    y_mod.Add(-1);
                    y_mod.Add(-2);
                    y_mod.Add(-2);
                    break;
                case 9:
                    x_mod.Add(-1);
                    x_mod.Add(-1);
                    x_mod.Add(-2);
                    x_mod.Add(-2);

                    y_mod.Add(-1);
                    y_mod.Add(-2);
                    y_mod.Add(-1);
                    y_mod.Add(-2);
                    break;
            }

            switch (type)
            {
                case "draw":
                    for (int i = 0; i < 4; i++)
                    {
                        if (((y + y_mod[i]) == a) && ((x + x_mod[i]) == b))
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            return good = false;
                        }
                    }
                    break;
                case "check":
                    for (int i = 0; i < 4; i++)
                    {
                        memory.Add(field_for_player[y + y_mod[i], x + x_mod[i]]);
                        buffer.Add(y + y_mod[i]);
                        buffer.Add(x + x_mod[i]);
                    }
                    break;
            }

            return good;
        }
        static void CheckElement()//Проверка на правильность поставленного елемента
        {

            List<int> memory = new List<int>(20);
            List<int> buffer = new List<int>();
            List<int> wrong_elm_cord = new List<int> { };

            buffer.Add(y);
            buffer.Add(x);

            for (int i = 0; i < field_size; i++)
            {
                if (i != y)
                {
                    memory.Add(field_for_player[i, x]);
                    buffer.Add(i);
                    buffer.Add(x);
                }
                if (i != x)
                {
                    memory.Add(field_for_player[y, i]);
                    buffer.Add(y);
                    buffer.Add(i);
                }
            }

            SectionLogic("check", ref memory, ref buffer, false, 0, 0);

            if (memory.Contains(field_for_player[y, x]))
            {

                for (int i = 0; i < buffer.Count; i += 2)
                {
                    if (field_for_player[y, x] == field_for_player[buffer[i], buffer[i + 1]])
                    {
                        wrong_elm_cord.Add(buffer[i]);
                        wrong_elm_cord.Add(buffer[i + 1]);
                    }
                }
                wrong_elm_cord_lists.Add(wrong_elm_cord);
            }
        }

        static void Draw() //Отрисовка
        {
            int check = 0;
            Console.Clear();
            
            for (int i = 0; i < field_size; i++)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("|");
                Console.ResetColor();
                for (int j = 0; j < field_size; j++)
                {
                    bool good = true;

                    Console.ForegroundColor = ConsoleColor.DarkBlue;

                    good = SectionLogic("draw", ref static_elements_coordinates, ref static_elements_coordinates, good, i, j);

                    for (int l = 0; l < wrong_elm_cord_lists.Count; l++)
                    {
                        bool exit = false;
                        for (int k = 0; k < wrong_elm_cord_lists[l].Count; k += 2)
                        {
                            if (i == wrong_elm_cord_lists[l][k] && j == wrong_elm_cord_lists[l][k + 1])
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                good = false;
                                exit = true;
                                break;
                            }
                        }
                        if (exit)
                            break;
                    }

                    if ((j == x || i == y) && good)
                        Console.BackgroundColor = ConsoleColor.Green;
                    else if (good)
                        Console.BackgroundColor = ConsoleColor.White;

                    if (check < static_elements_coordinates.Count)
                    {
                        if (i == static_elements_coordinates[check] && j == static_elements_coordinates[check + 1])
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            check += 2;
                        }
                    }

                    if (i == y && j == x)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;

                        Console.Write($" ");
                        if (field_for_player[i, j] != 0)
                            Console.Write($"{field_for_player[i, j]}");
                        else
                            Console.Write($" ");
                        Console.Write(" ");

                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        if ((j + 1) % 3 == 0)
                            Console.Write($"[]");
                        else
                            Console.Write($"|");
                    }
                    else
                    {
                        if (field_for_player[i, j] != 0)
                            Console.Write($" {field_for_player[i, j]} ");
                        else
                            Console.Write($"   ");
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        if ((j + 1) % 3 == 0)
                            Console.Write($"[]");
                        else
                            Console.Write($"|");
                    }
                }
                Console.WriteLine();

                if ((i + 1) % 3 == 0)
                {
                    for (int k = 0; k < field_size + 31; k++)
                    {
                        Console.Write("=");
                    }
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    for (int k = 0; k < field_size + 31; k++)
                    {
                        Console.Write("-");
                    }
                }

                Console.WriteLine();
            }
            Console.ResetColor();
            Console.WriteLine();
            if (!win)
            {
                Console.WriteLine("Controls:\nA,W,S,D\nEND - Close game");
                if (show_answer)
                {
                    Console.WriteLine("H - Hide Answer\n");
                    ShowArr(field);
                }
                else
                    Console.WriteLine("H - Show Answer");
            }
            else
            {
                Console.WriteLine("" +
                    "33333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333\n" +
                    "33333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333\n" +
                    "3333333333333333333333333333333333333333333333333333333333333333333333333333333333333333XX3XXX333X3X3333333333\n" +
                    "3333333333333333333333333333333333333333333333333333333333333333333333333333333333333333X           S333333333\n" +
                    "333X  X33333   3333      XX33  333    333333  33333    3X    X, :33333    X3    333333333           rSX3333333\n" +
                    "333X  iS3333   5S  33Si    33  S33    SS3333  S2333    Si    S    X333    SS    S5333                   X33333\n" +
                    "333X  SS33     5S  SS33    S5  S33    SS3333  S2333    5S    S  .S  X3    SS    S5333  2S           rS  2S3333\n" +
                    "333333        iiS  5S33    S5  S33    SS3333  S  X3    SS    S  .SXX      Si    S53333X               2XSS3333\n" +
                    "33333333    SSS33  5S33    S5  S33    SS3333  S  SS    5S    S  .SX333    SS    S53333333SS       .SSSSS333333\n" +
                    "33333333    SS333  Si3X    S2  S3X    SS3333  S  iS    SS    S  .SX333    SS32iiS533333333333    iSS2333333333\n" +
                    "33333333    5S33333      2SSS33     55Si333333 r3    55SS    S  ,SX333    iS    33333333333X3    SS33333333333\n" +
                    "3333333333SiiS3333323SSSS5S3333355SSSS33333333325X35SSi3X33SSi5X2iX33333SSiS33S5SS33333333X       ,33333333333\n" +
                    "3333333333255533333335255553333355555533333333325332555X333555X325333333555533555233333333X:;,,,,,rXX333333333\n" +
                    "333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333SSSSSSSX333333333\n" +
                    "33333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333\n" +
                    "33333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333");
            }
        }
        static void Input()//Отслежевание ввода с клавиатуры
        {
            do
            {
                button = Console.ReadKey(true);
                if ((int)Char.GetNumericValue(button.KeyChar) >= 0 && (int)Char.GetNumericValue(button.KeyChar) < 10)
                {
                    num_to_insert = (int)Char.GetNumericValue(button.KeyChar);
                    pressed = key.INSERT;
                }
                else
                    switch (button.KeyChar)
                    {
                        case 'A':
                        case 'a':
                            pressed = key.LEFT;
                            break;
                        case 'D':
                        case 'd':
                            pressed = key.RIGHT;
                            break;
                        case 'W':
                        case 'w':
                            pressed = key.UP;
                            break;
                        case 'S':
                        case 's':
                            pressed = key.DOWN;
                            break;
                        case 'H':
                        case 'h':
                            pressed = key.ANSWER;
                            break;
                        case '\u001b':
                            pressed = key.END;
                            break;
                        default:
                            pressed = key.TRASH;
                            break;
                    }
            } while (pressed == key.TRASH && end_game);
        }
        static void Logic()//Логика игры
        {
            bool insert = false;
            switch (pressed)
            {
                case key.UP:
                    y--;
                    border_Y--;
                    break;
                case key.DOWN:
                    y++;
                    border_Y++;
                    break;
                case key.LEFT:
                    x--;
                    border_X--;
                    break;
                case key.RIGHT:
                    x++;
                    border_X++;
                    break;
                case key.INSERT:
                    for (int i = 0; i < static_elements_coordinates.Count; i += 2)
                    {
                        if (y == static_elements_coordinates[i] && x == static_elements_coordinates[i + 1])
                        {
                            insert = false;
                            break;
                        }
                        else
                            insert = true;
                    } //Проверка на статичный елемент

                    if (insert)
                    {
                        DeleteWrngEl(y, x);
                        field_for_player[y, x] = num_to_insert;
                    }
                    break;
                case key.ANSWER:
                    show_answer = !show_answer;
                    break;
                case key.END:
                    end_game = false;
                    break;
            }

            if (x < 0)
                x = field_size - 1;
            else if (x == field_size)
                x = 0;
            if (y < 0)
                y = field_size - 1;
            else if (y == field_size)  
                y = 0;                  //Логика ограничения поля

            switch (border_X)
            {
                case 0:
                    border_X += 3;
                    break;
                case 4:
                    border_X = 1;
                    break;
                default:
                    break;
            }        
            switch (border_Y)
            {
                case 4:
                case 1:
                    border_Y = 1;
                    section = border_X;
                    break;
                case 2:
                    section = border_Y + border_X + 1;
                    break;
                case 0: 
                case 3:
                    section = (border_Y = 3) + border_X + 3;
                    break;
                default:
                    break;
            }       //Определение секции курсора

            if ((field_for_player[y, x] != 0) && insert)
                CheckElement();

            if (win)
                end_game = false;
            if (!ElExist(field_for_player, 0) && wrong_elm_cord_lists.Count == 0)
                win = true;
        }

        static void Main()
        {
            Generate();
            Logic();

            while (end_game)
            {
                try
                {
                    Draw();
                    Input();
                    Logic();
                }
                catch (Exception exaption)
                {
                    Console.Clear();
                    Console.WriteLine($"{exaption.Message}\n\nHmmm how did you broke my game?");
                    end_game = false;
                    Console.ReadKey(true);
                }
            }
        }
    }
}