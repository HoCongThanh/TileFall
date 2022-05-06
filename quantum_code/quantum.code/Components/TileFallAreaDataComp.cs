using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public partial struct TileFallAreaDataComp
    {
        public unsafe void Init(Frame f)
        {
            if (IsDone)
                return;
            if (f.TryResolveList(listTiles, out var listTileEntity))
            {
                var randomTiles = GenerateRandomTilesFall(f, TilesRow, TilesColumn);

                int tileCount = 0;

                // Lambda cannot access this so need to copy to temp variable
                int tilesColumn = TilesColumn;

                listTileEntity.Foreach((tileEntity) =>
                {
                    if (f.Unsafe.TryGetPointer<TileFallComp>(tileEntity, out var tileComp))
                    {
                        bool isCorrectTile = randomTiles[tileCount / tilesColumn, tileCount % tilesColumn];
                        Log.Info("TileCount " + tileCount + "_" + isCorrectTile);
                        tileComp->IsFallable = !isCorrectTile;

                        tileCount++;
                    }
                });
            }
            IsDone = true;
        }

        public unsafe bool[,] GenerateRandomTilesFall(Frame f, int row, int column)
        {
            Log.Info("GenerateRandomTilesFall " + row + "_" + column + "_" + f.RuntimeConfig.Seed);

            bool[,] result = new bool[row, column];

            int currentColumnIndex = f.RNG->Next(0, column);

            int currentRowIndex = 0;

            Log.Info("GenerateRandomTilesFall First " + currentRowIndex + "_" + currentColumnIndex + "_" + result.Length);

            result[currentRowIndex, currentColumnIndex] = true;

            while(currentRowIndex < row - 1)
            {
                Log.Info("While " + currentRowIndex + "_" + currentColumnIndex);

                int leftColumnIndex = currentColumnIndex - 1;
                bool isLeftDirectionValid = !(leftColumnIndex < 0 || leftColumnIndex >= column || 
                                             result[currentRowIndex, leftColumnIndex] == true);

                int rightColumnIndex = currentColumnIndex + 1;
                bool isRightDirectionValid = !(rightColumnIndex < 0 || rightColumnIndex >= column || 
                                              result[currentRowIndex, rightColumnIndex] == true);

                int direction = f.RNG->NextInclusive(isLeftDirectionValid ? 0 : 1 , isRightDirectionValid ? 2 : 1);

                Log.Info("Random direction " + isLeftDirectionValid + "_" + isRightDirectionValid + "_" + direction);

                int nextRowIndex = currentRowIndex;
                int nextColumnIndex = currentColumnIndex;

                switch (direction)
                {
                    case 0: // left
                        nextColumnIndex -= 1;
                        break;
                    case 1: // foward
                        nextRowIndex += 1;
                        break;
                    case 2: // right
                        nextColumnIndex += 1;
                        break;
                }

                result[nextRowIndex, nextColumnIndex] = true;
                currentRowIndex = nextRowIndex;
                currentColumnIndex = nextColumnIndex;
            }

            return result;
        }
    }
}
