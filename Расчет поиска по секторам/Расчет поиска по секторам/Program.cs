
using System;
using System.Data;

double xTek, yTek, xNach, yNach, Vp, Do;

Console.WriteLine("Расчет поиска по секторам");
Console.WriteLine();
Console.WriteLine("Введите абсциссу текущего местоположения поискового средства: ");
xTek = double.Parse(Console.ReadLine());
Console.WriteLine("Введите ординату текущего местоположения поискового средства: ");
yTek = double.Parse(Console.ReadLine());
Console.WriteLine("Точка начала поиска (x нач.) : ");
xNach = double.Parse(Console.ReadLine());
Console.WriteLine("Текущее местоположение (y нач.) : ");
yNach = double.Parse(Console.ReadLine());
Console.WriteLine("Скорость поиска : ");
Vp = double.Parse(Console.ReadLine());
Console.WriteLine("Дальность обнаружения : ");
Do = double.Parse(Console.ReadLine());

double alpha = Math.PI / 3.0; // Угол α в радианах
double beta = Math.PI / 3.0; // Угол β в радианах

// Расчет времени движения до точки начала поиска
double tUst = Math.Sqrt(Math.Pow(xNach - xTek, 2.0) + Math.Pow(yNach - yTek, 2.0)) / Vp;

// Расчет количества галсов
double n = Math.Ceiling(Math.PI / beta);

// Создание массивов для хранения параметров галсов
double[] xJ = new double[(int)n]; //абсциссы окончания
double[] yJ = new double[(int)n]; //ординаты окончания
double[] kJ = new double[(int)n]; //курсы лежания
double[] dtGJ = new double[(int)n]; //отрезок времени лежания
double[] tGJ = new double[(int)n]; //отрезок с начала поиска до окончания

// Расчет параметров первого галса в массивы
xJ[0] = xTek + Do * Math.Cos(alpha);
yJ[0] = yTek + Do * Math.Sin(alpha);
kJ[0] = Math.Atan2(yJ[0] - yTek, xJ[0] - xTek);
dtGJ[0] = beta * Do / Vp;
tGJ[0] = tUst;

for (int i = 1; i < n; i++)
{
    kJ[i] = kJ[i - 1] + beta;
    xJ[i] = xTek + Do * Math.Cos(kJ[i]);
    yJ[i] = yTek + Do * Math.Sin(kJ[i]);
    dtGJ[i] = beta * Do / Vp;
    tGJ[i] = tGJ[i - 1] + dtGJ[i];
}
// Общее время поиска
double tPoisk = tUst + tGJ[(int)n - 1];

DataTable table = new DataTable();
table.Columns.Add("№", typeof(int));
table.Columns.Add("xJ", typeof(double));
table.Columns.Add("yJ", typeof(double));
table.Columns.Add("kJ", typeof(double));
table.Columns.Add("dtGJ", typeof(double));
table.Columns.Add("tGJ", typeof(double));

// Заполняем таблицу данными из массивов
for (int i = 0; i < xJ.Length; i++)
{
    table.Rows.Add(i + 1, Math.Round(xJ[i], 2), Math.Round(yJ[i], 2), Math.Round(kJ[i], 2), Math.Round(dtGJ[i], 2), Math.Round(tGJ[i], 2));
}

// Выводим таблицу в консоль
Console.WriteLine("Таблица:");
foreach (DataColumn column in table.Columns)
{
    Console.Write(column.ColumnName + "\t");
}
Console.WriteLine();
foreach (DataRow row in table.Rows)
{
    foreach (var item in row.ItemArray)
    {
        Console.Write(item + "\t");
    }
    Console.WriteLine();
}

Console.WriteLine("Колличество галсов : " + n);
Console.WriteLine("Время движения от точки текущего местоположения (xтек., yтек.) до точки начала поиска (xнач., yнач.) tуст. : " + tUst);
Console.WriteLine("Общее время поиска tп. :" + tPoisk);
