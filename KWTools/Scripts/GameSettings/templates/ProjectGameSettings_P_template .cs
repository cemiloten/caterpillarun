using UnityEngine;
using System.Collections;
using KWGameSettings.GameSettings;

[System.Serializable]
public class ProjectGameSettings : GameSettings
{
        {% for data in data_dictionary %} public {{ data.type}} {{ data.name}};
        {% endfor %}          
    protected override void AddMappings()
    {
        base.AddMappings();
        {% for data in data_dictionary %} AddMapping("{{ data.name }}", () => {{ data.name}});
        {% endfor %}
    }

}
