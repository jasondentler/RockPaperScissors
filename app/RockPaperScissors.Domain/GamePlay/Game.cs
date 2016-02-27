using System;
using System.Collections.Generic;
using System.Linq;
using RockPaperScissors.Messages;

namespace RockPaperScissors.Domain.GamePlay
{
    public class Game : IAggregateRoot, IEquatable<Game>
    {
        public Game()
        {
            Moves = new Dictionary<PlayerRef, Moves>();
            Players = new HashSet<PlayerRef>();
            Winners = new HashSet<PlayerRef>();
            SetState(GameStates.NotStarted);
        }
        
        public virtual int Id { get; set; }
        public ISet<PlayerRef> Players { get; protected set; }
        public IDictionary<PlayerRef, Moves> Moves { get; protected set; }
        public ISet<PlayerRef> Winners { get; protected set; }

        private StateBase _state;

        public GameStates State
        {
            get
            {
                return _state?.State ?? GameStates.NotStarted;
            }
            protected set
            {
                SetState(value);
            }
        }

        private void SetState(GameStates state)
        {
            switch (state)
            {
                case GameStates.NotStarted:
                    _state = new NotStartedState(this);
                    break;
                case GameStates.Started:
                    _state = new StartedState(this);
                    break;
                case GameStates.Completed:
                    _state = new CompletedState(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public bool Equals(Game other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Game);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public void Join(PlayerRef player)
        {
            _state.Join(player);
        }

        public void Start()
        {
            _state.Start();
        }

        public void Move(PlayerRef player, Moves move)
        {
            _state.Move(player, move);
        }

        public abstract class StateBase
        {
            public abstract GameStates State { get; }
            protected readonly Game Game;

            protected StateBase(Game game)
            {
                Game = game;
            }

            public virtual void Join(PlayerRef player)
            {
                throw new InvalidGameStateException(State);
            }

            public virtual void Start()
            {
                throw new InvalidGameStateException(State);
            }

            public virtual void Move(PlayerRef player, Moves move)
            {
                throw new InvalidGameStateException(State);
            }
        }

        private class NotStartedState : StateBase
        {
            public NotStartedState(Game game) : base(game)
            {
            }

            public override GameStates State
            {
                get { return GameStates.NotStarted; }
            }

            public override void Join(PlayerRef player)
            {
                if (player == null) throw new ArgumentNullException(nameof(player));
                if (Game.Players.Add(player))
                    DomainEvents.Raise(new PlayerJoined()
                    {
                        GameId = this.Game.Id,
                        PlayerId = player.PlayerId
                    });
            }

            public override void Start()
            {
                if (Game.Players.Count < 2)
                    throw new InvalidPlayerCountException(Game.Players.Count);

                Game.SetState(GameStates.Started);
                DomainEvents.Raise(new GameStarted() {GameId = Game.Id});
            }
        }

        private class StartedState : StateBase
        {
            public StartedState(Game game) : base(game)
            {
            }

            public override GameStates State
            {
                get { return GameStates.Started; }
            }

            public override void Start()
            {
                //NOOP
            }

            public override void Move(PlayerRef player, Moves move)
            {
                if (player == null) throw new ArgumentNullException(nameof(player));
                if (!Game.Players.Contains(player)) throw new PlayerNotInTheGameException(player);
                if (Game.Moves.ContainsKey(player)) throw new PlayerAlreadyMovedException(player);

                Game.Moves[player] = move;
                DomainEvents.Raise(new PlayerMoved()
                {
                    GameId = Game.Id,
                    PlayerId = player.PlayerId,
                    Move = (Messages.Moves) move
                });

                if (Game.Moves.Count == Game.Players.Count)
                    ProcessGameCompletion();
            }

            private void ProcessGameCompletion()
            {
                Game.SetState(GameStates.Completed);
                DomainEvents.Raise(new GameCompleted() {GameId = Game.Id});
                var losers = Losers.ToArray();
                var winners = Game.Players.Except(losers);
                if (!losers.Any())
                    DomainEvents.Raise(new GameTied() {GameId = Game.Id});
                else
                    foreach (var winner in winners)
                    {
                        Game.Winners.Add(winner);
                        DomainEvents.Raise(new GameWon() {GameId = Game.Id, PlayerId = winner.PlayerId});
                    }
            }

            private IEnumerable<PlayerRef> Losers
            {
                get
                {
                    foreach (var entry in Game.Moves)
                    {
                        var player = entry.Key;
                        var myMove = entry.Value;
                        if (IsLosingMove(myMove, Game.Moves.Values))
                            yield return player;
                    }
                }
            }

            private bool IsLosingMove(Moves myMove, IEnumerable<Moves> allMoves)
            {
                return allMoves.Any(otherMove => IsLosingMove(myMove, otherMove));
            }

            private bool IsLosingMove(Moves myMove, Moves otherMove)
            {
                switch (myMove)
                {
                    case GamePlay.Moves.Rock:
                        return otherMove == GamePlay.Moves.Paper;
                    case GamePlay.Moves.Paper:
                        return otherMove == GamePlay.Moves.Scissors;
                    case GamePlay.Moves.Scissors:
                        return otherMove == GamePlay.Moves.Rock;
                    default:
                        throw new NotSupportedException("Invalid move " + myMove);
                }
            }
        }

        private class CompletedState : StateBase
        {
            public CompletedState(Game game) : base(game)
            {
            }

            public override GameStates State
            {
                get { return GameStates.Completed; }
            }
        }
    }
}
