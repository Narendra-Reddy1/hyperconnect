using System;
using UnityEngine;

namespace HyperConnect
{
    public static class GlobalEventHandler
    {
        public static Action<TileEntity> OnTileEntitySelected = default;
        public static Action<TileEntity> OnTileEntityUnSelected = default;

        public static Action<int> OnNewLevelStarted = default;
    }
}