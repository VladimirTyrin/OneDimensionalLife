using System;
using System.IO;
using System.Text;
using OneDimensionalLife.Enums;

namespace OneDimensionalLife
{
    public class Game
    {
        #region public

        public Game(string filename)
        {
            Filename = filename;
        }

        public GameOperationStatus Initialize()
        {
            try
            {
                using (var fileStream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var reader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        var lengthString = reader.ReadLine();
                        if (lengthString == null)
                        {
                            return GameOperationStatus.Error;
                        }
                        if (!int.TryParse(lengthString, out _length))
                        {
                            return GameOperationStatus.Error;
                        }
                        if (_length < 2)
                        {
                            return GameOperationStatus.Error;
                        }

                        _oldField = new CellState[_length + 2];
                        _newField = new CellState[_length + 2];

                        AliveCount = 0;
                        string cellString;
                        while ((cellString = reader.ReadLine()) != null)
                        {
                            int cell;
                            if (!int.TryParse(cellString, out cell))
                            {
                                return GameOperationStatus.Error;
                            }

                            if (cell < 0 || cell >= _length)
                            {
                                return GameOperationStatus.Error;
                            }

                            _oldField[cell] = CellState.Alive;
                            AliveCount++;
                        }
                    }
                }

                return GameOperationStatus.Ok;
            }
            catch (Exception)
            {
                return GameOperationStatus.Error;
            }
        }

        public void Draw()
        {
            Console.Clear();
            for (var i = 1; i < _length + 1; i++)
            {
                if (_oldField[i] == CellState.Alive)
                    Console.Write(AliveChar);
                else
                    Console.Write(DeadChar);
            }
        }

        public void Update()
        {
            AliveCount = 0;
            for (var i = 1; i < _length + 1; i++)
            {
                if (NeighborCount(i) == 1)
                {
                    _newField[i] = CellState.Alive;
                    AliveCount++;
                }
                else
                    _newField[i] = CellState.Dead;
            }
            Array.Copy(_newField, _oldField, _length + 2);
        }

        public int AliveCount { get; private set; }
        public string Filename { get; }
        #endregion

        #region private

        private int NeighborCount(int index)
        {
            var result = 0;
            if (_oldField[index - 1] == CellState.Alive)
                result++;
            if (_oldField[index + 1] == CellState.Alive)
                result++;
            return result;
        }

        private const char AliveChar = 'O';
        private const char DeadChar = '.';

        private int _length;
        private CellState[] _oldField;
        private CellState[] _newField;

        #endregion
    }
}
