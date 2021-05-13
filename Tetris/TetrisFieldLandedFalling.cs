﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public partial class TetrisField
    {
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
        /// Fills a 2D-Array analogous to the field with the tetris. True = there is one.
        /// </summary>
        /// <returns></returns>
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
                        RemoveSquaresInFinishedLine(y);
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

        private void RemoveSquaresInFinishedLine(int lineNumber)
        {
            foreach (var entry in LandedTetri)
            {
                int i = 0;
                while (i < entry.Listing.Count)
                {
                    if (entry.Listing[i].Y == lineNumber)
                    {
                        entry.RemoveAt(i);
                        i--;
                    }
                    i++;
                };
            }
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
                        if (FallingDown(entry) == true)
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
                    if (FallingDown(entry) == true)
                        couldFall = true;
            }

            return couldFall;
        }

        /// <summary>
        /// Checks if falling is possible and lets the Tetris fall.
        /// </summary>
        /// <param name="landedTetri"></param>
        /// <returns>true if could fall, false if not</returns>
        private bool FallingDown(CoordListingTetri landedTetri)
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
    }
}