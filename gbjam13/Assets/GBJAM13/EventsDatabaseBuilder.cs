using UnityEngine;

namespace GBJAM13
{
    public class EventsDatabaseBuilder : MonoBehaviour
    {
        public GameObject eventsDatabasePrefab;

        private void Awake()
        {
            var eventsDatabase = EventsDatabase.Instance;
            if (!eventsDatabase)
            {
                var eventsDatabaseObject = Instantiate(eventsDatabasePrefab);
                eventsDatabaseObject.name = EventsDatabase.InstanceName;
                DontDestroyOnLoad(eventsDatabaseObject);
            }
        }
    }
}