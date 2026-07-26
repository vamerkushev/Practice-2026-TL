void PrintIntro()
{
    Console.WriteLine( """
                  ######       ###        #####     ######     ##   ##      ####
                  ##          ## ##      ##           ##       ###  ##     ##  ##
                  ##         ##   ##     ######       ##       ## # ##     ##  ##
                  ##         #######         ##       ##       ##  ###     ##  ##
                  ######     ##   ##     #####      ######     ##   ##      ####
                  """ );
}

void PrintMenu()
{
    Console.WriteLine();
    Console.WriteLine( "Привет, чемпион! Ознакомься с меню игры и выбери действие (Введи число):" );
    Console.WriteLine( "1. Ввести депозит" );
    Console.WriteLine( "2. Посмотреть баланс" );
    Console.WriteLine( "3. Сделать ставку" );
    Console.WriteLine( "4. Выйти из игры" );
    Console.WriteLine( "5. Меню команд" );
}

int CheckValidInt()
{
    while ( true )
    {
        string inputDep = Console.ReadLine() ?? string.Empty;
        if ( int.TryParse( inputDep, out int dep ) && dep > 0 )
        {
            return dep;
        }
        Console.WriteLine( "Введите целое положительное число!" );
    }
}

void GameIteration( ref int balance, int multiplicator )
{
    if ( balance <= 0 )
    {
        Console.WriteLine( "Денег нет! Пополните счёт! ('1'-для пополнения баланса)." );
        return;
    }

    int bet = CheckValidInt();

    if ( bet > balance )
    {
        Console.WriteLine( "Недостаточно денег на депозите! Пополните счёт! ('1'-для пополнения баланса)." );
        return;
    }

    int diff = CalcBalanceDiff( bet, multiplicator );
    balance += diff;

    Console.WriteLine( diff > 0 ? "Победа!" : "Поражение!" );
    Console.WriteLine( $"Ваш новый баланс: {balance}" );
}

int CalcBalanceDiff( int bet, int multiplicator )
{
    int randomNum = Random.Shared.Next( 1, 21 );
    bool isWon = randomNum >= 18;

    return isWon ? bet * ( 1 + ( multiplicator * randomNum % 17 ) ) : -bet;
}

void MakeDeposit( ref int balance )
{
    Console.WriteLine( $"Введите ваш депозит:" );
    int deposit = CheckValidInt();
    balance += deposit;
    Console.WriteLine( $"Счёт пополнен! Ваш текущий баланс: {balance}." );
}

void MakeBet( ref int balance, int multiplicator )
{
    Console.WriteLine( $"Введите ставку. Ваш текущий баланс: {balance}." );
    GameIteration( ref balance, multiplicator );
}

void RunGame()
{
    int balance = 0;
    const int multiplicator = 2;

    while ( true )
    {
        string command = Console.ReadLine() ?? string.Empty;
        switch ( command )
        {
            case "1":
                MakeDeposit( ref balance );
                break;
            case "2":
                Console.WriteLine( $"Ваш баланс: {balance}." );
                break;
            case "3":
                MakeBet( ref balance, multiplicator );
                break;
            case "4":
                Console.WriteLine( $"Спасибо за игру! Ваш конечный баланс: {balance}." );
                return;
            case "5":
                PrintMenu();
                break;
            default:
                Console.WriteLine( "Команда не найдена! Смотрите меню команд! ('5'-для просмотра меню)." );
                break;
        }
    }
}

void Main()
{
    PrintIntro();
    PrintMenu();
    RunGame();
}

Main();