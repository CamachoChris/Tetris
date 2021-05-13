using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public partial class TetrisField
    {
        /// <summary>
        /// Does the whole work around finished lines, removing, score and levelUp
        /// </summary>
        /// <returns>Returns the amount of found finished lines</returns>
        private int LineFinishCentral()
        {
            int currentFinishedLines = SeekAndDestroyFinishedLines();
            if (currentFinishedLines > 0)
            {
                switch (currentFinishedLines)
                {
                    case 1:
                        _score += 1;
                        break;
                    case 2:
                        _score += 4;
                        break;
                    case 3:
                        _score += 9;
                        break;
                    case 4:
                        _score += 32;
                        break;
                }
                TetriGameScoreChange(_score, EventArgs.Empty);

                _finishedLinesCount += currentFinishedLines;
                if (_finishedLinesCount / 5 + 1 > _level)
                {
                    _level = _finishedLinesCount / 5 + 1;
                    SpeedUp();
                    TetriGameLevelUp(_level, EventArgs.Empty);
                }
            }
            return currentFinishedLines;
        }

        /// <summary>
        /// Looks for finished lines and removes it
        /// </summary>
        /// <returns>Returns amount of finished lines found</returns>
        private int SeekAndDestroyFinishedLines()
        {
            int finishedLineCount = 0;
            bool[,] filledField = ReturnFilledField();

            for (int y = 0; y < FieldSizeY; y++)
            {
                for (int x = 0; x < FieldSizeX; x++)
                {
                    if (filledField[x, y] == false)
                    {
                        break;
                    }
                    if (x == FieldSizeX - 1)
                    {
                        _finishedLinesList.Add(y);
                        RemoveAllSquaresInLine(y);
                        finishedLineCount++;
                    }
                }
            }
            if (finishedLineCount > 0)
            {
                TidyUpLandedList();
                AddSplitTetriToLandedList();
            }

            return finishedLineCount;
        }

        /// <summary>
        /// Fills a 2D-Array analogous to the field with the tetris. True = there is one.
        /// </summary>
        /// <returns>Returns the 2D-Array</returns>
        private bool[,] ReturnFilledField()
        {
            bool[,] filledField = new bool[FieldSizeX, FieldSizeY];
            foreach (var entry in LandedTetri)
            {
                for (int i = 0; i < entry.Listing.Count; i++)
                {
                    if (!entry.IsFalling)
                        filledField[entry.Listing[i].X, entry.Listing[i].Y] = true;
                }
            }
            return filledField;
        }

        private void RemoveAllSquaresInLine(int number)
        {
            foreach (var entry in LandedTetri)
            {
                int i = 0;
                while (i < entry.Listing.Count)
                {
                    if (entry.Listing[i].Y == number)
                    {
                        entry.RemoveAt(i);
                        i--;
                    }
                    i++;
                };
            }
        }

        /// <summary>
        /// Removes all entries in LandedTetri with 0 squares left.
        /// </summary>
        private void TidyUpLandedList()
        {
            List<CoordListingTetri> emptyEntry = new List<CoordListingTetri>();
            foreach (var entry in LandedTetri)
            {
                if (entry.Listing.Count == 0)
                    emptyEntry.Add(entry);
            }
            foreach (var entry in emptyEntry)
            {
                LandedTetri.Remove(entry);
            }
        }

        /// <summary>
        /// If tetri is split, upper half becomes new tetri in landed list
        /// </summary>
        private void AddSplitTetriToLandedList()
        {
            List<CoordListingTetri> splittetTetri = new List<CoordListingTetri>();
            foreach (var entry in LandedTetri)
            {
                CoordListingTetri tmp = GetSplitTetri(entry);
                if (tmp != null)
                    splittetTetri.Add(tmp);
            }
            foreach (var entry in splittetTetri)
            {
                LandedTetri.Add(entry);
            }
        }

        /// <summary>
        /// Looks, if tetri is split. And returns upper half.
        /// </summary>
        /// <param name="tetri"></param>
        /// <returns>Returns null, if tetri is not split.</returns>
        private CoordListingTetri GetSplitTetri(CoordListingTetri tetri)
        {
            var (_, _, minY, maxY) = tetri.GetRange();
            if (maxY - minY < 2)
                return null;

            int count = 0;
            List<int> emptyLines = new List<int>();

            // Find empty lines.
            for (int y = minY + 1; y < maxY; y++)
            {
                for (int i = 0; i < tetri.Listing.Count; i++)
                {
                    if (y != tetri.Listing[i].Y)
                        count++;

                    if (count == tetri.Listing.Count)
                        emptyLines.Add(y);
                }
                count = 0;
            }

            if (emptyLines.Count > 0)
            {
                // Fill new Tetri with upper half of divided Tetri
                CoordListingTetri newTetri = new CoordListingTetri();
                newTetri.TetriType = tetri.TetriType;
                foreach (var entry in tetri.Listing)
                {
                    if (entry.Y < emptyLines[0])
                        newTetri.Listing.Add(entry);
                }

                // Remove squares moved to new Tetri
                foreach (var entry in newTetri.Listing)
                {
                    tetri.Listing.Remove(entry);
                }
                return newTetri;
            }
            else
                return null;
        }

        /// <summary>
        /// Checks if any tetri in landed list is falling
        /// </summary>
        /// <returns>true if any is falling</returns>
        private bool IsAnyFalling()
        {
            if (_finishedLinesList.Count > 0)
                return true;

            foreach (var entry in LandedTetri)
            {
                if (entry.IsFalling)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Lets the tetri in the LandedTetri List fall.
        /// </summary>
        /// <returns>return true, if any could fall</returns>
        private bool LetThemFall()
        {
            bool couldFall = false;
            int lowestRow = -1;

            if (_finishedLinesList.Count > 0)
                lowestRow = _finishedLinesList[^1];

            if (lowestRow > -1)
            {
                foreach (var entry in LandedTetri)
                {
                    var (_, _, minY, maxY) = entry.GetRange();
                    if (maxY < lowestRow)
                    {
                        entry.FallOne();
                        couldFall = true;
                    }
                    if (minY > lowestRow)
                    {
                        if (TetriCanFall(entry) == true)
                            couldFall = true;
                    }
                }
                _finishedLinesList.RemoveAt(_finishedLinesList.Count - 1);
                if (_finishedLinesList.Count > 0)
                {
                    for (int i = 0; i < _finishedLinesList.Count; i++)
                    {
                        _finishedLinesList[i]++;
                    }
                }
            }
            else
            {
                foreach (var entry in LandedTetri)
                    if (TetriCanFall(entry) == true)
                        couldFall = true;
            }

            return couldFall;
        }

        /// <summary>
        /// Checks if falling is possible and lets the Tetris fall.
        /// </summary>
        /// <param name="landedTetri"></param>
        /// <returns>true if could fall, false if not</returns>
        private bool TetriCanFall(CoordListingTetri landedTetri)
        {
            CoordListingTetri copiedTetri = landedTetri.GetCopy();
            copiedTetri.FallOne();

            bool borderCollision = CollisionWithBorder(copiedTetri);
            bool squareCollision = CollisionWithSquare(copiedTetri, landedTetri);

            if (!borderCollision && !squareCollision)
            {
                landedTetri.IsFalling = true;
                landedTetri.FallOne();
                return true;
            }
            else
            {
                landedTetri.IsFalling = false;
                return false;
            }
        }
    }
}
