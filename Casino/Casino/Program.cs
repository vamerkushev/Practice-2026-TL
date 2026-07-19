var printTitle = new[]
{
    "######       ###        #####     ######     ##   ##      #### ",
    "##          ## ##      ##           ##       ###  ##     ##  ##",
    "##         ##   ##     ######       ##       ## # ##     ##  ##",
    "##         #######         ##       ##       ##  ###     ##  ##",
    "######     ##   ##     #####      ######     ##   ##      #### "
};

foreach ( var line in printTitle )
{
    Console.WriteLine( line );
}

Console.WriteLine();
Console.WriteLine( "Привет, чемпион! Ознакомься с меню игры и выбери действие (Введи число):" );
Console.WriteLine( "1. Ввести депозит" );
Console.WriteLine( "2. Посмотреть баланс" );
Console.WriteLine( "3. Сделать ставку" );
Console.WriteLine( "4. Выйти из игры" );
Console.WriteLine( "5. Меню команд" );

int balance = 0;
const int multiplicator = 2;

string command = string.Empty;
command = Console.ReadLine() ?? string.Empty;
while ( ParseCommand( command ) )
{
    command = Console.ReadLine() ?? string.Empty;
}

int ReadDeposit()
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


void GameIteration()
{
    if ( balance <= 0 )
    {
        Console.WriteLine( "Денег нет! Пополните счёт! ('1'-для пополнения баланса)." );
        return;
    }

    string betStr = Console.ReadLine() ?? string.Empty;
    if ( !int.TryParse( betStr, out int bet ) )
    {
        Console.WriteLine( "Введите целое положительное число!" );
        return;
    }

    if ( bet > balance )
    {
        Console.WriteLine( "Недостаточно денег на депозите! Пополните счёт! ('1'-для пополнения баланса)." );
        return;
    }

    if ( bet <= 0 )
    {
        Console.WriteLine( "Введите целое положительное число!" );
        return;
    }

    int diff = CalcBalanceDiff( bet );
    balance += diff;

    Console.WriteLine( diff > 0 ? "Победа!" : "Поражение!" );
    Console.WriteLine( $"Ваш новый баланс: {balance}" );
}

int CalcBalanceDiff( int bet )
{
    int randomNum = Random.Shared.Next( 1, 21 );
    bool isWon = randomNum >= 18;

    return isWon ? bet * ( 1 + ( multiplicator * randomNum % 17 ) ) : -bet;
}

bool ParseCommand( string command )
{
    switch ( command )
    {
        case "1":
            Console.WriteLine( $"Введите ваш депозит:" );
            int deposit = ReadDeposit();
            balance += deposit;
            Console.WriteLine( $"Счёт пополнен! Ваш текущий баланс: {balance}." );
            return true;
        case "2":
            Console.WriteLine( $"Ваш баланс: {balance}." );
            return true;
        case "3":
            Console.WriteLine( $"Введите ставку. Ваш текущий баланс: {balance}." );
            GameIteration();
            return true;
        case "4":
            Console.WriteLine( $"Спасибо за игру! Ваш конечный баланс: {balance}." );
            return false;
        case "5":
            Console.WriteLine( "1. Ввести депозит." );
            Console.WriteLine( "2. Посмотреть баланс." );
            Console.WriteLine( "3. Сделать ставку." );
            Console.WriteLine( "4. Выйти из игры." );
            Console.WriteLine( "5. Меню команд." );
            return true;
        default:
            Console.WriteLine( "Команда не найдена! Смотрите меню команд! ('5'-для просмотра меню)." );
            return true;
    }
}