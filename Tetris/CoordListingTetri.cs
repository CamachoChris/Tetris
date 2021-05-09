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

        public event EventHandler RemoveOne;

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

        public void Fall(int lineCount)
        {
            for (int i = 0; i < Listing.Length; i++)
            {
                Listing[i].Y += lineCount;
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

            if (RemoveOne != null)
                RemoveOne(null, EventArgs.Empty);
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
