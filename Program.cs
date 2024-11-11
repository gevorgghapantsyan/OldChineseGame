namespace OldChineseGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var game = new StickGame(10,Player.Human);
            game.MachinePlayed += Game_MachinePlayed;
            game.HumanTurn += Game_HumanTurn;
            game.EndOfGame += Game_EndOfGame;
            game.Start();
        }

        private static void Game_EndOfGame(Player player)
        {
            Console.WriteLine($"Winner: {player}");
        }

        private static void Game_HumanTurn(object sender, int sticks)
        {
            Console.WriteLine($"Remaining sticks: {sticks}");
            Console.WriteLine("Take some sticks");
            bool takenCorrectly = false;
            while(!takenCorrectly) 
            {
                if(int.TryParse(Console.ReadLine(), out int TakenSticks)) 
                {
                    var game = (StickGame)sender;
                    try
                    {
                        game.HumanTakes(TakenSticks);
                        takenCorrectly = true;
                        
                    }
                    catch(ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message );
                    }
                }
            }
        }

        private static void Game_MachinePlayed(int obj)
        {
            Console.WriteLine($"Machine took: {obj}");
        }
    }
}