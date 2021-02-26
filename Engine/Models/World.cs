using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class World
    {
        private List<Location> _locations = new List<Location>(); //creating a list to store sets of variables(coords, text, images) for locations

        internal void AddLocation(int xcord, int ycord, string name, string desc, string imageName, string button) //method to streamline creating variable sets for locations and to add these sets to the list above, 
                                                                                                    //only needing to write out object initialization(down below) once
        {
            Location loc = new Location(); //object initialization
            loc.Xcord = xcord;
            loc.Ycord = ycord;
            loc.Name = name;
            loc.Desc = desc;
            loc.ImageName = $"/WPFUI;component/image/locations/{imageName}";
            loc.Button = button;

            _locations.Add(loc);
        }

        public Location LocationAt(int xCord, int yCord)
        {
            foreach(Location loc in _locations) // cycle through each location in the "_locations" list, looking in the Location variable "loc" for matches in coordinates
            {
                if(loc.Xcord == xCord && loc.Ycord == yCord)
                {
                    return loc; //if the coordinates correspond to ones that are part of an existing location set, the information is returned to the gamesession tab where it can be called in the xaml UI, otherwise...
                }
            }

            return null; //...it returns nothing
        }
    }
}
