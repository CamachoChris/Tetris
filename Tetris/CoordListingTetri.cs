using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TetrisModel
{
    public class CoordListingTetri
    {
        public StandardTetriType TetriType;
        public Coord[] Listing;

        public CoordListingTetri()
        {
            Listing = new Coord[4];
        }

        public CoordListingTetri(MatrixTetri matrixTetri)
        {
            Listing = new Coord[4];
            GetFromMatrix(matrixTetri, matrixTetri.PositionX, matrixTetri.PositionY);
        }

        public CoordListingTetri(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            Listing = new Coord[4];
            GetFromMatrix(matrixTetri, positionX, positionY);
        }

        public CoordListingTetri GetCopy()
        {
            CoordListingTetri copiedTetri = new CoordListingTetri();

            copiedTetri.Listing = new Coord[this.Listing.Length];
            for (int i = 0; i < this.Listing.Length; i++)
            {
                copiedTetri.Listing[i] = this.Listing[i];
            }

            copiedTetri.TetriType = this.TetriType;

            return copiedTetri;
        }

        public void FallOne()
        {
            for (int i = 0; i < Listing.Length; i++)
            {
                Listing[i].Y++;
            }
        }

        public void GetFromMatrix(MatrixTetri matrixTetri, int positionX, int positionY)
        {
            int count = 0;
            TetriType = matrixTetri.StandardType;

            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                {
                    if (matrixTetri.Matrix[x, y] == true)
                    {
                        Listing[count].X = x + positionX;
                        Listing[count].Y = y + positionY;
                        count++;
                    }                    
                }
        }

        public void CompressUpDown()
        {
            int count = 0;
            List<int> emptyLines = new List<int>();

            var (_, _, minY, maxY) = GetRange();

            // Find empty lines.
            for (int y = minY; y < maxY; y++)
            {
                for (int i = 0; i < Listing.Length; i++)
                {
                    if (y != Listing[i].Y)
                        count++;

                    if (count == Listing.Length)
                        emptyLines.Add(y);
                }
                count = 0;
            }

            // Move squares above empty lines 1 down.
            foreach (var entry in emptyLines)
            {
                for (int i = 0; i < Listing.Length; i++)
                {
                    if (Listing[i].Y < entry)
                        Listing[i].Y++;
                }
            }
        }

        public void RemoveAt(int index)
        {
            Coord[] shortenedListing = new Coord[Listing.Length - 1];
            int ListingCount = 0;
            for (int i = 0; i < Listing.Length; i++)
            {
                if (i != index)
                    shortenedListing[ListingCount++] = Listing[i];
            }
            Listing = shortenedListing;
        }

        public (int minX, int maxX, int minY, int maxY) GetRange()
        {
            int minX = Listing[0].X;
            int maxX = Listing[0].X;
            int minY = Listing[0].Y;
            int maxY = Listing[0].Y;
            for (int i = 1; i < Listing.Length; i++)
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
