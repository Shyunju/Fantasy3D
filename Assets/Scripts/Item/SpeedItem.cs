using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Fantasy3D
{
    public class SpeedItem : ItemData
    {


        protected override void SendItemInfo()
        {
            _playerMove.PickUpItem(_itemType, this.gameObject);
        }

                         
        
    }
}
