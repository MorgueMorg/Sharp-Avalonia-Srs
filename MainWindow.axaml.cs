using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Diagnostics;
using System.Linq;

namespace FormochkaTwo
{
    public partial class MainWindow : Window
    {
        private int[] arrayToSort;

        public MainWindow()
        {
            InitializeComponent();
            arrayToSort = new int[0]; 
        }

        private void GenerateArray_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ArrayLengthTextBox.Text, out int length) && length > 0)
            {
                arrayToSort = GenerateRandomArray(length);
                InputTextBox.Text = string.Join(" ", arrayToSort);
                DrawChart(arrayToSort); 
            }
        }

        private void Sort(int[] array, Action<int[]> sortingMethod)
        {
            if (array.Length > 0)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                sortingMethod(array);

                stopwatch.Stop();
                TimeSpan elapsedTime = stopwatch.Elapsed;

                UpdateResultText(array, elapsedTime);
                DrawChart(array); // Обновить график
            }
        }

        private void CompareAlgorithms_Click(object sender, RoutedEventArgs e)
        {
            string comparisonText = "Сравнение алгоритмов:\n\n" +
                "Радиксная сортировка: Эффективная сортировка для целых чисел, работает за линейное время.\n\n" +
                "Сортировка Бэтчера: Интересный алгоритм сортировки, использующий сортировку вставками с различными интервалами.\n\n" +
                "Сортировка слиянием: Стабильная сортировка, разделяющая массив на более мелкие части и сливающая их в порядке.";

            var comparisonWindow = new Window()
            {
                Title = "Сравнение алгоритмов",
                Width = 400,
                Height = 300,
                Content = new TextBlock
                {
                    Text = comparisonText,
                    TextWrapping = TextWrapping.Wrap,
                }
            };

            comparisonWindow.ShowDialog(this);
        }



        private void SortRadix_Click(object sender, RoutedEventArgs e)
{
    ParseInputArray();
    Sort(arrayToSort, RadixSort);
    DescriptionText.Text = "Сортировка радиксом (Radix Sort) является устойчивой сортировкой и используется для упорядочивания элементов в массиве, как правило, целых чисел. Этот алгоритм основан на разрядной сортировке и работает путем разбиения чисел на более мелкие разряды и сортировки их по каждому разряду по отдельности, начиная с наименее значащего разряда и заканчивая наиболее значащим.";
}


private void SortBatcher_Click(object sender, RoutedEventArgs e)
{
    ParseInputArray();
    Sort(arrayToSort, BatcherSort);
    DescriptionText.Text = "Сортировка Бэтчера: Сортировка, использующая сортировку вставками с различными интервалами. Сортировка Бэтчера (Batcher's Odd-Even Mergesort) - это алгоритм сортировки, который применяется для упорядочивания элементов в массиве. Он примечателен тем, что является одним из нескольких сортировок, которые могут быть эффективно реализованы в параллельном режиме, что делает его привлекательным для многозадачных сред и многоядерных процессоров. ";
}

 private void SortMerge_Click(object sender, RoutedEventArgs e)
{
    ParseInputArray();
    Sort(arrayToSort, MergeSort);
    DescriptionText.Text = "Алгоритм слияния (Merge Sort) - это эффективный и устойчивый алгоритм сортировки, который разделяет массив на две равные части, сортирует каждую из них и затем объединяет их в упорядоченный массив. Этот метод сортировки базируется на стратегии \"разделяй и властвуй\", что делает его эффективным даже для больших наборов данных.";
}


        private void UpdateResultText(int[] sortedArray, TimeSpan elapsedTime)
        {
            ResultText.Text = "Отсортированный массив: " + string.Join(", ", sortedArray) + Environment.NewLine +
                             "Время выполнения: " + elapsedTime.TotalMilliseconds + " мс";
        }

        private void ParseInputArray()
        {
            string input = InputTextBox.Text;
            string[] values = input.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length > 0)
            {
                arrayToSort = values.Select(int.Parse).ToArray();
            }
            else
            {
                arrayToSort = new int[0];
            }
        }

        private int[] GenerateRandomArray(int length)
        {
            Random random = new Random();
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = random.Next(100); // Генерация случайных чисел от 0 до 99
            }
            return array;
        }

        private void DrawChart(int[] array)
        {
            // Canvas clear
            ChartCanvas.Children.Clear();

            int barWidth = (int)ChartCanvas.Width / array.Length;
            int maxHeight = (int)ChartCanvas.Height;

            for (int i = 0; i < array.Length; i++)
            {
                int barHeight = array[i] * maxHeight / array.Max(); // Пропорциональная высота столбца

                Rectangle bar = new Rectangle
                {
                    Width = barWidth,
                    Height = barHeight,
                    Fill = new SolidColorBrush(Colors.Blue), // Создание кисти 
                    Margin = new Thickness(i * barWidth, maxHeight - barHeight, 0, 0)
                };

                ChartCanvas.Children.Add(bar);
            }
        }

        private void RadixSort(int[] array)
        {
            int max = array.Max();
            for (int exp = 1; max / exp > 0; exp *= 10)
            {
                CountingSort(array, exp);
            }
        }

        private void CountingSort(int[] array, int exp)
        {
            int length = array.Length;
            int[] output = new int[length];
            int[] count = new int[10]; // Для десятичной системы счисления

            for (int i = 0; i < 10; i++)
            {
                count[i] = 0;
            }

            for (int i = 0; i < length; i++)
            {
                count[(array[i] / exp) % 10]++;
            }

            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = length - 1; i >= 0; i--)
            {
                output[count[(array[i] / exp) % 10] - 1] = array[i];
                count[(array[i] / exp) % 10]--;
            }

            for (int i = 0; i < length; i++)
            {
                array[i] = output[i];
            }
        }

        private void BatcherSort(int[] array)
        {
            int length = array.Length;
            for (int gap = length / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < length; i++)
                {
                    int temp = array[i];
                    int j;
                    for (j = i; j >= gap && array[j - gap] > temp; j -= gap)
                    {
                        array[j] = array[j - gap];
                    }
                    array[j] = temp;
                }
            }
        }


        private void MergeSort(int[] array)
        {
            int length = array.Length;
            if (length < 2)
            {
                return;
            }

            int mid = length / 2;
            int[] left = new int[mid];
            int[] right = new int[length - mid];

            Array.Copy(array, left, mid);
            Array.Copy(array, mid, right, 0, length - mid);

            MergeSort(left);
            MergeSort(right);

            Merge(array, left, right);
        }

        private void Merge(int[] array, int[] left, int[] right)
        {
            int i = 0, j = 0, k = 0;

            while (i < left.Length && j < right.Length)
            {
                if (left[i] <= right[j])
                {
                    array[k++] = left[i++];
                }
                else
                {
                    array[k++] = right[j++];
                }
            }

            while (i < left.Length)
            {
                array[k++] = left[i++];
            }

            while (j < right.Length)
            {
                array[k++] = right[j++];
            }
        }
    }
}
