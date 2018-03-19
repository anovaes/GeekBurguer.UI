using System;
using System.Collections.Generic;

namespace GeekBurguer.UI.Contract
{
    public class User
    {
        public Guid UserId { get; set; }
        public List<Allergy> Allergies { get; set; }

        public int TesteCommit { get; set; }

    }
}
