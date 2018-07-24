using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Snap
{
    public abstract class Widget
    {
        string styleName;

        public abstract GameObject createGameObject(Dictionary<string,GameObject> lookAndFeel);
    }
}
