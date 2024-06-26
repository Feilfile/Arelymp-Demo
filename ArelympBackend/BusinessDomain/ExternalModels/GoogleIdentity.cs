using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.ExternalModels
{
    public class GoogleIdentity
    {
        public string Player_id { get; set; }

        public string Alternate_player_id { get; set; }

        public string Kind { get; set; }

        public GoogleIdentity(string playerId, string alternate_player_id, string kind) 
        {
            Player_id = playerId;
            Alternate_player_id = alternate_player_id;
            Kind = kind;
        }
    }
}
