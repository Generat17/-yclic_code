using System;
using System.Collections.Generic;

namespace HomeWork
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> denominator = new List<int>() { 0, 0, 0, 1, 0, 1, 1 }; // Образующий векторкливер, фр
            List<int> result = new List<int>();
            List<int> infoVector = new List<int>();
            List<int> tmp = new List<int>();
            List<int> res = new List<int>();
            List<int> listError = new List<int>();
            int[] No = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 }; // Число обнаруженных ошибок
            int[] Cik = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 }; // Число комбинаций
            float[] Co = new float[8] { 0, 0, 0, 0, 0, 0, 0, 0 }; // Обнаруживающая способность
            int check = new int(); // Переменная для проверки
            int good = 0; // кол-во исправленных ошибок


            infoVector = EnterInfoVector();
            tmp = CopyList(infoVector);

            for (int i = 0; i < 3; i++) infoVector.Add(0);

            result = BinaryDivision(infoVector, denominator);

            // Прибавляет остаток от деления  информационному вектору
            switch (result.Count)
            {
                case 0:
                    tmp.Add(0);
                    tmp.Add(0);
                    tmp.Add(0);
                    break;
                case 1:
                    tmp.Add(0);
                    tmp.Add(0);
                    tmp.Add(1);
                    break;
                case 2:
                    tmp.Add(0);
                    tmp.Add(1);
                    tmp.Add(result[0]);
                    break;
                case 3:
                    tmp.Add(1);
                    tmp.Add(result[1]);
                    tmp.Add(result[0]);
                    break;
            }

            Console.Write("\nЗакодированный вектор: ");
            // Вывод результата кодирования
            foreach (var item in tmp)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();

            infoVector.RemoveRange(4, 3);

            // Генерация ошибок
            for (int i = 0; i < 128; i++)
            {
                int count = 0; // Кол-во 1-иц в векторе
                result.Clear();
                result = DecToBin(i);
                res.Clear();
                res = BinaryDivision(result, denominator);
                if (res.Count == 2)
                    res.Insert(0, 0);
                if (res.Count == 1)
                {
                    res.Insert(0, 0);
                    res.Insert(0, 0);
                } 

                if (!((res[0] == 0) && (res[1] == 0) && (res[2] == 0))) // Проверка наличия ошибки
                {
                    count = Count1(result);
                    No[count]++;
                }

                listError.Clear();
                listError = CopyList(tmp);
                // Определение синдрома ошибки
                if (res[0] == 1 && res[1] == 0 && res[2] == 0) // x0
                    listError[0] = NotBin(listError[0]);

                if (res[0] == 0 && res[1] == 1 && res[2] == 0) // x1
                    listError[1] = NotBin(listError[1]);

                if (res[0] == 0 && res[1] == 0 && res[2] == 1) // x2
                    listError[2] = NotBin(listError[2]);

                if (res[0] == 1 && res[1] == 1 && res[2] == 0) // x3
                    listError[3] = NotBin(listError[3]);

                if (res[0] == 0 && res[1] == 1 && res[2] == 1) // x4
                    listError[4] = NotBin(listError[4]);

                if (res[0] == 1 && res[1] == 1 && res[2] == 1) // x5
                    listError[5] = NotBin(listError[5]);

                if (res[0] == 1 && res[1] == 0 && res[2] == 1) // x6 
                    listError[6] = NotBin(listError[6]);

                listError.RemoveRange(4, 3);

                check = 1;
                for (int n = 0; n < infoVector.Count; n++)
                {
                    if (!(listError[n] == infoVector[n])) check = 0;
                }
                if (check == 1) good++;

            }

            // Алгоритм определения обнаруживающей способности кода
            Console.WriteLine();
            Console.WriteLine($"i\tCin\tNo\tCo");
            for (int i = 0; i < 8; i++)
            {
                Cik[i] = GetCombination(7, i);
                Co[i] = (float)No[i] / (float)Cik[i];
                Console.Write($"{i}\t{Cik[i]}\t");
                Console.Write($"{No[i]}\t");
                Console.WriteLine(Co[i]);
            }

            Console.WriteLine("\n\nНажмите любую клавишу для выхода.");
            Console.ReadKey();
        }

        /// <summary>
        /// Считает кол-во 1 в list int
        /// </summary>
        /// <param name="inputList">list в котором надо посчитать 1</param>
        /// <returns>возвращает кол-во 1</returns>
        public static int Count1(List<int> inputList)
        {
            int count = 0;
            for (int i = 0; i < inputList.Count; i++)
                if (inputList[i] == 1)
                    count++;
            return count;
        }

        /// <summary>
        /// Функция менят 0 на 1, а 1 на 0
        /// </summary>
        /// <param name="num">принимает цифру, которую надо инвертировать, либо 1, либо 0</param>
        /// <returns>возвращает инвертированое значение</returns>
        public static int NotBin(int num)
        {
            return num == 1 ? 0 : 1;
        }
        /// <summary>
        /// Функция перевода из десятичной системы счисления в двоичную
        /// </summary>
        /// <param name="dec">Принимает int число в десятичной системе счисления</param>
        /// <returns>Возвращает число в двоичной записи, где каждый элемент List int это один бит</returns>
        public static List<int> DecToBin(int dec)
        {
            List<int> outputList = new List<int>();

            int v = 64;
            for (int j = 0; j < 7; j++)
            {
                if (dec >= v)
                {
                    outputList.Add(1);
                    dec -= v;
                }
                else
                    outputList.Add(0);

                v /= 2;
            }

            return outputList;
        }

        /// <summary>
        /// Функция убирает незначащие нули в двоичном числе
        /// </summary>
        /// <param name="tmp">Принимает List int, где каждый элемент массива либо 0, либо 1</param>
        /// <returns></returns>
        public static List<int> RemovingZeros(List<int> tmp)
        {
            int index = -1;
            for (int i = 0; i < tmp.Count; i++)
                if (tmp[i] == 1) index = 1;
            if (index == -1)
            {
                tmp.Clear();
                tmp.Add(0);
                return tmp;
            }

            index = -1;

            for (int i = 0; i < tmp.Count; i++)
            {
                if (tmp[i] == 1)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                tmp.RemoveRange(0, index);
            }

            return tmp;
        }

        /// <summary>
        /// Функция для получения остатка от деления двух двоичных чисел
        /// </summary>
        /// <param name="numerator">Делимое</param>
        /// <param name="denominator">Делитель</param>
        /// <returns>Остаток от деления</returns>
        public static List<int> BinaryDivision(List<int> numerator, List<int> denominator)
        {
            numerator = RemovingZeros(numerator);
            denominator = RemovingZeros(denominator);
            int k = numerator.Count;
            int n = denominator.Count;
            int current = 0; // Текущее крайнее значение в делимом
            int count = 0; // Длина результата, через нее смотрит сколько битов нужно добавить
            List<int> result = new List<int>();

            if (k == 4)
            {
                if (numerator[0] == 1 && numerator[1] == 1 && numerator[2] == 0 && numerator[3] == 1)
                {
                    result.Clear();
                    for (int i = 4; i < k; i++)
                        result.Add(numerator[i]);
                    result = RemovingZeros(result);
                    return result;
                }
            }

            if (k < n)
                return numerator;

            for (int j = 0; j < 99; j++)
            {
                if (current >= k) break;
                result = RemovingZeros(result);
                count = result.Count;
                for (int i = count + 1; i < n + 1; i++)
                {
                    result.Add(numerator[current]);
                    current++;
                    if (current >= k) break;
                }

                if (result.Count < denominator.Count) break;
                for (int i = 0; i < n; i++)
                {
                    result[i] = result[i] ^ denominator[i];
                }
            }


            result = RemovingZeros(result);
            return result;
        }

        /// <summary>
        /// Функция для ввода информационного вектора,
        /// Также выполняет проверку на правильность ввода
        /// </summary>
        /// <returns>Возвращает информационный вектор</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static List<int> EnterInfoVector()
        {
            List<int> infoVector = new List<int>();
            Console.Write("Введите информационный вектор: ");
            var readLine = Console.ReadLine();
            if (readLine == null) throw new ArgumentNullException();
            var buf = readLine.ToCharArray();
            for (var i = 0; i < 4; i++)
            {
                switch (buf[i])
                {
                    case '0':
                        infoVector.Add(0);
                        break;
                    case '1':
                        infoVector.Add(1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("infoVector", "Info Vector must contain just 0 or 1");
                }
            }
            return infoVector;
        }

        /// <summary>
        /// Функция для нахождения факториала
        /// </summary>
        /// <param name="a">Принимает число для возведения в факториал</param>
        /// <returns>возвращает факториал принятого числа</returns>
        public static int Fact(int a)
        {
            var result = 1;
            for (var i = 1; i <= a; i++)
                result *= i;
            return result;
        }

        /// <summary>
        /// Функция считает число сочетаний из n по i
        /// </summary>
        /// <param name="n">Длина кодовой комбинации</param>
        /// <param name="i">кратность ошибки - число единиц в векторе ошибок</param>
        /// <returns>Возвращает число сочетаний из n по i</returns>
        public static int GetCombination(int n, int i)
        {
            return Fact(n) / (Fact(n - i) * Fact(i));
        }

        /// <summary>
        /// Функция копирования List int
        /// </summary>
        /// <param name="inputList">На вход отправляем вектор, который надо скопировать</param>
        /// <returns>Возвращает копию листа</returns>
        public static List<int> CopyList(List<int> inputList)
        {
            List<int> outputList = new List<int>();
            for (int i = 0; i < inputList.Count; i++)
            {
                outputList.Add(inputList[i]);
            }

            return outputList;
        }
    }
}