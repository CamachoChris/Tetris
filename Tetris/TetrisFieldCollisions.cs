using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public partial class TetrisField
    {
        private bool GameOverCheck(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            bool gameOver = false;

            bool squareCollision = CollisionWithSquare(matrixTetri, positionX, positionY);
            var (_, _, minY, _) = matrixTetri.GetRange();
            if (squareCollision && (minY + positionY - 1) < 0)
                gameOver = true;

            return gameOver;
        }

        /// <summary>
        /// Tries to find a place by moving the Tetri horizontally from -2 to +2.
        /// </summary>
        /// <param name="matrixTetri"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <returns>collision: true, when collision everywhere. false, when no collision when moving by moveValue</returns>
        private (bool collision, int moveValue) HorizontalPlaceFinder(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            int moveValue = 0;
            bool borderCollision;
            bool squareCollision;
            int[] moveOrder = new int[] { 0, 1, -1, 2, -2 };
            for (int i = 0; i < moveOrder.Length; i++)
            {
                moveValue = moveOrder[i];

                borderCollision = CollisionWithBorder(matrixTetri, positionX + moveValue, positionY);
                squareCollision = CollisionWithSquare(matrixTetri, positionX + moveValue, positionY);

                if (!borderCollision && !squareCollision)
                    return (false, moveValue);
            }
            return (true, moveValue);
        }

        private bool CollisionWithSquare(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            CoordListingTetri coordTetri = new CoordListingTetri(matrixTetri, positionX, positionY);            
            return CollisionWithSquare(coordTetri, null);
        }

        private bool CollisionWithSquare(CoordListingTetri coordTetri, CoordListingTetri exceptTetri)
        {
            if (LandedTetri.Count == 0)
                return false;

            foreach (var entry in LandedTetri)
            {
                if (entry != exceptTetri)
                    for (int i = 0; i < coordTetri.Listing.Count; i++)
                        for (int j = 0; j < entry.Listing.Count; j++)
                            if (coordTetri.Listing[i].X == entry.Listing[j].X && coordTetri.Listing[i].Y == entry.Listing[j].Y)
                                return true;
            }
            return false;
        }

        private bool CollisionWithBorder(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            CoordListingTetri coordTetri = new CoordListingTetri(matrixTetri, positionX, positionY);
            return CollisionWithBorder(coordTetri);
        }

        private bool CollisionWithBorder(CoordListingTetri coordTetri)
        {
            bool collision = false;
            var (minX, maxX, _, maxY) = coordTetri.GetRange();
            if ((minX < 0) || (maxX >= FieldSizeX) || (maxY >= FieldSizeY))
                collision = true;
            return collision;
        }

        private (bool collision, int moveValue) RightRotationCollision(MatrixTetri matrixTetri)
        {
            MatrixTetri rotatedTetri = matrixTetri.GetCopy();
            rotatedTetri.RotateRight();
            return HorizontalPlaceFinder(rotatedTetri, CurrentTetri.PositionX, CurrentTetri.PositionY);
        }

        private (bool collision, int moveValue) LeftRotationCollision(MatrixTetri matrixTetri)
        {
            MatrixTetri rotatedTetri = matrixTetri.GetCopy();
            rotatedTetri.RotateLeft();
            return HorizontalPlaceFinder(rotatedTetri, CurrentTetri.PositionX, CurrentTetri.PositionY);
        }
    }
}
