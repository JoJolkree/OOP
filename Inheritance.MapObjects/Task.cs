﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    public class Dwelling
    {
        public int Owner { get; set; }
    }

    public class Mine
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
    }

    public class Creeps
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class ResourcePile
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is Dwelling)
            {
                ((Dwelling)mapObject).Owner = player.Id;
                return;
            }
            if (mapObject is Mine)
            {
                if (player.CanBeat(((Mine)mapObject).Army))
                    ((Mine)mapObject).Owner = player.Id;
                else player.Die();
                return;
            }
            if (mapObject is Creeps)
            {
                if (player.CanBeat(((Creeps)mapObject).Army))
                    player.Comsume(((Creeps)mapObject).Treasure);
                else
                    player.Die();
                return;
            }
            if (mapObject is ResourcePile)
            {
                player.Comsume(((ResourcePile)mapObject).Treasure);
                return;
            }
        }
    }
}
