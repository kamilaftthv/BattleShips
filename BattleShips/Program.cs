class BattleShips
{
    static char[,] playerField = new char[10, 10];
    static char[,] computerField = new char[10, 10];
    static (int, int)? lastHit = null;

    static void Main()
    {
        InitializeFields();

        Console.WriteLine("Выберите способ расстановки кораблей:");
        Console.WriteLine("1 - Автоматически");
        Console.WriteLine("2 - Вручную");
        string setupOption = Console.ReadLine();

        if (setupOption == "1")
        {
            PlaceShipsAutomatically(playerField);
        }
        else if (setupOption == "2")
        {
            Console.WriteLine("Ввод вручную пока не работает, но он есть");
            PlaceShipsManually(playerField);
        }
        else
        {
            Console.WriteLine("Некорректный выбор. Пожалуйста, выберите 1 или 2.");
            return;
        }

        InitializeFields();
        PlaceShips(playerField);
        PlaceShips(computerField);

        while (true)
        {
            Console.Clear();
            DisplayFields();

            Console.WriteLine("Ваш ход. Введите координаты (например: A5):");
            string playerInput = Console.ReadLine().ToUpper();
            if (IsValidInput(playerInput))
            {
                ProcessPlayerTurn(playerInput);
                if (CheckGameOver(computerField))
                {
                    Console.WriteLine("Вы выиграли!!!");
                    break;
                }

                ProcessComputerTurn();
                if (CheckGameOver(playerField))
                {
                    Console.WriteLine("Вы проиграли :(");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Попробуйте ещё раз. Ваш ход. Введите координаты (например: A5).");
            }
        }

        Console.ReadLine();
    }

    static void InitializeFields()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                playerField[i, j] = '~';
                computerField[i, j] = '~';
            }
        }
    }

    static void PlaceShipsAutomatically(char[,] field)
    {
        Random random = new Random();

        PlaceShipAutomatically(field, random, 4, "четырехпалубный");
        for (int i = 0; i < 2; i++)
        {
            PlaceShipAutomatically(field, random, 3, "трехпалубный");
        }
        for (int i = 0; i < 3; i++)
        {
            PlaceShipAutomatically(field, random, 2, "двухпалубный");
        }
        for (int i = 0; i < 4; i++)
        {
            PlaceShipAutomatically(field, random, 1, "однопалубный");
        }
    }

    static void PlaceShipAutomatically(char[,] field, Random random, int shipSize, string shipName)
    {
        while (true)
        {
            int orientation = random.Next(2);
            int row = random.Next(10);
            int col = random.Next(10);

            if (CanPlaceShip(field, row, col, orientation, shipSize))
            {
                if (orientation == 0)
                {
                    for (int i = col; i < col + shipSize; i++)
                    {
                        field[row, i] = 'O';
                    }
                }
                else
                {
                    for (int i = row; i < row + shipSize; i++)
                    {
                        field[i, col] = 'O';
                    }
                }
                break;
            }
        }
    }

    static void PlaceShipsManually(char[,] field)
    {
        PlaceShipManually(field, 4, "четырехпалубного");
        for (int i = 0; i < 2; i++)
        {
            PlaceShipManually(field, 3, "трехпалубного");
        }
        for (int i = 0; i < 3; i++)
        {
            PlaceShipManually(field, 2, "двухпалубного");
        }
        for (int i = 0; i < 4; i++)
        {
            PlaceShipManually(field, 1, "однопалубного");
        }

        Console.WriteLine("Корабли успешно расставлены!");
    }

    static void PlaceShipManually(char[,] field, int shipSize, string shipName)
    {

        Console.WriteLine($"Введите положение {shipName} корабля: 1 - горизонтальное, 2 - вертикальное.");
        int orientation;
        while (!int.TryParse(Console.ReadLine(), out orientation) || (orientation != 1 && orientation != 2))
        {
            Console.WriteLine("Некорректный ввод. Попробуйте ещё раз.");
        }
        Console.WriteLine($"Введите координаты для {shipName} корабля. Например: A1.");

        bool isValidPlacement = false;
        while (!isValidPlacement)
        {
            string playerInput = Console.ReadLine().ToUpper();
            isValidPlacement = TryPlaceShipManually(field, playerInput, orientation, shipSize, shipName);
            if (!isValidPlacement)
            {
                Console.WriteLine("Неправильное расположение корабля. Попробуйте ещё раз.");
            }
        }
    }

    static bool TryPlaceShipManually(char[,] field, string input, int orientation, int shipSize, string shipName)
    {
        char[] validColumns = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
        if (input.Length < 2 || input.Length > 3)
        {
            return false;
        }
        int row, col;
        if (!int.TryParse(input.Substring(1), out row))
        {
            return false;
        }
        if (row < 1 || row > 10)
        {
            return false;
        }
        if (!char.TryParse(input.Substring(0, 1).ToUpper(), out char column))
        {
            return false;
        }
        if (!validColumns.Contains(column))
        {
            return false;
        }
        col = Array.IndexOf(validColumns, column);

        if (orientation == 1 && col + shipSize > 10)
        {
            return false;
        }
        else if (orientation == 2 && row + shipSize > 10)
        {
            return false;
        }

        if (orientation == 1)
        {
            for (int i = col; i < col + shipSize; i++)
            {
                if (i >= 10 || field[row, i] == 'O')
                {
                    Console.WriteLine("Неправильное расположение корабля. Попробуйте ещё раз.");
                    return false;
                }
            }
            for (int i = col; i < col + shipSize; i++)
            {
                field[row, i] = 'O';
            }
        }
        else
        {
            for (int i = row; i < row + shipSize; i++)
            {
                if (i >= 10 || field[i, col] == 'O')
                {
                    Console.WriteLine("Неправильное расположение корабля. Попробуйте ещё раз.");
                    return false;
                }
            }
            for (int i = row; i < row + shipSize; i++)
            {
                field[i, col] = 'O';
            }
        }
        return true;
    }

    static void PlaceShips(char[,] field)
    {
        Random random = new Random();
        PlaceShip(field, 4);
        for (int i = 0; i < 2; i++)
        {
            PlaceShip(field, 3);
        }
        for (int i = 0; i < 3; i++)
        {
            PlaceShip(field, 2);
        }
        for (int i = 0; i < 4; i++)
        {
            PlaceShip(field, 1);
        }
    }

    static void PlaceShip(char[,] field, int shipSize)
    {
        Random random = new Random();

        while (true)
        {
            int orientation = random.Next(2);
            int row = random.Next(10);
            int col = random.Next(10);

            if (CanPlaceShip(field, row, col, orientation, shipSize))
            {
                if (orientation == 0)
                {
                    for (int i = col; i < col + shipSize; i++)
                    {
                        field[row, i] = 'O';
                    }
                }
                else
                {
                    for (int i = row; i < row + shipSize; i++)
                    {
                        field[i, col] = 'O';
                    }
                }
                break;
            }
        }
    }

    static bool CanPlaceShip(char[,] field, int row, int col, int orientation, int shipSize)
    {
        if (orientation == 0 && col + shipSize > 10)
        {
            return false;
        }
        else if (orientation == 1 && row + shipSize > 10)
        {
            return false;
        }

        for (int i = Math.Max(0, row - 1); i < Math.Min(10, row + shipSize + 1); i++)
        {
            for (int j = Math.Max(0, col - 1); j < Math.Min(10, col + shipSize + 1); j++)
            {
                if (field[i, j] == 'O')
                {
                    return false;
                }
            }
        }
        return true;
    }

    static void DisplayFields()
    {
        Console.WriteLine("Поле человека:");
        DisplayField(playerField);
        Console.WriteLine("\nПоле компьютера:");
        DisplayComputerField();
    }

    static void DisplayField(char[,] field)
    {
        Console.Write("  A B C D E F G H I J\n");
        for (int i = 0; i < 10; i++)
        {
            Console.Write((i + 1) + " ");
            for (int j = 0; j < 10; j++)
            {
                Console.Write(field[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static void DisplayComputerField()
    {
        char[,] hiddenComputerField = new char[10, 10];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                hiddenComputerField[i, j] = computerField[i, j] == 'O' ? '~' : computerField[i, j];
            }
        }
        DisplayField(hiddenComputerField);
    }

    static bool IsValidInput(string input)
    {
        if (input.Length < 2 || input.Length > 3)
        {
            return false;
        }
        char[] validColumns = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
        int row, col;
        if (!int.TryParse(input.Substring(1), out row))
        {
            return false;
        }
        if (row < 1 || row > 10)
        {
            return false;
        }
        if (!char.TryParse(input.Substring(0, 1).ToUpper(), out char column))
        {
            return false;
        }
        if (!validColumns.Contains(column))
        {
            return false;
        }
        col = Array.IndexOf(validColumns, column) + 1;
        return true;
    }

    static void ProcessPlayerTurn(string input)
    {
        char[] validColumns = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
        int row, col;
        char column = input[0];
        row = int.Parse(input.Substring(1)) - 1;
        col = Array.IndexOf(validColumns, column);

        if (computerField[row, col] == 'O')
        {
            computerField[row, col] = 'X';
        }
        else
        {
            computerField[row, col] = '·';
        }
    }

    static void ProcessComputerTurn()
    {
        Random random = new Random();
        char[] validColumns = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

        if (lastHit != null)
        {
            (int row, int col) = lastHit.Value;
            if (TryHitAdjacentCell(row, col, out int newRow, out int newCol))
            {
                if (playerField[newRow, newCol] == 'O')
                {
                    playerField[newRow, newCol] = 'X';
                    lastHit = (newRow, newCol);
                    return;
                }
                else if (playerField[newRow, newCol] == '~')
                {
                    playerField[newRow, newCol] = '·';
                    lastHit = null;
                    return;
                }
            }
            else
            {
                lastHit = null;
            }
        }
        while (true)
        {
            int row = random.Next(10);
            int col = random.Next(10);

            if (playerField[row, col] == 'O')
            {
                playerField[row, col] = 'X';
                lastHit = (row, col);
                break;
            }
            else if (playerField[row, col] == '~')
            {
                playerField[row, col] = '·';
                break;
            }
        }
    }

    static bool TryHitAdjacentCell(int row, int col, out int newRow, out int newCol)
    {
        int[][] directions = { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { -1, 0 } };

        foreach (var direction in directions)
        {
            int newRowTemp = row + direction[0];
            int newColTemp = col + direction[1];

            if (newRowTemp >= 0 && newRowTemp < 10 && newColTemp >= 0 && newColTemp < 10 &&
                playerField[newRowTemp, newColTemp] == 'O')
            {
                newRow = newRowTemp;
                newCol = newColTemp;
                return true;
            }
        }
        newRow = 0;
        newCol = 0;
        return false;
    }

    static bool CheckGameOver(char[,] field)
    {
        int totalShips = 0;

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (field[i, j] == 'O')
                {
                    totalShips++;
                }
            }
        }
        return totalShips == 0;
    }
}