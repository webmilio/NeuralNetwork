// See https://aka.ms/new-console-template for more information

const int Columns = 3, Rows = 3;

double sum = 0;
int sign = 1;

/*double[,] M = 
{
    {  5,  1, 50, 7, 10 },
    { 10,  1,  3, 6, 13 },
    { 60, 15, 37, 8, 21 },
    { 11,  8, 66, 4, 70 },
    { 88,  4,  4, 9,  6 }
};*/
double[,] M =
{
    { 5, 10, 2 },
    { 6,  8, 1 },
    { 5, 44, 0 }
};

int upperBound = Columns - 1;
for (int c = 0; !(c == Columns && sign != 1); c++)
{
    double tmp = sign;

    for (int i = 0; i < Rows; i++)
    {
        int c2 = c + i * sign;

        if (c2 >= Columns)
            c2 -= Columns;
        else if (c2 < 0) 
            c2 += Columns;

        var a = M[i, c2];
        Console.Write("[{1}, {2}] {0, 3} ", a, c2, i);

        tmp *= a;
    }

    Console.WriteLine();
    Console.WriteLine("RES {0, 8}", tmp);

    sum += tmp;

    if (c == upperBound && sign != -1)
    {
        c = -1;
        sign = -1;
    }
}

Console.WriteLine("SUM {0}", sum);