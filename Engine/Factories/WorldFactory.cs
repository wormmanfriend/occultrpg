using System;
using System.Collections.Generic;
using System.Text;
using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory //this is so i can put all my location variables in a specific class for organization
    {
        internal static World CreateWorld() 
        {
            World newWorld = new World();

            newWorld.AddLocation(
                0,
                -1,
                "CABIN",
                "Your cabin. \n\n", //Filled with ritual ingredients and found artifacts
                "cabin.gif",
                null
                );

            newWorld.AddLocation(
                -1,
                -1,
                "WORKSHOP",
                "An open suitcase on a table. \n\nContains tools for gardening and surgery.",
                "workshop.gif",
                "CRAFT");

            newWorld.AddLocation(
                1,
                -1,
                "RITUAL GROUNDS",
                "Behind your cabin, psychogenic candles and homegrown herbs on a patch of grass.",
                "ritualgrounds.gif",
                "");

            newWorld.AddLocation
                (
                    0,0, 
                    "DEEP WOODS",
                    "The middle of the woods.\n\nAn ugly nomad sits under a tree. He's playing with dirt and sticks.\n\nThere's a pale light coming from the West.",
                    "woods.gif",
                    "TALK"
                );

            newWorld.LocationAt(0, 0).QuestsHere.Add(QuestFactory.GetQuest(1));


            newWorld.AddLocation
               (
                   -1, 0,
                   "CLEARING",
                   "A small clearing with cut grass.\n\nA salesman is sitting next to his carriage. He's moving his hand around in his coat pocket.",
                   "clearing.gif",
                   "TALK"
               );
            newWorld.LocationAt(-1, 0).TraderHere = TraderFactory.GetTraderbyName("Apothecary");

            newWorld.AddLocation
               (
                   1, 0,
                   "CAVE ENTRANCE",
                   "An entrance to a small cave. \n\nThe wanderer is sitting outside drumming and singing.",
                   "cave.gif",
                   "ATTACK"
               );

            newWorld.LocationAt(1, 0).AddMonster(1, 100);


            return newWorld;

        }
    }
}
