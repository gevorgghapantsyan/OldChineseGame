namespace OldChineseGame
{
    enum Player
    {
        Human,
        Machine
    }
    enum GameStatus
    {
        InProgress,
        NotStarted,
        GameIsOver
    }
    internal class StickGame
    {
        private readonly Random randomizer;
        public int InitialSticksNumber { get; }
        public Player Turn { get; private set; }
        public int RemainingSticksNumber { get; private set; }
        public GameStatus GameStatus { get; private set; }
        public event Action<int> MachinePlayed;
        public event EventHandler<int> HumanTurn;
        public event Action<Player> EndOfGame;

        public StickGame(int InitialSticksNumber, Player FirstPlayer)
        {
            if (InitialSticksNumber < 7 || InitialSticksNumber > 30)
            {
                throw new ArgumentException("Initial stick's number should be >= 7 and <=30");
            }
            this.InitialSticksNumber = InitialSticksNumber;
            randomizer = new Random();
            GameStatus = GameStatus.NotStarted;
            RemainingSticksNumber = InitialSticksNumber;
            Turn = FirstPlayer;
        }

        public void Start()
        {
            if(GameStatus == GameStatus.GameIsOver)
            {
                RemainingSticksNumber = InitialSticksNumber;
            }
            if(GameStatus == GameStatus.InProgress)
            {
                throw new InvalidOperationException("Can't call Start when game is already in progress");
            }

            GameStatus=GameStatus.InProgress;
            while(GameStatus == GameStatus.InProgress)
            {
                if(Turn == Player.Machine)
                {
                    MachineMakesMove();
                }
                else
                {
                    HumanMakesMove();
                }
                FireEndOfGameIfRequired();
                Turn = Turn == Player.Machine ? Player.Human : Player.Machine;
            }
        }

        public void HumanTakes(int sticks)
        {
            if(sticks < 1 || sticks > 3)
            {
                throw new ArgumentException("You can take 1 2 or 3 sticks");
            }
            if(sticks > RemainingSticksNumber)
            {
                throw new ArgumentException($"You can't take more than remaining sticks: {RemainingSticksNumber}");
            }
            RemainingSticksNumber -= sticks; ;
        }

        private void FireEndOfGameIfRequired()
        {
            if(RemainingSticksNumber == 0)
            {
                GameStatus = GameStatus.GameIsOver;
                if(EndOfGame != null)
                {
                    EndOfGame(Turn = Turn == Player.Machine ? Player.Human : Player.Machine);
                }
            }
        }

        private void MachineMakesMove()
        {
            int maxNum = RemainingSticksNumber >= 3 ? 3 : RemainingSticksNumber;
            int sticks = randomizer.Next(1,maxNum);
            TakeSticks(sticks);
            if(MachinePlayed != null)
            {
                MachinePlayed(sticks);
            }
        }

        private void TakeSticks(int sticks)
        {
            RemainingSticksNumber -= sticks;
        }

        private void HumanMakesMove()
        {
            if(HumanTurn != null)
            {
                HumanTurn(this, RemainingSticksNumber);
            }
        }

    }
}
