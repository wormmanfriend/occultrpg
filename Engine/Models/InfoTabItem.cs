using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class InfoTabItem
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string Desc { get; set; }
        public string ButtonContent { get; set; }
        public bool ItemIsInteractable { get; set; }
        public InfoTabItem(string name, string imageName, string desc, string buttonContent, bool interactable)
        {
            Name = name;
            ImageName = imageName;
            Desc = desc;
            ButtonContent = buttonContent;
            ItemIsInteractable = interactable;
        }
    }
}
