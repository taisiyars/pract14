
using Microsoft.Win32;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace libmas
{
    public class ArrayMatrix
    {

        public static void InitMas(out int[,] mas, int row, int column)
        {
            Random Rnd = new Random();
            mas = new Int32[row, column];


            for (int i = 0; i < mas.GetLength(0); i++)
                for (int j = 0; j < mas.GetLength(1); j++) mas[i, j] = Rnd.Next(1, 5);
        }

        public static void Open(ref int[,] matr)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".txt";
            open.Filter = "Все файлы (*.*) |*.*| Текстовые файлы | *.txt*";
            open.FilterIndex = 2;
            open.Title = "Открытие таблицы";
            if (open.ShowDialog() == true)
            {
                    StreamReader file = new StreamReader(open.FileName);
                    int row = Convert.ToInt32(file.ReadLine());
                    int col = Convert.ToInt32(file.ReadLine());
                    matr = new int[row, col];
                    for (int i = 0; i < matr.GetLength(0); i++)
                    {
                        for (int j = 0; j < matr.GetLength(1); j++)
                        {
                            string a = file.ReadLine();
                            bool f1;
                            int value;
                            f1 = int.TryParse(a, out value);
                            if (f1)
                            {
                                matr[i, j] = value;

                            }

                            else MessageBox.Show("Некоректные значения");
                        }
                    }
                    file.Close();
            }
        }

        public static void Save(int[,] array)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".txt";
            save.Filter = "Все файлы (*.*) |*.*| Текстовые файлы | *.txt*";
            save.FilterIndex = 1;
            save.Title = "Сохранение таблицы";
            if (save.ShowDialog() == true)
            {
                StreamWriter outfile = new StreamWriter(save.FileName, false);
                outfile.WriteLine(array.GetLength(0));
                outfile.WriteLine(array.GetLength(1));
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        outfile.WriteLine(array[i, j]);
                    }
                }
                outfile.Close();
            }
            else MessageBox.Show("Не удалось открыть окно");
        }
    }

    public static class VisualArray
    {
        //Метод для двухмерного массива
        public static DataTable ToDataTable<T>(this T[,] matrix)
        {
            var res = new DataTable();
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                res.Columns.Add("col" + (i + 1), typeof(T));
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var row = res.NewRow();

                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    row[j] = matrix[i, j];
                }

                res.Rows.Add(row);
            }

            return res;
        }
    } 
}
