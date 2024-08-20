using System;

class Program
{
    static char[] options;
    static List<int> optList;
    static string message;
    static int player;
    static int selected;
    static int endgame;
    static bool playing = true;

    static void Main(string[] args)
    {
        while (playing)
        {
            Start();                           // Inicializa variáveis

            while (endgame == 0)
            {
                Console.Clear();               // Limpa o console a cada jogada
                DisplayPlayer();               // Desenha o cabeçalho do jogo contendo os jogadores
                ShowPlayer();                  // Jogador da vez
                PrintMessage();                // Caso exista mensagem de erro, imprime um alerta
                PrintBoard();                  // Desenha o jogo da velha
                GetPlayerSelectedOption();     // Obtem a jogada
                SelectOption();                // Efetua a jogada
                endgame = CheckEndGame();      // Valida fim do jogo
            }

            GameOver();                        // Exibe vencedor ou empate
            RestartOrExit();                   // Reiniciar ou sair do jogo
        }
    }

    private static void Start()
    {
        options = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        optList = new List<int>();
        message = "";
        player = 1;
        endgame = 0;

        foreach (char option in options)
        {
            int i = int.Parse(option.ToString());
            optList.Add(i);
        }
    }
  
    private static void DisplayPlayer()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Jogador 1: X");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Jogador 2: O");
        Console.ResetColor();
        Console.WriteLine("\n");
    }

    private static void ShowPlayer()
    {
        string playing = "Aguardando seleção do jogador ";
        if (player % 2 == 1)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            playing += "1";
            Console.WriteLine(playing);
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            playing += "2";
            Console.WriteLine(playing);
            Console.ResetColor();
        }
    }

    private static void PrintMessage()
    {
        if (!string.IsNullOrEmpty(message))
        {
            Console.WriteLine(message);
        }

        Console.WriteLine("\n");
    }

    
    private static void PrintBoard()
    {
        string column = ("     ||     ||     ");
        string line = ("=====||=====||=====");
        string cell = ("||");

        for (int i = 0; i < 9; i += 3)
        {
            Console.WriteLine(column);
            PrintCell(options[i]);
            Console.Write(cell);
            PrintCell(options[i + 1]);
            Console.Write(cell);
            PrintCell(options[i + 2]);
            Console.WriteLine();
            Console.WriteLine(column);
            if (i < 6) Console.WriteLine(line);
        }
    }

    // Pinta o quadro quando for X, O ou um número
    private static void PrintCell(char c)
    {
        if (c == 'X')
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else if (c == 'O')
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        else if (int.TryParse(c.ToString(), out int result))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        else
        {
            Console.ResetColor();
        }

        Console.Write($"  {c}  ");
        Console.ResetColor();
    }


    private static void GetPlayerSelectedOption()
    {
        // Trata o RL null
        try
        {
            Console.WriteLine("\n");
            Console.WriteLine("Digite um número disponível no quadro acima e pressione ENTER.");
            Console.Write("Sua opção: ");
            selected = int.Parse(Console.ReadLine());
        }
        catch
        {
            // A opção é inválida
        }
    }

    
    private static void SelectOption()
    {
        // Se a opção é válida, altera a posição de options para o símbolo do respectivo jogador
        if (CheckOpt(selected))
        {
            message = "";
            selected--;
            options[selected] = player % 2 == 0 ? 'O' : 'X';
            player++;
        }
        // Se a posição já está marcada, o jogador tem que escolher novamente
        else
        {
            message = " - As opções disponíveis são: " + string.Join(", ", optList) + ".";
        }
    }

    // Valida as opções disponíveis
    private static bool CheckOpt(int selectedOpt)
    {
        foreach (int ol in optList)
        {
            if (ol == selectedOpt)
            {
                //Remove da lista de opções o número informado
                optList.Remove(ol);
                return true;
            }
        }
        // A opção informada é inválida
        return false;
    }

    private static int CheckEndGame()
    {
        // Se uma destas sequencias for formada, um jogador venceu
        if (WinConditions())
        {
            return 1;
        }

        // Se todas opções foram selecionadas há um empate
        else if (optList.Count == 0)
        {
            return 2;
        }
        // Caso contrário, o jogo ainda está em andamento
        else
        {
            return 0;
        }
    }

    private static bool WinConditions()
    {
        int[][] conditions = new int[][]
        {
            new int[] { 0, 1, 2 }, // linha 1
            new int[] { 3, 4, 5 }, // linha 2
            new int[] { 6, 7, 8 }, // linha 3
            new int[] { 0, 3, 6 }, // coluna 1
            new int[] { 1, 4, 7 }, // coluna 2
            new int[] { 2, 5, 8 }, // coluna 3
            new int[] { 0, 4, 8 }, // diagonal 1
            new int[] { 2, 4, 6 }  // diagonal 2
        };

        foreach (var condition in conditions)
        {
            if (options[condition[0]] == options[condition[1]] && options[condition[1]] == options[condition[2]])
            {
                return true;
            }
        }
        return false; 
    }
   
    private static void GameOver()
    {
        Console.Clear();
        
        // Há um ganhador
        if (endgame == 1)
        {
            Console.ForegroundColor = player % 2 == 0 ? ConsoleColor.Red : ConsoleColor.Cyan;
            Console.WriteLine($"\nO jogador {(player % 2) + 1} venceu a partida!\n");
        }

        // Empatou
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nTemos um empate!\n");
        }
        Console.ResetColor();

        // Exibe o resultado do quadro
        PrintBoard();
    }

    public static void RestartOrExit()
    {
        // Monitora teclas pressionadas para saber se a intenção do jogador é...
        Console.WriteLine("\nPressione ESC para encerrar a partida ou pressione F1 para reiniciar o jogo");
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Esc, encerrar a partida
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                playing = false;
                break;
            }

            // F1, reinciar a partida
            if (keyInfo.Key == ConsoleKey.F1)
            {
                break;
            }
        }
    }
    
}
