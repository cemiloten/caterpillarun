using System.Collections;

namespace KWGameSettings
{
    namespace GameSettings
    {
        [System.Serializable]
        public class {{ClassName}} 
   
        : KWServer.Requests.ServerObjectBase
        {
             {% for data in data_dictionary %} public {{ data.type }} {{ data.name }};
             {% endfor %}       
            protected override void AddMappings()
            {
               {% for data in data_dictionary %} AddMapping("{{ data.name }}", () => {{ data.name}});
               {% endfor %}
            }        

        }
    }
}
