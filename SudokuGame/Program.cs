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

        static int x = 0, y = 1,border_X = x + 1, border_Y = y + 1, section = 0, num_to_insert = 0;

        static bool element_not_valid = false;

        const int field_size = 9;
        static int[,] field = new int[field_size, field_size];
        static int[,] field_for_player = new int[field_size, field_size];

        static List<int> static_elements_coordinates = new List<int> { };
        static List<int> wrong_elm_cord = new List<int> { };

        static ConsoleKeyInfo button;
        enum key { UP, DOWN, RIGHT, LEFT, INSERT, TRASH }
        static key pressed;

        static void ShowArr(int [,] arr)
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
                    bool good = false;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    switch (section)
                    {
                        case 1:
                            if (((y + 1) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if (((y + 1) == i) && ((x + 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if (((y + 2) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if (((y + 2) == i) && ((x + 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else good = true;
                            break;
                        case 2:
                            if(((y + 1) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y + 1) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y + 2) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y + 2) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else good = true;
                            break;
                        case 3:
                            if(((y + 1) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y + 1) == i) && ((x - 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y + 2) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if (((y + 2) == i) && ((x - 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else good = true;
                            break;
                        case 4:
                            if(((y - 1) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 1) == i) && ((x + 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y + 1) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y + 1) == i) && ((x + 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else good = true;
                            break;
                        case 5:
                            if(((y - 1) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y + 1) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 1) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y + 1) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else good = true;
                            break;
                        case 6:
                            if(((y - 1) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 1) == i) && ((x - 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y + 1) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y + 1) == i) && ((x - 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else good = true;
                            break;
                        case 7:
                            if(((y - 1) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 1) == i) && ((x + 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 2) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 2) == i) && ((x + 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else good = true;
                            break;
                        case 8:
                            if(((y - 1) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 1) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 2) == i) && ((x + 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 2) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else good = true;
                            break;
                        case 9:
                            if(((y - 1) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 2) == i) && ((x - 1) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 1) == i) && ((x - 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else if(((y - 2) == i) && ((x - 2) == j))
                                Console.BackgroundColor = ConsoleColor.Green;
                            else good = true;
                            break;
                        default:
                            good = true;
                            break;
                    }
                    for (int k = 0; k < wrong_elm_cord.Count; k += 2)
                    {
                        if (i == wrong_elm_cord[k] && j == wrong_elm_cord[k + 1])
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            good = false;
                            break;
                        }
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
                        if (element_not_valid)
                            Console.BackgroundColor = ConsoleColor.Red;
                        else
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
                    else {
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
            //ShowArr(field);
            //foreach (int i in wrong_elm_cord)
            //{
            //    Console.Write(i);
            //}
        }
        static void Input()//Отслежевание ввода с клавиатуры
        {
                button = Console.ReadKey();
                if ((int)Char.GetNumericValue(button.KeyChar) >= 0 && (int)Char.GetNumericValue(button.KeyChar) < 10)
                {
                    num_to_insert = (int)Char.GetNumericValue(button.KeyChar);
                    pressed = key.INSERT;
                }
                else
                    switch (button.KeyChar)
                    {
                        case 'a':
                            pressed = key.LEFT;
                            break;
                        case 'd':
                            pressed = key.RIGHT;
                            break;
                        case 'w':
                            pressed = key.UP;
                            break;
                        case 's':
                            pressed = key.DOWN;
                            break;
                        default:
                            pressed = key.TRASH;
                            break;
                    }
        }

        static void CheckElement()//Проверка на правильность поставленного елемента
        {
            element_not_valid = false;
            
            List<int> memory = new List<int>(20);
            List<int> buffer = new List<int>();
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
            switch (section)
            {
                case 1:
                    memory.Add(field_for_player[y + 1, x + 1]);
                    buffer.Add(y + 1);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y + 1, x + 2]);
                    buffer.Add(y + 1);
                    buffer.Add(x + 2);
                    memory.Add(field_for_player[y + 2, x + 1]);
                    buffer.Add(y + 2);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y + 2, x + 2]);
                    buffer.Add(y + 2);
                    buffer.Add(x + 2);
                    break;
                case 2:
                    memory.Add(field_for_player[y + 1, x + 1]);
                    buffer.Add(y + 1);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y + 1, x - 1]);
                    buffer.Add(y + 1);
                    buffer.Add(x - 1);
                    memory.Add(field_for_player[y + 2, x + 1]);
                    buffer.Add(y + 2);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y + 2, x - 1]);
                    buffer.Add(y + 2);
                    buffer.Add(x - 1);
                    break;
                case 3:
                    memory.Add(field_for_player[y + 1, x - 1]);
                    buffer.Add(y + 1);
                    buffer.Add(x - 1);
                    memory.Add(field_for_player[y + 1, x - 2]);
                    buffer.Add(y + 1);
                    buffer.Add(x - 2);
                    memory.Add(field_for_player[y + 2, x - 1]);
                    buffer.Add(y + 2);
                    buffer.Add(x - 1);
                    memory.Add(field_for_player[y + 2, x - 2]);
                    buffer.Add(y + 2);
                    buffer.Add(x - 2);
                    break;
                case 4:
                    memory.Add(field_for_player[y - 1, x + 1]);
                    buffer.Add(y - 1);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y - 1, x + 2]);
                    buffer.Add(y - 1);
                    buffer.Add(x + 2);
                    memory.Add(field_for_player[y + 1, x + 1]);
                    buffer.Add(y + 1);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y + 1, x + 2]);
                    buffer.Add(y + 1);
                    buffer.Add(x + 2);
                    break;
                case 5:
                    memory.Add(field_for_player[y - 1, x - 1]);
                    buffer.Add(y - 1);
                    buffer.Add(x - 1);
                    memory.Add(field_for_player[y + 1, x + 1]);
                    buffer.Add(y + 1);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y - 1, x + 1]);
                    buffer.Add(y - 1);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y + 1, x - 1]);
                    buffer.Add(y + 1);
                    buffer.Add(x - 1);
                    break;
                case 6:
                    memory.Add(field_for_player[y - 1, x - 1]);
                    buffer.Add(y - 1);
                    buffer.Add(x - 1);
                    memory.Add(field_for_player[y - 1, x - 2]);
                    buffer.Add(y - 1);
                    buffer.Add(x - 2);
                    memory.Add(field_for_player[y + 1, x - 1]);
                    buffer.Add(y + 1);
                    buffer.Add(x - 1);
                    memory.Add(field_for_player[y + 1, x - 2]);
                    buffer.Add(y + 1);
                    buffer.Add(x - 2);
                    break;
                case 7:
                    memory.Add(field_for_player[y - 1, x + 1]);
                    buffer.Add(y - 1);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y - 1, x + 2]);
                    buffer.Add(y - 1);
                    buffer.Add(x + 2);
                    memory.Add(field_for_player[y - 2, x + 1]);
                    buffer.Add(y - 2);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y - 2, x + 2]);
                    buffer.Add(y - 2);
                    buffer.Add(x + 2);
                    break;
                case 8:
                    memory.Add(field_for_player[y - 1, x + 1]);
                    buffer.Add(y - 1);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y - 1, x - 1]);
                    buffer.Add(y - 1);
                    buffer.Add(x - 1);
                    memory.Add(field_for_player[y - 2, x + 1]);
                    buffer.Add(y - 2);
                    buffer.Add(x + 1);
                    memory.Add(field_for_player[y - 2, x - 1]);
                    buffer.Add(y - 2);
                    buffer.Add(x - 1);
                    break;
                case 9:
                    memory.Add(field_for_player[y - 1, x - 1]);
                    buffer.Add(y - 1);
                    buffer.Add(x - 1);
                    memory.Add(field_for_player[y - 2, x - 1]);
                    buffer.Add(y - 2);
                    buffer.Add(x - 1);
                    memory.Add(field_for_player[y - 1, x - 2]);
                    buffer.Add(y - 1);
                    buffer.Add(x - 2);
                    memory.Add(field_for_player[y - 2, x - 2]);
                    buffer.Add(y - 2);
                    buffer.Add(x - 2);
                    break;
                default:
                    break;
            }
            buffer.Add(y);
            buffer.Add(x);
            if (memory.Contains(field_for_player[y, x]))
            {
                element_not_valid = true;
                wrong_elm_cord.Clear();
                for (int i = 0; i < buffer.Count; i += 2)
                {
                    if (field_for_player[y, x] == field_for_player[buffer[i],buffer[i+1]])
                    {
                        wrong_elm_cord.Add(buffer[i]);
                        wrong_elm_cord.Add(buffer[i+1]);
                    }
                }
            }
            buffer.Clear();
        }
        static void Logic()//Логика игры
        {
            bool insert = true;
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
                    } //Проверка на статичный елемент
                    if (insert)
                    {
                        if (field_for_player[y, x] != 0 && num_to_insert == 0)
                            wrong_elm_cord.Clear();
                        field_for_player[y, x] = num_to_insert;
                    }

                    break;
                default:
                    break;
            }
            if (insert)
            {
                for (int i = 0; i < static_elements_coordinates.Count; i += 2)
                {
                    if (y == static_elements_coordinates[i] && x == static_elements_coordinates[i + 1])
                    {
                        insert = false;
                        break;
                    }
                } //Проверка на статичный елемент
            }

            if (x < 0)
                x = field_size - 1;
            else if (x == field_size)
                x = 0;
            if (y < 0)
                y = field_size - 1;
            else if (y == field_size)  
                y = 0;                  //Логика ограничения курсора

            if (border_X == 0)
                border_X += 3;
            else if (border_X == 4)
                border_X = 1;
            if (border_Y == 0)
                border_Y += 3;
            else if (border_Y == 4)
                border_Y = 1;           //Логика для секций         

            switch (border_Y)
            {
                case 1:
                    section = border_X;
                    break;
                case 2:
                    section = border_Y + border_X + 1;
                    break;
                case 3:
                    section = border_Y + border_X + 3;
                    break;
                default:
                    break;
            }       //Определение секции курсора

            if ((field_for_player[y, x] != 0) && insert)
                CheckElement();
            else
                element_not_valid = false;
        }


        static void Main()
        {
            Generate();
            Logic();
            while (true)
            {
                Draw();
                Input();
                Logic();
            }
        }
    }
}