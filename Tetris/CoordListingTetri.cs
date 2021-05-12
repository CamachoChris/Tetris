using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public class CoordListingTetri
    {
        public StandardTetriType TetriType;
        public List<Coord> Listing;

        public CoordListingTetri()
        {
            Listing = new List<Coord>();
        }

        public CoordListingTetri(MatrixTetri matrixTetri) : this()
        {
            GetFromMatrix(matrixTetri, matrixTetri.PositionX, matrixTetri.PositionY);
        }

        public CoordListingTetri(MatrixTetri matrixTetri, int positionX, int positionY) : this()
        {
            GetFromMatrix(matrixTetri, positionX, positionY);
        }

        public CoordListingTetri GetCopy()
        {
            CoordListingTetri copiedTetri = new CoordListingTetri
            {
                Listing = new List<Coord>()
            };

            for (int i = 0; i < this.Listing.Count; i++)
            {
                Coord nextCoord = new Coord
                {
                    X = this.Listing[i].X,
                    Y = this.Listing[i].Y
                };
                copiedTetri.Listing.Add(nextCoord);
            }

            copiedTetri.TetriType = this.TetriType;

            return copiedTetri;
        }

        public void FallOne()
        {
            for (int i = 0; i < Listing.Count; i++)
            {
                Listing[i].Y++;
            }
        }

        public void GetFromMatrix(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            TetriType = matrixTetri.StandardType;

            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                {
                    if (matrixTetri.Matrix[x, y] == true)
                    {
                        Coord nextCoord = new Coord
                        {
                            X = x + positionX,
                            Y = y + positionY
                        };
                        Listing.Add(nextCoord);
                    }                    
                }
        }

        public void RemoveAt(int index)
        {
            Listing.RemoveAt(index);
        }

        public (int minX, int maxX, int minY, int maxY) GetRange()
        {
            int minX = Listing[0].X;
            int maxX = Listing[0].X;
            int minY = Listing[0].Y;
            int maxY = Listing[0].Y;
            for (int i = 1; i < Listing.Count; i++)
            {
                if (Listing[i].X < minX) minX = Listing[i].X;
                if (Listing[i].X > maxX) maxX = Listing[i].X;
                if (Listing[i].Y < minY) minY = Listing[i].Y;
                if (Listing[i].Y > maxY) maxY = Listing[i].Y;
            }
            return (minX, maxX, minY, maxY);
        }

        public void Show()
        {
            for (int i = 0; i < 4; i++)
            {
                Debug.Write($"x:{Listing[i].X} y:{Listing[i].Y}, ");
            }
            Debug.WriteLine(TetriType);
        }
    }
}
