#if GAME_SETTINGS
using UnityEngine;
using System.Collections;

namespace KWGameSettings
{
    namespace GameSettings
    {
        [System.Serializable]
        public partial class GameSettings
#if KANGA
        : KWServer.Requests.ServerObjectDocument
#endif
        {
             {% for data in data_dictionary %} public {{ data.type }} {{ data.name }};
             {% endfor %}          
#if KANGA
            partial void ProjectAddMappings()
            {
               {% for data in data_dictionary %} AddMapping("{{ data.name }}", () => {{ data.name}});
               {% endfor %}
            }
#endif
        }
    }
}
#endif
